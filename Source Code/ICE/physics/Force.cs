//-----------------------------------------------------------------------
// <copyright file="Force.cs" company="International Monetary Fund">
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
// <origins>
//   this file was inspired from TRAER PHYSICS engine implementation 
//   (traer.physics.net, license GNU/GPL, Priceton University, see also http://www.cs.princeton.edu/~traer/physics/)
// </origins>
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

namespace ICE.physics
{
    /// <summary>
    /// This interface represents forces that can apply on particules
    /// Force id defined by two particules that interract
    /// and whether it is active or not
    /// </summary>
    public abstract class Force
    {
        #region Fields

        /// <summary>
        /// Particule at first end of the force
        /// </summary>
        private Particle end1;

        /// <summary>
        /// Particule at second end of the force
        /// </summary>
        private Particle end2;

        /// <summary>
        /// Define whether the force is active or not
        /// </summary>
        private bool on;

        /// <summary>
        /// Define whether the force is disposed or not
        /// </summary>
        private bool isDisposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Force class.
        /// </summary>
        public Force()
        {
            this.on = true;
            this.isDisposed = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the force is enabled or not
        /// </summary>
        public bool IsEnable
        {
            get { return this.on; }
            set { this.on = value; }
        }

        /// <summary>
        /// Gets or sets first end of the force
        /// </summary>
        protected Particle End1
        {
            get { return this.end1; }
            set { this.end1 = value; }
        }

        /// <summary>
        /// Gets or sets second end of the force
        /// </summary>
        protected Particle End2
        {
            get { return this.end2; }
            set { this.end2 = value; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Inactivates force
        /// </summary>
        public void TurnOff()
        {
            this.on = false;
        }

        /// <summary>
        /// Activates force
        /// </summary>
        public void TurnOn()
        {
            this.on = true;
        }

        /// <summary>
        /// Test whether the force is active
        /// </summary>
        /// <returns>
        /// True if force active
        /// False otherwise
        /// </returns>
        public bool IsOn()
        {
            return this.on && this.end1.IsEnable && this.end2.IsEnable;
        }

        /// <summary>
        /// Test whether the force is inactive
        /// </summary>
        /// <returns>
        /// True if force inactive
        /// False otherwise
        /// </returns>
        public bool IsOff()
        {
            return !(this.on && this.end1.IsEnable && this.end2.IsEnable);
        }

        /// <summary>
        /// Get first end of force
        /// </summary>
        /// <returns>Particule at first end of force</returns>
        public Particle GetOneEnd()
        {
            return this.end1;
        }

        /// <summary>
        /// Get second end of force
        /// </summary>
        /// <returns>Particule at second end of force</returns>
        public Particle GetTheOtherEnd()
        {
            return this.end2;
        }

        /// <summary>
        /// Test whether a force is still valid
        /// </summary>
        /// <returns>
        /// True if one of the ends is dead
        /// False if both of its ends are still valid (not disposed)
        /// </returns>
        public bool IsDisposed()
        {
            return this.isDisposed || this.end1.IsDisposed() || this.end2.IsDisposed();
        }

        /// <summary>
        /// Dispose the force and release all particles from this force
        /// </summary>
        public void Dispose()
        {
            this.isDisposed = true;
        }

        /// <summary>
        /// Calculate next state of force
        /// </summary>
        public abstract void Apply();

        #endregion
    }
}
