using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Technical.ScriptUtils.Editor.ListEditorResources
{
    public interface ISearchable
    {
        public bool Ready { get; }

        public void Reset();
        public void StartDraw();
        public bool DrawNextEntry(string match, string lowerMatch);
    }

    public class SearchableListWindow : EditorWindow
    {
        protected ISearchable MSearchable;
        [FormerlySerializedAs("_scrollPos")] [SerializeField]
        private Vector2 scrollPos;
        [FormerlySerializedAs("_match")] [SerializeField]
        private string match = "";
        [FormerlySerializedAs("_shouldCloseOnFocusLost")] [SerializeField]
        private bool shouldCloseOnFocusLost;

        // protected SearchableListWindow(ISearchable m_searchable) => _searchable = m_searchable;

        public void OnEnable() => MSearchable?.Reset();

        private void OnFocus()
        {
            if (MSearchable is { Ready: false })
                MSearchable.Reset();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.LabelField("Double-click name to copy", EditorStyles.centeredGreyMiniLabel);
            GUILayout.FlexibleSpace();
            match = EditorGUILayout.TextField(match, EditorStyles.toolbarTextField);
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (MSearchable == null)
            {
                GUILayout.Label("Initialization error, reopen window", EditorStyles.boldLabel);
                return;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUIUtility.labelWidth = 100;
            var lowerMatch = match.ToLower();
            MSearchable.StartDraw();
            if (!MSearchable.DrawNextEntry(match, lowerMatch))
                EditorGUILayout.LabelField("Error while fetching list");
            while (MSearchable.DrawNextEntry(match, lowerMatch))
            {
            }

            EditorGUILayout.EndScrollView();
        }

        private void OnLostFocus()
        {
            // Hack because calling Close() in OnLostFocus causes Editor crash
            if (shouldCloseOnFocusLost)
                EditorApplication.update += HackDueToCloseOnLostFocusCrashing;
        }

        void HackDueToCloseOnLostFocusCrashing()
        {
            Close();
            EditorApplication.update -= HackDueToCloseOnLostFocusCrashing;
        }

        public ISearchable Searchable
        {
            set
            {
                MSearchable = value;
                OnEnable();
            }
        }

        public bool ShouldCloseOnFocusLost
        {
            set => shouldCloseOnFocusLost = value;
        }
    }
}