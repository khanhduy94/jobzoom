//-----------------------------------------------------------------------
// <copyright file="JavaScriptManager.cs" company="International Monetary Fund">
//    This file is part of "Information Connections Engine". See more information at http://ICEdotNet.codeplex.com
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
//
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

namespace ICE
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using ICE.download;

    /// <summary>
    /// this class aim to centralize all Java Script Interactions
    /// </summary>
    public class JavaScriptManager
    {
        /// <summary>
        /// this is the link to the main manager
        /// </summary>
        private MainManager mainManager;

        /// <summary>
        /// Initializes a new instance of the JavaScriptManager class.
        /// </summary>
        /// <param name="mainManager">the main manager</param>
        public JavaScriptManager(MainManager mainManager)
        {
            this.mainManager = mainManager;
            HtmlPage.RegisterScriptableObject("JS2SL_Bridge", this);
        }

        /// <summary>
        /// This function try to invoke the corresponding JS event
        /// </summary>
        /// <param name="sender">the sender of the JS event</param>
        /// <param name="functionName">the name of the function you want to invoke</param>
        /// <param name="argument">Argument of you function call</param>
        public static void Invoke(object sender, string functionName, string argument)
        {
            try
            {
                HtmlPage.Window.Invoke(functionName, argument);
            }
            catch (Exception)
            {
                /* do nothing */
            }
        }

        /// <summary>
        /// this function ask the application to download the file at the current Uri.
        /// if it's a relation file, the node will be selected.
        /// </summary>
        /// <param name="url">the URL of the file you wand to download</param>
        [ScriptableMember]
        public void Load(string url)
        {
            this.mainManager.FileDownloadManager.LoadFile(url, Priority.Important);
        }

        /// <summary>
        /// this function ask the model to suppress all reference about an object
        /// </summary>
        /// <param name="id">Id name of the object you want to suppress</param>
        [ScriptableMember]
        public void DisposeObject(string id)
        {
            try
            {
                this.mainManager.ModelManager.DisposeObject(id);
            }
            catch (Exception)
            {
                this.mainManager.ViewManager.AddDebugMessage("The JavaScript function \"DisposeObject(id)\" received a bad argument (" + id + ")");
            }
        }
    }
}
