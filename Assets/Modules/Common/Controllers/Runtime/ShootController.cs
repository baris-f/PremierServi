using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptableField.Implementations;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public abstract class ShootController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected ScriptableFloat gameSpeed;
        [SerializeField] protected PlayerEvent playerFire;

        protected bool Disabled;
        protected readonly PlayerEvent.PlayerData PlayerData = new();
        private int shotsLeft;

        protected void Init(int newPlayerId, int humanId, int maxBullets)
        {
            PlayerData.id = newPlayerId;
            PlayerData.type = PlayerEvent.Type.Human;
            PlayerData.typeId = humanId;
            shotsLeft = maxBullets;
        }

        public abstract void Move(Vector2 amount);
        public abstract void Fire();
        protected abstract void DisableShooter();

        protected bool FireInternal()
        {
            if (gameSpeed.Value <= 0 || Disabled) return false;
            playerFire.Raise(PlayerData);
            shotsLeft--;
            if (shotsLeft <= 0) DisableShooter();
            return true;
        }


        protected void DisableShooterInternal() => Disabled = true;

        public void OnPlayerDeath(MinimalData data)
        {
            if (!Disabled
                && data is PlayerEvent.PlayerData receivedPlayerData
                && receivedPlayerData.id == PlayerData.id)
                DisableShooter();
        }
    }
}