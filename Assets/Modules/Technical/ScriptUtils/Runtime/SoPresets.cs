using System;
using System.Reflection;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;
#if UNITY_EDITOR
using Modules.Technical.ScriptUtils.Core;
#endif

namespace Modules.Technical.ScriptUtils.Runtime
{
    public class SaveInPreset : Attribute
    {
    }

    public abstract class SoPresets : RuntimeSo
    {
#if UNITY_EDITOR
        private SoPresets preset1;

        protected SoPresets()
        {
            OnEnterPlayMode += Save;
            OnExitPlayMode += LoadSaved;
        }

        [Button(header: "Scriptable Presets functions", horizontal: true)]
        private void Save()
        {
            if (this == null) return;
            Debug.Log($"Saved data for Scriptable {name}");
            preset1 = this.Clone();
        }

        [Button(horizontal: true)]
        private void LoadSaved()
        {
            if (preset1 == null)
            {
                Debug.Log($"No saved data in {name} to Revert to");
                return;
            }

            var properties = ReflectionUtility.GetAllFields(this,
                BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
                info => info.GetCustomAttributes(typeof(SaveInPreset), true).Length > 0);

            foreach (var property in properties)
            {
                var value = property.GetValue(preset1);
                property.SetValue(this, value);
            }

            Debug.Log($"Reverted {name} to saved data");
        }
#endif
    }
}