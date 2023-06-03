using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Modules.MainMenu.Runtime
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset;

        private readonly List<InputUser> users = new();

        private void Awake()
        {
            foreach (var device in InputUser.GetUnpairedInputDevices())
            {
                if (device is Mouse) continue;
                CreateUser(device);
            }

            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change != InputDeviceChange.Added) return;
            var user = InputUser.FindUserPairedToDevice(device);
            if (user is null)
                CreateUser(device);
        }

        private void CreateUser(InputDevice device)
        {
            var user = InputUser.PerformPairingWithDevice(device);
            if (device is Keyboard)
            {
                var mouse = InputSystem.GetDevice(typeof(Mouse));
                if (mouse != null)
                    InputUser.PerformPairingWithDevice(mouse, user);
            }

            user.AssociateActionsWithUser(inputActionAsset);
            var scheme =
                InputControlScheme.FindControlSchemeForDevice(user.pairedDevices[0], user.actions.controlSchemes);
            if (scheme is not null)
                user.ActivateControlScheme((InputControlScheme)scheme);
            users.Add(user);
        }
    }
}