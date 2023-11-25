using Modules.Technical.GameConfig.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Modules.Common.Inputs.Runtime
{
    public class HumanInput : IInput
    {
        private InputUser user;

        public HumanInput(GameConfig.Human human)
        {
            human.Device ??= InputSystem.GetDevice(human.deviceName);
            // todo trouver un moyen de gerer, async qui tente la connection en boucle ?
            if (human.Device == null)
            {
                Debug.LogError($"No Device found with name {human.deviceName}, skipping");
                return;
            }
            user = InputUser.PerformPairingWithDevice(human.Device);
        }
        // traduit les inputs du joueur vers un character controller et un canon controller
        
        public void WhatShouldIDo()
        {
            
        }
    }
}