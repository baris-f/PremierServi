using System;
using System.Collections.Generic;
using Modules.ScriptableEvents.Runtime.LocalEvents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.MainMenu.Runtime
{
    public class SetupUI : MonoBehaviour
    {
        [Serializable]
        public class Player
        {
            public InputDevice Device;
            public string deviceName;
        }

        [Serializable]
        public class Card
        {
            public GameObject notConnected;
            public GameObject connected;
        }

        [Header("Refs")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Card[] cards = new Card[4];

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;
        [SerializeField] private SimpleLocalEvent nextState;

        [Header("Debug")]
        [SerializeField] private Player[] players = new Player[4];

        private InputAction submit;
        private InputAction start;
        private InputAction cancel;

        #region Setup

        private void Awake()
        {
            submit = input.actions["Submit"];
            start = input.actions["Menu"];
            cancel = input.actions["Cancel"];
        }

        private void OnEnable()
        {
            submit.performed += OnSubmit;
            start.performed += OnStart;
            cancel.performed += OnCancel;
        }

        private void OnDisable()
        {
            submit.performed -= OnSubmit;
            start.performed -= OnStart;
            cancel.performed -= OnCancel;
        }

        #endregion

        #region Input Callbacks

        private void OnSubmit(InputAction.CallbackContext context)
        {
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].Device != null && players[i].Device == context.control.device)
                    return;

                if (players[i].Device != null) continue;
                players[i].Device = context.control.device;
                players[i].deviceName = context.control.device.name;
                AddPlayer(i);
                return;
            }
            Debug.Log("Too much players");
        }

        private void OnStart(InputAction.CallbackContext context)
        {
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].Device != null && players[i].Device == context.control.device)
                {
                    // registered player used start
                    // sauvegarde data des player qqpart
                    nextState.Raise();
                    return;
                }
            }
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            var count = 0;
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].Device == null) continue;
                count++;
                if (players[i].Device != context.control.device) continue;
                players[i] = null;
                RemovePlayer(i);
                return;
            }
            if (count <= 0)
                prevState.Raise();
        }

        #endregion

        #region UI

        private void AddPlayer(int id)
        {
            var card = cards[id];
            card.connected.SetActive(true);
            card.notConnected.SetActive(false);
        }

        private void RemovePlayer(int id)
        {
            var card = cards[id];
            card.connected.SetActive(false);
            card.notConnected.SetActive(true);
        }

        #endregion
    }
}