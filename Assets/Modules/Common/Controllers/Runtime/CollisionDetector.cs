using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Modules.Common.Controllers.Runtime
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class CollisionDetector : MonoBehaviour
    {
        [Header("Collision")]
        [SerializeField] private UnityEvent<Collision2D> onCollisionEnter2D;
        [SerializeField] private UnityEvent<Collision2D> onCollisionStay2D;
        [SerializeField] private UnityEvent<Collision2D> onCollisionExit2D;

        [Header("Trigger")]
        [SerializeField] private UnityEvent<Collider2D> onTriggerEnter2D;
        [SerializeField] private UnityEvent<Collider2D> onTriggerStay2D;
        [SerializeField] private UnityEvent<Collider2D> onTriggerExit2D;

        // Collision
        private void OnCollisionEnter2D(Collision2D other) => onCollisionEnter2D.Invoke(other);
        private void OnCollisionStay2D(Collision2D other) => onCollisionStay2D.Invoke(other);
        private void OnCollisionExit2D(Collision2D other) => onCollisionExit2D.Invoke(other);

        // Trigger
        private void OnTriggerEnter2D(Collider2D other) => onTriggerEnter2D.Invoke(other);
        private void OnTriggerStay2D(Collider2D other) => onTriggerStay2D.Invoke(other);
        private void OnTriggerExit2D(Collider2D other) => onTriggerExit2D.Invoke(other);
    }
}