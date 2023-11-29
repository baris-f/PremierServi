using System;
using Modules.Common.Controllers.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime
{
    public class RobotInput : MonoBehaviour
    {
        public class GameState
        {
            public bool Started;
            public bool Paused;
        }

        [Header("Settings")]
        [SerializeField] private BaseIa ia;
        [SerializeField] public PlayerController player;

        [Header("Fields")]
        [SerializeField] private ScriptableFloat gameSpeed;

        private readonly GameState state = new();

        public void StartGame()
        {
            state.Started = true;
            gameSpeed.OnValueChanged += speed => state.Paused = speed <= 0;
            try
            {
                ia.StartThinking(state, player, name);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void EndGame() => state.Started = false;
        private void OnDisable() => state.Started = false;
    }
}