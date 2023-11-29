using System;
using Eflatun.SceneReference;
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
        [SerializeField] private SceneReference scene; 
        
        [Button] public void Raise()
        {
            data.sceneName = scene.Name;
            Raise(data);
        }
    }
}