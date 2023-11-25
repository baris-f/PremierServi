using System;
using System.Collections;
using System.Threading.Tasks;
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

        private readonly GameState state = new ();

        public void StartGame()
        {
            state.Started = true;
            ia.Think(state, player);
        }

        public void PauseGame() => state.Paused = true;
        public void ResumeGame() => state.Paused = false;
        public void EndGame() => state.Started = false;
    }
}