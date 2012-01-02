//-----------------------------------------------------------------------
// <copyright file="IModelSettings.cs" company="International Monetary Fund">
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
    /// This interface is used to access to all parameter related to the model management
    /// </summary>
    public interface IModelSettings
    {
        /// <summary>
        /// This event is raised whether the settings has changed
        /// </summary>
        event EventHandler Changed;

        /// <summary>
        /// Gets the initial depth of exploration use to set node's visibility
        /// </summary>
        int InitialGraphVisibilityDepth { get; }

        /// <summary>
        /// Gets the limit of node in the model 
        /// </summary>
        int MaximumNodeLimit { get; }

        /// <summary>
        /// Gets the additional value of the depth use to know which part of the model should be suppressed.
        /// To know the current depth of exploration for the cleanup process, just add this value to the current visibility depth
        /// </summary>
        int CleanUpAdditionalDepth { get; }

        /// <summary>
        /// Gets a value indicating whether multi-selection is enabled or disabled
        /// </summary>
        bool MultiSelectionNodeMode { get; }
    }
}
