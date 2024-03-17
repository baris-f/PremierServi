using System;
using System.Collections.Generic;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptableField.Implementations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Modules.A_RANGER.In_Game
{
    public class Car : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private float refSpeed = 0.02f;
        [SerializeField] private float boundUp = 6f;
        [SerializeField] private float boundDown = -7f;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Transform container;
        [SerializeField] private ScriptableColorArray colors;

        private float speed = 0.02f;

        private void Start() => RandomizeAttributes();

        private void FixedUpdate()
        {
            transform.Translate(container.up * -speed);
            if (transform.position.y > boundUp || transform.position.y < boundDown)
                ChangeDirection();
        }

        void ChangeDirection()
        {
            RandomizeAttributes();
            container.Rotate(Vector3.forward, 180);
        }

        private void RandomizeAttributes()
        {
            speed = refSpeed + Random.Range(-0.01f, 0.01f);
            sprite.color = colors.GetRandom();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var up = Vector3.up * boundUp;
            var down = Vector3.up * boundDown;
            up.x = down.x = transform.position.x;
            Gizmos.DrawLine(up + Vector3.left, up + Vector3.right);
            Gizmos.DrawLine(down + Vector3.left, down + Vector3.right);
        }
    }
}