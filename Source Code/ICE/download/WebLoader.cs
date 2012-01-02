//-----------------------------------------------------------------------
// <copyright file="WebLoader.cs" company="International Monetary Fund">
//   This file is part of "Information Connections Engine". See more information at http://ICEdotNet.codeplex.com
//
//   "Information Connections Engine" is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 2 of the License, or
//   (at your option) any later version.
//
//   "Information Connections Engine" is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with "Information Connections Engine".  If not, see http://www.gnu.org/license.
// </copyright>
// <authors>
//      Poirot Clément (Project Officer)
// </authors>
// <context>
//      for the benefit of the the Open Source Community
// </context>
// <supervisors>
//      Hervé Tourpe (Team Leader)
//      Jeffrey Hatton (Project Manager)
// </supervisors>
// <years>2008 - 2009</years>
// <contributors>
//      <!-- any contributors (except for authors) to this file should be listed here -->
// </contributors>
//-----------------------------------------------------------------------

namespace ICE.download
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Windows;
    
    /// <summary>
    /// This class is an implementation of Loader us to access external file via HTTP
    /// </summary>
    public class WebLoader : Loader
    {
        /// <summary>
        /// This is the http client used to get the file
        /// </summary>
        private WebClient client;

        /// <summary>
        /// this is the timeout thread
        /// </summary>
        private Thread thread;

        /// <summary>
        /// This function release all resources and reset the component.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// This function start the loading process asyncronouly
        /// </summary>
        public override void StartLoading()
        {
            try
            {
                // create the client
                this.client = new WebClient();
                this.client.OpenReadCompleted += new OpenReadCompletedEventHandler(this.Client_OpenReadCompleted);

                // create the timeout (Silverlight technology restriction)
                this.thread = new Thread((ParameterizedThreadStart)delegate
                {
                    Thread.Sleep(this.Timeout);
                    if (this.client.IsBusy)
                    {
                        try
                        {
                            this.client.CancelAsync();
                        }
                        catch (Exception error)
                        {
                            /* do nothing */
                        }
                    }
                });

                // start asyncronous activity
                this.client.OpenReadAsync(new Uri(this.Url));
                this.thread.Start();
            }
            catch (Exception exception)
            {
                this.SetError("An error occured while loading the file \"" + this.Url + "\" (" + exception.Message + ")");
            }
        }

        /// <summary>
        /// This function is called when the client end to download
        /// </summary>
        /// <param name="sender">our web client</param>
        /// <param name="e">the event arguments</param>
        private void Client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.SetError("An error occured while loading the file \"" + this.Url + "\" (Timeout)");
                return;
            }

            if (e.Error != null)
            {
                this.SetError("An error occured while loading the file \"" + this.Url + "\" (" + e.Error.Message + ")");
                return;
            }

            this.SetResult(e.Result);
        }
    }
}
