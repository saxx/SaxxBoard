namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidget : SimpleWidget<RandomWidgetCollector, SimplePresenter>
    {
        public override bool IsScaledToPercents { get { return true; } }
    }
}