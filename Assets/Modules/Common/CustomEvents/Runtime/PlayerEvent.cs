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
        [Serializable]
        public class PlayerData : MinimalData
        {
            public int id;
        }

        [SerializeField] private PlayerData data = new();

        [Button] public void Raise(int id)
        {
            data.id = id;
            Raise(data);
        }
    }
}