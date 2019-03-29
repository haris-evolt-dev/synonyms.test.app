using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Synonyms.Test.App
{
    public static class SettingsHelper
    {
        public static string BaseUrl {
            get
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                string url = localSettings.Values["BaseUrl"] as string;
                return url;
            }
            set
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["BaseUrl"] = value;
            }
        }
    }
}
