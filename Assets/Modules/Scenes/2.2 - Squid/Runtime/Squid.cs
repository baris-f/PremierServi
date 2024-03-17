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
        [SerializeField] private Color enabledColor = Color.red;
        [SerializeField] private Color disabledColor = Color.black;

        private List<Vector3> targets;

        private void Awake() => HideLaser();

        public void Shoot(List<Vector3> newTargets)
        {
            targets = newTargets;
            animator.SetTrigger(Fire);
        }

        public void DisplayLaser()
        {
            if (targets is not { Count: > 0 }) return;
            var lineID = 0;
            var positions = new Vector3[targets.Count * 2];
            laser.positionCount = targets.Count * 2;
            foreach (var pos in targets)
            {
                positions[lineID++] = transform.position;
                positions[lineID++] = pos;
            }

            laser.SetPositions(positions);

            laser.startColor = enabledColor;
            laser.endColor = enabledColor;
        }

        public void HideLaser()
        {
            laser.positionCount = 0;
            laser.startColor = disabledColor;
            laser.endColor = disabledColor;
        }
    }
}