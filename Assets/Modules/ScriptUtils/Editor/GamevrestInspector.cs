using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Modules.ScriptUtils.Runtime;
using UnityEditor;
using UnityEngine;

namespace Modules.ScriptUtils.Editor
{
    [CustomEditor(typeof(Object), true)]
    public class GamevrestInspector : UnityEditor.Editor
    {
        private IEnumerable<MethodInfo> buttonMethods;

        private void OnEnable()
        {
            buttonMethods = ReflectionUtility.GetAllMethods(target,
                info => info.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawMethodsButtons();
        }

        private void DrawMethodsButtons()
        {
            foreach (var buttonMethod in buttonMethods)
            {
                var buttonAttribute =
                    (ButtonAttribute)buttonMethod.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                var buttonText = string.IsNullOrEmpty(buttonAttribute.Text)
                    ? ObjectNames.NicifyVariableName(buttonMethod.Name)
                    : buttonAttribute.Text;
                if (GUILayout.Button(buttonText))
                {
                    if (!Application.isPlaying)
                        Debug.LogError("Currently not supported");
                    else
                    {
                        var defaultParams = buttonMethod.GetParameters().Select(p => p.DefaultValue).ToArray();
                        buttonMethod.Invoke(target, defaultParams);
                    }
                }
            }
        }
    }
}