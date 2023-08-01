using System;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor
{
    [CustomEditor(typeof(MonoScript), editorForChildClasses: true)]
    public class CreateSoInstanceFromInspector : UnityEditor.Editor
    {
        private static void CreateNew(Type monoClass)
        {
            var path = EditorUtility.SaveFilePanelInProject(
                $"Create new {monoClass}",
                $"{monoClass.Name}",
                "asset",
                $"Select where to create new {monoClass}");
            if (string.IsNullOrWhiteSpace(path)) return;
            var instance = CreateInstance(monoClass);
            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.ImportAsset(path);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var monoScript = target as MonoScript;
            if (monoScript == null) return;
            var monoClass = monoScript.GetClass();
            if (monoClass == null ||
                !monoScript.GetClass().IsSubclassOf(typeof(ScriptableObject)))
                return;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create new instance"))
                CreateNew(monoClass);
            GUILayout.EndHorizontal();
        }
    }
}