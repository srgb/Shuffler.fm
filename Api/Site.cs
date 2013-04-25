// -----------------------------------------------------------------------
// <copyright file="Site.cs" company="Nokia">
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
    /// Represents a Shuffler Site
    /// </summary>
    public class Site : BaseItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public Uri Link { get; set; }

        /// <summary>
        /// Creates a Site from json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>A Site object</returns>
        internal static Site FromJson(JToken json)
        {
            return new Site()
            {
                Id = json.Value<string>("id"),
                Name = json.Value<string>("title"),
                Link = new Uri(json.Value<string>("url"))
            };
        }
    }
}
