//-----------------------------------------------------------------------
// <copyright file="Spring.cs" company="International Monetary Fund">
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
    using System;
    using ICE.mathematics;

    /// <summary>
    /// This class handles springs seens has forces between two particules
    /// A spring is defined by its strength, its dampling, its rest length
    /// the two end particules, and whether it is active or not
    /// </summary>
    public class Spring : Force
    {
        #region Fields

        /// <summary>
        /// Spring strength :
        /// the higher the value, the harder the spring
        /// </summary>
        private float springConstant;

        /// <summary>
        /// Damping value of spring :
        /// the higher the value, the quicker the spring will stop quivering
        /// </summary>
        private float damping;

        /// <summary>
        /// Rest lenght of spring
        /// </summary>
        private float restLength;

        #endregion

        /// <summary>
        /// Initializes a new instance of the Spring class
        /// </summary>
        /// <param name="end1">Particule at first end of the spring</param>
        /// <param name="end2">Particule at second end of the spring</param>
        /// <param name="sk">Spring strength</param>
        /// <param name="d">Damping value of spring</param>
        /// <param name="rl">Rest lenght of spring</param>
        public Spring(Particle end1, Particle end2, float sk, float d, float rl) : base()
        {
            this.springConstant = sk;
            this.damping = d;
            this.restLength = rl;
            this.End1 = end1;
            this.End2 = end2;
        }

        #region Properties

        /// <summary>
        /// Gets or sets rest lenght
        /// </summary>
        public float RestLength
        {
            get { return this.restLength; }
            set { this.restLength = value; }
        }

        /// <summary>
        /// Gets or sets strength
        /// </summary>
        public float Strength
        {
            get { return this.springConstant; }
            set { this.springConstant = value; }
        }

        /// <summary>
        /// Gets or sets damping
        /// </summary>
        public float Damping
        {
            get { return this.damping; }
            set { this.damping = value; }
        }

        #endregion

        #region Functions
        
        /// <summary>
        /// Get actual length of spring,
        /// (that can be different from rest length)
        /// </summary>
        /// <returns>Distance between two ends of spring</returns>
        public float CurrentLength()
        {
            return this.End1.Position.DistanceTo(this.End2.Position);
        }

        /// <summary>
        /// Calculate next state of spring depending on its ends' positions
        /// Apply the spring force to both ends.
        /// </summary>
        public override void Apply()
        {
            if (this.IsOn())
            {
                // Calculate distance between ends over the 3 dimensions
                float distX = this.End1.Position.X - this.End2.Position.X;
                float distY = this.End1.Position.Y - this.End2.Position.Y;
                float distZ = this.End1.Position.Z - this.End2.Position.Z;

                // Calculate distance between two ends of spring
                // Thats : sqrt( (x1-x2)² + (y1-y2)² + (z1-z2)² )
                // For optimisation we first calculate 1 / sqrt( (x1-x2)² + (y1-y2)² + (z1-z2)² )
                // Then we calculte the inverse
                float oneOverDist = Arithmetic.FastInverseSqrt((distX * distX) + (distY * distY) + (distZ * distZ));
                float dist = 1.0F / oneOverDist;

                // Distance calculation is fast but not very precise, so :                
                if (dist == 0.0F)
                {
                    // if actual distance is approximatively null, then distance is set to null for next step
                    distX = 0.0F;
                    distY = 0.0F;
                    distZ = 0.0F;
                }
                else
                {
                    // else normalize the distance coordinate by coordinate with global distance
                    distX *= oneOverDist;
                    distY *= oneOverDist;
                    distZ *= oneOverDist;
                }

                // Calculate the force of the spring (with usual physical formula)
                float springForce = this.springConstant * (this.restLength - dist);

                // Calculate velocity differences between ends over the 3 dimensions
                float speedX = this.End1.Velocity.X - this.End2.Velocity.X;
                float speedY = this.End1.Velocity.Y - this.End2.Velocity.Y;
                float speedZ = this.End1.Velocity.Z - this.End2.Velocity.Z;

                // Calculate the damping force of the spring (with usual physical formula)
                float dampingForce = -this.damping * ((distX * speedX) + (distY * speedY) + (distZ * speedZ));

                // Resultant of forces
                float r = springForce + dampingForce;

                // Corrects distances depending on forces
                distX *= r;
                distY *= r;
                distZ *= r;

                // If end1 is able to move, adds the a new force to it
                if (this.End1.IsFree())
                {
                    this.End1.Force.Add(distX, distY, distZ);
                }

                // If end1 is able to move, adds the a new force to it
                if (this.End2.IsFree())
                {
                    this.End2.Force.Add(-distX, -distY, -distZ);
                }

            // if (this.IsOn())
            }

        // public void Apply()
        }

        #endregion

    // public class Spring : Force
    }

// namespace ICE.physics
}