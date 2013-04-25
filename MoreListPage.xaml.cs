// -----------------------------------------------------------------------
// <copyright file="MoreListPage.xaml.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ShufflerFM
{
    /// <summary>
    /// The More details page
    /// </summary>
    public partial class MoreListPage : PhoneApplicationPage
    {
        public const string ParamKey = "key";

        private string _key = null;
        private ProgressIndicator _busyIndicator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoreListPage"/> class.
        /// </summary>
        public MoreListPage()
        {
            this.InitializeComponent();

            this.Loaded += this.MoreListPage_Loaded;
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                this.GetProgressIndicator();
                this.SetBusy(true);

                if (NavigationContext.QueryString.ContainsKey(ParamKey))
                {
                    this._key = NavigationContext.QueryString[ParamKey];
                    this.PageTitle.Text = App.ShufflerApiClient.GetDisplayNameForKey(this._key);
                }
            }
        }

        /// <summary>
        /// Handles the Loaded event of the MoreListPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MoreListPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.GetProgressIndicator();

            if (!string.IsNullOrEmpty(this._key) && this.List.ItemsSource == null)
            {
                this.List.ItemsSource = App.ShufflerApiClient.GetCachedItems(this._key);
                this.SetBusy(false);
            }
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

        /// <summary>
        /// Sets the progress indicator to busy.
        /// </summary>
        /// <param name="show">if set to <c>true</c> show progress.</param>
        private void SetBusy(bool show)
        {
            if (this._busyIndicator != null)
            {
                this._busyIndicator.IsVisible = show;
            }
        }

        private void GetProgressIndicator()
        {
            // Store the progress indicator...
            if (this._busyIndicator == null)
            {
                this._busyIndicator = SystemTray.ProgressIndicator;
                this.SetBusy(false);
            }
        }
    }
}