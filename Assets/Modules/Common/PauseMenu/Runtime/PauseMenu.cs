using System;
using Modules.Technical.AwaitablePopup.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.Common.PauseMenu.Runtime
{
    public class PauseMenu : CanvasPopup<PauseMenu.ResponseType>
    {
        public enum ResponseType
        {
            Resume,
            Restart,
            MainMenu
        }

        [Header("References")]
        [SerializeField] private SettingsMenu settingsMenu;
        
        public void OnResume() => Response = ResponseType.Resume;
        public void OnRestart() => Response = ResponseType.Restart;
        public void OnMainMEnu() => Response = ResponseType.MainMenu;

        public async void OnSettings()
        {
            var shouldReload  = await settingsMenu.Open();
            if (shouldReload) throw new NotImplementedException();
        }
    }
}