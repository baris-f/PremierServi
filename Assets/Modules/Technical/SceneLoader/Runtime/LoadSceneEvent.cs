using System;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

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

        private readonly LoadSceneData data = new();
        public string sceneName;
        public string scenePath;
        public string sceneGuid;
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
        
        [Button] public void Raise()
        {
            data.sceneName = sceneName;
            Raise(data);
        }
    }
}