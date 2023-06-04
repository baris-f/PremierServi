using System;
using System.Collections.Generic;
using Modules.ScriptableEvents.Runtime.LocalEvents;
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

        public Vector2 navDebug;
        [Header("Refs")]
        [SerializeField] private PlayerInput input;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;

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
                Debug.Log("Passer au jeu yay");
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
            var vector = context.ReadValue<Vector2>();
            navDebug = vector;
            var choice = choices[currentChoice];
            choice.curOption += (int)vector.x;
            choice.curOption = Mathf.Clamp(choice.curOption, 0, choice.options.Length - 1);
            choice.text.text = choice.options[choice.curOption];
        }

        #endregion
    }
}