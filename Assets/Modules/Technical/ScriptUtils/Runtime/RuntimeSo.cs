#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public abstract class RuntimeSo : ScriptableObject
    {
        protected event Action OnEnterPlayMode;
        protected event Action OnExitPlayMode;
        protected event Action OnEnterEditMode;
        protected event Action OnExitEditMode;

#if UNITY_EDITOR
        protected RuntimeSo() => EditorApplication.playModeStateChanged += OnPLayModeStateChanged;
        private void OnPLayModeStateChanged(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    OnEnterPlayMode?.Invoke();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    OnExitPlayMode?.Invoke();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    OnEnterEditMode?.Invoke();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    OnExitEditMode?.Invoke();
                    break;
            }
        }
#endif
    }
}