﻿using System.Collections.Generic;

namespace Modules.Scenes.MainMenu.Runtime.Choices
{
    public class StartChoice : OptionsProvider, IGetResult<string>
    {
        public override List<string> Options { get; } = new() { "Start" };
        public string GetResult(int id) => "Start";
    }
}