using System.Threading.Tasks;
using Modules.Technical.ScriptableField;
using UnityEngine;

namespace Modules.Technical.AwaitablePopup.Runtime
{
    public abstract class AwaitablePopup<T> : MonoBehaviour
    {
        [Header("PopupSettings")]
        [SerializeField] private int checkDelay = 500;
        [SerializeField] private ScriptableBool optionalOpenedField;
        
        private T response;
        private bool userResponded;

        private bool opened;
        
        public bool Opened
        {
            get => opened;
            private set
            {
                if (optionalOpenedField != null) optionalOpenedField.Value = value;
                opened = value;
            }
        }
        
        protected T Response
        {
            get => response;
            set
            {
                userResponded = true;
                response = value;
            }
        }

        private void Awake() => Hide();

        protected async Task<T> Open()
        {
            Opened = true;
            userResponded = false;
            Show();
            while (!userResponded)
                await Task.Delay(checkDelay);
            Hide();
            Opened = false;
            return Response;
        }

        protected abstract void Show();
        protected abstract void Hide();
    }
}