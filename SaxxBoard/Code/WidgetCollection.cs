using ePunkt.Utilities;
using SaxxBoard.Widgets;
using System;
using System.Collections.Generic;

namespace SaxxBoard
{
    public class WidgetCollection
    {
        public WidgetCollection()
        {
            BuildWidgets();
        }

        private void BuildWidgets()
        {
            var widgets = new List<IWidget>();

            var count = 0;
            while (true)
            {
                count++;
                if (Settings.Get("Widget" + count + "::ClassName", "").HasValue())
                {
                    var widgetClassName = Settings.Get("Widget" + count + "::ClassName", "");

                    var type = Type.GetType(widgetClassName);
                    if (type != null)
                    {
                        var widget = (IWidget) Activator.CreateInstance(type);
                        widget.InternalIdentifier = Settings.Get("Widget" + count + "::InternalIdentifier", "");
                        widget.Title = Settings.Get("Widget" + count + "::Title", "");
                        widgets.Add(widget);
                    }
                }
                else
                    break;
            }

            CurrentWidgets = widgets;
        }

        public IEnumerable<IWidget> CurrentWidgets { get; private set; }
        public CollectedDelegate OnCollectedCallback { get; set; }
    }

    public delegate void CollectedDelegate(IWidget widget);
}