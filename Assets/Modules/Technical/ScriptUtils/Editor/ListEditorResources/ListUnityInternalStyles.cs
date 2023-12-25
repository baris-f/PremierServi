using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor.ListEditorResources
{
    public class ListUnityInternalStyles : SearchableListWindow
    {
        private class StylesSearchable : ISearchable
        {
            private struct Entry
            {
                public bool Header;
                public GUIContent Content;
                public string Copy;
                public GUIStyle Style;
            }

            private List<Entry> styles = new();
            private int index;
            public bool Ready => styles is { Count: > 1 };

            private List<Entry> ScanClass(Type container)
            {
                var type = container.ToString();
                var newStyles = new List<Entry>
                {
                    new() { Content = new GUIContent(type), Header = true }
                };
                var members = container.GetMethods(BindingFlags.Static | BindingFlags.Public);
                foreach (var member in members)
                {
                    if (!member.Name.Contains("get_") || member.ReturnType != typeof(GUIStyle)) continue;
                    var split = member.Name.Split('_');
                    var obj = member.Invoke(null, null);
                    if (obj is not GUIStyle style) continue;
                    newStyles.Add(new Entry
                    {
                        Content = new GUIContent(split[1]),
                        Copy = $"{type}.{split[1]}",
                        Header = false,
                        Style = style
                    });
                }

                return newStyles;
            }

            public void Reset()
            {
                try
                {
                    styles = ScanClass(typeof(EditorStyles));
                }
                catch (TargetInvocationException)
                {
                    // ignored -> always fails after reloading Scripts
                }
            }

            public void StartDraw()
            {
                index = -1;
            }

            public bool DrawNextEntry(string match, string lowerMatch)
            {
                index++;
                if (index > styles.Count - 1) return false;
                var entry = styles[index];
                if (!string.IsNullOrWhiteSpace(match) && !entry.Content.text.ToLower().Contains(lowerMatch))
                    return true;
                if (entry.Header)
                {
                    EditorGUILayout.LabelField(entry.Content, EditorStyles.boldLabel);
                    return true;
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(entry.Content);
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)
                    && Event.current.type == EventType.MouseDown && Event.current.clickCount > 1)
                {
                    EditorGUIUtility.systemCopyBuffer = entry.Copy;
                    Debug.Log($"{entry.Copy} copied to clipboard.");
                }

                EditorGUILayout.LabelField("Example", entry.Style);
                GUILayout.EndHorizontal();
                return true;
            }
        }

        protected ListUnityInternalStyles() => MSearchable = new StylesSearchable();

        [MenuItem("Window/Gamevrest/List Unity Internal Styles")]
        public static void ShowWindow()
        {
            var window = GetWindow<ListUnityInternalStyles>();
            window.titleContent = new GUIContent("Internal Styles");
            window.ShowUtility();
        }
    }
}