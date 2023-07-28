using System;
using Eflatun.SceneReference;
using Modules.ScriptableEvents.Runtime;
using Modules.ScriptableEvents.Runtime.LocalEvents;
using Modules.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.SceneLoader.Runtime
{
    [CreateAssetMenu(fileName = "New Load Scene Event", menuName = "ScriptableEvents/Load Scene Event")]
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