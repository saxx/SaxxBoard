
namespace SaxxBoard.Widgets
{
    public interface IWidget
    {
        string Name { get; set; }
        string Identifier { get; set; }
        ICollector GetCollector();
        IPresenter GetPresenter();
        event CollectedDelegate OnCollected;
    }

    public delegate void CollectedDelegate();
}