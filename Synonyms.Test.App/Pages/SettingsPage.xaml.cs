using Windows.UI.Xaml.Controls;

namespace Synonyms.Test.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        public string BaseUrl
        {
            get
            {
                return SettingsHelper.BaseUrl;
            }
            set
            {
                SettingsHelper.BaseUrl = value;
            }
        }
    }
}
