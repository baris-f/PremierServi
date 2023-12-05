using System.Collections.Generic;
using Modules.Technical.GameConfig.Runtime.RoundsProvider;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Scenes.MainMenu.Runtime
{
    public class ModeUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private PlayerInput input;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;

        [Header("Settings")]
        [SerializeField] private Color32 selectedColor;
        [SerializeField] private Color32 unSelectedColor;
        [SerializeField] private List<Choice> choices = new();

        [Header("Mode")]
        [SerializeField] private TextMeshProUGUI modesText;
        [SerializeField] private RectTransform modesContainer;
        [SerializeField] private List<BaseRoundsProvider> roundsProviders = new();

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

        private void Start()
        {
            // instancier les choix
            CurrentChoice = 0;
        }

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
            choice.curOptionIndex += dir;
            choice.curOptionIndex = Mathf.Clamp(choice.curOptionIndex, 0, choice.options.Length - 1);
            choice.text.text = choice.options[choice.curOptionIndex];
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
            // CurrentChoice = choices.Count - 1;
            // var setting = choices[0].GetCurValue<RoundsSetter.GameSetting>();
            // var diff = choices[1].GetCurValue<Round.GameDifficulty>();
            // var length = choices[2].GetCurValue<RoundsSetter.GameLenght>();
         
            //dis au round setter de generer et de foutre dans la gameConfig
            
            // roundsSetter.SetupRounds(length, setting, diff);
            //todo envoyer les bons trucs
            //commencer la game
        }

        #endregion

        [Button] private void SetCurOptions()
        {
            foreach (var choice in choices)
                choice.text.text = choice.CurOption;
        }
    }
}