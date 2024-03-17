using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.CustomEvents.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.RoundRunner.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Scenes._2._1___Frogger.Runtime
{
    public class RoundRunnerFrogger : RoundRunner
    {
        [Header("Frogger Config")]
        [SerializeField] private float startFroggerDelay = 1;
        [SerializeField] private PlayerEvent playerDeath;

        private void Start()
        {
            var robotCount = 0;
            var modeDescriptor = config.CurrentModeDescriptor;
            var randomPrefabArray = CreatePlayerPrefabArray(modeDescriptor.NbPlayers);

            Reset();
            InitHumans(modeDescriptor.NbPlayers);

            for (var playerId = 0; playerId < modeDescriptor.NbPlayers; playerId++)
            {
                var player = InstantiatePlayer(randomPrefabArray, playerId);
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

        private PlayerController InstantiatePlayer(List<PlayerController> randomPrefabArray, int playerId)
        {
            var player = Instantiate(randomPrefabArray.PickRandom(), playersLayout.transform);
            player.Collider.gameObject.GetComponent<CollisionDetector>()
                .Init(player.PlayerData, new CollisionDetector.CollisionResponse
                    { @event = playerDeath, destroy = false, tag = "Car" });
            return player;
        }
    }
}