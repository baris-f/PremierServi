using System;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Common.CustomEvents.Runtime
{
    [CreateAssetMenu(fileName = "New Player Event", menuName = "Scriptable Events/Player")]
    public class PlayerEvent : LocalEvent<MinimalData>
    {
        public enum Type
        {
            Unknown,
            Robot,
            Human
        }

        [Serializable]
        public class PlayerData : MinimalData
        {
            public int id;
            public Type type;
            public int typeId;
        }

        [SerializeField] private PlayerData data = new();

        public void Raise(PlayerData newData) => Raise(newData.id, newData.type, newData.typeId);

        public void Raise(int id, Type type, int typeId)
        {
            data.id = id;
            data.type = type;
            data.typeId = typeId;
            Raise();
        }

        public void Raise(int id) => Raise(id, Type.Unknown, -1);

        [Button] public void Raise() => base.Raise(data);
    }
}