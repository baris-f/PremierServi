using UnityEditor;
using UnityEngine;

namespace Modules.ScriptUtils.Runtime
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                CheckExisting();
#endif
                _instance ??= Resources.Load<T>($"{typeof(T).Name}");
                (_instance as ScriptableObjectSingleton<T>)?.OnInitialize();
                return _instance;
            }
        }

        // Optional overridable method for initializing the instance.
        protected virtual void OnInitialize()
        {
        }

#if UNITY_EDITOR
        private static void CheckExisting()
        {
            var name = typeof(T).Name;
            if (Resources.Load<T>(name) != null) return;
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            // if (!AssetDatabase.IsValidFolder("Assets/Resources/ScriptableSingletons"))
            //     AssetDatabase.CreateFolder("Assets/Resources", "ScriptableSingletons");
            var instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, $"Assets/Resources/{name}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}