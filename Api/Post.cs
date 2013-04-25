// -----------------------------------------------------------------------
// <copyright file="Post.cs" company="Nokia">
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
    /// Represents a Shuffler Post
    /// </summary>
    public class Post : BaseItem
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public Uri Link { get; set; }

        /// <summary>
        /// Creates a Post from json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>A Post object</returns>
        internal static Post FromJson(JToken json)
        {
            return new Post()
            {
                Description = App.StripHtml(json.Value<string>("description")),
                Name = App.StripHtml(json.Value<string>("title")),
                Link = new Uri(json.Value<string>("url"))
            };
        }
    }
}
