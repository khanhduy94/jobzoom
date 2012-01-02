//-----------------------------------------------------------------------
// <copyright file="NodeType.cs" company="International Monetary Fund">
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

namespace ICE.model
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// This class represents a type of node
    /// </summary>
    public class NodeType
    {
        /// <summary>
        /// name of the current type.
        /// </summary>
        private string name;
       
        /// <summary>
        /// value indicating whether the type is enabled in the model or not
        /// </summary>
        private bool isEnable;

        /// <summary>
        /// value indicating whether the type is visible in the graph or not
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Initializes a new instance of the NodeType class.
        /// </summary>
        /// <param name="name">the name of the new type</param>
        public NodeType(string name)
        {
            this.name = name;
            this.isEnable = true;
            this.isVisible = true;
        }

        /// <summary>
        /// Gets the name of the type
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the type is enabled or not in the model
        /// </summary>
        public bool IsEnable
        {
            get { return this.isEnable; }
            set { this.isEnable = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the type is visible or not.
        /// </summary>
        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.isVisible = value; }
        }
    }
}
