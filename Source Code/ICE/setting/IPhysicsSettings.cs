//-----------------------------------------------------------------------
// <copyright file="IPhysicsSettings.cs" company="International Monetary Fund">
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

namespace ICE.setting
{
    using System;

    /// <summary>
    /// this is the interface use to acces to physics parameters
    /// </summary>
    public interface IPhysicsSettings
    {
        /// <summary>
        /// This event is raised whether the settings has changed
        /// </summary>
        event EventHandler Changed;

        /// <summary>
        /// Gets the drag force
        /// </summary>
        float DragForce { get; }

        /// <summary>
        /// Gets the rest length of a link.
        /// </summary>
        float LinkRestLength { get; }

        /// <summary>
        /// Gets the gravity
        /// </summary>
        int Gravity { get; }

        /// <summary>
        /// Gets replution force strength
        /// </summary>
        float RepultionForce { get; }
    }
}