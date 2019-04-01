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

namespace Synonyms.Test.App.Pages
{
    /// <summary>
    /// This page is used for adding a new word along with it's synonyms
    /// </summary>
    public sealed partial class NewWordPage : Page
    {
        #region === Fields ===

        public WordDetails ViewModel { get; set; } = new WordDetails();
        public string Synonym { get; set; }

        #endregion

        #region === Constructors ===

        public NewWordPage()
        {
            this.InitializeComponent(); ;
            KeyEventHandler handler = new KeyEventHandler(sbSynonyms_KeyDown);
            sbSynonyms.AddHandler(UIElement.KeyDownEvent, handler, handledEventsToo: true);
        }

        #endregion

        #region === Methods ===

        private async void sbSynonyms_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0);
                try
                {
                    var suggestions = await HttpClientHelper.Get<IEnumerable<string>>(string.Format("Word/Autocomplete?query={0}&limit=5", sender.Text));
                    sbSynonyms.ItemsSource = suggestions;
                    sbSynonyms_w.ItemsSource = suggestions;
                }
                finally
                {
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
                }
                if (Synonym == ViewModel.Name)
                    ViewModel.Error = "Word cannot have itself as synonym!";
                else
                    ViewModel.Error = "";
            }
        }

        private void sbSynonyms_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if(ViewModel.HasNoError)
                AddSynonym();
        }

        private async void AddWord()
        {
            try
            {
                await HttpClientHelper.Post<string>("Word", ViewModel.Name);
                foreach (var s in ViewModel.Synonyms)
                {
                    try
                    {
                        await HttpClientHelper.Put<string>($"synonym/{ViewModel.Name}", s);
                    }
                    catch (Exception ex)
                    {
                    }
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

            ViewModel.OnPropertyChanged("HasNoSynonyms");
            ViewModel.OnPropertyChanged("HasSynonyms");
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

            ViewModel.OnPropertyChanged("HasNoSynonyms");
            ViewModel.OnPropertyChanged("HasSynonyms");
        }

        private void sbSynonyms_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (ViewModel.HasNoError && e.Key == Windows.System.VirtualKey.Enter)
            {
                AddSynonym();
            }
        }

        #endregion
    }
}
