using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Technical.ScriptableEvents.Runtime
{
    public class MultiEventsListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent<MinimalData> callBack;
        [SerializeField] private List<ScriptableEvent<MinimalData>> events = new();

        private void OnEnable()
        {
            if (callBack == null) return;
            foreach (var @event in events)
            {
                if (@event == null) continue;
                var listener = new EventListener<MinimalData>()
                {
                    @event = @event,
                    callback = callBack
                };
                @event.Register(listener, this);
            }
        }

        private void OnDisable()
        {
            if (callBack == null) return;
            foreach (var @event in events)
            {
                if (@event == null) continue;
                @event.UnRegisterAll(this);
            }
        }
    }
}