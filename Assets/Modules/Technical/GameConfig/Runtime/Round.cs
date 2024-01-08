using System;
using Modules.Common.Cake.Runtime;
using Modules.Technical.GameConfig.Runtime.Modes;

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
        public Cake cake;
        
        public Round(ModeDescriptor newMode, GameDifficulty newDiff)
        {
            mode = newMode;
            difficulty = newDiff;
        }
    }
}