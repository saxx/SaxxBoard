using System.Collections.Generic;
using System.Linq;
using ePunkt.Utilities;

namespace SaxxBoard.Widgets.NewRelicWidget
{
    public class NewRelicConfiguration : SimpleConfiguration
    {
        public string ApiKey
        {
            get { return GetSetting("ApiKey", ""); }
        }

        public string Account
        {
            get { return GetSetting("Account", ""); }
        }

        public IEnumerable<string> Metrics
        {
            get { return GetSetting("Metrics", "").Split(';').Select(x => x.Trim()).Where(x => x.HasValue()); }
        }

        public IEnumerable<string> Agents
        {
            get { return GetSetting("Agents", "").Split(';').Select(x => x.Trim()).Where(x => x.HasValue()); }
        }

        public string Field
        {
            get { return GetSetting("Field", "average_value"); }
        }
    }
}