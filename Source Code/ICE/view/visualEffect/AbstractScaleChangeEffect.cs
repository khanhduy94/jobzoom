//-----------------------------------------------------------------------
// <copyright file="AbstractScaleChangeEffect.cs" company="International Monetary Fund">
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
// <contributors>
//      <!-- any contributors (except for authors) to this file should be listed here -->
// </contributors>
// <years>2008 - 2009</years>
//-----------------------------------------------------------------------

namespace ICE.view.visualEffect
{
    /// <summary>
    /// The abstract class is used to create an animation for a progressive scale change
    /// </summary>
    public abstract class AbstractScaleChangeEffect : AbstractVisualEffect
    {
        /// <summary>
        /// Final scale value
        /// </summary>
        private double finalScaleValue;

        /// <summary>
        /// Initializes a new instance of the AbstractScaleChangeEffect class
        /// </summary>
        /// <param name="viewManager">The view manager to be affected</param>
        /// <param name="finalScaleValue">The value wanted for the scale</param>
        public AbstractScaleChangeEffect(ViewManager viewManager, double finalScaleValue)
            : base(viewManager)
        {
            this.finalScaleValue = finalScaleValue;
        }

        /// <summary>
        /// Gets the final scale vaule
        /// </summary>
        public double FinalScaleValue
        {
            get { return this.finalScaleValue; }
        }

        /// <summary>
        /// Ends the scale change
        /// </summary>
        /// <remarks>
        /// Must be called to enact the transformation
        /// </remarks>
        public override void End()
        {
            Dispose();
        }
    }
}
