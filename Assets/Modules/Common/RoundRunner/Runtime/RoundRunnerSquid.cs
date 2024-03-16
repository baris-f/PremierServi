using System;
using Modules.Common.Inputs.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.RoundRunner.Runtime
{
    public class RoundRunnerSquid : RoundRunner
    {
        [Header("Squid Config")] 
        [SerializeField] private float startSquidDelay = 1;
        
        private void Start()
        {
            int robotCount = 0, humanCount = 0;
            var modeDescriptor = config.CurrentModeDescriptor;
            var randomPrefabArray = CreatePlayerPrefabArray(modeDescriptor.NbPlayers);

            Reset();
            InitHumans(modeDescriptor.NbPlayers);

            for (var playerId = 0; playerId < modeDescriptor.NbPlayers; playerId++)
            {
                var player = Instantiate(randomPrefabArray.PickRandom(), playersLayout.transform);
                var human = config.Humans.Find(h => h.playerId == playerId);
                if (human == null)
                {
                    SetupRobot(player, playerId, robotCount, modeDescriptor);
                    robotCount++;
                }
                else
                {
                    var input = SetupHuman(player, playerId, humanCount, modeDescriptor, human);
                    // faire des faux canons qui declenchent la squid a la place ?
                    // var canon = Instantiate(canonPrefab, canonsLayout.transform);
                    // canon.Init(playerId, human.color, humanCount, modeDescriptor.NbBullets);
                    // input.Init(player, canon);
                    humanCount++;
                }
            }

            if (humanCount <= 0)
            {
                var humanInput = HumanInput.InstantiateDummy(humanPrefab, humansContainer);
                humanInput.name = "Dummy human to get inputs";
            }

            RefreshLayouts();
            Invoke(nameof(StartGame), startSquidDelay);
        }
    }
}