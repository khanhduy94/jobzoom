//-----------------------------------------------------------------------
// <copyright file="ObjectFilterListItem.xaml.cs" company="International Monetary Fund">
//   This file is part of "Information Connections Engine". See more information at http://ICEdotNet.codeplex.com
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// This control is an item in the filter menu
    /// </summary>
    public partial class ObjectFilterListItem : UserControl
    {
        /// <summary>
        /// Value indicating whether the current object is in enable state or not.
        /// </summary>
        private bool isEnable = true;

        /// <summary>
        /// This is the field where the designated object type is stored.
        /// </summary>
        private object targetType;

        /// <summary>
        /// Initializes a new instance of the ObjectFilterListItem class.
        /// </summary>
        public ObjectFilterListItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is in enable state or not.
        /// </summary>
        public bool IsVisualyEnable
        {
            get 
            { 
                return this.isEnable; 
            }

            set 
            {
                this.isEnable = value;
                if (this.isEnable)
                {
                    this.label.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    this.label.Foreground = new SolidColorBrush(Colors.DarkGray);
                }
            }
        }

        /// <summary>
        /// Gets or sets the designated object type is stored.
        /// </summary>
        public object TargetType
        {
            get { return this.targetType; }
            set { this.targetType = value; }
        }
    }
}
