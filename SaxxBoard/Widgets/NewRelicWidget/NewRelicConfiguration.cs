using System;
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
            get { return GetSetting("Agents", "").Split('|').Select(x => x.Trim()); }
        }

        public string Field
        {
            get { return GetSetting("Field", "average_value"); }
        }


        public override double? MaxValueOnChart
        {
            get
            {
                if (ValueIsPercent)
                    return 100;
                if (ValueIsApdex)
                    return 1.4;
                return null;
            }
         }

        public override bool HigherValueIsBetter
        {
            get
            {
                return ValueIsApdex || Field.Is("requests_per_minute");
            }
        }

        public override bool SumInsteadOfAverage
        {
            get
            {
                return ValueIsBytes || Field.Is("requests_per_minute");
            }
        }

        public bool ValueIsPercent
        {
            get { return Metrics.Any(x => x.EndsWith("percent", StringComparison.InvariantCultureIgnoreCase)); }
        }

        public bool ValueIsApdex
        {
            get { return Metrics.Any(x => x.Is("apdex")); }
        }

        public bool ValueIsBytes
        {
            get { return Metrics.Any(x => x.EndsWith("bytes/sec", StringComparison.InvariantCultureIgnoreCase)); }
        }

        public bool ValueIsSeconds
        {
            get { return Field.Is("average_response_time"); }
        }
    }
}