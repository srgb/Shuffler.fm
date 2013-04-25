// -----------------------------------------------------------------------
// <copyright file="BaseItem.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShufflerFM.Api
{
    /// <summary>
    /// A Base Api Item
    /// </summary>
    public abstract class BaseItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Used by Silverlight to find out if we change properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the group tag.
        /// </summary>
        /// <value>
        /// The group tag.
        /// </value>
        public string GroupTag { get; set; }

        /// <summary>
        /// Notifies the UI that a property has changed
        /// </summary>
        /// <param name="propertyName">The property that has changed</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                });
            }
        }
    }
}
