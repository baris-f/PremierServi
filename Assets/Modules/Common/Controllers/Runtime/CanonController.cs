using Modules.Common.CustomEvents.Runtime;
using Modules.Technical.GameConfig.Runtime;
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
        [SerializeField] private LineRenderer line;
        [SerializeField] private AudioSource audioSource;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;

        [Header("Events")]
        [SerializeField] private PlayerEvent playerFire;


        [Header("Debug")]
        [SerializeField] private int curAmmo;

        private RaycastHit2D hit;
        private Transform cachedTransform;
        private JoyConColors color;
        private bool disabled;
        private readonly PlayerEvent.PlayerData playerData = new();
        private int maxBullets;


        void UpdateLine()
        {
            line.SetPosition(0, projectileStart.position);
            hit = Physics2D.Raycast(transform.position, Vector2.left);

            if (hit.collider != null)
            {
                line.SetPosition(1, hit.point);
            }
            else
                line.SetPosition(1, transform.position + (Vector3.left * 16)); // set au loin quand pas de cible
        }

        void FixedUpdate()
        {
            if (!disabled)
                UpdateLine();
        }

        public void Init(int newPlayerId, JoyConColors newColor, int humanId, int maxB)
        {
            playerData.id = newPlayerId;
            playerData.type = PlayerEvent.Type.Human;
            playerData.typeId = humanId;
            color = newColor;
            line.startColor = color.BodyColor;
            sprite.color = color.BodyColor;
            name = $"Canon {playerData.typeId} (player {playerData.id}, human {playerData.typeId})";
            maxBullets = maxB;
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
            audioSource.Play();
            playerFire.Raise(playerData);
            curAmmo--;
            var obj = Instantiate(projectile);
            obj.transform.position = projectileStart.position;
            obj.name = $"projectile from {name}";
            obj.GetComponentInChildren<SpriteRenderer>().color = color.BodyColor;
            if (curAmmo <= 0) DisableCanon();
        }

        private void DisableCanon()
        {
            disabled = true;
            sprite.color = disabledColor;
            line.gameObject.SetActive(false);
        }

        public void OnPlayerDeath(MinimalData data)
        {
            if (!disabled
                && data is PlayerEvent.PlayerData receivedPlayerData
                && receivedPlayerData.id == playerData.id)
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