using System.Collections.Generic;
using System.Linq;
using ePunkt.Utilities;

namespace SaxxBoard.Widgets.TrelloWidget
{
    public class TrelloConfiguration : SimpleConfiguration
    {
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

        public IEnumerable<string> Lists
        {
            get { return GetSetting("Lists", "").Split(';').Select(x => x.Trim()).Where(x => x.HasValue()); }
        }
    }
}