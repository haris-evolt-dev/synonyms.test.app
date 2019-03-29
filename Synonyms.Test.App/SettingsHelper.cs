using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Synonyms.Test.App
{
    /// <summary>
    /// Helper for getting local settings values
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// Gets or sets base URL for API endpoints that are used in application
        /// </summary>
        public static string BaseUrl {
            get
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                string url = localSettings.Values["BaseUrl"] as string;
                return url ?? "https://synonymstest.azurewebsites.net/api/";
            }
            set
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["BaseUrl"] = value;
            }
        }
    }
}
