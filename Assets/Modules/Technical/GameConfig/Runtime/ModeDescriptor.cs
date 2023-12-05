using Modules.Technical.SceneLoader.Runtime;
using UnityEngine;

namespace Modules.Technical.GameConfig.Runtime
{    
    [CreateAssetMenu(fileName = "NewModeDescriptor", menuName = "Scriptable Objects/New ModeDescriptor")]
    public class ModeDescriptor : ScriptableObject
    {
        [Header("Config")]
        [SerializeField] private LoadSceneEvent sceneEvent;
    }
}