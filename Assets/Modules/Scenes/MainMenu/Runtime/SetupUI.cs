using System;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules.Scenes.MainMenu.Runtime
{
    public class SetupUI : MonoBehaviour
    {
    [Serializable]
        public class Card
        {
            public GameObject notConnected;
            public GameObject connected;
        }

        [Header("Refs")]
        [SerializeField] private PlayerInput input;
        [SerializeField] private Card[] cards = new Card[4];
        [SerializeField] private GameConfig gameConfig;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent prevState;
        [SerializeField] private SimpleLocalEvent nextState;

        [Header("Debug")]
        [SerializeField] private GameConfig.Human[] players = new GameConfig.Human[4];

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
                {
                    ValidatePlayers();
                    return;
                }

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
            foreach (var player in players)
            {
                if (player.Device == null || player.Device != context.control.device) continue;
                ValidatePlayers();
                return;
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
                players[i].Device = null;
                players[i].deviceName = "";
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

        public void ValidatePlayers()
        {
            gameConfig.SetPlayersFromArray(players);
            nextState.Raise();
        }

        #endregion
    }
}