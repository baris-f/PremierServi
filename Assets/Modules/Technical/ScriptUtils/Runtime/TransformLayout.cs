using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public class TransformLayout : MonoBehaviour
    {
        private enum Alignment
        {
            Top,
            Bottom,
            Center
        }

        [Header("Config")]
        [SerializeField] private bool clearOnStart = true;
        [SerializeField] private Alignment startFrom = Alignment.Top;
        [SerializeField] private float maxYPosition = -10;
        [SerializeField] private float minYPosition = 10;
        [SerializeField] private float spacing = 1;
        [SerializeField] private bool expand = true;
        [SerializeField] private bool overflow;

        private void Awake()
        {
            if (clearOnStart) Clear();
        }

        [Button]
        public void RefreshLayout()
        {
            if (maxYPosition < minYPosition) (maxYPosition, minYPosition) = (minYPosition, maxYPosition);

            var cachedTransform = transform;
            var position = cachedTransform.position;
            var childCount = GetEnabledChildCount();
            var layoutSize = maxYPosition - minYPosition;
            var step = spacing;
            var wantsToOverflow = childCount * step > layoutSize;
            if ((!overflow && wantsToOverflow) || (expand && !wantsToOverflow))
                step = layoutSize / (childCount - 1);

            switch (startFrom)
            {
                case Alignment.Top:
                    ApplyPositions(maxYPosition, -step);
                    break;
                case Alignment.Bottom:
                    ApplyPositions(minYPosition, step);
                    break;
                case Alignment.Center:
                    var start = maxYPosition;
                    if ((overflow && wantsToOverflow) || (!expand && !wantsToOverflow))
                        start = (childCount - 1) * step / 2f + position.y;
                    ApplyPositions(start, -step);
                    break;
            }
        }

        private void ApplyPositions(float start, float step)
        {
            var position = transform.position;
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeSelf) continue;
                child.position = new Vector3(position.x, start, position.z);
                start += step;
            }
        }

        private int GetEnabledChildCount()
        {
            var i = 0;
            foreach (Transform child in transform)
                if (child.gameObject.activeSelf)
                    i++;

            return i;
        }

        private void Clear() => transform.DestroyAllChildren();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(transform.position.x, (maxYPosition + minYPosition) / 2f),
                new Vector3(1, maxYPosition - minYPosition));
        }
    }
}