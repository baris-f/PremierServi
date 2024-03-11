using System;
using System.Collections.Generic;
using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int Walking = Animator.StringToHash("Walking");
        private static readonly int Running = Animator.StringToHash("Running");
        private static readonly int Taunting = Animator.StringToHash("Taunting");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int Paused = Animator.StringToHash("Paused");

        [Serializable] private enum Status
        {
            Stopped,
            Walking,
            Running,
            Taunting
        }

        [Header("Settings")]
        [SerializeField] private int playerId;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        [Header("References")]
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource audioSource;

        [Header("Assets")]
        [SerializeField] private AudioClip walkClip;
        [SerializeField] private AudioClip runClip;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;
        [SerializeField] private ScriptableFloat goal;

        [Header("Events")]
        [SerializeField] private PlayerEvent playerWin;
        [SerializeField] private PlayerEvent playerDeath;

        [Header("Debug")]
        [SerializeField] private Status currentStatus;
        [SerializeField] private List<Status> statusHistory = new();

        private Transform cachedTransform;
        private bool disabled;
        private PlayerEvent.Type playerType;
        private Status CurrentStatus
        {
            get => currentStatus;
            set
            {
                currentStatus = value;
                animator.SetBool(Walking, value == Status.Walking);
                animator.SetBool(Running, value == Status.Running);
                animator.SetBool(Taunting, value == Status.Taunting);
                statusHistory.Add(value);
            }
        }

        public void Init(PlayerEvent.Type type, int newPlayerId, int typeId, float newWalkSpeed, float newRunSpeed)
        {
            playerId = newPlayerId;
            playerType = type;
            var typeName = type switch
            {
                PlayerEvent.Type.Human => "Human", PlayerEvent.Type.Robot => "Robot",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            name = $"Player {playerId} - {typeName} {typeId}";
            walkSpeed = newWalkSpeed;
            runSpeed = newRunSpeed;
            gameSpeed.OnValueChanged += SetPauseState;
        }

        private void Start() => cachedTransform = transform;
        private void OnDisable() => gameSpeed.OnValueChanged -= SetPauseState;
        private void SetPauseState(float speed) => animator.SetBool(Paused, speed <= 0);

        protected void Update()
        {
            if (gameSpeed.Value <= 0 || disabled) return;
            if (transform.position.x > goal.Value)
            {
                playerWin.Raise(playerId, playerType);
                // animator.SetTrigger(Win); todo faire l'animation
                DisablePlayer();
            }

            if (CurrentStatus is Status.Stopped or Status.Taunting) return;
            var speed = CurrentStatus == Status.Running ? runSpeed : walkSpeed;
            cachedTransform.position += Time.deltaTime * gameSpeed.Value * speed * cachedTransform.right;
        }

        public void OnProjectileHit(Collider2D other)
        {
            if (!other.transform.CompareTag("Projectile")) return;
            Destroy(other.gameObject); // en vrai juste instantiation d'une animation one shot sur le hit.point et c op
            playerDeath.Raise(playerId, playerType);
            animator.SetTrigger(Death);
            DisablePlayer();
        }

        public void StartWalking()
        {
            audioSource.clip = walkClip;
            audioSource.Play();
            CurrentStatus = Status.Walking;
        }

        public void StartRunning()
        {
            audioSource.clip = runClip;
            audioSource.Play();
            CurrentStatus = Status.Running;
        }

        public void StartTaunt()
        {
            CurrentStatus = Status.Taunting;
        }

        public void Stop()
        {
            audioSource.Stop();
            CurrentStatus = Status.Stopped;
        }

        private void DisablePlayer()
        {
            Stop();
            disabled = true;
        }
    }
}