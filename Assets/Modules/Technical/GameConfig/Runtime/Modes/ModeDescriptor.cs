using Modules.Technical.SceneLoader.Runtime;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime.Modes
{
    [CreateAssetMenu(fileName = "NewModeDescriptor", menuName = "Scriptable Objects/New ModeDescriptor")]
    public class ModeDescriptor : ScriptableObject
    {
        [Header("Config")]
        public string displayName;
        [SerializeField] private LoadSceneEvent sceneEvent;
        [SerializeField] private int nbPlayers = 8;

        [Header("Players Config")]
        [SerializeField] private int nbBullets = 1;
        [SerializeField] private float walkSpeed = 1;
        [SerializeField] private float runSpeed = 3;

        public void ApplyNameChange() => name = displayName;

        public void LoadMode() => sceneEvent.Raise();
        public int NbPlayers => nbPlayers;
        public int NbBullets => nbBullets;
        public float WalkSpeed => walkSpeed;
        public float RunSpeed => runSpeed;
    }
}