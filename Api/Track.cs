// -----------------------------------------------------------------------
// <copyright file="Track.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ShufflerFM.Api
{
    /// <summary>
    /// Represents a Shuffler Track
    /// </summary>
    public class Track : BaseItem
    {
        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>
        /// The artist.
        /// </value>
        public Artist Artist { get; set; }

        /// <summary>
        /// Gets or sets the feature count.
        /// </summary>
        /// <value>
        /// The feature count.
        /// </value>
        public int FeatureCount { get; set; }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        /// <value>
        /// The post.
        /// </value>
        public Post Post { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public Site Site { get; set; }

        /// <summary>
        /// Gets or sets the shuffler link.
        /// </summary>
        /// <value>
        /// The shuffler link.
        /// </value>
        public Uri ShufflerLink { get; set; }

        /// <summary>
        /// Gets or sets the 240x240 thumb.
        /// </summary>
        /// <value>
        /// The 240x240 thumb.
        /// </value>
        public Uri Thumb240 { get; set; }

        /// <summary>
        /// Creates an Track from json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>An Track object</returns>
        internal static Track FromJson(JToken json)
        {
            JToken chartData = json["chart_data"];
            JToken images = json["images"];
            JToken links = json["links"];
            JToken metadata = json["metadata"];
            JToken post = json["post"];
            JToken site = json["site"];

            if (chartData.Type != JTokenType.Null && links.Type != JTokenType.Null && metadata.Type != JTokenType.Null && post.Type != JTokenType.Null && site.Type != JTokenType.Null && images.Type != JTokenType.Null && metadata["artist"].Type != JTokenType.Null)
            {
                JToken artist = metadata["artist"];
                Track track = new Track()
                {
                    Name = metadata.Value<string>("title"),
                    Artist = Artist.FromJson(artist),
                    FeatureCount = chartData.Value<int>("feature_count"),
                    Post = Post.FromJson(post),
                    Site = Site.FromJson(site),
                    ShufflerLink = new Uri(links.Value<string>("player_url"))
                };

                if (images["240x240"].Type != JTokenType.Null)
                {
                    track.Thumb240 = new Uri(images["240x240"].Value<string>("url"));
                }

                return track;
            }
            else
            {
                return null;
            }
        }
    }
}
