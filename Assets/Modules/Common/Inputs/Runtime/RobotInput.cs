using System;
using Modules.Common.Controllers.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
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

        private readonly GameState state = new();

        public void StartGame()
        {
            state.Started = true;
            try
            {
                ia.StartThinking(state, player, name);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void PauseGame() => state.Paused = true;
        public void ResumeGame() => state.Paused = false;
        public void EndGame() => state.Started = false;

        private void OnDisable()
        {
            state.Started = false;
        }
    }
}