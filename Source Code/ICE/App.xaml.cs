//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="International Monetary Fund">
//
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
//      Lorenzin Aurélia,
//      Poirot Clément,
//      Mayer Nicolas,
//      Transler Jean-Christophe,
// </authors>
// <context>
//      Industrial Project realized at ESIAL (Ecole Supérieure d'Informatique et Applications de Lorraine)
//      for the benefit of the the Open Source Community
// </context>
// <supervisors>
//      Hervé Tourpe (industry supervisor)
//      Suzanne Collin (university supervisor)
// </supervisors>
// <years>2008 - 2009</years>
// <contributors>
//      <!-- any contributors (except for authors) to this file should be listed here -->
// </contributors>
//-----------------------------------------------------------------------

namespace ICE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// This class is the "Silverlight Application Class"
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// This constant is the title of the current licence under which this program is.
        /// </summary>
        public const string Licence = "GNU General Public Licence v2";

        /// <summary>
        /// This is the page main management system of this application
        /// </summary>
        private MainManager mainManager;

        /// <summary>
        /// Initializes a new instance of the App class.
        /// </summary>
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        /// <summary>
        /// This function is called when the application startup
        /// </summary>
        /// <param name="sender">the application itself</param>
        /// <param name="e">event arguments</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new Page();

            // Graphical module initialization
            this.mainManager = new MainManager(e.InitParams, typeof(ICE.download.WebLoader));
            this.mainManager.ListenKeyPress((Page)RootVisual);

            ((Page)this.RootVisual).LayoutRoot.Children.Add(this.mainManager.GetICEView());            
        }

        /// <summary>
        /// This function is called when the application close
        /// </summary>
        /// <param name="sender">the application itself</param>
        /// <param name="e">event arguments</param>
        private void Application_Exit(object sender, EventArgs e)
        {
            /* do nothing */
        }

        /// <summary>
        /// This function is called when an unhandled error occur in the application
        /// </summary>
        /// <param name="sender">the application itself</param>
        /// <param name="e">the error argument</param>
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { this.ReportErrorToDOM(e); });
            }
        }

        /// <summary>
        /// This function repport an error via DOM
        /// </summary>
        /// <param name="e">the error argument</param>
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
    }
}
