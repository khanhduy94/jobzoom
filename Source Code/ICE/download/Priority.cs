//-----------------------------------------------------------------------
// <copyright file="Priority.cs" company="International Monetary Fund">
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
    /// <summary>
    /// This enumeration is the list of all existing download priority.
    /// </summary>
    public enum Priority
    {
        /// <summary>
        /// This value is used whether download is vital to the application.
        /// </summary>
        Absolute = 0,

        /// <summary>
        /// This value is used whether download is really important to the application.
        /// </summary>
        Important = 1,

        /// <summary>
        /// This value is used whether download is required to the application.
        /// </summary>
        Required = 2,

        /// <summary>
        /// This value is used whether download could enhance the application.
        /// </summary>
        Desired = 3
    }
}
