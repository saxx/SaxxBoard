namespace SaxxBoard.Widgets
{
    public class SimpleWidget<TCollector, TPresenter, TConfiguration> : IWidget
        where TCollector : class, ICollector, new()
        where TPresenter : class, IPresenter, new()
        where TConfiguration : class, IConfiguration, new()
    {

        private TCollector _collector;
        private TPresenter _presenter;
        private TConfiguration _configuration;

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

        public IConfiguration GetConfiguration()
        {
            return _configuration ?? (_configuration = new TConfiguration
            {
                Widget = this
            });
        }

        public string Title { get; set; }
        public string InternalIdentifier { get; set; }

        protected void FireOnCollectedEvent()
        {
            if (OnCollected != null)
                OnCollected();
        }
        public event CollectedDelegate OnCollected;
    }
}