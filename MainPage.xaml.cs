// -----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ShufflerFM.Resources;

namespace ShufflerFM
{
    /// <summary>
    /// The Main Page
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.BuildLocalisedApplicationBar();
        }

        protected async override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var popularTracks = await App.ShufflerApiClient.GetPopularTracks();
            this.PopularLoading.Visibility = Visibility.Collapsed;
            this.PopularList.ItemsSource = popularTracks.Take(4);
        }

        private void BuildLocalisedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            this.ApplicationBar = new ApplicationBar();
            this.ApplicationBar.Mode = ApplicationBarMode.Minimized;

            ////// Create a new button and set the text value to the localized string from AppResources.
            ////ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
            ////appBarButton.Text = AppResources.AppBarButtonText;
            ////ApplicationBar.Buttons.Add(appBarButton);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarAbout = new ApplicationBarMenuItem(AppResources.AppBarAboutText);
            appBarAbout.Click += this.About_Clicked;
            this.ApplicationBar.MenuItems.Add(appBarAbout);
        }

        /// <summary>
        /// Routes clicks
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = sender as ListBox;
            if (list.SelectedIndex != -1)
            {
                App.RouteItemClick(list.SelectedItem);
            }

            list.SelectedIndex = -1;
        }

        private void ShowMore(object sender, RoutedEventArgs e)
        {
            FrameworkElement elem = sender as FrameworkElement;
            if (elem != null)
            {
                string key = elem.Tag as string;
                if (key != null)
                {
                    this.NavigationService.Navigate(new Uri("/MoreListPage.xaml?" + MoreListPage.ParamKey + "=" + key, UriKind.Relative));
                }
            }
        }

        private void About_Clicked(object sender, EventArgs e)
        {
            MessageBox.Show("Work in progress!");
        }
    }
}