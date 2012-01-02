//-----------------------------------------------------------------------
// <copyright file="SplashScreen.xaml.cs" company="International Monetary Fund">
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

namespace ICE.view
{
    using System.Windows.Controls;

    /// <summary>
    /// Splash screen to keep the user waiting while is ICE launching
    /// </summary>
    public partial class SplashScreen : UserControl
    {
        /// <summary>
        /// Number of loading errors
        /// </summary>
        private int number;

        /// <summary>
        /// List of all errors appear in the loading screen
        /// </summary>
        private string errors = "Listing errors:";

        /// <summary>
        /// Initializes a new instance of the SplashScreen class
        /// </summary>
        public SplashScreen()
        {
            this.number = 1;
            InitializeComponent();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetCallingAssembly();
            this.VersionLabel.Text = "I.C.E. version " + assembly.FullName.Split(',')[1].Split('=')[1] + " under " + App.Licence;
        }

        /// <summary>
        /// Gets the progress bar for ICE launching
        /// </summary>
        public ProgressBar ProgressBar
        {
            get { return this.progressBar; }
        }

        /// <summary>
        /// Add an error on the loading screen 
        /// </summary>
        /// <param name="strError">Error message</param>
        public void AddMessage(string strError)
        {
            if (this.number > 20)
            {
                int index = this.errors.IndexOf('\n');
                this.errors = this.errors.Substring(index + 1);
            }

            this.errors += "\nError: " + this.number + " " + strError;
            TextError.Text = this.errors;
            this.number++;
        }
    }
}