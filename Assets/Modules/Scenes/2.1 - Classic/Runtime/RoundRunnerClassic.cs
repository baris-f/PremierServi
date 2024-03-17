using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.CustomEvents.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Common.RoundRunner.Runtime;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Scenes._2._1___Classic.Runtime
{
    public class RoundRunnerClassic : RoundRunner
    {
        [Header("Classic Config")]
        [SerializeField] private float startClassicDelay = 1;
        [SerializeField] private Transform arenasContainer;
        [SerializeField] private PlayerEvent playerDeath;

        [Header("Canons")]
        [SerializeField] protected CanonController canonPrefab;
        [SerializeField] protected TransformLayout canonsLayout;

        protected void Start()
        {
            var robotCount = 0;
            var modeDescriptor = config.CurrentModeDescriptor;
            var randomPrefabArray = CreatePlayerPrefabArray(modeDescriptor.NbPlayers);

            Reset();
            ChooseRandomBackground();
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
                    var canon = Instantiate(canonPrefab, canonsLayout.transform);
                    canon.Init(playerId, human.color, human.humanId, modeDescriptor.NbBullets);
                    input.Init(player, canon);
                }
            }

            if (config.Humans.Count <= 0)
            {
                var humanInput = HumanInput.InstantiateDummy(humanPrefab, humansContainer);
                humanInput.name = "Dummy human to get inputs";
            }

            RefreshLayouts();
            Invoke(nameof(StartGame), startClassicDelay);
        }
        
        protected new void Reset()
        {
            base.Reset();
            canonsLayout.Clear();
        }

        protected PlayerController InstantiatePlayer(List<PlayerController> randomPrefabArray, int playerId)
        {
            var player = Instantiate(randomPrefabArray.PickRandom(), playersLayout.transform);
            player.Collider.gameObject.AddComponent<OnProjectileHit>().Init(playerId, playerDeath);
            return player;
        }
        
        protected new void RefreshLayouts()
        {
            base.RefreshLayouts();
            canonsLayout.RefreshLayout();
        }

        private void ChooseRandomBackground()
        {
            var rndBackId = Random.Range(0, arenasContainer.childCount);
            for (var i = 0; i < arenasContainer.childCount; i++)
                arenasContainer.GetChild(i).gameObject.SetActive(i == rndBackId);
        }
    }
}