using Modules.Technical.ScriptableCollections.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.Technical.ScriptableEvents.Runtime.Inspectors
{
    public abstract class CollectionEventsListener<T> : MonoBehaviour where T : ScriptableEvent<MinimalData>
    {
        [SerializeField] private UnityEvent<MinimalData> callBack;
        [SerializeField] private ScriptableCollection<T> collection;

        private void OnEnable()
        {
            if (callBack == null) return;
            foreach (var @event in collection.collection)
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
            foreach (var @event in collection.collection)
            {
                if (@event == null) continue;
                @event.UnRegisterAll(this);
            }
        }
    }
}