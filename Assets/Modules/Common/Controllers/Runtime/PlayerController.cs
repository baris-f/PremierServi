﻿using System;
using System.Collections.Generic;
using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
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

        [Header("References")]
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource audioSource;

        [Header("Assets")]
        [SerializeField] private AudioClip walkClip;
        [SerializeField] private AudioClip runClip;
        [SerializeField] private AudioClip deathClip;

        [Header("Scriptables")]
        [SerializeField] private ScriptableFloat gameSpeed;
        [SerializeField] private ScriptableFloat goal;
        [SerializeField] private PlayerEvent playerWin;

        [Header("Debug")]
        [SerializeField] private Status currentStatus;
        [SerializeField] private List<Status> statusHistory = new();

        private float walkSpeed;
        private float runSpeed;
        private Transform cachedTransform;
        private bool disabled;
        private readonly PlayerEvent.PlayerData playerData = new();

        public int Id => playerData.id;

        public bool IsMoving => !disabled && CurrentStatus is Status.Running or Status.Walking or Status.Taunting;

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
            playerData.id = newPlayerId;
            playerData.type = type;
            playerData.typeId = typeId;
            var typeName = playerData.type switch
            {
                PlayerEvent.Type.Human => "Human", PlayerEvent.Type.Robot => "Robot",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            name = $"Player {playerData.id} - {typeName} {playerData.typeId}";
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
                playerWin.Raise(playerData);
                // animator.SetTrigger(Win); todo faire l'animation
                DisablePlayer();
            }

            if (CurrentStatus is Status.Stopped or Status.Taunting) return;
            var speed = CurrentStatus == Status.Running ? runSpeed : walkSpeed;
            cachedTransform.position += Time.deltaTime * gameSpeed.Value * speed * cachedTransform.right;
        }

        public void OnPlayerDeath(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData receivedPlayerData
                || receivedPlayerData.id != playerData.id) return;
            animator.SetTrigger(Death);
            if (deathClip != null) audioSource.PlayOneShot(deathClip);
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