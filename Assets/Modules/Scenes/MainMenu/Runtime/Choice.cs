using System;
using TMPro;
using UnityEngine.UI;

namespace Modules.Scenes.MainMenu.Runtime
{
    [Serializable]
    public class Choice
    {
        public Image selection;
        public TextMeshProUGUI text;
        public string[] options;
        public int curOptionIndex;

        public string CurOption => options[curOptionIndex];

        public T GetCurValue<T>() where T : struct
        {
            var success = Enum.TryParse<T>(CurOption, out var val);
            return success ? val : default;
        }
    }
}