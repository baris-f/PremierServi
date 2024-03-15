using System.Threading.Tasks;
using Modules.Technical.AwaitablePopup.Runtime;
using Modules.Technical.ScriptableEvents.Runtime.LocalEvents;
using Modules.Technical.ScriptableField;
using UnityEditor;
using UnityEngine;

namespace Modules.Common.PauseMenu.Runtime
{
    public class PauseMenu : CanvasPopup<PauseMenu.ResponseType>
    {
        public enum ResponseType
        {
            Resume,
            MainMenu
        }

        [Header("Refs")]
        [SerializeField] private ScriptableFloat gameSpeed;
        [SerializeField] private SettingsMenu settings;

        public async void OpenFromEvent()
        {
            if (Opened)
            {
                if (settings.Opened)
                    settings.OnCancel();
                else
                    Response = ResponseType.Resume;
            }
            else
                await Open();
        }

        private new async Task Open(bool shouldBlockBackground = true)
        {
            gameSpeed.Value = 0;
            var r = await base.Open(shouldBlockBackground);
            if (r == ResponseType.Resume)
                gameSpeed.Value = 1;
        }

        public async void OpenSettings()
        {
            await settings.Open();
            SelectButton();
        }

        public void OnResume() => Response = ResponseType.Resume;
        public void OnMainMenu() => Response = ResponseType.MainMenu;
    }
}