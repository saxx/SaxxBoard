
namespace SaxxBoard.Widgets
{
    public interface IWidget
    {
        string Title { get; set; }
        string InternalIdentifier { get; set; }

        ICollector GetCollector();
        IPresenter GetPresenter();
        IConfiguration GetConfiguration();
    }
}