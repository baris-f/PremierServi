﻿using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class CanonController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed;
        [SerializeField] private int maxAmmo = 2;
        [SerializeField] private Color disabledColor;

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

        public void Init(int newPlayerId, int humanId)
        {
            playerId = newPlayerId;
            name = $"Canon for player {playerId}/human {humanId}";
        }

        private void Start()
        {
            cachedTransform = transform;
            curAmmo = maxAmmo;
        }

        public void Move(Vector2 amount)
        {
            if (gameSpeed.Value <= 0 || disabled) return;
            cachedTransform.position += Time.deltaTime * gameSpeed.Value * speed * amount.y * cachedTransform.up;
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
    }
}