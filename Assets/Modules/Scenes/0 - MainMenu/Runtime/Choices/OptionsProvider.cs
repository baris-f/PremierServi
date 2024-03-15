using System.Collections.Generic;
using UnityEngine;

namespace Modules.Scenes.MainMenu.Runtime.Choices
{
    public abstract class OptionsProvider : ScriptableObject
    {
        public abstract List<string> Options { get; }
    }
}