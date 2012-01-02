//-----------------------------------------------------------------------
// <copyright file="Arithmetic.cs" company="International Monetary Fund">
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
//   this file was inspired from TRAER PHYSICS engine implementation and from Quake (the game) source code
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
    /// This class contains some arithemtical operations
    /// </summary>
    public class Arithmetic
    {
        /// <summary>
        /// Numercial constant to fasten sqare root inversion
        /// </summary>
        private const int LomonMagicNumber = 0x5f3759df;

        /// <summary>
        /// Calculate 1 over square root of parameter
        /// WARNING : when input is 0, behaviour is undefined
        /// </summary>
        /// <param name="x">Input value</param>
        /// <returns>result = 1/sqrt(x)</returns>
        public static float FastInverseSqrt(float x)
        {
            float half = 0.5F * x;
            int i = BitConverter.ToInt32(BitConverter.GetBytes(x), 0);
            i = LomonMagicNumber - (i >> 1);
            x = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
            return x * (1.5F - (half * (x * x)));
        }
    }
}