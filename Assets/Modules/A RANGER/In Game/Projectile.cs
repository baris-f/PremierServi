using UnityEngine;

namespace Modules.A_RANGER.In_Game
{
    public class Projectile : MonoBehaviour
    {
        public GameObject winText;

        private void Start()
        {
            winText = GameObject.Find("win");
        }

        void FixedUpdate()
        {
            transform.Translate(Vector2.left);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col || !col.CompareTag("Player"))
                return;
            col.GetComponent<Character>().Die();
            Destroy(gameObject);
        }
    }
}
