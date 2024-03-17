using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Scenes._0___MainMenu.Runtime.Choices
{
    [Serializable]
    public class Choice : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int startIndex;
        [SerializeField] private Color32 selectedColor;
        [SerializeField] private Color32 unSelectedColor;
        [SerializeField] public OptionsProvider optionsProvider;

        [Header("References")]
        [SerializeField] private Image selection;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ModeUI modeUI;


        private int curOptionIndex;

        public int CurOption
        {
            get => curOptionIndex;
            set
            {
                curOptionIndex = Mathf.Clamp(value, 0, optionsProvider.Options.Count - 1);
                text.text = optionsProvider.Options[curOptionIndex];
            }
        }
        public bool Selected
        {
            set => selection.color = value ? selectedColor : unSelectedColor;
        }

        public void Start() => CurOption = startIndex;
        public void OnClickCurOption() => modeUI.OnclickOption(this);

        public void OnClickPrevOption()
        {
            CurOption--;
            modeUI.OnclickOption(this);
        }

        public void OnClickNextOption()
        {
            CurOption++;
            modeUI.OnclickOption(this);
        }

        public T GetResult<T>()
        {
            if (optionsProvider is not IGetResult<T> result)
                return default;
            return result.GetResult(curOptionIndex);
        }
    }
}