using System;
using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
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
        [SerializeField] private Transform goal;
        [SerializeField] private int playerId;
        [SerializeField] private Color disabledColor;

        [Header("References")]
        [SerializeField] private SpriteRenderer sprite;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;

        [Header("Events")]
        [SerializeField] private PlayerEvent playerWin;
        [SerializeField] private PlayerEvent playerDeath;

        [Header("Debug")]
        [SerializeField] private Status currentStatus;
        
        private Transform cachedTransform;
        private bool disabled;

        public void Init(int newPlayerId, int humanId)
        {
            playerId = newPlayerId;
            name = $"Player {playerId} for human {humanId}";
        }


        private void Start() => cachedTransform = transform;

        protected void Update()
        {
            if (gameSpeed.Value <= 0 || disabled) return;
            if (transform.position.x > goal.position.x)
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
            Destroy(other); // en vrai juste instantiation d'une animation one shot sur le hit.point et c op
            playerDeath.Raise(playerId);
            DisablePlayer();
        }

        public void StartWalking() => currentStatus = Status.Walking;

        public void StartRunning() => currentStatus = Status.Running;

        public void Stop() => currentStatus = Status.Stopped;

        public void Taunt()
        {
            Debug.Log("I am Taunting wow");
            // independant : prends le pas temporairement pour faire le taunt puis reprends action precedente
        }

        private void DisablePlayer()
        {
            disabled = true;
            sprite.color = disabledColor;
        }
    }
}