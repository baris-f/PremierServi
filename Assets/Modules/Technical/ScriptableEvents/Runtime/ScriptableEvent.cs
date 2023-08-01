﻿using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Modules.Technical.ScriptableEvents.Runtime
{
    public class MinimalData
    {
        [JsonProperty("timestamp")] public long timestamp;
        [JsonProperty("event_name")] public string eventName;
    }

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
    }
}