using Elmah;
using ePunkt.Utilities;
using Newtonsoft.Json.Linq;
using SaxxBoard.Widgets;
using SaxxBoard.Widgets.Interfaces;
using SaxxBoard.Widgets.NewRelicWidget;
using SaxxBoard.Widgets.Pop3Widget;
using SaxxBoard.Widgets.RandomWidget;
using SaxxBoard.Widgets.RssWidget;
using SaxxBoard.Widgets.TrelloWidget;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

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
            var json = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/Widgets.json") ?? "");
            dynamic config = JObject.Parse(json);

            var widgets = new List<IWidget>();
            foreach (var widgetConfig in config.widgets)
                widgets.Add(BuildWidget(widgetConfig));
            Widgets = widgets;
        }

        private IWidget BuildWidget(dynamic widgetConfig)
        {
            IWidget widget;

            var type = (string)widgetConfig.type;
            switch (type)
            {
                case "RandomWidget":
                    widget = new RandomWidget(new WidgetConfigurationBase(widgetConfig.configuration));
                    break;
                case "RssWidget":
                    widget = new RssWidget(new RssWidgetConfiguration(widgetConfig.configuration));
                    break;
                case "Pop3Widget":
                    widget = new Pop3Widget(new Pop3WidgetConfiguration(widgetConfig.configuration));
                    break;
                case "TrelloWidget":
                    widget = new TrelloWidget(new TrelloWidgetConfiguration(widgetConfig.configuration));
                    break;
                case "NewRelicWidget":
                    widget = new NewRelicWidget(new NewRelicWidgetConfiguration(widgetConfig.configuration));
                    break;
                default:
                    throw new ApplicationException("Widget '" + type + "' does not exist.");
            }

            widget.Title = widgetConfig.title;
            widget.InternalIdentifier = widgetConfig.InternalIdentifier;
            if (widget.InternalIdentifier.IsNoE())
                widget.InternalIdentifier = type + " " + widget.Title;

            return widget;
        }

        public IEnumerable<IWidget> Widgets { get; private set; }
        public CollectedDelegate OnCollectedCallback { get; set; }
    }

    public delegate void CollectedDelegate(IWidget widget, bool updateCallerOnly);
}