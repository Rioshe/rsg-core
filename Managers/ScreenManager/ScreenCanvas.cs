namespace RSG
{
    public class ScreenCanvas : CanvasBase
    {
        protected override string SortingLayerName
        {
            get => "Screen";
        }

        protected override void Awake()
        {
            base.Awake();
            ScreenManager.Instance.RegisterCanvas(Canvas);
        }
    }
}