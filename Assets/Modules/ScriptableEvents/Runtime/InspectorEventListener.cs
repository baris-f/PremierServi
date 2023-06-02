using System.Collections.Generic;
using UnityEngine;

namespace Modules.ScriptableEvents.Runtime
{
    public class InspectorEventListener : MonoBehaviour
    {
        [SerializeField] private List<EventListener<MinimalData>> eventsToListen = new();

        private void OnEnable()
        {
            foreach (var listener in eventsToListen)
                listener.@event.Register(listener);
        }

        private void OnDisable()
        {
            foreach (var listener in eventsToListen)
                listener.@event.UnRegister(listener);
        }
    }
}