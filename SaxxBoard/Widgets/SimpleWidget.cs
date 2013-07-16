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
            return _collector ?? (_collector = new TCollector
                {
                    Widget = this
                });
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
    }
}