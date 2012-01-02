//-----------------------------------------------------------------------
// <copyright file="Attraction.cs" company="International Monetary Fund">
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
    /// This class handles attraction force between two particules
    /// Attraction is defined by its strength, the minimal distance under which strength is limited
    /// the two end particules, and whether it is active or not
    /// </summary>
    public class Attraction : Force
    {
        #region Fields

        /// <summary>
        /// Attraction force between particules at two ends
        /// Negative value means repulsion
        /// </summary>
        private float strength;

        /// <summary>
        /// Minimum Distance :
        /// In practice, we want to place a limit on how strong a force can be
        /// otherwise once two objects get too near they will never let go
        /// This limits the distance in calculating the above force,
        /// but the force is always acting at all distances.
        /// </summary>
        private float minDist;

        #endregion

        /// <summary>
        /// Initializes a new instance of the Attraction class
        /// </summary>
        /// <param name="end1">Particule at first end of the spring</param>
        /// <param name="end2">Particule at second end of the spring</param>
        /// <param name="k">Attraction strength</param>
        /// <param name="d">Minimal distance under which attraction is restricted</param>
        public Attraction(Particle end1, Particle end2, float k, float d) : base()
        {
            this.strength = k;
            this.minDist = d;
            this.End1 = end1;
            this.End2 = end2;
        }

        /// <summary>
        /// Gets or sets attraction strength
        /// </summary>
        public float Strength
        {
            get { return this.strength; }
            set { this.strength = value; }
        }

        /// <summary>
        /// Gets or sets the minimum distance, which limits how strong the force can get close up
        /// </summary>
        public float MinDist
        {
            get { return this.minDist; }
            set { this.minDist = value; }
        }

        /// <summary>
        /// Calculate next state of attraction force depending on its ends' positions
        /// Apply the attaction force to both ends.
        /// </summary>
        public override void Apply()
        {
            if (this.IsOn())
            {
                // Calculate distance between ends over the 3 dimensions
                float distX = End1.Position.X - End2.Position.X;
                float distY = End1.Position.Y - End2.Position.Y;
                float distZ = End1.Position.Z - End2.Position.Z;

                // Calculate distance between two ends of spring
                // Thats : sqrt( (x1-x2)² + (y1-y2)² + (z1-z2)² )
                // For optimisation we first calculate 1 / sqrt( (x1-x2)² + (y1-y2)² + (z1-z2)² )
                // Then we calculte the inverse
                float oneOverDist = Arithmetic.FastInverseSqrt((distX * distX) + (distY * distY) + (distZ * distZ));
                float dist = 1.0F / oneOverDist;

                // Distance calculation is fast but not very precise, so : 
                if (dist == 0.0F)
                {
                    // if actual distance distance is approximatively null, do nothing
                }
                else
                {
                    // else :
                    // normalize the distance coordinate by coordinate with global distance
                    distX *= oneOverDist;
                    distY *= oneOverDist;
                    distZ *= oneOverDist;

                    // First part of the calculation for the attraction force
                    float force = this.strength;

                    // If distance is smaller thant set minum,
                    if (dist < this.minDist)
                    {
                        // limit the strength of attraction force
                        force /= this.minDist * this.minDist;
                    }
                    else
                    {
                        // else, apply usual formula
                        force *= oneOverDist * oneOverDist;
                    }

                    // Correct distances depending on force
                    distX *= force;
                    distY *= force;
                    distZ *= force;

                    // Add the a new force to both ends
                    End1.Force.Add(-distX, -distY, -distZ);
                    End2.Force.Add(distX, distY, distZ);

                // if (dist == 0.0F)
                }

            // if(this.IsOn())
            }

        // public void Apply()
        }

    // public class Attraction:Force
    }

// namespace ICE.physics
}
