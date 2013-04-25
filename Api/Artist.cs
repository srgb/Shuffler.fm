// -----------------------------------------------------------------------
// <copyright file="Artist.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nokia.Music;

namespace ShufflerFM.Api
{
    /// <summary>
    /// Represents a Shuffler Artist
    /// </summary>
    public class Artist : BaseItem
    {
        private bool _lookupAttempted = false;
        private Nokia.Music.Types.Artist _nokiaMusicArtist = null;

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is available on nokia music.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is available on nokia music; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailableOnNokiaMusic { get; set; }

        /// <summary>
        /// Plays the mix in nokia music.
        /// </summary>
        public void PlayMixInNokiaMusic()
        {
            if (this._nokiaMusicArtist != null)
            {
                this._nokiaMusicArtist.PlayMix();
            }
        }

        /// <summary>
        /// Attempts the nokia lookup.
        /// </summary>
        public async void AttemptNokiaLookup()
        {
            if (!this._lookupAttempted)
            {
                this._lookupAttempted = true;

                string name = this.Name;

                int index = name.IndexOf("with", StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                {
                    name = name.Substring(0, index - 1);
                }

                index = name.IndexOf("ft.", StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                {
                    name = name.Substring(0, index - 1);
                }

                index = name.IndexOf("feat.", StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                {
                    name = name.Substring(0, index - 1);
                }

                index = name.IndexOf("f/", StringComparison.OrdinalIgnoreCase);
                if (index > -1)
                {
                    name = name.Substring(0, index - 1);
                }

                if (!string.IsNullOrEmpty(name))
                {
                    ListResponse<Nokia.Music.Types.Artist> artists = await App.NokiaMusicApiClient.SearchArtistsAsync(name);

                    if (artists.Result != null && artists.Result.Count > 0)
                    {
                        if (string.Compare(artists.Result[0].Name, name, CultureInfo.CurrentUICulture, CompareOptions.IgnoreNonSpace) == 0)
                        {
                            this.IsAvailableOnNokiaMusic = true;
                            this._nokiaMusicArtist = artists.Result[0];
                            this.NotifyPropertyChanged("IsAvailableOnNokiaMusic");
                            Debug.WriteLine("Found " + name + " on Nokia Music");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an Artist from json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>An Artist object</returns>
        internal static Artist FromJson(JToken json)
        {
            return new Artist()
            {
                Id = json.Value<string>("id"),
                Name = json.Value<string>("name")
            };
        }
    }
}
