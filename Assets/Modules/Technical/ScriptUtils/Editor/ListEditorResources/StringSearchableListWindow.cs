using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Modules.Technical.ScriptUtils.Editor.ListEditorResources
{
    public class StringSearchableListWindow : SearchableListWindow
    {
        private class StringSearchable : ISearchable
        {
            private readonly Func<List<string>> getList;
            private readonly Action<string> doubleClickCallback;
            private List<string> entries;
            private int index;
            public bool Ready => entries is { Count: > 0 };

            public StringSearchable(Func<List<string>> getList, Action<string> doubleClickCallback)
            {
                this.getList = getList;
                this.doubleClickCallback = doubleClickCallback;
            }

            public void Reset()
            {
                entries = getList();
            }

            public void StartDraw()
            {
                index = -1;
            }

            public bool DrawNextEntry(string match, string lowerMatch)
            {
                index++;
                if (index > entries.Count - 1) return false;
                var entry = entries[index];
                if (!string.IsNullOrWhiteSpace(match) && !entry.ToLower().Contains(lowerMatch))
                    return true;
                EditorGUILayout.LabelField(entry);
                if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)
                    && Event.current.type == EventType.MouseDown && Event.current.clickCount > 1)
                {
                }

                return true;
            }
        }

        protected StringSearchableListWindow(Func<List<string>> getList, Action<string> doubleClickCallback) =>
            MSearchable = new StringSearchable(getList, doubleClickCallback);
    }
}