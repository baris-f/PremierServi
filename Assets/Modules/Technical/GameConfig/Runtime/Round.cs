using System;

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

        public ModeDescriptor mode;
        public GameDifficulty difficulty;
        
        public Round(ModeDescriptor newMode, GameDifficulty newDiff)
        {
            mode = newMode;
            difficulty = newDiff;
        }
    }
}