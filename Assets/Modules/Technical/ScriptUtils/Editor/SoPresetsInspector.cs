using Modules.Technical.ScriptUtils.Runtime;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor
{
    [CustomEditor(typeof(SoPresets), true)]
    public class SoPresetsInspector : UnityEditor.Editor
    {
        protected override void OnHeaderGUI()
        {
            EditorGUILayout.LabelField("OnHeaderGUI RSO", EditorStyles.boldLabel);
            base.OnHeaderGUI();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("OnInspectorGUI RSO", EditorStyles.boldLabel);
            base.OnInspectorGUI();
        }
    }
}