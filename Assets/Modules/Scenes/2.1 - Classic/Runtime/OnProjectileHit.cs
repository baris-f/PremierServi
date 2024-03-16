using Modules.Common.CustomEvents.Runtime;
using UnityEngine;

namespace Modules.Scenes._2._1___Classic.Runtime
{
    public class OnProjectileHit : MonoBehaviour
    {
        private PlayerEvent playerDeath;
        private int playerId;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.transform.CompareTag("Projectile")) return;
            Destroy(other.transform.parent.gameObject);
            playerDeath.Raise(playerId);
        }

        public void Init(int newPlayerId, PlayerEvent newEvent)
        {
            playerId = newPlayerId;
            playerDeath = newEvent;
        }
    }
}