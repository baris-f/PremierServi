using System.Collections.Generic;
using Modules.Scenes.MainMenu.Runtime.Choices;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.GameConfig.Runtime.RoundsProvider;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime;
using Modules.Technical.ScriptUtils.Runtime.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Scenes._0___MainMenu.Runtime
{
    public class ModeUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private InGameConfig inGameConfig;
        [SerializeField] private BaseRoundsProvider demoProvider;
        [SerializeField] private BaseRoundsProvider automaticProvider;
        [SerializeField] private BaseRoundsProvider tutoProvider;
        [SerializeField] private List<Choice> choices = new();

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;

        private int currentChoiceId;

        private int CurrentChoiceId
        {
            get => currentChoiceId;
            set
            {
                currentChoiceId = Mathf.Clamp(value, 0, choices.Count - 1);
                for (var i = 0; i < choices.Count; i++)
                    choices[i].Selected = i == currentChoiceId;
            }
        }
        private Choice CurrentChoice => choices[currentChoiceId];

        private void Start() => CurrentChoiceId = 0;

        #region Actions setup

        private InputAction submit;
        private InputAction cancel;
        private InputAction navigate;

        private void Awake()
        {
            if (inGameConfig.DemoMode)
                StartGameDemoMode();
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
            if (submit != null) submit.performed -= OnSubmit;
            if (cancel != null) cancel.performed -= OnCancel;
            if (navigate != null) navigate.performed -= OnNavigate;
        }

        #endregion

        #region Input Callbacks

        private void OnSubmit(InputAction.CallbackContext context)
        {
            if (currentChoiceId >= choices.Count - 1)
                StartGame();
            CurrentChoiceId++;
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            if (currentChoiceId <= 0)
                prevState.Raise();
            CurrentChoiceId--;
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            var vector = context.ReadValue<Vector2>().RoundToInt();
            if (vector.x != 0)
            {
                CurrentChoice.CurOption += vector.x;
                return;
            }

            if (vector.y != 0)
                CurrentChoiceId -= vector.y;
        }

        #endregion

        #region Public

        public void OnclickOption(Choice clickedChoice)
        {
            var clickedChoiceId = choices.FindIndex(c => c == clickedChoice);
            CurrentChoiceId = clickedChoiceId;
        }

        public void StartGame()
        {
            var length = choices[0].GetResult<BaseRoundsProvider.GameLength>();
            var diff = choices[1].GetResult<Round.GameDifficulty>();
            var provider = choices[2].GetResult<BaseRoundsProvider>();
            inGameConfig.SetRounds(provider, length, diff);
            
            inGameConfig.LoadRound();
        }

        private void StartGameDemoMode()
        {
            gameObject.SetActive(false);
            inGameConfig.SetRounds(demoProvider, BaseRoundsProvider.GameLength.Short, Round.GameDifficulty.Normal);
            inGameConfig.LoadRound();
        }

        public void StartAutoMode()
        {
            inGameConfig.SetHumans(null);
            inGameConfig.SetRounds(automaticProvider, BaseRoundsProvider.GameLength.Infinite, Round.GameDifficulty.Normal);
            inGameConfig.LoadRound();

        }

        public void StartTuto()
        {
            gameObject.SetActive(false);
            inGameConfig.SetRounds(tutoProvider, BaseRoundsProvider.GameLength.Single, Round.GameDifficulty.Normal);
            inGameConfig.LoadRound();
        }
        
        #endregion

        [Button] private void SetCurOptions()
        {
            foreach (var choice in choices)
                choice.Start();
        }
    }
}