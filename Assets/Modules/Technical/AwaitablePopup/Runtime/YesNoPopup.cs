namespace Modules.Technical.AwaitablePopup.Runtime
{
    public class YesNoPopup : CanvasPopup<YesNoPopup.ResponseType>
    {
        public enum ResponseType
        {
            Close,
            Yes,
            No
        }

        public void OnClose() => Response = ResponseType.Close;
        public void OnYes() => Response = ResponseType.Yes;
        public void OnNo() => Response = ResponseType.No;
    }
}