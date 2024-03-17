using Modules.Common.Inputs.Runtime;
using Modules.Common.RoundRunner.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Scenes._2._1___Frogger.Runtime
{
    public class RoundRunnerFrogger : RoundRunner
    {
        [FormerlySerializedAs("startSquidDelay")]
        [Header("Frogger Config")]
        [SerializeField] private float startFroggerDelay = 1;
        
        private void Start()
        {
            var robotCount = 0;
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
                    var input = SetupHuman(player, playerId, modeDescriptor, human);
                    input.Init(player, null);
                }
            }

            if (config.Humans.Count <= 0)
            {
                var humanInput = HumanInput.InstantiateDummy(humanPrefab, humansContainer);
                humanInput.name = "Dummy human to get inputs";
            }

            RefreshLayouts();
            Invoke(nameof(StartGame), startFroggerDelay);
        }
    }
}