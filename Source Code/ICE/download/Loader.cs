//-----------------------------------------------------------------------
// <copyright file="Loader.cs" company="International Monetary Fund">
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

    /// <summary>
    /// This class represents an asyncronous loading unit.
    /// </summary>
    public abstract class Loader : ICE.download.IPrioritized
    {
        #region Fields

        /// <summary>
        /// This field is the timeout value use during the londing procedure
        /// </summary>
        private int timeout;

        /// <summary>
        /// This field is the path to the file
        /// </summary>
        private string url;

        /// <summary>
        /// this field is the content of the file when it is succefully loaded
        /// </summary>
        private Stream result = null;

        /// <summary>
        /// This is the current priority of this loader (@see priority queue usage)
        /// </summary>
        private Priority priority;

        /// <summary>
        /// This is the current error message
        /// </summary>
        private string error = null;

        #endregion

        #region Events

        /// <summary>
        /// This event occurs whether the loading process ended successfully.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// This event occurs whether the loading process failed.
        /// </summary>
        public event EventHandler Failed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the timeout value.
        /// </summary>
        public int Timeout
        {
            get { return this.timeout; }
            set { this.timeout = value; }
        }

        /// <summary>
        /// Gets or sets the path to the file/assembly.
        /// </summary>
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        /// <summary>
        /// Gets or sets the current priority.
        /// </summary>
        public Priority Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }

        /// <summary>
        /// Gets the result stream
        /// </summary>
        public Stream Result
        {
            get { return this.result; }
        }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Error
        {
            get { return this.error; }
        }

        #endregion

        #region Public functions

        /// <summary>
        /// This function start the loading process asyncronously.
        /// </summary>
        public abstract void StartLoading();

        /// <summary>
        /// This function release all ressource of the current object.
        /// </summary>
        /// <remarks>
        /// This function can be use to reuse the current object.
        /// </remarks>
        public virtual void Dispose()
        {
            if (this.result != null)
            {
                this.result.Dispose();
                this.result = null;
            }
        }

        /// <summary>
        /// This function compare the URL of the object in argument to the current url
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns>true if the object in argument is a loader and is used to load the same file</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType().IsSubclassOf(typeof(Loader)))
            {
                return ((Loader)obj).Url == this.Url;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Function use to get the corresponding hash-code
        /// </summary>
        /// <returns>hash code of the current instance</returns>
        /// <remarks>This function is implemented for compatibility purpose.</remarks>
        public override int GetHashCode()
        {
            return this.url.GetHashCode();
        }

        #endregion

        #region Protected functions

        /// <summary>
        /// This function is used by any subclass to set the result field and call the "Finished" event.
        /// </summary>
        /// <param name="stream">the stream of the loaded file</param>
        protected void SetResult(Stream stream)
        {
            this.result = stream;
            if (this.Finished != null)
            {
                 this.Finished(this, new EventArgs());
            }
        }

        /// <summary>
        /// This function is used by any subclass to set the error field and call the "Failed" event.
        /// </summary>
        /// <param name="error">an error message to set</param>
        protected void SetError(string error)
        {
            this.error = error;
            if (this.Failed != null)
            {
                this.Failed(this, new EventArgs());
            }
        }

        #endregion
    }
}
