using Modules.Common.CustomEvents.Runtime;
using UnityEngine;

namespace Modules.Scenes._2._1___Classic.Runtime
{
    public class OnProjectileHit : MonoBehaviour
    {
        [SerializeField] private int playerId;
        [SerializeField] private PlayerEvent playerDeath;

        public void SetPlayerId(int id) => playerId = id;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.transform.CompareTag("Projectile")) return;
            Destroy(other.gameObject);
            playerDeath.Raise(playerId);
        }
    }
}