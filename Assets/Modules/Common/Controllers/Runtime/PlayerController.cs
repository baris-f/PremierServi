using System;
using Modules.Common.CustomEvents.Runtime;
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

        [Header("Events")]
        [SerializeField] private PlayerWinEvent playerWin;

        [Header("Debug")]
        [SerializeField] private Status currentStatus;
        private Transform cachedTransform;

        private void Start() => cachedTransform = transform;

        protected void Update()
        {
            if (transform.position.x > goal.position.x)
            {
                Debug.Log("win yay");
                playerWin.Raise(playerId);
            }

            if (currentStatus == Status.Stopped) return;
            var speed = currentStatus == Status.Running ? runSpeed : walkSpeed;
            cachedTransform.position += Time.deltaTime * speed * cachedTransform.right;
        }

        public void StartWalking() => currentStatus = Status.Walking;

        public void StartRunning() => currentStatus = Status.Running;

        public void Stop() => currentStatus = Status.Stopped;

        public void Taunt()
        {
            Debug.Log("I am Taunting wow");
            // independant : prends le pas temporairement pour faire le taunt puis reprends action precedente
        }
    }
}