using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Common.RoundRunner.Runtime
{
    public class RoundRunnerClassic : RoundRunner
    {
        [Header("Classic Config")]
        [SerializeField] private float startClassicDelay = 1;
        [SerializeField] private Transform arenasContainer;

        [Header("Canons")]
        [SerializeField]
        protected CanonController canonPrefab;
        [SerializeField] protected TransformLayout canonsLayout;

        protected void Start()
        {
            int robotCount = 0, humanCount = 0;
            var modeDescriptor = config.CurrentModeDescriptor;
            var randomPrefabArray = CreatePlayerPrefabArray(modeDescriptor.NbPlayers);

            Reset();
            ChooseRandomBackground();
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
                    var canon = Instantiate(canonPrefab, canonsLayout.transform);
                    canon.Init(playerId, human.color, humanCount, modeDescriptor.NbBullets);
                    input.Init(player, canon);
                    humanCount++;
                }
            }

            if (humanCount <= 0)
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