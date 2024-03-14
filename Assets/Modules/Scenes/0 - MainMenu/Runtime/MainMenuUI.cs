using System;
using System.Collections.Generic;
using Modules.Common.PauseMenu.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Scenes.MainMenu.Runtime
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
                    if (current && state.buttonToSelect != null)
                        state.buttonToSelect.Select();
                    state.container.SetActive(current);
                }
            }
        }
        
        [Header("References")]
        [SerializeField] private SettingsMenu settingsMenu;
        
        [Header("States")]
        [SerializeField] private List<StateData> states = new();

        private void Awake() => CurrentState = StateEnum.Menu;

        [Button] public void PrevState() => CurrentState--;
        [Button] public void NextState() => CurrentState++;
        public void SelectState(StateEnum state) => CurrentState = state;

        [Button] public void InitList()
        {
            states = new List<StateData>();
            foreach (StateEnum value in Enum.GetValues(typeof(StateEnum)))
                states.Add(new StateData { stateEnum = value });
        }
        
        public void Quit() => Application.Quit();
    }
}