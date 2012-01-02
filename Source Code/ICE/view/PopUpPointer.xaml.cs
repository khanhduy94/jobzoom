//-----------------------------------------------------------------------
// <copyright file="PopUpPointer.xaml.cs" company="International Monetary Fund">
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
    /// this class is an implementation of a popup pointer
    /// </summary>
    public partial class PopUpPointer : UserControl, ICE.view.IPopUpPointer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PopUpPointer class
        /// </summary>
        public PopUpPointer()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the first point of the pointer's tail 
        /// </summary>
        public Point TailPointA
        {
            get
            {
                return new Point(this.pathFigure.StartPoint.X, this.pathFigure.StartPoint.Y);
            }

            set
            {
                this.pathFigure.StartPoint = value;
            }
        }

        /// <summary>
        /// Gets or sets the second point of the pointer's tail 
        /// </summary>
        public Point TailPointB
        {
            get
            {
                return new Point(this.segment2.Point.X, this.segment2.Point.Y);
            }

            set
            {
                this.segment2.Point = value;
            }
        }

        /// <summary>
        /// Gets or sets the point of the pointer's head 
        /// </summary>
        public Point HeadPoint
        {
            get
            {
                return new Point(this.segment1.Point.X, this.segment1.Point.Y);
            }

            set
            {
                this.segment1.Point = value;
            }
        }

        #endregion
    }
}
