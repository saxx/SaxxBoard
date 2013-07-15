using System.Collections.Generic;

namespace SaxxBoard.Widgets
{
    public class WidgetCollection
    {
        public WidgetCollection()
        {
            BuildWidgets();
            foreach (var widget in AvailableWidgets)
                widget.OnCollected += () =>
                    {
                        if (OnCollectedCallback != null)
                            OnCollectedCallback();
                    };
        }

        private void BuildWidgets()
        {
            AvailableWidgets = new IWidget[]
                {
                    new Pop3Widget.Pop3Widget 
                        {
                            Title = "support@epunkt.net e-mails",
                            InternalIdentifier = "support@epunkt.net"
                        },
                        new RssWidget.RssWidget
                        {
                            Title = "IssueTracker new items",
                            InternalIdentifier = "IssueTracker"
                        },
                        new RandomWidget.RandomWidget
                        {
                            Title = "Random Widget",
                            InternalIdentifier = "RandomWidget",
                            CollectIntervalInSeconds = 10
                        }
                };
        }

        public IEnumerable<IWidget> AvailableWidgets { get; private set; }

        public CollectedDelegate OnCollectedCallback { get; set; }
    }
}