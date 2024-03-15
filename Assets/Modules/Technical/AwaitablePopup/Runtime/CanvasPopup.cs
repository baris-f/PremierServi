using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public abstract class CanvasPopup<T> : AwaitablePopup<T>
    {
        [Header("Canvas Popup Settings")]
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject blocker;
        [SerializeField] private Button buttonToSelect;

        private bool blockBackground;

        private void Awake() => Hide();

        public async Task<T> Open(bool shouldBlockBackground = false)
        {
            SelectButton();
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

        protected void SelectButton()
        {
            if (buttonToSelect != null) buttonToSelect.Select();
        }
    }
}