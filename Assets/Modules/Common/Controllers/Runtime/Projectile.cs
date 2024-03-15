using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;

        private Transform cachedTransform;

        private void Start() => cachedTransform = transform;

        private void Update() =>
            cachedTransform.position +=
                Time.deltaTime * Mathf.Max(gameSpeed.Value, 0) * speed * -1 * cachedTransform.right;
    }
}