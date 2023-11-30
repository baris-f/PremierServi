using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public class InfoPopup : AwaitablePopup<bool>
    {
        [Header("Ui Refs")]
        [SerializeField] private GameObject container;
        [SerializeField] private TextMeshProUGUI message;

        private void Awake() => HideDialog();

        public async Task<bool> OpenDialog(string newMessage)
        {
            message.text = newMessage;
            return await OpenDialog();
        }

        protected override void ShowDialog() => container.SetActive(true);
        protected override void HideDialog() => container.SetActive(false);

        public void OnClose() => Response = false;
        public void OnAcknowledge() => Response = true;
    }
}