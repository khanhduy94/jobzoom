//-----------------------------------------------------------------------
// <copyright file="ActionMenu.xaml.cs" company="International Monetary Fund">
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
    /// This class is a menu of actions
    /// </summary>
    public partial class ActionMenu : UserControl
    {
        /// <summary>
        /// define whether the menu is opened or not
        /// </summary>
        private bool isOpen = false;

        /// <summary>
        /// define whether the menu is opening or not
        /// </summary>
        private bool isOpening = false;

        /// <summary>
        /// define whether the menu is closing or not
        /// </summary>
        private bool isClosing = false;

        /// <summary>
        /// Initializes a new instance of the ActionMenu class.
        /// </summary>
        public ActionMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This event is raised whether the menu ended closing
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// This event is raised whether the menu ended opening
        /// </summary>
        public event EventHandler Opened;

        /// <summary>
        /// Sets the color.
        /// </summary>
        public Color Color
        {
            set { this.cilclePanel.Color = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the menu is open.
        /// </summary>
        public bool IsOpen
        {
            get { return this.isOpen; }
        }

        /// <summary>
        /// Gets a value indicating whether the menu is opening.
        /// </summary>
        public bool IsOpening
        {
            get { return this.isOpening; }
        }

        /// <summary>
        /// Gets a value indicating whether the menu is closing.
        /// </summary>
        public bool IsClosing
        {
            get { return this.isClosing; }
        }

        /// <summary>
        /// This function adds the action to the menu
        /// </summary>
        /// <param name="actionView">a visual action representration</param>
        public void AddActionView(ActionView actionView)
        {
            ScaleTransform st = new ScaleTransform();
            actionView.RenderTransform = st;
            actionView.Measure(new Size(double.MaxValue, double.MaxValue));
            st.CenterX = actionView.DesiredSize.Width / 2;
            st.CenterY = actionView.DesiredSize.Height / 2;

            if (!this.isOpen)
            {
                st.ScaleX = 0;
                st.ScaleY = 0; 
            }

            actionView.Executed += new EventHandler(this.ActionView_Executed);
            actionView.MouseEnter += new MouseEventHandler(this.ActionView_MouseEnter);
            actionView.MouseLeave += new MouseEventHandler(this.ActionView_MouseLeave);

            cilclePanel.Children.Add(actionView);
        }

        /// <summary>
        /// this function opens the action menu
        /// </summary>
        public void Open()
        {
            if (this.isOpen)
            {
                return;
            }

            this.isOpening = true;
            this.closingStoryboard.Stop();
            this.openingStoryboard.Begin();
        }

        /// <summary>
        /// this function closes the action menu
        /// </summary>
        public void Close()
        {
            this.openedStoryboard.Stop();
            this.openingStoryboard.Stop();
            this.closingStoryboard.Begin();
            this.isOpen = false;
            this.isClosing = true;
        }

        /// <summary>
        /// this function pause the rotation of all actions 
        /// </summary>
        /// <param name="sender">an action view</param>
        /// <param name="e">the event argument</param>
        private void ActionView_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.isOpen)
            {
                this.openedStoryboard.Pause();
            }
        }

        /// <summary>
        /// this function is called when one of the action has been executed
        /// </summary>
        /// <param name="sender">action view</param>
        /// <param name="e">the event argument</param>
        private void ActionView_Executed(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// this function resume the rotation of all actions
        /// </summary>
        /// <param name="sender">an action view</param>
        /// <param name="e">the event argument</param>
        private void ActionView_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.isOpen)
            {
                this.openedStoryboard.Resume();
            }
        }

        /// <summary>
        /// Ensure the rotation of all actions
        /// </summary>
        /// <param name="sender">the rotation storyboard</param>
        /// <param name="e">the event argument</param>
        private void OpenedStoryboard_Completed(object sender, EventArgs e)
        {
            openedStoryboard.Begin();
        }

        /// <summary>
        /// begin the rotation of all actions
        /// </summary>
        /// <param name="sender">the opening storyboard</param>
        /// <param name="e">the event argument</param>
        private void OpeningStoryboard_Completed(object sender, EventArgs e)
        {
            this.isOpening = false;
            this.openedStoryboard.Begin();
            this.isOpen = true;
            if (this.Opened != null)
            {
                this.Opened(this, new EventArgs());
            }
        }

        /// <summary>
        /// Stop the rotation and begin the close animation
        /// </summary>
        /// <param name="sender">the closing storyboard</param>
        /// <param name="e">the event argument</param>
        private void ClosingStoryboard_Completed(object sender, EventArgs e)
        {
            this.isClosing = false;
            if (this.Closed != null)
            {
                this.Closed(this, new EventArgs());
            }
        }
    }
}
