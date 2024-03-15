using System;
using System.Collections.Generic;
using Modules.Technical.AwaitablePopup.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
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

        [Header("Config")]
        [SerializeField] private SimpleLocalEvent onSettingsClose;
        [SerializeField] private List<VolumeSliders> sliders = new();

        public void OpenFromEvent()
        {
            if (Opened)
                Response = false;
            else
                Open(true);
        }

        protected override void Show()
        {
            foreach (var tuple in sliders)
            {
                tuple.slider.value = tuple.field.Value;
                tuple.slider.onValueChanged.AddListener(f => tuple.field.Value = f);
            }

            base.Show();
        }

        protected override void Hide()
        {
            base.Hide();
            onSettingsClose.Raise();
        }

        public void OnApply() => Response = false;

        public void OnCancel() => Response = false;
    }
}