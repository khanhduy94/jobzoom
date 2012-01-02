//-----------------------------------------------------------------------
// <copyright file="UserNavigationBar.xaml.cs" company="International Monetary Fund">
//
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
//      Lorenzin Aurélia,
//      Poirot Clément,
//      Mayer Nicolas,
//      Transler Jean-Christophe,
// </authors>
// <context>
//      Industrial Project realized at ESIAL (Ecole Supérieure d'Informatique et Applications de Lorraine)
//      for the benefit of the the Open Source Community
// </context>
// <supervisors>
//      Hervé Tourpe (industry supervisor)
//      Suzanne Collin (university supervisor)
// </supervisors>
// <years>2008 - 2009</years>
// <contributors>
//      <!-- any contributors (except for authors) to this file should be listed here -->
// </contributors>
//-----------------------------------------------------------------------

namespace ICE.view
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    
    /// <summary>
    /// This class is the user navigation bar. 
    /// </summary>
    public partial class UserNavigationBar : UserControl
    {
        /// <summary>
        /// Pointer to the view management system
        /// </summary>
        private ViewManager viewManager;

        /// <summary>
        /// Initializes a new instance of the UserNavigationBar class.
        /// </summary>
        /// <param name="viewManager">
        /// The view management system used to enact commands from the user bar
        /// </param>
        public UserNavigationBar(ViewManager viewManager)
        {
            InitializeComponent();
            this.viewManager = viewManager;
            this.slidezoom.Minimum = -20;
            this.slidezoom.Maximum = 20;
           //// this.slidezoom.Value = 19;
            this.slidezoom.LargeChange = 3;
            this.slidezoom.SmallChange = 1;
           //// this.slidezoom.sm
        }
        
        /// <summary>
        /// This function is called when the user click on the "plus" button
        /// Zooms in
        /// </summary>
        /// <param name="sender">The "plus" button</param>
        /// <param name="e">The routed event arguments</param>
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.ZoomIn();
        }

        /// <summary>
        /// This function is called when the user click on the "minus" button
        /// Zooms out
        /// </summary>
        /// <param name="sender">The "minus" button</param>
        /// <param name="e">The routed event arguments</param>
        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.ZoomOut();
        }

        /// <summary>
        /// This function is called when the user click on the "center" button
        /// Centers view on next selected node
        /// </summary>
        /// <param name="sender">The "center" button</param>
        /// <param name="e">The routed event arguments</param>
        private void Center_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.GoToSelectedNode();
        }

        /// <summary>
        /// This function is called when the user click on the "up" button
        /// Moves view up
        /// </summary>
        /// <param name="sender">The "up" button</param>
        /// <param name="e">The routed event arguments</param>
        private void Up_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.GoUp();
        }

        /// <summary>
        /// This function is called when the user click on the "down" button
        /// Moves view down
        /// </summary>
        /// <param name="sender">The "down" button</param>
        /// <param name="e">The routed event arguments</param>
        private void Down_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.GoDown();
        }

        /// <summary>
        /// This function is called when the user click on the "left" button
        /// Moves view left
        /// </summary>
        /// <param name="sender">The "left" button</param>
        /// <param name="e">The routed event arguments</param>
        private void Left_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.GoLeft();
        }

        /// <summary>
        /// This function is called when the user click on the "right" button
        /// Moves view right
        /// </summary>
        /// <param name="sender">The "right" button</param>
        /// <param name="e">The routed event arguments</param>
        private void Right_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.GoRight();
        }

        /// <summary>
        /// This function is called when the user click on the "increase depth" button
        /// Shows deeper nodes
        /// </summary>
        /// <param name="sender">The "increase depth" button</param>
        /// <param name="e">The routed event arguments</param>
        private void IncreaseDepth_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.RaiseIncreaseDepth();
        }

        /// <summary>
        /// This function is called when the user click on the "decrease depth" button
        /// Hides the deepest nodes
        /// </summary>
        /// <param name="sender">The "decrease depth" button</param>
        /// <param name="e">The routed event arguments</param>
        private void DecreaseDepth_Click(object sender, RoutedEventArgs e)
        {
            this.viewManager.RaiseDecreaseDepth();
        }

        /// <summary>
        /// This function is called when the user use the slider bar for zoom
        /// Change zoom to new value
        /// </summary>
        /// <param name="sender">The zoom slider bar</param>
        /// <param name="e">The routed property change event arguments</param>
        private void Slidezoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int valeur = (int)e.NewValue;
            //// TODO : why +20 ?
            valeur += 20;
            System.Diagnostics.Debug.WriteLine("usernav : " + valeur);
            this.viewManager.Zoom(valeur);
        }

        /// <summary>
        /// This function is called when the slider bar for zoom get the focus
        /// Attachs the handler to detect changes on the slider value
        /// </summary>
        /// <param name="sender">The zoom slider bar</param>
        /// <param name="e">The routed event arguments</param>
        private void Slidezoom_GotFocus(object sender, RoutedEventArgs e)
        {
            this.slidezoom.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.Slidezoom_ValueChanged);
        }

        /// <summary>
        /// This function is called when the slider bar for zoom lose the focus
        /// Detachs the handler to detect changes on the slider value
        /// </summary>
        /// <param name="sender">The zoom slider bar</param>
        /// <param name="e">The routed event arguments</param>
        private void Slidezoom_LostFocus(object sender, RoutedEventArgs e)
        {
            this.slidezoom.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.Slidezoom_ValueChanged);
        }
    }
}
