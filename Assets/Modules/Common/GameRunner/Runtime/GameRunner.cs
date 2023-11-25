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
        [SerializeField] private int nbPlayers = 8;
        [SerializeField] private Transform goal;
        
        [Header("Humans")]
        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private CanonController canonPrefab;
        [SerializeField] private Transform playersContainer;
        [SerializeField] private Transform canonsContainer;

        [Header("Events")]
        [SerializeField] private SimpleLocalEvent GameStartEvent;

        private void Start()
        {
            canonsContainer.DestroyAllChildren();
            playersContainer.DestroyAllChildren();
            var players = new List<PlayerController>();

            for (var i = 0; i < nbPlayers; i++)
            {
                var player = Instantiate(playerPrefab, playersContainer);
                players.Add(player);
                player.Setup(i, goal);
                
            }

            for (var i = 0; i < config.Humans.Count; i++)
            {
                var human = config.Humans[i];
                if (string.IsNullOrWhiteSpace(human.deviceName)) continue;
                var playerPos = Random.Range(0, players.Count);
                var humanInput = new HumanInput(human);
                var canon = Instantiate(canonPrefab, canonsContainer);
                canon.name = $"Human {i}";
                canon.SetInput(humanInput);
                var player = players[playerPos];
                player.name = $"Human {i}";
                player.SetInput(humanInput);
                players.RemoveAt(playerPos);
            }
        }

        // mets en place la game : instanciation des players et canons et assignation des diferents inputs

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