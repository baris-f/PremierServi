using System;
using Modules.ScriptableEvents.Runtime.LocalEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Modules.MainMenu.Runtime
{
    public class SetupUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private PlayerInput input;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;

        private InputAction submit;
        private InputAction start;
        private InputAction cancel;

        private void Awake()
        {
            submit = input.actions["Submit"];
            start = input.actions["Start"];
            cancel = input.actions["Cancel"];
        }

        private void OnEnable()
        {
            submit.performed += OnSubmit;
            start.performed += OnStart;
            cancel.performed += OnCancel;
        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
            Debug.Log($"Submit pressed on {context.control.device.displayName}");
        }

        private void OnStart(InputAction.CallbackContext context)
        {
            Debug.Log($"Start pressed on {context.control.device.displayName}");
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            Debug.Log($"Cancel pressed on {context.control.device.displayName}");
            prevState.Raise();
        }

        private void OnDisable()
        {
            submit.performed -= OnSubmit;
            start.performed -= OnStart;
            cancel.performed -= OnCancel;
        }
    }
}