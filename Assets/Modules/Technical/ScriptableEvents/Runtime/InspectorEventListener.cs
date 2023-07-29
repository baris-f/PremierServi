using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.ScriptableEvents.Runtime
{
    public class InspectorEventListener : MonoBehaviour
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

        public void AddCallback(ScriptableEvent<MinimalData> @event, UnityAction<MinimalData> callback)
        {
            var listener = eventsToListen.Find(listener => listener.@event == @event);
            if (listener == null)
            {
                listener = new EventListener<MinimalData>
                {
                    @event = @event,
                    callback = new UnityEvent<MinimalData>()
                };
                listener.callback.AddListener(callback);
                eventsToListen.Add(listener);
                @event.Register(listener, this);
                return;
            }

            listener.callback.AddListener(callback);
        }

        public void RemoveCallback(ScriptableEvent<MinimalData> @event, UnityAction<MinimalData> callback)
        {
            var listener = eventsToListen.Find(listener => listener.@event == @event);
            if (listener == null)
            {
                Debug.LogError($"No listener with event {@event}");
                return;
            }
            listener.callback.RemoveListener(callback);
        }
    }
}