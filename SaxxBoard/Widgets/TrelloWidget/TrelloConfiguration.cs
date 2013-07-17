using System.Collections.Generic;
using System.Linq;

namespace SaxxBoard.Widgets.TrelloWidget
{
    public class TrelloConfiguration : SimpleConfiguration
    {
        public override double? MinTickSizeOnChart
        {
            get { return 1; }
        }

        public override bool SumInsteadOfAverage
        {
            get { return true; }
        }

        public string AppKey
        {
            get { return GetSetting("AppKey", ""); }
        }

        public string UserToken
        {
            get { return GetSetting("UserToken", ""); }
        }

        public string Board
        {
            get { return GetSetting("Board", ""); }
        }

        public IEnumerable<IEnumerable<string>> Lists
        {
            get
            {
                var series = GetSetting("Lists", "").Split('|').Select(x => x.Trim());
                return from x in series
                       select x.Split(';').Select(y => y.Trim());
            }
        }
    }
}