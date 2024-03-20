using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.CustomEvents.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.RoundRunner.Runtime;
using Modules.Technical.ScriptableEvents.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Scenes._2._2___Squid.Runtime
{
    public class RoundRunnerSquid : RoundRunner
    {
        [Header("Squid Config")]
        [SerializeField] private float startSquidDelay = 1;
        [SerializeField] private PlayerEvent playerDeath;
        [SerializeField] private Squid squid;

        [Header("Squid controllers")]
        [SerializeField] protected SquidController squidControllerPrefab;
        [SerializeField] protected Transform squidControllersContainer;
        [SerializeField] private float delayBeforeKill = 1;
        
        private readonly List<PlayerEvent.PlayerData> playersToKill = new();
        private readonly List<PlayerController> players = new();

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
                players.Add(player);
                var human = config.Humans.Find(h => h.playerId == playerId);
                if (human == null)
                {
                    SetupRobot(player, playerId, robotCount, modeDescriptor);
                    robotCount++;
                }
                else
                {
                    var input = SetupHuman(player, playerId, modeDescriptor, human);
                    var squidController = Instantiate(squidControllerPrefab, squidControllersContainer);
                    squidController.Init(playerId, human.humanId, modeDescriptor.NbBullets);
                    input.Init(player, squidController);
                }
            }

            if (config.Humans.Count <= 0)
            {
                var humanInput = HumanInput.InstantiateDummy(humanPrefab, humansContainer);
                humanInput.name = "Dummy human to get inputs";
            }

            RefreshLayouts();
            Invoke(nameof(StartGame), startSquidDelay);
        }

        protected new void Reset()
        {
            base.Reset();
            squidControllersContainer.DestroyAllChildren();
        }
        
        public void OnPlayerShoot(MinimalData data)
        {
            if (data is not PlayerEvent.PlayerData playerData
                || playerData.type == PlayerEvent.Type.Robot) return;
            var targets = new List<Vector3>();
            playersToKill.Clear();
            squid.Shoot(targets);
            foreach (var player in players)
            {
                if (!player.IsMoving) continue;
                targets.Add(player.transform.position);
                playersToKill.Add(player.PlayerData);
            }
            Invoke(nameof(KillPlayersAfterDelay), delayBeforeKill);
        }

        private void KillPlayersAfterDelay()
        {
            foreach (var player in playersToKill)
                playerDeath.Raise(player);
        }
    }
}