using System.Collections;
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
            AvailableWidgets = new[]
                {
                    new RandomWidget.RandomWidget
                        {
                            Name = "Randwom Widget I",
                            Identifier = "RandomWidget1"
                        },
                        new RandomWidget.RandomWidget
                        {
                            Name = "Random Widget II",
                            Identifier = "RandomWidget2"
                        },
                        new RandomWidget.RandomWidget
                        {
                            Name = "Random Widget III",
                            Identifier = "RandomWidget3"
                        }
                };
        }

        public IEnumerable<IWidget> AvailableWidgets { get; private set; }

        public CollectedDelegate OnCollectedCallback { get; set; }
    }
}