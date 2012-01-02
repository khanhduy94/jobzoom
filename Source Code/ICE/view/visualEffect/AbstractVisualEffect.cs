//-----------------------------------------------------------------------
// <copyright file="AbstractVisualEffect.cs" company="International Monetary Fund">
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
    using System;

    /// <summary>
    /// This class is an abstract representation of a visual effect
    /// </summary>
    public abstract class AbstractVisualEffect : IVisualEffect
    {
        /// <summary>
        /// The view manager to be affected by the visual effect
        /// </summary>
        private ViewManager viewManager;

        /// <summary>
        /// Initializes a new instance of the AbstractVisualEffect class
        /// </summary>
        /// <remarks>
        /// This constructor MUST BE CALL in ALL inheriting classes
        /// </remarks>
        /// <param name="viewManager">The view manager to be affected by the visual effect</param>
        public AbstractVisualEffect(ViewManager viewManager)
        {
            this.viewManager = viewManager;
        }

        /// <summary>
        /// Event warning that the visual effect is ended
        /// </summary>
        public event EventHandler Ended;

        /// <summary>
        /// Gets the view manager to be affected by the visual effect
        /// </summary>
        protected ViewManager ViewManager
        {
            get { return this.viewManager; }
        }

        /// <summary>
        /// This function activates the visual effect
        /// </summary>
        public void Begin()
        {
            this.viewManager.VisualEffectList.Add(this);
        }

        /// <summary>
        /// This function applies all the modifications related to current visual effect
        /// </summary>
        /// <remarks>
        /// This function is called at each frame, so to apply a progressive visual effect you must create a progressive implementation.
        /// At the end of your visual effect, you MUST call the Dispose function to release ressouces.
        /// </remarks>
        public abstract void Execute();

        /// <summary>
        /// This function disposes of the visual effect for the visual manager
        /// </summary>
        public void Dispose()
        {
            this.viewManager.VisualEffectList.Remove(this);
            if (this.Ended != null)
            {
                this.Ended(this, new EventArgs());
            }
        }
        
        /// <summary>
        /// This function make the visual effect go to it final state.
        /// It could be call to preserve CPU ressources.
        /// </summary>
        /// <remarks>
        /// At the end of this function you MUST call the Dispose function
        /// </remarks>
        public abstract void End();
    }
}
