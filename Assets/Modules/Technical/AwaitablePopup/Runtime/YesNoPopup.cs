using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public class YesNoPopup : AwaitablePopup<YesNoPopup.ResponseType>
    {
        public enum ResponseType
        {
            Close,
            Yes,
            No
        }
        
        [Header("Ui Refs")]
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI message;

        private void Awake() => HideDialog();
        
        public async Task<ResponseType> OpenDialog(string newMessage)
        {
            message.text = newMessage;
            return await OpenDialog();
        }

        protected override void ShowDialog() => container.SetActive(true);
        protected override void HideDialog() => container.SetActive(false);

        public void OnClose() => Response = ResponseType.Close;
        public void OnYes() => Response = ResponseType.Yes;
        public void OnNo() => Response = ResponseType.No;
    }
}