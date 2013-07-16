namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidget : SimpleWidget<RandomWidgetCollector, SimplePresenter, SimpleConfiguration>
    {
        public RandomWidget()
        {
            ((SimpleConfiguration)GetConfiguration()).IsScaledToPercents = true;
        }
    }
}