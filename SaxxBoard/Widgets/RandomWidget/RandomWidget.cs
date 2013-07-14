namespace SaxxBoard.Widgets.RandomWidget
{
    public class RandomWidget : WidgetBase
    {
        private RandomWidgetCollector _randomWidgetCollector;
        private RandomWidgetPresenter _randomWidgetPresenter;

        public override ICollector GetCollector()
        {
            if (_randomWidgetCollector == null)
            {
                _randomWidgetCollector = new RandomWidgetCollector
                    {
                        Widget = this
                    };
                _randomWidgetCollector.OnCollected += () =>
                    {
                        if (OnCollected != null)
                            OnCollected();
                    };
            }
            return _randomWidgetCollector;
        }

        public override IPresenter GetPresenter()
        {
            return _randomWidgetPresenter ?? (_randomWidgetPresenter = new RandomWidgetPresenter
                {
                    Widget = this
                });
        }

        public override event CollectedDelegate OnCollected;
    }
}