namespace RSG
{
    public class PopupCanvas : CanvasBase
    {
        protected override string SortingLayerName
        {
            get => "Popup";
        }

        protected override void Awake()
        {
            base.Awake();
            PopupManager.Instance.RegisterCanvas(Canvas);
        }
    }
}
