using System.Collections.Generic;
using Modules.Common.Controllers.Runtime;
using Modules.Common.Inputs.Runtime;
using Modules.Technical.GameConfig.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptUtils.Runtime;
using UnityEngine;

namespace Modules.Common.GameRunner.Runtime
{
    public class GameRunner : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private GameConfig config;
        [SerializeField] private List<RobotInput> robots;

        [Header("Prefabs")]
        [SerializeField] private CanonController canonPrefab;
        [SerializeField] private HumanInput humanPrefab;

        [Header("Containers")]
        [SerializeField] private Transform canonsContainer;
        [SerializeField] private Transform humansContainer;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent GameStartEvent;

        private void Start()
        {
            humansContainer.DestroyAllChildren();
            canonsContainer.DestroyAllChildren();

            for (var i = 0; i < config.Humans.Count; i++)
            {
                var human = config.Humans[i];
                if (string.IsNullOrWhiteSpace(human.deviceName)) continue;
                var robotId = Random.Range(0, robots.Count);
                var robotToReplace = robots[robotId];
                var canon = Instantiate(canonPrefab, canonsContainer);

                robotToReplace.gameObject.SetActive(false);
                robotToReplace.name = $"replaced by Human {i}";
                canon.name = $"Canon {i}";

                var humanInput =
                    HumanInput.Instantiate(humanPrefab.gameObject, humansContainer, human, robotToReplace.player,
                        canon);
                humanInput.name = $"Human {i}";

                Destroy(robotToReplace);
                robots.RemoveAt(robotId);
            }

            Invoke(nameof(StartGame), 1);
        }

        private void StartGame() => GameStartEvent.Raise();
    }
}