using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Navigation;

namespace Synonyms.Test.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WordPage : Page
    {
        public WordPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                var word = await HttpClientHelper.Get<Models.WordDetails>($"word/{e.Parameter}");

                if (word == null)
                    return;

                ViewModel.Name = word.Name;
                ViewModel.Definition = word.Definitions;

                lstSynonyms.Children.Clear();
                foreach (var s in word.Synonyms) {
                    var tb = new TextBlock();
                    var hl = new Hyperlink();
                    hl.Inlines.Add(new Run { Text = s });
                    hl.Click += (obj, arg) =>
                    {
                        this.Frame.Navigate(typeof(WordPage), s);
                    };
                    tb.Inlines.Add(hl);
                    lstSynonyms.Children.Add(tb);
                    lstSynonyms.Children.Add(new TextBlock { Text = ", ", Margin = new Windows.UI.Xaml.Thickness(0, 0, 10, 0) });
                }
                lstSynonyms.Children.RemoveAt(lstSynonyms.Children.Count - 1);
            }
        }
    }
}
