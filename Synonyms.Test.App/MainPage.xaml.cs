using Synonyms.Test.App.Pages;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Synonyms.Test.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly Random Rand = new Random();
        public MainPage()

        {
            this.InitializeComponent();
            //Load();
            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;

            ASB.Focus(FocusState.Programmatic);
        }

        // Handles system-level BackRequested events and page-level back button Click events
        private bool On_BackRequested()
        {
            if (ContentFrame.CanGoBack)
            {
                this.ContentFrame.GoBack();
                NavView.IsBackEnabled = ContentFrame.CanGoBack;
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            //NavView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            NavView.BackRequested += (s, args) =>
            {
                On_BackRequested();
            };
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else
            {

                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch (item.Tag)
                {
                    case "addWord":
                        ContentFrame.Navigate(typeof(NewWordPage));
                        break;
                }
            }
        }

        private async void SearchBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0);
                try
                {
                    var suggestions = await HttpClientHelper.Get<IEnumerable<string>>(string.Format("Word/Autocomplete?query={0}&limit=5", sender.Text));
                    ASB.ItemsSource = suggestions;
                }
                finally
                {
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0);
                }
            }
        }

        private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
                ContentFrame.Navigate(typeof(WordPage), args.ChosenSuggestion);
        }
    }
}
