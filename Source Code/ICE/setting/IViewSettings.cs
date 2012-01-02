//-----------------------------------------------------------------------
// <copyright file="IViewSettings.cs" company="International Monetary Fund">
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
    using System.Windows;
    using System.Windows.Media;
    using ICE.view.visualEffect;

    /// <summary>
    /// this is the interface use to acces all view parameter
    /// </summary>
    public interface IViewSettings
    {
        /// <summary>
        /// This event is raised whether this setting page changed
        /// </summary>
        event EventHandler Changed;

        /// <summary>
        /// Gets the delegate that contains all visual effects called when scale rate is changed
        /// </summary>
        MainViewVisualEffectChange ChangeScale { get; }

        /// <summary>
        /// Gets the background of the graph
        /// </summary>
        FrameworkElement Background { get; }

        /// <summary>
        /// Gets actions's circle color
        /// </summary>
        Color ActionsCircleColor { get; }

        /// <summary>
        /// Gets the initial zoom ratio
        /// </summary>
        double InitialZoomRatio { get; }

        /// <summary>
        /// Gets the maximal thickness for a link
        /// </summary>
        double LinkMaximalThickness { get; }

        /// <summary>
        /// Gets the minimal thickness for a link
        /// </summary>
        double LinkMinimalThickness { get; }

        /// <summary>
        /// Gets the size ratio of all nodes
        /// </summary>
        double NodeSizeRatio { get; }

        /// <summary>
        /// Gets the step use to change the opacity of all object in the graph
        /// </summary>
        double OpacityChangeStep { get; }

        /// <summary>
        /// Gets a value indicating whether debug messages are enabled or disabled
        /// </summary>
        bool DebugMode { get; }

        /// <summary>
        /// Gets a value indicating whether the navigation bar sould or souldn't add to the view
        /// </summary>
        bool NavigationBarMode { get; }

        /// <summary>
        /// Gets a value indicating whether we must show or not the navigation menu
        /// </summary>
        bool NavigationMenuMode { get; }

        /// <summary>
        /// Gets a NodeStyle from its name
        /// </summary>
        /// <param name="name">The name of the style</param>
        /// <returns>
        /// The NodeStyle corresponding to the name
        /// If no NodeStyle matches the name, the default node style is returned
        /// </returns>
        NodeStyle GetNodeStyle(string name);

        /// <summary>
        /// Gets the Linkstyle from its name
        /// </summary>
        /// <param name="name">The name of the style</param>
        /// <returns>
        /// The LinkStyle corresponding to the name
        /// If no NodeStyle matches the name, the default node style is returned
        /// </returns>
        LinkStyle GetLinkStyle(string name);

        /// <summary>
        /// Gets a Action from its name
        /// </summary>
        /// <param name="name">The name of the action</param>
        /// <returns>
        /// The Action corresponding to the name
        /// If no Action matches the name, null is returned
        /// </returns>
        action.Action GetAction(string name);
    }
}
