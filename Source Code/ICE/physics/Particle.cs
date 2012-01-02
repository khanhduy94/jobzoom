//-----------------------------------------------------------------------
// <copyright file="Particle.cs" company="International Monetary Fund">
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
    /// This class represents particles :
    /// Particles can represent objects, corners of 2D or 3D shapes or abstract things that won't even be drawn.
    /// They have four properties : mass, position, velocity and age and can be either fixed or free moving
    /// </summary>
    public class Particle
    {
        #region Fields
        /// <summary>
        /// Mass of particle
        /// </summary>
        private float mass;

        /// <summary>
        /// Age of particle
        /// </summary>
        private float age;

        /// <summary>
        /// Define whether the particle id disposed or not
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Define whether the particle is free to move or not
        /// </summary>
        private bool isFixed;

        /// <summary>
        /// Define if the particle model must or musn't be  calculated.
        /// </summary>
        private bool isEnable;

        /// <summary>
        /// Position of particle in 3D space
        /// </summary>
        private Vector3D position;

        /// <summary>
        /// Speed vector of particle
        /// </summary>
        private Vector3D velocity;

        /// <summary>
        /// Resultant vector of forces applied to particle
        /// </summary>
        private Vector3D force;

        #endregion

        /// <summary>
        /// Initializes a new instance of the Particle class
        /// </summary>
        /// <param name="m">Mass of particle</param>
        /// <param name="p">Initial position of particle</param>
        public Particle(float m, Vector3D p)
        {
            this.mass = m;
            this.position = p;
            this.isFixed = false;
            this.isEnable = false;
            this.age = 0.0F;
            this.isDisposed = false;
            this.velocity = new Vector3D();
            this.force = new Vector3D();
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the particle is enable
        /// </summary>
        public bool IsEnable
        {
            get { return this.isEnable; }
            set { this.isEnable = value; }
        }

        /// <summary>
        /// Gets or sets velocity of particle
        /// </summary>
        public Vector3D Velocity
        {
            get { return this.velocity; }
            set { this.velocity = value; }
        }

        /// <summary>
        /// Gets or sets mass of particle
        /// </summary>
        public float Mass
        {
            get { return this.mass; }
            set { this.mass = value; }
        }

        /// <summary>
        /// Gets or sets age of particle
        /// </summary>
        public float Age
        {
            get { return this.age; }
            set { this.age = value; }
        }

        /// <summary>
        /// Gets position of particle
        /// </summary>
        public Vector3D Position
        {
            get { return this.position; }
        }

        /// <summary>
        /// Gets force applied to particle
        /// </summary>
        public Vector3D Force
        {
            get { return this.force; }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Move particle to the given coordinates
        /// </summary>
        /// <param name="x">New X coordinate for particle position</param>
        /// <param name="y">New Y coordinate for particle position</param>
        /// <param name="z">New Z coordinate for particle position</param>
        public void MoveTo(float x, float y, float z)
        {
            this.position.Set(x, y, z);
        }

        /// <summary>
        /// Move particle by the given values
        /// </summary>
        /// <param name="x">Value to add to X coordinate of particle position</param>
        /// <param name="y">Value to add to Y coordinate of particle position</param>
        /// <param name="z">Value to add to Z coordinate of particle position</param>
        public void MoveBy(float x, float y, float z)
        {
            this.position.Add(x, y, z);
        }

        /// <summary>
        /// Set particle speed to given values
        /// </summary>
        /// <param name="x">New value for X coordinate of particle speed</param>
        /// <param name="y">New value for Y coordinate of particle speed</param>
        /// <param name="z">New value for Z coordinate of particle speed</param>
        public void SetVelocity(float x, float y, float z)
        {
            this.velocity.Set(x, y, z);
        }

        /// <summary>
        /// Add given values to the particle speed
        /// </summary>
        /// <param name="x">Value to add to X coordinate of particle velocity</param>
        /// <param name="y">Value to add to Y coordinate of particle velocity</param>
        /// <param name="z">Value to add to Z coordinate of particle velocity</param>
        public void AddVelocity(float x, float y, float z)
        {
            this.velocity.Add(x, y, z);
        }

        /// <summary>
        /// Set force applied to particle to the given values
        /// </summary>
        /// <param name="x">New X value for particle force</param>
        /// <param name="y">New Y value for particle force</param>
        /// <param name="z">New Z value for particle force</param>
        public void SetForce(float x, float y, float z)
        {
            this.force.Set(x, y, z);
        }

        /// <summary>
        /// Set particle to fixed
        /// </summary>
        public void MakeFixed()
        {
            this.isFixed = true;
            this.velocity.Clear();
        }

        /// <summary>
        /// Test whether the particle is fixed
        /// </summary>
        /// <returns>
        /// True if particle is fixed
        /// False if particle can move freely
        /// </returns>
        public bool IsFixed()
        {
            return this.isFixed;
        }

        /// <summary>
        /// Test whether the particle can move freely
        /// </summary>
        /// <returns>
        /// True if particle can move freely
        /// False if particle is fixed
        /// </returns>
        public bool IsFree()
        {
            return !this.isFixed;
        }

        /// <summary>
        /// Able a particle to move freely
        /// </summary>
        public void MakeFree()
        {
            this.isFixed = false;
        }

        /// <summary>
        /// release the particle from the engine and do the same for all force applied to it.
        /// </summary>
        public void Dispose()
        {
            this.isDisposed = true;
        }

        /// <summary>
        /// Test whether a particle is dead
        /// </summary>
        /// <returns>
        /// True if particle is dead
        /// False if it is still valid
        /// </returns>
        public bool IsDisposed()
        {
            return this.isDisposed;
        }

        #endregion

    // public class Particle
    }

// namespace ICE.physics
}