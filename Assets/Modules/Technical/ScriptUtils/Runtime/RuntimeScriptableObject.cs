﻿using System;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using Modules.Technical.ScriptUtils.Core;
using UnityEditor;
#endif

namespace Modules.Technical.ScriptUtils.Runtime
{
    public class SaveAtRuntime : Attribute
    {
    }

    public class RuntimeScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [Space, Header("RuntimeScriptableObject functions")]
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
                case PlayModeStateChange.EnteredEditMode:
                case PlayModeStateChange.ExitingEditMode:
                default:
                    break;
            }
        }

        [Button()]
        private void Apply()
        {
            Debug.Log($"Saved data for Scriptable {name}");
            savedData = this.Clone();
        }

        [Button()]
        private void Revert()
        {
            if (savedData == null)
            {
                Debug.Log($"No saved data in {name} to Revert to");
                return;
            }

            var properties = ReflectionUtility.GetAllFields(this,
                BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                info => info.GetCustomAttributes(typeof(SaveAtRuntime), true).Length > 0);

            foreach (var property in properties)
            {
                var value = property.GetValue(savedData);
                property.SetValue(this, value);
            }

            // savedData = null;
        }
#endif
    }
}