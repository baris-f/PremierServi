using System;
using Modules.Common.Controllers.Runtime;
using Modules.Common.Inputs.Runtime.IAs;
using Modules.Technical.ScriptableField;
using Modules.Technical.ScriptableField.Implementations;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.Inputs.Runtime
{
    [Serializable]
    public class RobotInput
    {
        [SerializeField] private string name;
        [SerializeField] private BaseIa ia;
        [SerializeField] private PlayerController player;
        [SerializeField] private ScriptableFloat gameSpeed;

        public RobotInput(string name, BaseIa ia, PlayerController player, ScriptableFloat gameSpeed)
        {
            this.name = name;
            this.ia = ia.Clone($"{ia.GetType()} - {name}");
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