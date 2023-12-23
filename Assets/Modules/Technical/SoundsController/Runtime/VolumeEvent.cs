using System;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Technical.SoundsController.Runtime
{
    public class VolumeEvent : LocalEvent<MinimalData>
    {
        [Serializable]
        public class VolumeData : MinimalData
        {
            public bool mute;
            public SoundsController.VolumeType output;
            public float value;
        }

        [SerializeField] private VolumeData data = new();

        [Button] public void Raise(float value, bool mute = false)
        {
            data.mute = mute;
            data.value = value;
            Raise(data);
        }
    }
}