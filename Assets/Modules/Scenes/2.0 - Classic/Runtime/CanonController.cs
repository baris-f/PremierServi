using Modules.Common.Controllers.Runtime;
using Modules.Technical.GameConfig.Runtime;
using UnityEngine;

namespace Modules.Scenes._2._0___Classic.Runtime
{
    public class CanonController : ShootController
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
        [SerializeField] private LineRenderer line;
        [SerializeField] private AudioSource audioSource;

        private RaycastHit2D hit;
        private Transform cachedTransform;
        private JoyConColors color;

        private void Start() => cachedTransform = transform;

        public void Init(int newPlayerId, JoyConColors newColor, int humanId, int maxB)
        {
            base.Init(newPlayerId, humanId, maxB);
            color = newColor;
            line.startColor = color.BodyColor;
            sprite.color = color.BodyColor;
            name = $"Canon {PlayerData.typeId} (player {PlayerData.id}, human {PlayerData.typeId})";
        }


        public override void Move(Vector2 amount)
        {
            if (gameSpeed.Value <= 0 || Disabled) return;
            var newPos = cachedTransform.position;
            newPos.y += Time.deltaTime * gameSpeed.Value * speed * amount.y;
            newPos.y = Mathf.Clamp(newPos.y, minYPosition, maxYPosition);
            cachedTransform.position = newPos;
        }

        public override void Fire()
        {
            if (!FireInternal()) return;
            audioSource.Play();
            Instantiate(projectile).Init(projectileStart.position, name, color.BodyColor);
        }

        private void FixedUpdate()
        {
            if (!Disabled) UpdateLine();
        }

        private void UpdateLine()
        {
            line.SetPosition(0, projectileStart.position);
            hit = Physics2D.Raycast(transform.position, Vector2.left);
            if (hit.collider != null)
                line.SetPosition(1, hit.point);
            else
                line.SetPosition(1, transform.position + Vector3.left * 16); // set au loin quand pas de cible
        }

        protected override void DisableShooter()
        {
            DisableShooterInternal();
            sprite.color = disabledColor;
            line.gameObject.SetActive(false);
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