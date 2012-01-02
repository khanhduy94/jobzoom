//-----------------------------------------------------------------------
// <copyright file="ProportionalScaleChangeEffect.cs" company="International Monetary Fund">
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

namespace ICE.view.visualEffect
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The class is used to create an animation for a proportional scale change
    /// </summary>
    public class ProportionalScaleChangeEffect : AbstractScaleChangeEffect
    {
        /// <summary>
        /// Current progression on resizing
        /// </summary>
        /// <remarks>
        /// Counted backward : starts at numberOfIntermediateValue and go to 0
        /// </remarks>
        private int numberOfIntermediateValueLeft;

        /// <summary>
        /// Number by which to resize the node at each step, in percent
        /// </summary>
        private double proportionAffectPerFrameInPercent;

        /// <summary>
        /// Initializes a new instance of the ProportionalScaleChangeEffect class
        /// </summary>
        /// <param name="viewManager">The view manager to be affected</param>
        /// <param name="finalScaleValue">Final scale wanted</param>
        /// <param name="proportionAffectPerFrameInPercent">Number by which to resize the node at each step, in percent</param>
        /// <param name="numberOfIntermediateValue">The number of steps for the size change</param>
        public ProportionalScaleChangeEffect(
                ViewManager viewManager,
                double finalScaleValue,
                double proportionAffectPerFrameInPercent,
                int numberOfIntermediateValue)
            : base(viewManager, finalScaleValue)
        {
            this.numberOfIntermediateValueLeft = numberOfIntermediateValue;
            this.proportionAffectPerFrameInPercent = proportionAffectPerFrameInPercent;
        }

        /// <summary>
        /// Execute one step of the scaling
        /// </summary>
        public override void Execute()
        {
            // TODO understand method
            ViewManager.CurrentZoomRatio += (FinalScaleValue - ViewManager.CurrentZoomRatio) * this.proportionAffectPerFrameInPercent * 0.01;

            this.numberOfIntermediateValueLeft--;

            // if the resizing is finished, dispose of the visual effect
            if (this.numberOfIntermediateValueLeft <= 0)
            {
                Dispose();
            }
        }
    }
}
