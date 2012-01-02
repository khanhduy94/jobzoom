//-----------------------------------------------------------------------
// <copyright file="Vector3D.cs" company="International Monetary Fund">
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

namespace ICE.mathematics
{
    using System;

    /// <summary>
    /// This class groups mathemical operations over 3D vectors
    /// </summary>
    public class Vector3D
    {
        #region Fields
        /// <summary>
        /// X coordinate of vector
        /// </summary>
        private float x;

        /// <summary>
        /// Y coordinate of vector
        /// </summary>
        private float y;

        /// <summary>
        /// Z coodinate of vector
        /// </summary>
        private float z;
        #endregion        

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Vector3D class
        /// Parameters are values of the vector coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Initializes a new instance of the Vector3D class
        /// Default constructor (no parameters) return vector null
        /// </summary>
        public Vector3D()
        {
            this.x = 0.0F;
            this.y = 0.0F;
            this.z = 0.0F;
        }

        /// <summary>
        /// Initializes a new instance of the Vector3D class
        /// copying the coordinate of the argument vector
        /// </summary>
        /// <param name="p">Vector to copy</param>
        public Vector3D(Vector3D p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets Z coordinate
        /// </summary>
        public float Z
        {
            get { return this.z; }
            set { this.z = value; }
        }

        /// <summary>
        /// Gets or sets Y coordinate
        /// </summary>
        public float Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        /// <summary>
        /// Gets or sets X coordinate
        /// </summary>
        public float X
        {
            get { return this.x; }
            set { this.x = value; }
        }
        #endregion

        #region Functions

        /// <summary>
        /// Sets all three coordinates of vector at a time
        /// </summary>
        /// <param name="x">New X value</param>
        /// <param name="y">New Y value</param>
        /// <param name="z">New Z value</param>
        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Sets all three coordinates of vector at a time
        /// copy coordinates of parameter vector
        /// </summary>
        /// <param name="p">Vector to copy</param>
        public void Set(Vector3D p)
        {
            this.x = p.x;
            this.y = p.y;
            this.z = p.z;
        }

        /// <summary>
        /// Adds in place two vectors, coordinates by coordinates
        /// </summary>
        /// <param name="p">Vector to add</param>
        /// <returns>Current vector added with p</returns>
        public Vector3D Add(Vector3D p)
        {
            this.x += p.x;
            this.y += p.y;
            this.z += p.z;
            return this;
        }

        /// <summary>
        /// Substracts in place two vectors, coordinates by coordinates
        /// </summary>
        /// <param name="p">Vector to substract</param>
        /// <returns>Current vector substracted of p</returns>
        public Vector3D Subtract(Vector3D p)
        {
            this.x -= p.x;
            this.y -= p.y;
            this.z -= p.z;
            return this;
        }

        /// <summary>
        /// Adds in place given values to vector coordinates
        /// </summary>
        /// <param name="a">Value to add to X coordinate</param>
        /// <param name="b">Value to add to Y coordinate</param>
        /// <param name="c">Value to add to Z coordinate</param>
        /// <returns>Current vector added with coordinates a, b and c</returns>
        public Vector3D Add(float a, float b, float c)
        {
            this.x += a;
            this.y += b;
            this.z += c;
            return this;
        }

        /// <summary>
        /// Calculate the resultant of two vectors
        /// </summary>
        /// <param name="p">Vector to add</param>
        /// <returns>Resultant vector</returns>
        public Vector3D Plus(Vector3D p)
        {
            return new Vector3D(this.x + p.x, this.y + p.y, this.z + p.z);
        }

        /// <summary>
        /// Multiply vector coordinates by a constant
        /// </summary>
        /// <param name="f">Multiplying value</param>
        /// <returns>New vector f times longer</returns>
        public Vector3D Times(float f)
        {
            return new Vector3D(this.x * f, this.y * f, this.z * f);
        }

        /// <summary>
        /// Divides vector coordinates by a constant
        /// </summary>
        /// <param name="f">Dividing value</param>
        /// <returns>New vector f times shorter</returns>
        public Vector3D Over(float f)
        {
            return new Vector3D(this.x / f, this.y / f, this.z / f);
        }

        /// <summary>
        /// Calculate the resultant of two vectors
        /// </summary>
        /// <param name="p">Vector to substract</param>
        /// <returns>Resultant vector</returns>
        public Vector3D Minus(Vector3D p)
        {
            return new Vector3D(this.x - p.x, this.y - p.y, this.z - p.z);
        }

        /// <summary>
        /// Multiply in place vector coordinates by a constant
        /// </summary>
        /// <param name="f">Multiplying value</param>
        /// <returns>Current vector multiplyed by f</returns>
        public Vector3D MultiplyBy(float f)
        {
            this.x *= f;
            this.y *= f;
            this.z *= f;
            return this;
        }

        /// <summary>
        /// Calculate distance to given vector
        /// </summary>
        /// <param name="p">Distant vector</param>
        /// <returns>Euclydian distance</returns>
        public float DistanceTo(Vector3D p)
        {
            float dx = this.x - p.x;
            float dy = this.y - p.y;
            float dz = this.z - p.z;
            return (float)Math.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }

        /// <summary>
        /// Calculate distance to given coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <returns>Euclydian distance</returns>
        public float DistanceTo(float x, float y, float z)
        {
            float dx = this.x - x;
            float dy = this.y - y;
            float dz = this.z - z;
            return 1.0F / Arithmetic.FastInverseSqrt((dx * dx) + (dy * dy) + (dz * dz));
        }

        /// <summary>
        /// Multiply two vectors, coordinate by coordinate
        /// </summary>
        /// <param name="p">Vector to multiply</param>
        /// <returns>New vector that holds multiplication result</returns>
        public float Dot(Vector3D p)
        {
            return (this.x * p.x) + (this.y * p.y) + (this.z * p.z);
        }

        /// <summary>
        /// Calculate vector length
        /// </summary>
        /// <returns>Euclydian length</returns>
        public float Length()
        {
            return (float)Math.Sqrt((this.x * this.x) + (this.y * this.y) + (this.z * this.z));
        }

        /// <summary>
        /// Normalize a vector
        /// </summary>
        /// <returns>
        /// If vector is not null returns collinear vector with length 1
        /// Else returns vector null
        /// </returns>
        public Vector3D Unit()
        {
            float l = this.Length();
            return l != 0.0F ? this.Over(l) : new Vector3D();
        }

        /// <summary>
        /// Resets vector to vector null
        /// </summary>
        public void Clear()
        {
            this.x = 0.0F;
            this.y = 0.0F;
            this.z = 0.0F;
        }

        /// <summary>
        /// Format vector for string output
        /// </summary>
        /// <returns>String representing vector</returns>
        public override string ToString()
        {
            return "(" + this.x + ", " + this.y + ", " + this.z + ")";
        }

        /// <summary>
        /// Calculate vectorial product
        /// u.Cross(v)
        /// </summary>
        /// <param name="p">Second term of vectorial product</param>
        /// <returns>Vectorial product u^v</returns>
        public Vector3D Cross(Vector3D p)
        {
            return new Vector3D(
                (this.y * p.z) - (this.z * p.y),
                (this.z * p.x) - (this.x * p.z),
                (this.x * p.y) - (this.y * p.x));
        }

        #endregion

    // public class Vector3D
    }

// namespace ICE.mathematics
}
