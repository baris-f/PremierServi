using System;
using System.Collections.Generic;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.Technical.ScriptUtils.Editor.ListEditorResources
{
    public class ListScriptable : SearchableListWindow
    {
        [Serializable]
        private struct ObjsSearchable : ISearchable
        {
            private readonly Type target;
            private List<Object> entries;
            private int index;
            public bool Ready => entries is { Count: > 0 };

            public ObjsSearchable(Type target)
            {
                this.target = target;
                entries = new List<Object>();
                index = -1;
            }

            public void Reset()
            {
                entries.Clear();
                var query = $"t:{target}";
                var guids = AssetDatabase.FindAssets(query);
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath(path, target);
                    entries.Add(asset);
                }
            }

            public void StartDraw()
            {
                index = -1;
            }

            public bool DrawNextEntry(string match, string lowerMatch)
            {
                index++;
                if (entries == null || index > entries.Count - 1) return false;
                var entry = entries[index];
                if (!string.IsNullOrWhiteSpace(match) && !entry.name.ToLower().Contains(lowerMatch))
                    return true;
                EditorGUILayout.ObjectField(entry, target, false);
                return true;
            }
        }

        protected ListScriptable()
        {
        }

        public static void ShowWindowScriptableObjects() => ShowWindowWithType(typeof(ScriptableObject));
        public static void ShowWindowSimpleLocalEvent() => ShowWindowWithType(typeof(SimpleLocalEvent));
        public static void ShowWindowScriptableFloats() => ShowWindowWithType(typeof(ScriptableFloat));
        public static void ShowWindowScriptableBools() => ShowWindowWithType(typeof(ScriptableBool));
        public static void ShowWindowScriptableInts() => ShowWindowWithType(typeof(ScriptableInt));

        public static void ShowWindowWithType(Type classType)
        {
            var window = GetWindow<ListScriptable>();
            window.MSearchable = new ObjsSearchable(classType);
            window.ShouldCloseOnFocusLost = true;
            window.titleContent = new GUIContent($"{classType.Name} list");
            window.OnEnable();
            window.ShowUtility();
        }
    }
}