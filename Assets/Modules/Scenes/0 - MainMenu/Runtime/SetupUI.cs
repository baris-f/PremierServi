using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptableField.Implementations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Scenes._0___MainMenu.Runtime
{
    public class SetupUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private PlayerCard[] cards;
        [SerializeField] private InGameConfig inGameConfig;
        [SerializeField] private ScriptableBool settingsOpened;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;
        [SerializeField] private SimpleLocalEvent nextState;

        [Header("Debug")]
        [SerializeField] private Human[] players = new Human[4];

        private InputAction submit;
        private InputAction cancel;

        #region Setup

        private void Awake()
        {
            submit = input.actions["Submit"];
            cancel = input.actions["Cancel"];
        }

        private void OnEnable()
        {
            submit.performed += OnSubmit;
            cancel.performed += OnCancel;
        }

        private void OnDisable()
        {
            submit.performed -= OnSubmit;
            cancel.performed -= OnCancel;
        }

        #endregion

        #region Input Callbacks

        private void OnSubmit(InputAction.CallbackContext context)
        {
            if (settingsOpened.Value) return;
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].Device != null && players[i].Device == context.control.device)
                {
                    ValidatePlayers();
                    return;
                }

                if (players[i].Device != null) continue;
                players[i].Device = context.control.device;
                players[i].deviceName = context.control.device.name;
                players[i].color = cards[i].Color;
                cards[i].Connected(true, players[i].Device.displayName);
                return;
            }

            Debug.Log("Too much players");
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            if (settingsOpened.Value) return;
            var count = 0;
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].Device == null) continue;
                count++;
                if (players[i].Device != context.control.device) continue;
                players[i].Device = null;
                players[i].deviceName = "";
                players[i].color = new JoyConColors();
                cards[i].Connected(false);
                return;
            }

            if (count <= 0)
                prevState.Raise();
        }

        #endregion

        #region UI

        public void ValidatePlayers()
        {
            inGameConfig.SetHumans(players);
            nextState.Raise();
        }
        
        #endregion
    }
}