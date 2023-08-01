using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.Technical.ScriptableEvents.Runtime.LocalEvents
{
    public class TriggerLocalEventsInspector : MonoBehaviour
    {
        [Flags]
        private enum ActivationRule
        {
            None,
            Awake,
            Start,
            Enable,
            Disable
        }

        [Header("Settings")]
        [SerializeField] private ActivationRule whenToTrigger;
        [SerializeField] private List<SimpleLocalEvent> eventsToTrigger = new();

        private void Start()
        {
            if (whenToTrigger.HasFlag(ActivationRule.Start)) ActivateEvents();
        }
        
        private void Awake()
        {
            if (whenToTrigger.HasFlag(ActivationRule.Awake)) ActivateEvents();
        }
        
        private void OnEnable()
        {
            if (whenToTrigger.HasFlag(ActivationRule.Enable)) ActivateEvents();
        }
        
        private void OnDisable()
        {
            if (whenToTrigger.HasFlag(ActivationRule.Disable)) ActivateEvents();
        }

        private void ActivateEvents()
        {
            foreach (var @event in eventsToTrigger)
                @event.Raise();
        }
    }
}