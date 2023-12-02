using System;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Technical.ScriptableEvents.Runtime
{
    [Serializable]
    public class EventListener<T> where T : MinimalData
    {
        [HideInInspector] public Component origin;
        public ScriptableEvent<T> @event;
        public UnityEvent<T> callback;

        public void OnEventReceived(T data) => callback.Invoke(data);
    }
}