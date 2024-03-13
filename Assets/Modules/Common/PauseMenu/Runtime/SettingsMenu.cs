using System;
using System.Collections.Generic;
using Modules.Technical.AwaitablePopup.Runtime;
using Modules.Technical.ScriptableField;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Common.PauseMenu.Runtime
{
    public class SettingsMenu : CanvasPopup<bool>
    {
        [Serializable]
        private class VolumeSliders
        {
            public ScriptableFloat field;
            public Slider slider;
        }

        [Header("Volume sliders")]
        [SerializeField] private List<VolumeSliders> sliders = new();

        protected override void Show()
        {
            foreach (var tuple in sliders)
            {
                tuple.slider.value = tuple.field.Value;
                tuple.slider.onValueChanged.AddListener(f => tuple.field.Value = f);
            }

            base.Show();
        }

        public void OnApply()
        {
            Response = false;
        }

        public void OnCancel()
        {
            Response = false;
        }
    }
}