namespace SaxxBoard.Widgets.Pop3Widget
{
    public class Pop3Configuration : SimpleConfiguration
    {
        public string Host
        {
            get { return GetSetting("Host", "localhost"); }
        }

        public int Port
        {
            get { return GetSetting("Port", 110); }
        }

        public bool UseSsl
        {
            get { return GetSetting("UseSsl", false); }
        }

        public string Username
        {
            get { return GetSetting("Username", ""); }
        }

        public string Password
        {
            get { return GetSetting("Password", ""); }
        }

        public override double? MinTickSizeOnChart
        {
            get { return 1; }
        }

        public override bool SumInsteadOfAverage
        {
            get { return true; }
        }
    }
}