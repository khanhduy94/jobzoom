//-----------------------------------------------------------------------
// <copyright file="PhysicsConstants.cs" company="International Monetary Fund">
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

namespace ICE
{
    /// <summary>
    /// Here are grouped all constants used by the physical engine
    /// Modify values to change global behaviour
    /// </summary>
    public abstract class PhysicsConstants
    {
        /// <summary>
        /// Gravity for the all particule system, along the Y direction
        /// 0 means no gravity (floating particules)
        /// positive value will get particules dropping
        /// negative value will get particules flying
        /// </summary>
        public const float InitialGravityValue = 0f;

        /// <summary>
        /// Default mass for a particule
        /// </summary>
        public static readonly float ParticleDefaultMass = 1f;

        /// <summary>
        /// Max value for spring strength
        /// </summary>
        public static readonly float MaximalSpringStrength = 0.4f;

        /// <summary>
        /// Min value for spring strength
        /// </summary>
        public static readonly float MinimalSpringStrength = 0.05f;
      
        /// <summary>
        /// Default damping value of the spring
        /// The higher the value, the quicker the spring will stop quivering
        /// </summary>
        public static readonly float SpringDamping = 0.2f;

        /// <summary>
        /// Minimum Distance :
        /// To limit how strong an attraction force can be
        /// otherwise once two objects get too close they will never let go.
        /// This limits the distance in calculating the attraction force,
        /// but the force is always acting at all distances.
        /// </summary>
        public static readonly float AttractionEffectMinimalDistance = 20f;
    }
}
