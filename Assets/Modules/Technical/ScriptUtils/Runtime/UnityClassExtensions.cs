using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public static class UnityClassExtensions
    {
        public static Vector2Int RoundToInt(this Vector2 src) => Vector2Int.RoundToInt(src);

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
        public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];

        /// <summary>
        /// Creates and returns a clone of any given scriptable object.
        /// </summary>
        public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
        {
            if (scriptableObject == null)
            {
                Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
                return (T)ScriptableObject.CreateInstance(typeof(T));
            }

            var instance = Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name; // remove (Clone) from name
            return instance;
        }
    }
}