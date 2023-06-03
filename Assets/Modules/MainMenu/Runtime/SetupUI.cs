using System;
using System.Collections.Generic;
using Modules.ScriptableEvents.Runtime.LocalEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

namespace Modules.MainMenu.Runtime
{
    public class SetupUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private InputManager inputManager;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;

        private InputAction submit;
        private InputAction start;
        private InputAction cancel;

        private void Awake()
        {
        }

        private void OnEnable()
        {
        }

        private void OnSubmit(InputAction.CallbackContext context)
        {
        }

        private void OnStart(InputAction.CallbackContext context)
        {
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
        }

        private void OnDisable()
        {
          
        }
    }
}