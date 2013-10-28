using System;

namespace SaxxBoard.Widgets
{
    public class WidgetBase<TCollector, TPresenter, TConfiguration> : IWidget
        where TCollector : class, IWidgetCollector, new()
        where TPresenter : class, IWidgetPresenter, new()
        where TConfiguration : class, IWidgetConfiguration
    {

        private TCollector _collector;
        private TPresenter _presenter;

        public WidgetBase(TConfiguration configuration)
        {
            Configuration = configuration;
        } 

        public IWidgetCollector GetCollector()
        {
            return _collector ?? (_collector = new TCollector
                {
                    Widget = this
                });
        }

        public IWidgetPresenter GetPresenter()
        {
            return _presenter ?? (_presenter = new TPresenter
            {
                Widget = this
            });
        }

        public IWidgetConfiguration Configuration { get; private set; }

        public string Title { get; set; }
        public string InternalIdentifier { get; set; }

        public DateTime? LastUpdate { get; set; }
        public DateTime? NextUpdate { get; set; }
    }
}