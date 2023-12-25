using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor
{
    [CustomEditor(typeof(MonoScript), editorForChildClasses: true)]
    public class ButtonForCustomTypes : UnityEditor.Editor
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

        private static void CheckOpenWindow(Type monoClass)
        {
            var methods = monoClass.GetMethods();
            foreach (var method in methods)
            {
                if (!method.IsStatic || method.IsPrivate || !method.Name.Contains("ShowWindow") ||
                    method.GetParameters().Length > 0) continue;
                if (GUILayout.Button(method.Name))
                    method.Invoke(null, null);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var monoScript = target as MonoScript;
            if (monoScript == null) return;
            var monoClass = monoScript.GetClass();
            if (monoClass == null || monoScript.GetClass().IsAbstract) return;
            if (monoScript.GetClass().IsSubclassOf(typeof(EditorWindow)))
                CheckOpenWindow(monoClass);
            else if (monoScript.GetClass().IsSubclassOf(typeof(ScriptableObject)))
            {
                if (GUILayout.Button("Create new instance"))
                    CreateNew(monoClass);
            }
        }
    }
}