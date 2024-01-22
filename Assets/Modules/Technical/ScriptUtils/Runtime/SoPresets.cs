using System;
using System.Linq;
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

    public class ForceReset : Attribute
    {
        public object Value { get; }
        public ForceReset(int value) => Value = value;
        public ForceReset(string value) => Value = value;
    }

    public abstract class SoPresets : RuntimeSo
    {
#if UNITY_EDITOR
        private const BindingFlags Flags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public |
                                           BindingFlags.Instance;

        [Header("Debug"), SerializeField] private SoPresets preset1;

        protected SoPresets()
        {
            OnExitEditMode += Save;
            OnEnterEditMode += LoadSaved;
        }

        ~SoPresets()
        {
            OnExitEditMode -= Save;
            OnEnterEditMode -= LoadSaved;
        }

        private void ResetFields()
        {
            var propertiesToReset = ReflectionUtility.GetAllFields(this, Flags,
                info => info.GetCustomAttributes(typeof(ForceReset), true).Length > 0);
            foreach (var property in propertiesToReset)
            {
                if (property.GetCustomAttributes(typeof(ForceReset)).FirstOrDefault() is not ForceReset attribute)
                    break;
                var value = attribute.Value;
                property.SetValue(this, value);
            }
        }

        private void RevertFields()
        {
            var propertiesToSave = ReflectionUtility.GetAllFields(this, Flags,
                info => info.GetCustomAttributes(typeof(SaveInPreset), true).Length > 0);
            foreach (var property in propertiesToSave)
            {
                var value = property.GetValue(preset1);
                property.SetValue(this, value);
            }
        }

        [Button(header: "Scriptable Presets functions", horizontal: true)]
        private void Save()
        {
            if (this == null)
            {
                Debug.LogError("This should not be null");
                return;
            }

            ResetFields();
            Debug.Log($"Saved data for Scriptable {name}");
            preset1 = this.Clone();
        }

        [Button(horizontal: true)]
        private void LoadSaved()
        {
            if (this == null)
            {
                Debug.LogError("This should not be null");
                return;
            }

            if (preset1 == null)
            {
                Debug.Log($"No saved data in {name} to Revert to");
                return;
            }

            RevertFields();
            ResetFields();
            Debug.Log($"Reverted {name} to saved data");
        }
#endif
    }
}