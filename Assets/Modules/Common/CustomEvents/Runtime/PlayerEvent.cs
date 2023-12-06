using System;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.CustomEvents.Runtime
{
    [CreateAssetMenu(fileName = "New Player Event", menuName = "Scriptable Events/Player")]
    public class PlayerEvent : LocalEvent<MinimalData>
    {
        public enum Type
        {
            Robot, Human
        }
        
        [Serializable]
        public class PlayerData : MinimalData
        {
            public int id;
            public Type type;
        }

        [SerializeField] private PlayerData data = new();

        [Button] public void Raise(int id, Type type)
        {
            data.id = id;
            data.type = type;
            Raise(data);
        }
    }
}