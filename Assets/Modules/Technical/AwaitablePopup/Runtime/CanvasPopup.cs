using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public abstract class CanvasPopup<T> : AwaitablePopup<T>
    {
        [Header("Canvas Popup Settings")]
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject blocker;
        [SerializeField] private TextMeshProUGUI message;

        private bool blockBackground;

        private void Awake() => Hide();

        public async Task<T> Open(string newMessage = "", bool shouldBlockBackground = false)
        {
            if (!string.IsNullOrWhiteSpace(newMessage)) message.text = newMessage;
            blockBackground = shouldBlockBackground;
            return await base.Open();
        }

        protected override void Show()
        {
            blocker.SetActive(blockBackground);
            container.SetActive(true);
        }

        protected override void Hide()
        {
            blocker.SetActive(false);
            container.SetActive(false);
        }
    }
}