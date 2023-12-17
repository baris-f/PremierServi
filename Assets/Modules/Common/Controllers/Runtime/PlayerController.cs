using System;
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

        [Serializable] private enum Status
        {
            Stopped,
            Walking,
            Running,
            Taunting
        }

        [Header("Settings")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private int playerId;

        [Header("References")]
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;
        [SerializeField] private ScriptableFloat goal;

        [Header("Events")]
        [SerializeField] private PlayerEvent playerWin;
        [SerializeField] private PlayerEvent playerDeath;

        [Header("Debug")]
        [SerializeField] private Status currentStatus;

        private Transform cachedTransform;
        private bool disabled;
        private PlayerEvent.Type playerType;

        public void Init(PlayerEvent.Type type, int newPlayerId, int typeId)
        {
            playerId = newPlayerId;
            playerType = type;
            var typeName = type switch
            {
                PlayerEvent.Type.Human => "Human", PlayerEvent.Type.Robot => "Robot"
            };
            name = $"Player {playerId} - {typeName} {typeId}";
        }

        private void Start() => cachedTransform = transform;

        protected void Update()
        {
            if (gameSpeed.Value <= 0 || disabled) return;
            if (transform.position.x > goal.Value)
            {
                playerWin.Raise(playerId, playerType);
                DisablePlayer();
            }

            if (currentStatus is Status.Stopped or Status.Taunting) return;
            var speed = currentStatus == Status.Running ? runSpeed : walkSpeed;
            cachedTransform.position += Time.deltaTime * gameSpeed.Value * speed * cachedTransform.right;
        }

        public void OnProjectileHit(Collider2D other)
        {
            if (!other.transform.CompareTag("Projectile")) return;
            Destroy(other.gameObject); // en vrai juste instantiation d'une animation one shot sur le hit.point et c op
            playerDeath.Raise(playerId, playerType);
            DisablePlayer();
        }

        public void StartWalking()
        {
            animator.SetBool(Walking, true);
            currentStatus = Status.Walking;
        }

        public void StartRunning()
        {
            animator.SetBool(Running, true);
            currentStatus = Status.Running;
        }

        public void Stop()
        {
            animator.SetBool(Walking, false);
            animator.SetBool(Running, false);
            animator.SetBool(Taunting, false);
            currentStatus = Status.Stopped;
        }

        public void Taunt()
        {
            animator.SetBool(Taunting, true);
            currentStatus = Status.Taunting;
        }

        private void DisablePlayer()
        {
            animator.SetTrigger(Death);
            disabled = true;
        }
    }
}