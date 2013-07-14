namespace SaxxBoard.Widgets
{
    public abstract class WidgetBase : IWidget
    {
        public string Name { get; set; }
        public string Identifier { get; set; }

        public abstract ICollector GetCollector();
        public abstract IPresenter GetPresenter();
        public abstract event CollectedDelegate OnCollected;
    }
}