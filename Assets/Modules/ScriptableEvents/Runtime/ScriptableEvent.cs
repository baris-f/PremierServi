using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Modules.ScriptableEvents.Runtime
{
    public class MinimalData
    {
        [JsonProperty("timestamp")] public long timestamp;
        [JsonProperty("event_name")] public string eventName;
    }

    public abstract class ScriptableEvent<T> : ScriptableObject where T : MinimalData
    {
        protected const bool Verbose = false;
        [SerializeField] protected List<EventListener<T>> listeners = new();

        public abstract void Raise(T data);

        public void Register(EventListener<T> listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnRegister(EventListener<T> listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}