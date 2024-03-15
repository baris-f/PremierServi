using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Common.Inputs.Runtime
{
    public class HumanInput : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private SimpleLocalEvent openPauseMenu;

        [Header("Debug")]
        [SerializeField] private PlayerController player;
        [SerializeField] private CanonController canon;
        [SerializeField] private PlayerInput input;

        // Player
        private InputAction walk;
        private InputAction run;
        private InputAction taunt;

        // Canon
        private InputAction move;
        private InputAction fire;

        // Other
        private InputAction pause;

        private enum MovementActions
        {
            Walk,
            Run,
            Taunt,
            Stop
        }

        private List<MovementActions> activeActions = new() { MovementActions.Stop };
        private MovementActions CurMovementAction => activeActions[^1];

        public static HumanInput Instantiate(HumanInput prefab, Transform container, Human human,
            PlayerController player, CanonController canon)
        {
            var inputDevice = InputSystem.GetDevice(human.deviceName);
            var playerInput = PlayerInput.Instantiate(prefab.gameObject, pairWithDevice: inputDevice);
            playerInput.transform.SetParent(container);
            var humanInput = playerInput.GetComponent<HumanInput>();
            humanInput.player = player;
            humanInput.canon = canon;
            return humanInput;
        }

        private void Start()
        {
            // PlayerController
            walk = input.actions["Walk"];
            walk.started += _ => AddAction(MovementActions.Walk);
            walk.canceled += _ => RemoveAction(MovementActions.Walk);

            run = input.actions["Run"];
            run.started += _ => AddAction(MovementActions.Run);
            run.canceled += _ => RemoveAction(MovementActions.Run);

            taunt = input.actions["Taunt"];
            taunt.started += _ => AddAction(MovementActions.Taunt);
            taunt.canceled += _ => RemoveAction(MovementActions.Taunt);

            // CanonController
            move = input.actions["Move"];
            fire = input.actions["Fire"];
            fire.started += _ => canon.Fire();

            // Pause
            pause = input.actions["Pause"];
            pause.performed += _ => openPauseMenu.Raise();
        }

        private void AddAction(MovementActions movementActions)
        {
            if (activeActions.Contains(movementActions))
                activeActions.Remove(movementActions);
            activeActions.Add(movementActions);
            CheckAction();
        }

        private void RemoveAction(MovementActions movementActions)
        {
            if (activeActions.Contains(movementActions))
                activeActions.Remove(movementActions);
            CheckAction();
        }

        private void CheckAction()
        {
            switch (CurMovementAction)
            {
                case MovementActions.Run:
                    player.StartRunning();
                    break;
                case MovementActions.Walk:
                    player.StartWalking();
                    break;
                case MovementActions.Taunt:
                    player.StartTaunt();
                    break;
                case MovementActions.Stop:
                    player.Stop();
                    break;
            }
        }

        private void Update() =>
            canon.Move(move.ReadValue<Vector2>());
    }
}