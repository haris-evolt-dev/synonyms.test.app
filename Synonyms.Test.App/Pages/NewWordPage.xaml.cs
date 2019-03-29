using Synonyms.Test.App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Synonyms.Test.App.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewWordPage : Page
    {
        public WordDetails ViewModel { get; set; } = new WordDetails();
        public string Synonym { get; set; }

        public NewWordPage()
        {
            this.InitializeComponent(); ;
            KeyEventHandler handler = new KeyEventHandler(sbSynonyms_KeyDown);
            sbSynonyms.AddHandler(UIElement.KeyDownEvent, handler, handledEventsToo: true);
        }

        private async void sbSynonyms_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suggestions = await HttpClientHelper.Get<IEnumerable<string>>(string.Format("Word/Autocomplete?query={0}&limit=5", sender.Text));
                sbSynonyms.ItemsSource = suggestions;
            }
        }

        private void sbSynonyms_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            AddSynonym();
        }

        private async void AddWord()
        {
            try
            {
                await HttpClientHelper.Post<string>("Word", ViewModel.Name);
                foreach (var s in ViewModel.Synonyms)
                {
                    await HttpClientHelper.Put<string>($"synonym/{ViewModel.Name}", s);
                }
                ViewModel.Name = "";
                ViewModel.Definition = "";
                ViewModel.Synonyms.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        private void AddSynonym()
        {
            if (string.IsNullOrEmpty(Synonym))
                return;
            ViewModel.Synonyms.Add(Synonym);
            Synonym = string.Empty;
            sbSynonyms.Text = string.Empty;
            sbSynonyms_w.Text = string.Empty;
            sbSynonyms.Focus(FocusState.Keyboard);
        }

        private void btnAddWord_Click(object sender, RoutedEventArgs e)
        {
            AddWord();
        }

        private void btnAddSynonym_Click(object sender, RoutedEventArgs e)
        {
            AddSynonym();
        }

        private void SymbolIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string word = (e.OriginalSource as TextBlock)?.DataContext?.ToString();
            ViewModel.Synonyms.Remove(word);
        }

        private void sbSynonyms_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddSynonym();
            }
        }
    }
}
