using System;
using System.Collections.Generic;
using Modules.Technical.ScriptUtils.Runtime;
using Newtonsoft.Json;
using UnityEngine;

namespace Modules.Technical.ScriptableEvents.Runtime
{
    public class MinimalData
    {
        [JsonProperty("timestamp"), HideInInspector]
        public long Timestamp;
        [JsonProperty("event_name"), HideInInspector]
        public string EventName;
    }

    [Serializable]
    public abstract class ScriptableEvent<T> : ScriptableObject where T : MinimalData
    {
        [SerializeField] protected List<EventListener<T>> listeners = new();

        protected abstract void Raise(T data);

        public void Register(EventListener<T> listener, Component origin)
        {
            listener.origin = origin;
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnRegister(EventListener<T> listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        public void UnRegisterAll(Component origin) => listeners.RemoveAll(listener => listener.origin == origin);

        [Button] private void ClearEvents() => listeners.Clear();
    }
}