using System;
using Modules.Technical.SceneLoader.Runtime;

namespace Modules.Technical.GameConfig.Runtime
{
    [Serializable]
    public class Round
    {
        public enum GameDifficulty
        {
            Easy,
            Normal,
            Hard
        }

        // Scene a load
        // Other useful infos
        public LoadSceneEvent sceneEvent;
        public GameDifficulty difficulty;
    }
}