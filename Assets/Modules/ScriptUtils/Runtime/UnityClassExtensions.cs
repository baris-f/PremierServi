using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Modules.ScriptUtils.Runtime
{
    public static class UnityClassExtensions
    {
        public static void DestroyAllChildren(this Transform parent)
        {
            if (Application.isPlaying)
                foreach (Transform child in parent)
                    Object.Destroy(child.gameObject);
            else
            {
                var toDelete = from Transform transform in parent select transform.gameObject;
                foreach (var child in toDelete)
                    Object.DestroyImmediate(child);
            }
        }

        public static T GetRandom<T>(this T[] array) => array[Random.Range(0, array.Length)];
    }
}