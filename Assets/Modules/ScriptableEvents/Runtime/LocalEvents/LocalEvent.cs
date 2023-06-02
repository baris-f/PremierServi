using System;
using UnityEngine;

namespace Modules.ScriptableEvents.Runtime.LocalEvents
{
    public abstract class LocalEvent : ScriptableEvent<MinimalData>
    {
        public override void Raise(MinimalData data)
        {
            data.timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            foreach (var listener in listeners)
                listener.OnEventReceived(data);
        }
    }
}