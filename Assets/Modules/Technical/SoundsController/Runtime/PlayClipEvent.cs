using System;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;

namespace Modules.Technical.SoundsController.Runtime
{
    public class PlayClipEvent : LocalEvent<MinimalData>
    {
        [Serializable]
        public class ClipData : MinimalData
        {
            public SoundsController.Output output;
            public bool oneShot;
            public AudioClip clip;
        }

        [SerializeField] private ClipData data = new();

        [Button] public void Raise(AudioClip clip, bool oneShot = true)
        {
            data.oneShot = oneShot;
            data.clip = clip;
            Raise(data);
        }
    }
}