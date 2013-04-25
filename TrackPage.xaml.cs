// -----------------------------------------------------------------------
// <copyright file="TrackPage.xaml.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using ShufflerFM.Api;

namespace ShufflerFM
{
    /// <summary>
    /// The Track Page
    /// </summary>
    public partial class TrackPage : PhoneApplicationPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackPage"/> class.
        /// </summary>
        public TrackPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the target track.
        /// </summary>
        /// <value>
        /// The target track.
        /// </value>
        public Track TargetTrack { get; set; }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                this.TargetTrack = App.SelectedItem as Track;
                this.TargetTrack.Artist.AttemptNokiaLookup();
            }
        }

        /// <summary>
        /// Plays the artist mix.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void PlayArtistMix(object sender, RoutedEventArgs e)
        {
            this.TargetTrack.Artist.PlayMixInNokiaMusic();
        }

        /// <summary>
        /// Opens web links
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OpenWebLink(object sender, RoutedEventArgs e)
        {
            FrameworkElement elem = sender as FrameworkElement;
            if (elem != null)
            {
                Uri link = elem.Tag as Uri;
                if (link != null)
                {
                    new WebBrowserTask() { Uri = link }.Show();
                }
            }
        }
    }
}