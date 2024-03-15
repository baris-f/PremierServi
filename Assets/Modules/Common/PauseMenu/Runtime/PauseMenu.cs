using System.Threading.Tasks;
using Modules.Technical.AwaitablePopup.Runtime;
using Modules.Technical.ScriptableField;
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
        public void OpenFromEvent()
        {
            if (Opened)
                Response = ResponseType.Resume;
            else
                Open();
        }

        private new async Task Open(bool shouldBlockBackground = true)
        {
            gameSpeed.Value = 0;
            var r = await base.Open(shouldBlockBackground);
            if (r == ResponseType.Resume)
                gameSpeed.Value = 1;
        }

        public void OnResume() => Response = ResponseType.Resume;
        public void OnMainMenu() => Response = ResponseType.MainMenu;
    }
}