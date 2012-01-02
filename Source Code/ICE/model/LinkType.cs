//-----------------------------------------------------------------------
// <copyright file="LinkType.cs" company="International Monetary Fund">
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
    /// This class represents the a type of link
    /// </summary>
    public class LinkType
    {
        /// <summary>
        /// name of the current type.
        /// </summary>
        private string verb;

        /// <summary>
        /// type of the origine node
        /// </summary>
        private string from;

        /// <summary>
        /// type of the end node
        /// </summary>
        private string to;
       
        /// <summary>
        /// value indicating whether the type is enabled in the model or not
        /// </summary>
        private bool isEnable;

        /// <summary>
        /// value indicating whether the type is visible in the graph or not
        /// </summary>
        private bool isVisible;

        /// <summary>
        /// Initializes a new instance of the LinkType class.
        /// </summary>
        /// <param name="verb">the verb of the link</param>
        /// <param name="from">the "From" node type name</param>
        /// <param name="to">the "To" node type name</param>
        public LinkType(string verb, string from, string to)
        {
            this.verb = verb;
            this.from = from;
            this.to = to;
            this.isEnable = true;
            this.isVisible = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the type is visible or not.
        /// </summary>
        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.isVisible = value; }
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
        /// Gets the description of the current type
        /// </summary>
        public string Name
        {
            get { return this.verb + " (" + this.from + " -> " + this.to + ")"; }
        }

        /// <summary>
        /// This function tests if the current type is related to the type in argument
        /// </summary>
        /// <param name="type">type that could be related to the current type</param>
        /// <returns>true if the type in argument is a part of the current type</returns>
        public bool IsRelatedTo(NodeType type)
        {
            return (type.Name == this.from) || (type.Name == this.to);
        }
    }
}
