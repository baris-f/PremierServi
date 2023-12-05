using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Modules.Scenes.MainMenu.Runtime.Choices
{
    public class StartChoice : OptionsProvider, IGetResult<string>
    {
        public override List<string> Options { get; } = new() { "Start" };
        public string GetResult(int id) => "Start";
    }
}