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
                var humanInput = Instantiate(humanPrefab, humansContainer);
                var canon = Instantiate(canonPrefab, canonsContainer);

                robotToReplace.gameObject.SetActive(false);
                robotToReplace.name = $"replaced by Human {i}";
                humanInput.Init(human, robotToReplace.player, canon);
                humanInput.name = $"Human {i}";
                canon.name = $"Canon {i}";

                robots.RemoveAt(robotId);
            }
        }

        // mets en place la game : instantiation des players et canons et assignation des diferents inputs

        // events :
        //  game start
        //  (game pause)
        //  (game resume)
        //  game end (winner)

        public void OnGameStart()
        {
        }
    }
}