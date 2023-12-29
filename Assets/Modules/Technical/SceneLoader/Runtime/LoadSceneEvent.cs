using System;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif


namespace Modules.Technical.SceneLoader.Runtime
{
    [CreateAssetMenu(fileName = "New Load Scene Event", menuName = "Scriptable Events/Load Scene")]
    public class LoadSceneEvent : LocalEvent<MinimalData>
    {
        [Serializable]
        public class LoadSceneData : MinimalData
        {
            public string sceneName;
        }

        [Header("Scene event Data")]
        [SerializeField] private string sceneName;
        [SerializeField] internal string scenePath;
        [SerializeField] internal string sceneGuid;

        private readonly LoadSceneData data = new();
        public bool Valid => !string.IsNullOrWhiteSpace(sceneGuid)
                             && !string.IsNullOrWhiteSpace(scenePath)
                             && !string.IsNullOrWhiteSpace(sceneName);

        public void Initialize(string guid, string path, string newName)
        {
            sceneGuid = guid;
            scenePath = path;
            sceneName = newName;
            name = newName;
        }

#if UNITY_EDITOR
        [Button("Open Corresponding Scene", "Scene Functions")]
        public void OpenScene()
            => EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
#endif

        [Button("Raise Event", "Events Functions")]
        public void Raise()
        {
            data.sceneName = sceneName;
            Raise(data);
        }
    }
}