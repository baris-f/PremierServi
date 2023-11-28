using UnityEngine;

namespace Modules.Common.Controllers.Runtime
{
    public class Projectile : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed;

        private bool paused;
        private Transform cachedTransform;

        private void Start() => cachedTransform = transform;

        private void Update()
        {
            if (paused) return;
            cachedTransform.position += Time.deltaTime * speed * -1 * cachedTransform.right;
        }

        public void PauseGame() => paused = true;
        public void ResumeGame() => paused = false;
    }
}