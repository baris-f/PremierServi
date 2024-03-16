using System.Collections.Generic;
using UnityEngine;

namespace Modules.Scenes._2._2___Squid.Runtime
{
    public class Squid : MonoBehaviour
    {
        private static readonly int Fire = Animator.StringToHash("Fire");

        [Header("References")]
        [SerializeField] private LineRenderer laser;
        [SerializeField] private Animator animator;

        private List<Vector3> targets;

        public void Shoot(List<Vector3> newTargets)
        {
            targets = newTargets;
            animator.SetTrigger(Fire);
        }

        public void DisplayLaser()
        {
            var lineID = 0;
            laser.positionCount = targets.Count * 2;
            foreach (var pos in targets)
            {
                laser.SetPosition(lineID++, transform.position);
                laser.SetPosition(lineID++, pos);
            }
        }
    }
}