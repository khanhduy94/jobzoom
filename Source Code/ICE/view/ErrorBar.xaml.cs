//-----------------------------------------------------------------------
// <copyright file="ErrorBar.xaml.cs" company="International Monetary Fund">
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
    using System.Windows.Input;

    /// <summary>
    /// This class handles errors display when ICE is running
    /// </summary>
    public partial class ErrorBar : UserControl
    {
        /// <summary>
        /// Number of errors
        /// </summary>
        private int number;

        /// <summary>
        /// Initializes a new instance of the ErrorBar class
        /// </summary>
        public ErrorBar()
        {
            InitializeComponent();
            TextError.MouseLeftButtonUp += new MouseButtonEventHandler(this.LeftMouseButtonUpAction);
            this.number = 1;
        }

        /// <summary>
        /// Add an error on the loading screen 
        /// </summary>
        /// <param name="strError">The error message</param>
        public void AddMessage(string strError)
        {
            TextError.Text = "Error: " + this.number + " " + strError;
            this.number++;
        }

        /// <summary>
        /// This function sets the left button down action
        /// </summary>
        /// <param name="sender">The element</param>
        /// <param name="e">The mouse event args</param>
        private void LeftMouseButtonUpAction(object sender, MouseButtonEventArgs e)
        {
            TextError.Text = string.Empty;
        }
    }
}
