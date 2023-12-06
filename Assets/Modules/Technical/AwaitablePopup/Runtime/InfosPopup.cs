namespace Modules.Technical.AwaitablePopup.Runtime
{
    public class InfosPopup : CanvasPopup<InfosPopup.ResponseType>
    {
        public enum ResponseType
        {
            Close,
            Acknowledge
        }
        
        public void OnClose() => Response = ResponseType.Close;
        public void OnAcknowledge() => Response = ResponseType.Acknowledge;
    }
}