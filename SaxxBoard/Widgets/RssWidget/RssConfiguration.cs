using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets.RssWidget
{
    public class RssConfiguration : SimpleConfiguration
    {
        public override double? MinTickSizeOnChart
        {
            get { return 1; }
        }

        public IEnumerable<string> Urls
        {
            get { return GetSetting("Urls", "").Split('|').Select(x => x.Trim()); }
        }

        public override bool SumInsteadOfAverage
        {
            get { return true; }
        }
    }
}