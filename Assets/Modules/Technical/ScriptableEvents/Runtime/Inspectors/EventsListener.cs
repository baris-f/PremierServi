﻿using System.Collections.Generic;
using UnityEngine;

namespace Modules.Technical.ScriptableEvents.Runtime.Inspectors
{
    public class EventsListener : MonoBehaviour
    {
        [SerializeField] private List<EventListener<MinimalData>> eventsToListen = new();

        private void OnEnable()
        {
            foreach (var listener in eventsToListen)
                listener.@event.Register(listener, this);
        }

        private void OnDisable()
        {
            foreach (var listener in eventsToListen)
                listener.@event.UnRegister(listener);
        }
    }
}