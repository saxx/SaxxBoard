namespace SaxxBoard.Widgets.RssWidget
{
    public class RssConfiguration : SimpleConfiguration
    {
        public string Url
        {
            get { return GetSetting("Url", ""); }
        }
    }
}