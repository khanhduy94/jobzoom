//-----------------------------------------------------------------------
// <copyright file="HidePopUpEffect.cs" company="International Monetary Fund">
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

// THIS IS A DEAD CODE

////namespace ICE.view.visualEffect
////{
////    using System.Windows;

////    /// <summary>
////    /// The class is used to create an animation when a pop-up is hidden
////    /// </summary>
////    public class HidePopUpEffect : AbstractVisualEffect
////    {
////        /// <summary>
////        /// Pop-up to hide
////        /// </summary>
////        private IPopUp popup;

////        /// <summary>
////        /// Number of steps for horizontal collapsing
////        /// </summary>
////        private int numberOfHorizontalStep;

////        /// <summary>
////        /// Number of steps for vertical collapsing
////        /// </summary>
////        private int numberOfVerticalStep;

////        /// <summary>
////        /// Current progression on horizontal collapsing
////        /// </summary>
////        /// <remarks>
////        /// Counted backward : starts at numberOfHorizontalStep and go to 0
////        /// </remarks>
////        private int currentHorizontalStep;

////        /// <summary>
////        /// Current progression on vertical collapsing
////        /// </summary>
////        /// <remarks>
////        /// Counted backward : starts at numberOfVerticalStep and go to 0
////        /// </remarks>
////        private int currentVerticalStep;

////        /// <summary>
////        /// Number by which to decrease the horizontal size at each step
////        /// </summary>
////        private double horizontalStep;

////        /// <summary>
////        /// Number by which to decrease the vertical size at each step
////        /// </summary>
////        private double verticalStep;

////        /// <summary>
////        /// Initializes a new instance of the HidePopUpEffect class
////        /// </summary>
////        /// <param name="viewManager">The view manager to be affected</param>
////        /// <param name="popup">Pop-up to hide</param>
////        /// <param name="numberOfHorizontalStep">Number of steps for horizontal collapsing</param>
////        /// <param name="numberOfVerticalStep">Number of steps for vertical collapsing</param>
////        public HidePopUpEffect(
////                ViewManager viewManager,
////                IPopUp popup,
////                int numberOfHorizontalStep,
////                int numberOfVerticalStep)
////            : base(viewManager)
////        {
////            // initializes fields
////            this.popup = popup;
////            this.numberOfHorizontalStep = numberOfHorizontalStep;
////            this.numberOfVerticalStep = numberOfVerticalStep;
////            this.currentHorizontalStep = numberOfHorizontalStep;
////            this.currentVerticalStep = numberOfVerticalStep;

////            // calculates the number by which to decrease the pop-up size at each step
////            // depending on initial size, final (minimal) size and number of steps
////            ////this.verticalStep =
////            ////    (viewManager.Settings.PopUpSize.Height - this.popup.MinimalSize.Height) / this.numberOfVerticalStep;
////            ////this.horizontalStep =
////            ////    (viewManager.Settings.PopUpSize.Width - this.popup.MinimalSize.Width) / this.numberOfHorizontalStep;
////        }

////        /// <summary>
////        /// Enacts the pop-up hiding animation
////        /// </summary>
////        public override void Execute()
////        {
////            // Initialisation, get the current pop-up size
////            if (this.currentHorizontalStep == this.numberOfHorizontalStep && this.currentVerticalStep == this.numberOfVerticalStep)
////            {
////                //this.popup.Size = ViewManager.Settings.PopUpSize;
////            }

////            if (this.currentVerticalStep > 0)
////            {
////                // First : decrease vertical size
////                // keeps the initial width of the pop-up
////                //Size size = new Size(ViewManager.Settings.PopUpSize.Width, this.popup.MinimalSize.Height + (this.verticalStep * this.currentVerticalStep));
////                //this.popup.Size = size;
////                //this.currentVerticalStep--;
////            }
////            else
////            {
////                // Second : decrease horizontal size
////                // keeps the minimal height of the pop-up
////                //Size size = new Size(this.popup.MinimalSize.Width + (this.horizontalStep * this.currentHorizontalStep), this.popup.MinimalSize.Height);
////                //this.popup.Size = size;
////                //this.currentHorizontalStep--;
////            }

////            // Ends visual effect
////            if (this.currentVerticalStep <= 0 && this.currentHorizontalStep <= 0)
////            {
////                this.popup.Size = this.popup.MinimalSize;

////                // calling Dispose() to free the visual effect resources
////                Dispose();
////            }
////        }

////        /// <summary>
////        /// Ends the pop-up hiding
////        /// </summary>
////        /// <remarks>
////        /// Calling Dispose() to free the visual effect resources
////        /// </remarks>
////        public override void End()
////        {
////            this.popup.Size = this.popup.MinimalSize;
////            Dispose();
////        }
////    }
////}
