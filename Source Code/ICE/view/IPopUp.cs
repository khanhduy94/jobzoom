//-----------------------------------------------------------------------
// <copyright file="IPopUp.cs" company="International Monetary Fund">
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// This interface provides all services needed by the view management system to use a pop-up
    /// </summary>
    public interface IPopUp
    {
        /// <summary>
        /// Event for closing the pop-up
        /// </summary>
        event MouseButtonEventHandler Close;

        /// <summary>
        /// Gets the container for the pop-up data
        /// </summary>
        Grid InfoContainer
        {
            get;
        }

        /// <summary>
        /// Gets or sets the color of the pop-up
        /// </summary>
        Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the pop-up
        /// </summary>
        Size Size
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the minimal size the pop-up will have when it appears/disappears
        /// </summary>
        Size MinimalSize
        {
            get;
        }
    }
}
