// -----------------------------------------------------------------------
// <copyright file="ApiClient.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ShufflerFM.Resources;

namespace ShufflerFM.Api
{
    /// <summary>
    /// The Shuffler.FM Api Client
    /// </summary>
    public class ApiClient
    {
        public const string PopularCacheKey = "popular";

        private const string RootUri = "https://api.shuffler.fm/v2/";
        private const string ParamApiKey = "app-key";
        private const string ParamFilter = "filter";

        private string _apiKey;
        private Dictionary<string, IList> _cache = new Dictionary<string, IList>();

        //////////////////////////////////////////////////////////
        // TODO: persist fetches, don't re-fetch if we have it
        //////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClient"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public ApiClient(string apiKey)
        {
            this._apiKey = apiKey;
        }

        /// <summary>
        /// A parser delegate
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="tag">The tag.</param>
        /// <param name="result">The result.</param>
        /// <returns>A list of items from the API</returns>
        private delegate List<T> ResultParser<T>(string tag, string result);

        /// <summary>
        /// Gets cached items.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A list of items</returns>
        public IList GetCachedItems(string key)
        {
            return this._cache[key];
        }

        /// <summary>
        /// Gets the display name for key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>A display name</returns>
        public string GetDisplayNameForKey(string key)
        {
            switch (key)
            {
                case PopularCacheKey:
                    return AppResources.HeaderPopular;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the popular tracks.
        /// </summary>
        /// <returns>A list of popular tracks</returns>
        public Task<List<Track>> GetPopularTracks()
        {
            if (!this.ExistsinCache(PopularCacheKey))
            {
                return this.GetItemsAsync<Track>(string.Format("{0}charts/popular?app-key={1}", RootUri, this._apiKey), PopularCacheKey, ParseTracks);
            }
            else
            {
                return this.GetItemsFromCache<Track>(PopularCacheKey);
            }
        }

        /// <summary>
        /// Parses the tracks.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="result">The result.</param>
        /// <returns>A list of tracks</returns>
        private static List<Track> ParseTracks(string tag, string result)
        {
            JArray items = JArray.Parse(result);
            List<Track> tracks = new List<Track>();

            if (items != null)
            {
                foreach (JToken item in items)
                {
                    Track track = Track.FromJson(item);
                    if (track != null)
                    {
                        tracks.Add(track);
                    }
                }
            }

            return tracks;
        }

        /// <summary>
        /// Gets a list of items from the API
        /// </summary>
        /// <typeparam name="T">The type of the list items expected</typeparam>
        /// <param name="uri">The URI to hit.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="parser">The parser.</param>
        /// <returns>
        /// A list of items from the API
        /// </returns>
        private Task<List<T>> GetItemsAsync<T>(string uri, string tag, ResultParser<T> parser)
        {
            var tcs = new TaskCompletionSource<List<T>>();

            var client = new WebClient();

            client.DownloadStringCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    try
                    {
                        Debug.WriteLine("Success from " + uri);
                        var resultList = parser(tag, e.Result);
                        this.StoreItemsInCache(tag, resultList);
                        tcs.SetResult(resultList);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error from " + uri + " " + ex.Message);
                        tcs.SetException(ex);
                    }
                }
                else
                {
                    tcs.SetException(e.Error);
                }
            };

            client.DownloadStringAsync(new Uri(uri));

            return tcs.Task;
        }

        /// <summary>
        /// Gets a bool indicating whether the specified item exists in our cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>A bool indicating whether the specified item exists in our cache</returns>
        private bool ExistsinCache(string cacheKey)
        {
            lock (this._cache)
            {
                return this._cache.ContainsKey(cacheKey);
            }
        }

        /// <summary>
        /// Gets items from cache.
        /// </summary>
        /// <typeparam name="T">The type to return a list of</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>A list of type T</returns>
        private Task<List<T>> GetItemsFromCache<T>(string cacheKey)
        {
            if (this.ExistsinCache(cacheKey))
            {
                var tcs = new TaskCompletionSource<List<T>>();
                tcs.SetResult(this._cache[cacheKey] as List<T>);
                return tcs.Task;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Stores items in cache.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="resultList">The result list.</param>
        private void StoreItemsInCache(string cacheKey, IList resultList)
        {
            lock (this._cache)
            {
                if (!this._cache.ContainsKey(cacheKey))
                {
                    this._cache.Add(cacheKey, resultList);
                }
            }
        }
    }
}
