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
        private string name;
        private BaseIa ia;
        private PlayerController player;
        private ScriptableFloat gameSpeed;

        public RobotInput(string name, BaseIa ia, PlayerController player, ScriptableFloat gameSpeed)
        {
            this.name = name;
            this.ia = ia;
            this.player = player;
            this.gameSpeed = gameSpeed;
        }

        public void StartGame()
        {
            try
            {
                ia.StartThinking(gameSpeed, player);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}