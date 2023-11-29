using System;
using Modules.Common.Controllers.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime
{
    [Serializable]
    public class RobotInput
    {
        public class GameState
        {
            public bool Started;
            public bool Paused;
        }

        [SerializeField] private string name;
        [SerializeField] private BaseIa ia;
        [SerializeField] private PlayerController player;

        private ScriptableFloat gameSpeed;
        private readonly GameState state = new();

        public RobotInput(string name, BaseIa ia, PlayerController player, ScriptableFloat gameSpeed)
        {
            this.name = name;
            this.ia = ia;
            this.player = player;
            this.gameSpeed = gameSpeed;
        }

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