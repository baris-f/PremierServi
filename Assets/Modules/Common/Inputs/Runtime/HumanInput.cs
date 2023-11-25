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
        }
    }
}