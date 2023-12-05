using Modules.Common.Controllers.Runtime;
using Modules.Technical.GameConfig.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Common.Inputs.Runtime
{
    public class HumanInput : MonoBehaviour
    {
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
            walk.started += _ => player.StartWalking();
            walk.canceled += _ => player.Stop();
            run = input.actions["Run"];
            run.started += _ => player.StartRunning();
            run.canceled += _ => player.Stop();
            taunt = input.actions["Taunt"];
            taunt.started += _ => player.Taunt();

            // CanonController
            move = input.actions["Move"];
            // move.performed += ctx => canon.Move(ctx.ReadValue<Vector2>());
            fire = input.actions["Fire"];
            fire.started += _ => canon.Fire();
        }

        private void Update() => 
            canon.Move(move.ReadValue<Vector2>());
    }
}