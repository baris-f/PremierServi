using System.Threading.Tasks;
using UnityEngine;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public abstract class AwaitablePopup<T> : MonoBehaviour
    {
        [Header("PopupSettings")]
        [SerializeField] private int checkDelay = 500;

        private T response;
        private bool userResponded;

        public bool Opened { get; private set; }
        protected T Response
        {
            get => response;
            set
            {
                userResponded = true;
                response = value;
            }
        }

        private void Awake() => HideDialog();

        public async Task<T> OpenDialog()
        {
            Opened = true;
            userResponded = false;
            ShowDialog();
            while (!userResponded)
                await Task.Delay(checkDelay);
            HideDialog();
            Opened = false;
            return Response;
        }

        protected abstract void ShowDialog();
        protected abstract void HideDialog();
    }
}