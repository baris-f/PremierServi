using System.Collections.Generic;
using Modules.Technical.GameConfig.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Modules.Common.InputManager.Runtime
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private List<string> users = new();

        private void Start()
        {
            foreach (var player in gameConfig.Players)
            {
                if (string.IsNullOrWhiteSpace(player.deviceName)) continue;
                player.Device ??= InputSystem.GetDevice(player.deviceName);
                var user = InputUser.PerformPairingWithDevice(player.Device);
                users.Add($"{user.id}:{user.pairedDevices[0].displayName}");
            }
        }
    }
}