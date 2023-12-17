using Modules.Technical.SceneLoader.Runtime;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime
{
    [CreateAssetMenu(fileName = "NewModeDescriptor", menuName = "Scriptable Objects/New ModeDescriptor")]
    public class ModeDescriptor : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] private LoadSceneEvent sceneEvent;
        [SerializeField] private int nbPlayers = 8;
        [SerializeField] private int nbBullets = 1;


        public void LoadMode() => sceneEvent.Raise();
        public int NbPlayers => nbPlayers;
        public int NbBullets => nbBullets;
    }
}