using Modules.Common.Controllers.Runtime;
using Modules.Technical.GameConfig.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Modules.Common.Inputs.Runtime
{
    public class HumanInput : MonoBehaviour
    {
        [SerializeField] private string userInfos;
        [SerializeField] private PlayerController player;
        [SerializeField] private CanonController canon;

        [Header("Inputs")]
        [SerializeField] private InputActionAsset asset;

        // Player
        private InputAction walk;
        private InputAction run;
        private InputAction taunt;

        // Canon
        private InputAction move;
        private InputAction fire;

        public void Init(GameConfig.Human human, PlayerController newPlayer, CanonController newCanon)
        {
            player = newPlayer;
            canon = newCanon;
            human.Device ??= InputSystem.GetDevice(human.deviceName);
            // todo trouver un moyen de gerer, async qui tente la connection en boucle ?
            if (human.Device == null)
            {
                Debug.LogError($"No Device found with name {human.deviceName}, skipping");
                return;
            }

            var user = InputUser.PerformPairingWithDevice(human.Device);
            userInfos = $"{user.id} : {user.pairedDevices[0].displayName}";
            // var scheme = InputControlScheme.FindControlSchemeForDevice(user.pairedDevices[0], asset.controlSchemes);
            // if (scheme == null)
            // {
            //     Debug.LogError($"Couldn't find scheme compatible with device {user.pairedDevices[0].displayName}");
            //     return;
            // }
            //
            // user.ActivateControlScheme((InputControlScheme)scheme);
            
        }
    }
}