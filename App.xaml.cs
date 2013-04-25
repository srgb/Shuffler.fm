// -----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Nokia.Music;
using ShufflerFM.Api;
using ShufflerFM.Resources;

namespace ShufflerFM
{
    /// <summary>
    /// The Application
    /// </summary>
    public partial class App : Application
    {
        // Get API Keys for Shuffler.fm from http://shuffler.fm/developers ...
        private const string ShufflerApiKey = null;

        // Get API Keys for Nokia Music from http://api.developer.nokia.com ...
        private const string NokiaMusicAppId = null;
        private const string NokiaMusicAppCode = null;

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            this.UnhandledException += this.Application_UnhandledException;

            // Standard XAML initialization
            this.InitializeComponent();

            // Phone-specific initialization
            this.InitializePhoneApplication();

            // Language display initialization
            this.InitializeLanguage();

            // Create API Clients...
            if (string.IsNullOrEmpty(ShufflerApiKey) || string.IsNullOrEmpty(NokiaMusicAppId))
            {
                MessageBox.Show("You need to get some dev keys before this will run!");
                throw new ArgumentNullException("You need to get some dev keys!");
            }

            App.ShufflerApiClient = new ApiClient(ShufflerApiKey);
            App.NokiaMusicApiClient = new MusicClient(NokiaMusicAppId);
        }

        /// <summary>
        /// Gets the API client.
        /// </summary>
        /// <value>
        /// The API client.
        /// </value>
        /// <returns>The ApiClient object.</returns>
        public static ApiClient ShufflerApiClient
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the API client.
        /// </summary>
        /// <value>
        /// The API client.
        /// </value>
        /// <returns>The ApiClient object.</returns>
        public static MusicClient NokiaMusicApiClient
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the selected item to display in details pages.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public static object SelectedItem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the root frame.
        /// </summary>
        /// <value>
        /// The root frame.
        /// </value>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Routes an item click.
        /// </summary>
        /// <param name="item">The item.</param>
        internal static void RouteItemClick(object item)
        {
            App.SelectedItem = item;

            Track track = item as Track;
            if (track != null)
            {
                App.RootFrame.Navigate(new Uri("/TrackPage.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// Strips HTML tags.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The string with HTML removed</returns>
        internal static string StripHtml(string source)
        {
            string clean = Regex.Replace(source, "<.*?>", string.Empty);
            clean = clean.Replace("&nbsp;", " ");
            clean = clean.Replace("&#8216;", " - ");
            clean = clean.Replace("&#8217;", "'");
            clean = clean.Replace("&#8220;", "\"");
            clean = clean.Replace("&#8221;", "\"");
            //// TODO: others to replace I expect!
            return clean;
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization
        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (this.phoneApplicationInitialized)
            {
                return;
            }

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            App.RootFrame = new TransitionFrame();
            App.RootFrame.Background = this.Resources["AppBackgroundBrush"] as SolidColorBrush;
            App.RootFrame.Navigated += this.CompleteInitializePhoneApplication;

            // Handle navigation failures
            App.RootFrame.NavigationFailed += this.RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            App.RootFrame.Navigated += this.CheckForResetNavigation;

            // Ensure we don't initialize again
            this.phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (this.RootVisual != App.RootFrame)
            {
                this.RootVisual = App.RootFrame;
            }

            // Remove this handler since it is no longer needed
            App.RootFrame.Navigated -= this.CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
            {
                App.RootFrame.Navigated += this.ClearBackStackAfterReset;
            }
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= this.ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
            {
                return;
            }

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }
}