using System;
using UnityEngine.Events;

namespace Modules.ScriptableEvents.Runtime
{
    [Serializable]
    public class EventListener<T> where T : MinimalData
    {
        public ScriptableEvent<T> @event;
        public UnityEvent<T> callback;

        public void OnEventReceived(T data) => callback.Invoke(data);
    }
}