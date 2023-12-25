using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor.ListEditorResources
{
    public class ListUnityInternalIcons : SearchableListWindow
    {
        private class IconsSearchable : ISearchable
        {
            private struct BuiltinIcon : IEquatable<BuiltinIcon>, IComparable<BuiltinIcon>
            {
                public GUIContent Icon;
                public GUIContent Name;

                public override bool Equals(object o) => o is BuiltinIcon builtinIcon && Equals(builtinIcon);
                public override int GetHashCode() => Name.GetHashCode();
                public bool Equals(BuiltinIcon o) => Name.text == o.Name.text;
                public int CompareTo(BuiltinIcon o) => string.Compare(Name.text, o.Name.text, StringComparison.Ordinal);
            }

            private readonly List<BuiltinIcon> icons = new List<BuiltinIcon>();
            private int index;
            public bool Ready => icons is { Count: > 0 };

            public void Reset()
            {
                icons.Clear();
                var allText2D = Resources.FindObjectsOfTypeAll<Texture2D>();
                foreach (var texture2D in allText2D)
                {
                    if (texture2D.name.Length == 0
                        || texture2D.hideFlags != HideFlags.HideAndDontSave
                        && texture2D.hideFlags != (HideFlags.HideInInspector | HideFlags.HideAndDontSave)
                        || !EditorUtility.IsPersistent(texture2D))
                        continue;

                    Debug.unityLogger.logEnabled = false;
                    var gc = EditorGUIUtility.IconContent(texture2D.name);
                    Debug.unityLogger.logEnabled = true;

                    if (gc == null || gc.image == null)
                        continue;

                    icons.Add(new BuiltinIcon
                    {
                        Icon = gc,
                        Name = new GUIContent(texture2D.name)
                    });
                }

                icons.Sort();
            }

            public void StartDraw()
            {
                index = -1;
            }

            public bool DrawNextEntry(string match, string lowerMatch)
            {
                index++;
                if (index > icons.Count - 1) return false;
                var entry = icons[index];
                if (!string.IsNullOrWhiteSpace(match) && !entry.Name.text.ToLower().Contains(lowerMatch))
                    return true;
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(entry.Name);
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)
                    && Event.current.type == EventType.MouseDown && Event.current.clickCount > 1)
                {
                    EditorGUIUtility.systemCopyBuffer = entry.Name.text;
                    Debug.Log($"{entry.Name.text} copied to clipboard.");
                }

                EditorGUILayout.LabelField(entry.Icon, GUILayout.MaxWidth(30));
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)
                    && Event.current.type == EventType.MouseDown && Event.current.clickCount > 1)
                {
                    if (entry.Icon.image is not Texture2D texture) return false;
                    var png = ImageConversion.EncodeArrayToPNG(texture.GetRawTextureData(), texture.graphicsFormat,
                        (uint)texture.width, (uint)texture.height);
                    var path = EditorUtility.SaveFilePanelInProject(
                        $"Save icon {entry.Name}",
                        $"{entry.Name}",
                        "png",
                        $"Select where to save {entry.Name}");
                    File.WriteAllBytes(path, png);
                }

                GUILayout.EndHorizontal();
                return true;
            }
        }


        protected ListUnityInternalIcons() => MSearchable = new IconsSearchable();

        [MenuItem("Window/Gamevrest/List Unity Internal icons")]
        public static void ShowWindow()
        {
            var window = GetWindow<ListUnityInternalIcons>();
            window.titleContent = new GUIContent("Internal Icons");
            window.ShowUtility();
        }
    }
}