using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Modules.Technical.ScriptUtils.Runtime
{
    public abstract class RuntimeScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        private RuntimeScriptableObject savedData;

        protected RuntimeScriptableObject() => EditorApplication.playModeStateChanged += OnplayModeStateChanged;

        private void OnplayModeStateChanged(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    Apply();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    Revert();
                    break;
            }
        }

        [Button()]
        private void Apply() => savedData = this.Clone();

        [Button()]
        private void Revert()
        {
            RevertTo(savedData);
            savedData = null;
        }
#endif

        // Not the best, maybe use reflection or something
        // or maybe exists something in unity
        protected abstract void RevertTo(RuntimeScriptableObject obj);
    }
}