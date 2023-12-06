using System;
using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class PlayerController : MonoBehaviour
    {
        [Serializable] private enum Status
        {
            Stopped,
            Walking,
            Running
        }

        [Header("Settings")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private int playerId;
        [SerializeField] private Color disabledColor;

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

        public int PlayerId
        {
            set
            {
                playerId = value;
                name = $"Player {playerId} (";
            }
        }

        public int HumanId
        {
            set => name += $"human {value})";
        }

        public int RobotId
        {
            set => name += $"robot {value})";
        }

        private void Start() => cachedTransform = transform;

        protected void Update()
        {
            if (gameSpeed.Value <= 0 || disabled) return;
            if (transform.position.x > goal.Value)
            {
                playerWin.Raise(playerId);
                DisablePlayer();
            }

            if (currentStatus == Status.Stopped) return;
            var speed = currentStatus == Status.Running ? runSpeed : walkSpeed;
            cachedTransform.position += Time.deltaTime * gameSpeed.Value * speed * cachedTransform.right;
        }

        public void OnProjectileHit(Collider2D other)
        {
            if (!other.transform.CompareTag("Projectile")) return;
            Destroy(other.gameObject); // en vrai juste instantiation d'une animation one shot sur le hit.point et c op
            playerDeath.Raise(playerId);
            DisablePlayer();
        }

        public void StartWalking()
        {
            animator.SetBool("Walking", true);
            currentStatus = Status.Walking;
        }

        public void StartRunning()
        {
            animator.SetBool("Running", true);
            currentStatus = Status.Running;
        }

        public void Stop()
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
            currentStatus = Status.Stopped;
        }

        public void Taunt()
        {
            Debug.Log("I am Taunting wow");
            // independant : prends le pas temporairement pour faire le taunt puis reprends action precedente
        }

        private void DisablePlayer()
        {
            animator.SetTrigger("Death");
            disabled = true;
            //sprite.color = disabledColor;
        }
    }
}