using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public class InfosPopup : CanvasPopup<InfosPopup.ResponseType>
    {
        public enum ResponseType
        {
            Close,
            Acknowledge
        }
        
        [Header("Infos Popup Settings")]
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Button buttonToSelect;
        
        public async Task<ResponseType> Open(string newMessage, bool shouldBlockBackground = false)
        {
            if (!string.IsNullOrWhiteSpace(newMessage)) message.text = newMessage;
           if (buttonToSelect != null) buttonToSelect.Select();
            return await base.Open(shouldBlockBackground);
        }
        public void OnClose() => Response = ResponseType.Close;
        public void OnAcknowledge() => Response = ResponseType.Acknowledge;
    }
}