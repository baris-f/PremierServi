using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class CanonController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed;
        [SerializeField] private Color disabledColor;
        [SerializeField] private float maxYPosition = 10;
        [SerializeField] private float minYPosition = -10;

        [Header("References")]
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Projectile projectile;
        [SerializeField] private Transform projectileStart;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;

        [Header("Debug")]
        [SerializeField] private int curAmmo;

        private Transform cachedTransform;
        private bool disabled;
        private int playerId;
        private int maxBullets;

        public void Init(int newPlayerId, int humanId, int maxB)
        {
            playerId = newPlayerId;
            name = $"Canon {humanId} (player {playerId}, human {humanId})";
            this.maxBullets = maxB;
        }

        private void Start()
        {
            cachedTransform = transform;
            curAmmo = maxBullets;
        }

        public void Move(Vector2 amount)
        {
            if (gameSpeed.Value <= 0 || disabled) return;
            var newPos = cachedTransform.position;
            newPos.y += Time.deltaTime * gameSpeed.Value * speed * amount.y;
            newPos.y = Mathf.Clamp(newPos.y, minYPosition, maxYPosition);
            cachedTransform.position = newPos;
        }

        public void Fire()
        {
            if (gameSpeed.Value <= 0 || disabled) return;
            curAmmo--;
            var obj = Instantiate(projectile);
            obj.transform.position = projectileStart.position;
            obj.name = $"projectile from {name}";
            if (curAmmo <= 0) DisableCanon();
        }

        private void DisableCanon()
        {
            disabled = true;
            sprite.color = disabledColor;
        }

        public void OnPlayerDeath(MinimalData data)
        {
            if (!disabled
                && data is PlayerEvent.PlayerData playerData
                && playerData.id == playerId)
                DisableCanon();
        }

        private void OnDrawGizmosSelected()
        {
            if (cachedTransform == null) cachedTransform = transform;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(cachedTransform.position.x, (maxYPosition + minYPosition) / 2f),
                new Vector3(1, maxYPosition - minYPosition));
        }
    }
}