namespace SaxxBoard.Widgets
{
    public class SimpleWidget<TCollector, TPresenter> : IWidget
        where TCollector : class, ICollector, new()
        where TPresenter : class, IPresenter, new()
    {

        private TCollector _collector;
        private TPresenter _presenter;

        public SimpleWidget()
        {
            CollectIntervalInSeconds = 60;
            NumberOfDataPoints = 128;
        }

        public ICollector GetCollector()
        {
            if (_collector == null)
            {
                _collector = new TCollector
                {
                    Widget = this
                };
                _collector.OnCollected += FireOnCollectedEvent;
            }
            return _collector;
        }

        public IPresenter GetPresenter()
        {
            return _presenter ?? (_presenter = new TPresenter
            {
                Widget = this
            });
        }

        public string Title { get; set; }
        public string InternalIdentifier { get; set; }
        public int NumberOfDataPoints { get; set; }
        public int CollectIntervalInSeconds { get; set; }

        public virtual bool IsScaledToPercents { get { return false; } }

        protected void FireOnCollectedEvent()
        {
            if (OnCollected != null)
                OnCollected();
        }
        public event CollectedDelegate OnCollected;
    }
}