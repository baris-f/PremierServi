using System.Threading.Tasks;
using UnityEngine;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public abstract class CanvasPopup<T> : AwaitablePopup<T>
    {
        [Header("Canvas Popup Settings")]
        [SerializeField] private GameObject container;
        [SerializeField] private GameObject blocker;

        private bool blockBackground;

        private void Awake() => Hide();

        public async Task<T> Open(bool shouldBlockBackground = false)
        {
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