using System;
using System.Collections.Generic;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Modules.Scenes._0___MainMenu.Runtime
{
    public class MainMenuUI : MonoBehaviour
    {
        [Serializable]
        public enum StateEnum
        {
            Menu = 0,
            Setup = 1,
            Mode = 2
        }

        [Serializable]
        public class StateData
        {
            public StateEnum stateEnum;
            public GameObject container;
            public Button buttonToSelect;
        }

        private StateEnum currentState;
        private StateEnum CurrentState
        {
            get => currentState;
            set
            {
                currentState = value;
                if (currentState < StateEnum.Menu) currentState = StateEnum.Menu;
                if (currentState > StateEnum.Mode) currentState = StateEnum.Mode;
                foreach (var state in states)
                {
                    var current = state.stateEnum == currentState;
                    if (current)
                    {
                        cachedButtonToSelect = state.buttonToSelect;
                       SelectCachedButton();
                    }

                    state.container.SetActive(current);
                }
            }
        }

        [Header("References")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private SimpleLocalEvent openSettings;
    
        [Header("States")]
        [SerializeField] private List<StateData> states = new();

        [SerializeField] private Button cachedButtonToSelect;
        private InputAction pause;

        private void Awake()
        {
            pause = input.actions["Pause"];
            CurrentState = StateEnum.Menu;
        }

        private void OnEnable() => pause.performed += OnPause;
        private void OnDisable() => pause.performed -= OnPause;

        private void OnPause(InputAction.CallbackContext _) => openSettings.Raise();

        public void SelectCachedButton()
        {
            if (cachedButtonToSelect != null)
                cachedButtonToSelect.Select();
        }
        
        [Button] public void PrevState() => CurrentState--;
        [Button] public void NextState() => CurrentState++;
        public void Quit() => Application.Quit();
    }
}