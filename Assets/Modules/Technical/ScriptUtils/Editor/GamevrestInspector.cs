using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Modules.Technical.ScriptUtils.Core;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor
{
    [CustomEditor(typeof(Object), true)]
    public class GamevrestInspector : UnityEditor.Editor
    {
        private IEnumerable<MethodInfo> buttonMethods;

        private void OnEnable()
        {
            buttonMethods = ReflectionUtility.GetAllMethods(target,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public |
                BindingFlags.DeclaredOnly,
                info => info.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawMethodsButtons();
        }

        private void DrawMethodsButtons()
        {
            Horizontal = false;
            foreach (var buttonMethod in buttonMethods)
            {
                var attributes = buttonMethod.GetCustomAttributes(true);
                if (attributes.FirstOrDefault(attr => attr is ButtonAttribute) is not ButtonAttribute buttonAttribute)
                    return;

                var buttonText = string.IsNullOrEmpty(buttonAttribute.Text)
                    ? ObjectNames.NicifyVariableName(buttonMethod.Name)
                    : buttonAttribute.Text;
                if (!string.IsNullOrWhiteSpace(buttonAttribute.Header))
                {
                    Horizontal = false;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField(buttonAttribute.Header, EditorStyles.boldLabel);
                }

                Horizontal = buttonAttribute.Horizontal;
                if (!GUILayout.Button(buttonText)) continue;
                var defaultParams = buttonMethod.GetParameters().Select(p => p.DefaultValue).ToArray();
                buttonMethod.Invoke(target, defaultParams);
            }

            Horizontal = false;
        }

        private void DrawDebugFields()
        {
        }

        private bool horizontal;
        private bool Horizontal
        {
            set
            {
                switch (value)
                {
                    case true when !horizontal:
                        GUILayout.BeginHorizontal();
                        break;
                    case false when horizontal:
                        GUILayout.EndHorizontal();
                        break;
                }

                horizontal = value;
            }
        }
    }
}