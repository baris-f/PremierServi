// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace Modules.ScriptableEvents.Runtime.LocalEvents
// {
//     public abstract class ScriptableEventWithData<T> : ScriptableObject where T : MinimalData
//     {
//         [SerializeField] protected List<EventListenerWithData<T>> listeners = new();
//
//         public abstract void Raise(T data);
//         public abstract void OnEventReceived(T data);
//
//         public void Register(EventListenerWithData<T> listener)
//         {
//             if (!listeners.Contains(listener))
//                 listeners.Add(listener);
//         }
//
//         public void UnRegister(EventListenerWithData<T> listener)
//         {
//             if (listeners.Contains(listener))
//                 listeners.Remove(listener);
//         }
//     }
//
//     [Serializable]
//     public class EventListenerWithData <T>
//     {
//         public ScriptableEvent @event;
//         public UnityEvent<T> callback;
//
//         public void OnEventReceived(T data) => callback.Invoke(data);
//     }
//     
//     public class MinimalData
//     {
//         public string timestamp;
//         public string eventName;
//     }
//
//     public class LocalEventWithData<T> : ScriptableEventWithData<T> where T : MinimalData
//     {
//         public override void Raise(T data)
//         {
//             data.timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
//             data.eventName = name;
//             OnEventReceived(data);
//         }
//
//         public override void OnEventReceived(T data)
//         {
//             Debug.Log($"Raise event {name} locally");
//             foreach (var listener in listeners)
//                 listener.OnEventReceived(data);
//         }
//     }
//
//     [CreateAssetMenu(fileName = "New Simple Local Event", menuName = "ScriptableEvents/Simple Local Event")]
//     public class TestSimpleEvent : LocalEventWithData<MinimalData>
//     {
//         public void Raise() => base.Raise(new MinimalData());
//     }
//
//     
//     public class SampleData : MinimalData
//     {
//         private string data;
//     }
//     
//     [CreateAssetMenu(fileName = "New Sample Data Local Event", menuName = "ScriptableEvents/Sample Data Local Event")]
//     public class TestDataEvent : LocalEventWithData<SampleData>
//     { }
// }