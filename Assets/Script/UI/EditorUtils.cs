using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace GamevrestUtils
{
#if UNITY_EDITOR
    internal static class CustomEditorUtils
    {
        public static void DrawUiLine(Color color, int thickness = 2, int padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2.0f;
            // r.width -= padding / 2.0f;
            EditorGUI.DrawRect(r, color);
        }

        public static void DrawHeader(string str)
        {
            EditorGUILayout.Separator();
            GUILayout.Label(str, EditorStyles.boldLabel);
        }
    }
#endif

    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property, label, true);

        public override void OnGUI(Rect position,
            SerializedProperty property,
            GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}