using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Technical.GameConfig.Runtime.RoundsProvider;

namespace Modules.Scenes.MainMenu.Runtime.Choices
{
    public class GameLengthChoice : OptionsProvider, IGetResult<BaseRoundsProvider.GameLength>
    {
        public override List<string> Options { get; } =
            Enum.GetNames(typeof(BaseRoundsProvider.GameLength)).ToList();

        public BaseRoundsProvider.GameLength GetResult(int id)
            => (BaseRoundsProvider.GameLength)id;
    }
}