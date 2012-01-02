//-----------------------------------------------------------------------
// <copyright file="IPopUpPointer.cs" company="International Monetary Fund">
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

namespace ICE.view
{
    using System;

    /// <summary>
    /// this is this interface of a PopUpPointer
    /// </summary>
    public interface IPopUpPointer
    {
        /// <summary>
        /// Gets or sets the point of the pointer's head 
        /// </summary>
        System.Windows.Point HeadPoint { get; set; }

        /// <summary>
        /// Gets or sets the first point of the pointer's tail 
        /// </summary>
        System.Windows.Point TailPointA { get; set; }

        /// <summary>
        /// Gets or sets the second point of the pointer's tail 
        /// </summary>
        System.Windows.Point TailPointB { get; set; }
    }
}
