using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Technical.GameConfig.Runtime;

namespace Modules.Scenes._0___MainMenu.Runtime.Choices
{
    public class GameDifficultyChoice : OptionsProvider, IGetResult<Round.GameDifficulty>
    {
        public override List<string> Options { get; } =
            Enum.GetNames(typeof(Round.GameDifficulty)).ToList();

        public Round.GameDifficulty GetResult(int id)
            => (Round.GameDifficulty)id;
    }
}