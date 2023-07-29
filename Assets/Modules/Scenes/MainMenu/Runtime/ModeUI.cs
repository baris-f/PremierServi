using System;
using System.Collections.Generic;
using Modules.SceneLoader.Runtime;
using Modules.ScriptableEvents.Runtime.LocalEvents;
using Modules.ScriptUtils.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Modules.MainMenu.Runtime
{
    public class ModeUI : MonoBehaviour
    {
        [Serializable]
        public class Choice
        {
            public Image selection;
            public TextMeshProUGUI text;
            public string[] options;
            public int curOption;
        }

        [Header("Refs")]
        [SerializeField] private PlayerInput input;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;
        [SerializeField] private LoadSceneEvent classic;
        [SerializeField] private LoadSceneEvent frogger;
        [SerializeField] private LoadSceneEvent squid;

        [Header("Settings")]
        [SerializeField] private Color32 selectedColor;
        [SerializeField] private Color32 unSelectedColor;
        [SerializeField] private List<Choice> choices = new();

        private int currentChoice;
        private int CurrentChoice
        {
            get => currentChoice;
            set
            {
                currentChoice = value;
                currentChoice = Mathf.Clamp(currentChoice, 0, choices.Count - 1);
                for (var i = 0; i < choices.Count; i++)
                    choices[i].selection.color = i == currentChoice ? selectedColor : unSelectedColor;
            }
        }

        private InputAction submit;
        private InputAction cancel;
        private InputAction navigate;

        #region Setup

        private void Awake()
        {
            submit = input.actions["Submit"];
            cancel = input.actions["Cancel"];
            navigate = input.actions["Navigate"];
        }

        private void OnEnable()
        {
            submit.performed += OnSubmit;
            cancel.performed += OnCancel;
            navigate.performed += OnNavigate;
        }

        private void OnDisable()
        {
            submit.performed -= OnSubmit;
            cancel.performed -= OnCancel;
            navigate.performed -= OnNavigate;
        }

        private void Start() => CurrentChoice = 0;

        #endregion

        #region Input Callbacks

        private void OnSubmit(InputAction.CallbackContext context)
        {
            if (CurrentChoice >= choices.Count - 1)
                StartGame();
            CurrentChoice++;
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            if (CurrentChoice <= 0)
                prevState.Raise();
            CurrentChoice--;
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            var vector = context.ReadValue<Vector2>().RoundToInt();
            if (vector.x != 0)
            {
                ChangeOption(vector.x);
                return;
            }

            CurrentChoice -= vector.y;
        }

        private void ChangeOption(int dir)
        {
            var choice = choices[CurrentChoice];
            choice.curOption += dir;
            choice.curOption = Mathf.Clamp(choice.curOption, 0, choice.options.Length - 1);
            choice.text.text = choice.options[choice.curOption];
        }

        #endregion

        #region Public

        public void NextOption(int id)
        {
            CurrentChoice = id;
            ChangeOption(1);
        }

        public void PrevOption(int id)
        {
            CurrentChoice = id;
            ChangeOption(-1);
        }

        public void StartGame()
        {
            CurrentChoice = choices.Count - 1;
            var diffChoice = choices[1];
            GameConfig.Runtime.GameConfig.Instance.SetDifficultyFromString(diffChoice.options[diffChoice.curOption]);
            var modeChoice = choices[0];
            var mode = GameConfig.Runtime.GameConfig.Instance.SetModeFromString(
                modeChoice.options[modeChoice.curOption]);
            switch (mode)
            {
                case GameConfig.Runtime.GameConfig.GameMode.Classic:
                    classic.Raise();
                    break;
                case GameConfig.Runtime.GameConfig.GameMode.Frogger:
                    frogger.Raise();
                    break;
                case GameConfig.Runtime.GameConfig.GameMode.Squid:
                    squid.Raise();
                    break;
            }
        }

        #endregion
    }
}