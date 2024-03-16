using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Scenes._2._1___Classic.Runtime
{
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ScriptableFloat gameSpeed;

        private Transform cachedTransform;

        public void Init(Vector3 startPos, string sender, Color bodyColor)
        {
            transform.position = startPos;
            name = $"projectile from {sender}";
            spriteRenderer.color = bodyColor;
        }

        private void Start() => cachedTransform = transform;

        private void Update() =>
            cachedTransform.position +=
                Time.deltaTime * Mathf.Max(gameSpeed.Value, 0) * speed * -1 * cachedTransform.right;
    }
}