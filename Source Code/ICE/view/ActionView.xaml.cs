//-----------------------------------------------------------------------
// <copyright file="ActionView.xaml.cs" company="International Monetary Fund">
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
    using System.Xml.Linq;

    /// <summary>
    /// This class is the visual representation of an action
    /// </summary>
    public partial class ActionView : UserControl
    {
        /// <summary>
        /// This is the action to execute
        /// </summary>
        private action.Action currentAction;

        /// <summary>
        /// This is the target of our action
        /// </summary>
        private IEnumerable<action.IActionable> targets;

        /// <summary>
        /// Initializes a new instance of the ActionView class.
        /// </summary>
        /// <param name="targets">the target of the action</param>
        /// <param name="action">the action you want to represent</param>
        public ActionView(IEnumerable<action.IActionable> targets, action.Action action)
        {
            this.InitializeComponent();
            this.currentAction = action;
            this.targets = targets;

            // change the image brush
            this.image.ImageSource = action.IconSource;

            // set the tool tip
            ToolTipService.SetToolTip(this, action.Description);

            // subscribe to the click
            this.MouseLeftButtonUp += new MouseButtonEventHandler(this.ActionView_MouseLeftButtonUp);
        }

        /// <summary>
        /// This event is raised when the action has been executed by the user ( by a click )
        /// </summary>
        public event EventHandler Executed;

        /// <summary>
        /// Gets or sets the action to execute
        /// </summary>
        public action.Action Action
        {
            get { return this.currentAction; }
            set { this.currentAction = value; }
        }

        /// <summary>
        /// This function is called when the user asks to execute the action
        /// </summary>
        /// <param name="sender">the current view shape</param>
        /// <param name="e">the event args</param>
        private void ActionView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.currentAction.PerformAction(this.targets);
            if (this.Executed != null)
            {
                this.Executed(this, new EventArgs());
            }
        }
    }
}
