using System.Collections.Generic;
using System.Linq;
using ePunkt.Utilities;

namespace SaxxBoard.Widgets
{
    public class SimpleConfiguration : IConfiguration
    {
        public IWidget Widget { get; set; }

        public int RefreshIntervalInSeconds
        {
            get { return GetSetting("RefreshIntervalInSeconds", 900); }
        }

        public int MaxDataPointsInChart
        {
            get { return GetSetting("MaxDataPointsInChart", 300); }
        }

        public int MaxDataPointsToStore
        {
            get { return GetSetting("MaxDataPointsToStore", 300); }
        }

        public IEnumerable<string> SeriesLabels
        {
            get
            {
                var setting = GetSetting("SeriesLabels", "");
                if (setting.HasValue())
                {
                    return setting.Split('|').Select(x => x.Trim());
                }
                return new string[0];
            }
        }

        public virtual double? MinTickSizeOnChart { get { return null; } }
        public virtual double? MaxValueOnChart { get { return null; } }
        public virtual bool HigherValueIsBetter { get { return false; } }
        public virtual bool SumInsteadOfAverage { get { return false; } }

        #region Get setting helpers
        protected string GetSetting(string key, string defaultValue)
        {
            return Settings.Get(Widget.InternalIdentifier + "::" + key, defaultValue);
        }

        protected int GetSetting(string key, int defaultValue)
        {
            return Settings.Get(Widget.InternalIdentifier + "::" + key, defaultValue);
        }

        protected bool GetSetting(string key, bool defaultValue)
        {
            return Settings.Get(Widget.InternalIdentifier + "::" + key, defaultValue);
        }
        #endregion

    }
}