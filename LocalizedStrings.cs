// -----------------------------------------------------------------------
// <copyright file="LocalizedStrings.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using ShufflerFM.Resources;

namespace ShufflerFM
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources
        {
            get
            {
                return LocalizedStrings._localizedResources;
            }
        }
    }
}