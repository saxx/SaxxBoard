using System;

namespace SaxxBoard.Widgets
{
    public interface IWidget
    {
        string Title { get; set; }
        string InternalIdentifier { get; set; }

        DateTime? LastUpdate { get; set; }
        DateTime? NextUpdate { get; set; }

        IWidgetCollector GetCollector();
        IWidgetPresenter GetPresenter();
        IWidgetConfiguration Configuration { get; }
    }
}