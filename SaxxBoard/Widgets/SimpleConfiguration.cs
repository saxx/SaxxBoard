using ePunkt.Utilities;

namespace SaxxBoard.Widgets
{
    public class SimpleConfiguration : IConfiguration
    {
        public IWidget Widget { get; set; }

        public int RefreshIntervalInSeconds
        {
            get { return GetSetting("RefreshIntervalInSeconds", 120); }
        }

        public int MaxDataPointsInChart
        {
            get { return GetSetting("MaxDataPointsInChart", 100); }
        }

        public int MaxDataPointsToStore
        {
            get { return GetSetting("MaxDataPointsToStore", 500); }
        }

        public bool IsScaledToPercents { get; set; }

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