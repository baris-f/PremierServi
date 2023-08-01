using System;

namespace Modules.Technical.ScriptableEvents.Runtime.LocalEvents
{
    public abstract class LocalEvent<T> : ScriptableEvent<T> where T : MinimalData
    {
        protected override void Raise(T data)
        {
            data.timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            foreach (var listener in listeners)
                listener.OnEventReceived(data);
        }
    }
}