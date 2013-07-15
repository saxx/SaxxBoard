
namespace SaxxBoard.Widgets
{
    public interface IWidget
    {
        string Title { get; set; }
        string InternalIdentifier { get; set; }
        int NumberOfDataPoints { get; set; }
        int CollectIntervalInSeconds { get; set; }

        bool IsScaledToPercents { get; }

        ICollector GetCollector();
        IPresenter GetPresenter();
        event CollectedDelegate OnCollected;
    }

    public delegate void CollectedDelegate();
}