//-----------------------------------------------------------------------
// <copyright file="Integrator.cs" company="International Monetary Fund">
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
    using System.Collections.Generic;
    using ICE.physics;

    /// <summary>
    /// Integrator based on Runge Kutta's method :
    /// iterative approximations of differential equations for particles' movements
    /// </summary>
    public class Integrator
    {
        #region Fields

        /// <summary>
        /// Particle System on which we want to calculate moves
        /// </summary>
        private readonly ParticleSystem sys;

        /// <summary>
        /// List of original positions of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> originalPositions;

        /// <summary>
        /// List of original velocities of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> originalVelocities;

        /// <summary>
        /// List of k1 force coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k1Forces;

        /// <summary>
        /// List of k1 velocity coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k1Velocities;

        /// <summary>
        /// List of k2 force coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k2Forces;

        /// <summary>
        /// List of k2 velocity coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k2Velocities;

        /// <summary>
        /// List of k3 force coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k3Forces;

        /// <summary>
        /// List of k3 velocity coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k3Velocities;

        /// <summary>
        /// List of k4 force coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k4Forces;

        /// <summary>
        /// List of k4 velocity coefficients of all particles in the system,
        /// necessary for the integration method
        /// </summary>
        private List<Vector3D> k4Velocities;

        #endregion

        /// <summary>
        /// Initializes a new instance of the Integrator class
        /// </summary>
        /// <param name="s">Particle System on which we want to calculate moves</param>
        public Integrator(ParticleSystem s)
        {
            this.sys = s;
            this.originalPositions = new List<Vector3D>();
            this.originalVelocities = new List<Vector3D>();
            this.k1Forces = new List<Vector3D>();
            this.k1Velocities = new List<Vector3D>();
            this.k2Forces = new List<Vector3D>();
            this.k2Velocities = new List<Vector3D>();
            this.k3Forces = new List<Vector3D>();
            this.k3Velocities = new List<Vector3D>();
            this.k4Forces = new List<Vector3D>();
            this.k4Velocities = new List<Vector3D>();

        // public Integrator(ParticleSystem sys)
        }

        /// <summary>
        /// Pre-allocate vectors for integration of particles' movements
        /// </summary>
        public void AllocateParticles()
        {
            // for all particles added to the system
            // (difference between number of particles and actual size of vector),
            // add null vector to the 10 vector lists
            for (; this.sys.NumberOfParticles() > this.originalPositions.Count; this.k4Velocities.Add(new Vector3D()))
            {
                this.originalPositions.Add(new Vector3D());
                this.originalVelocities.Add(new Vector3D());
                this.k1Forces.Add(new Vector3D());
                this.k1Velocities.Add(new Vector3D());
                this.k2Forces.Add(new Vector3D());
                this.k2Velocities.Add(new Vector3D());
                this.k3Forces.Add(new Vector3D());
                this.k3Velocities.Add(new Vector3D());
                this.k4Forces.Add(new Vector3D());
            }

        // public void AllocateParticles()
        }

        /// <summary>
        /// Calculate force and velocity of all particles in the system at next step in time
        /// </summary>
        /// <param name="deltaT">step of time</param>
        /// <remarks>
        /// Use the fourth-order Runge-Kutta method (RK4)
        /// </remarks>
        /////////////////////////////////////////////////////////////////////////////////////// 
        // For differential equation :
        //     y' = f(t,y)
        //     y(t_0) = y_0
        // 
        // Original RK4 algorithm is :
        // t_n+1 = t_n + deltaT
        // y_n+1 = y_n + (k1 + 2*k2 + 2*k3 + k4)*deltaT/6
        // with    k1 = f(t_n,y_n)
        //         k2 = f(t_n + deltaT/2, y_n + deltaT*k1/2)
        //         k3 = f(t_n + deltaT/2, y_n + deltaT*k2/2)
        //         k4 = f(t_n + deltaT, y_n + deltaT*k3)
        //
        // 
        // Here the problem is quite more complicated,
        // because position, velocity and resultant force on particle are thightly interwined
        public void Step(float deltaT)
        {
            // Necessary for further intermediary calculations
            Vector3D result;

            // Used in the following loops
            Particle p;

            // Locking the thread to ensure size of 10 vectors won't change
            // Supposingly number of particles is constant during function
            // because of lock() in ParicleSystem.tick()
            lock (this)
            {
                // 1) Get original position and velocity of each particle
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    { 
                        continue; 
                    }
                    
                    // If the particle is able to move freely
                    if (p.IsFree())
                    {
                        // Set original position and velocity values to actual position and velocity of particle
                        ((Vector3D)this.originalPositions[i]).Set(p.Position);
                        ((Vector3D)this.originalVelocities[i]).Set(p.Velocity);
                    }

                    // Anyway, re-initialise forces applied to it
                    p.Force.Clear();

                // for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                }

                // 2) Calculate forces for all system
                // depending on the original positions and velocities
                this.sys.ApplyForces();

                // 3) For each particle calculate k1 for force and velocity
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        ((Vector3D)this.k1Forces[i]).Set(p.Force);
                        ((Vector3D)this.k1Velocities[i]).Set(p.Velocity);
                    }

                    p.Force.Clear();
                }

                // 4) For each particle, set new position and velocity,
                // depending on original values and k1 values
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        // pos = originalPos + (k1Vel * deltaT)/2
                        /*
                        Vector3D originalPosition = (Vector3D)this.originalPositions[i];
                        Vector3D k1Velocity = (Vector3D)this.k1Velocities[i];
                        p.Position.X = originalPosition.X + (k1Velocity.X * 0.5F * deltaT);
                        p.Position.Y = originalPosition.Y + (k1Velocity.Y * 0.5F * deltaT);
                        p.Position.Z = originalPosition.Z + (k1Velocity.Z * 0.5F * deltaT);
                         */
                        p.Position.Set(
                            ((Vector3D)this.k1Velocities[i]).Times(0.5F * deltaT).Plus(
                                (Vector3D)this.originalPositions[i]));

                        // vel = originalVel + (k1Force * deltaT) / (2 * mass)
                        /*
                        Vector3D originalVelocity = (Vector3D)this.originalVelocities[i];
                        Vector3D k1Force = (Vector3D)this.k1Forces[i];
                        p.Velocity.X = originalVelocity.X + (k1Force.X * 0.5F * deltaT / p.Mass);
                        p.Velocity.Y = originalVelocity.Y + (k1Force.Y * 0.5F * deltaT / p.Mass);
                        p.Velocity.Z = originalVelocity.Z + (k1Force.Z * 0.5F * deltaT / p.Mass);
                         */
                        p.Velocity.Set(
                            ((Vector3D)this.k1Forces[i]).Times(0.5F * deltaT / p.Mass).Plus(
                                (Vector3D)this.originalVelocities[i]));
                    }
                }

                // 5) Calculate forces for all system
                // depending on new positions and velocities
                this.sys.ApplyForces();

                // 6) For each particle calculate k2 for force and velocity
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        ((Vector3D)this.k2Forces[i]).Set(p.Force);
                        ((Vector3D)this.k2Velocities[i]).Set(p.Velocity);
                    }

                    p.Force.Clear();
                }

                // 7) For each particle, set new position and velocity,
                // depending on original values and k2 values
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        // pos = originalPos + (k2Vel * deltaT)/2
                        /*
                        Vector3D originalPosition = (Vector3D)this.originalPositions[i];
                        Vector3D k2Velocity = (Vector3D)this.k2Velocities[i];
                        p.Position.X = originalPosition.X + (k2Velocity.X * 0.5F * deltaT);
                        p.Position.Y = originalPosition.Y + (k2Velocity.Y * 0.5F * deltaT);
                        p.Position.Z = originalPosition.Z + (k2Velocity.Z * 0.5F * deltaT);
                         */
                        p.Position.Set(
                            ((Vector3D)this.k2Velocities[i]).Times(0.5F * deltaT).Plus(
                                (Vector3D)this.originalPositions[i]));

                        // vel = originalVel + (k2Force * deltaT) / (2 * mass)
                        /*
                        Vector3D originalVelocity = (Vector3D)this.originalVelocities[i];
                        Vector3D k2Force = (Vector3D)this.k2Forces[i];
                        p.Velocity.X = originalVelocity.X + ((k2Force.X * 0.5F * deltaT) / p.Mass);
                        p.Velocity.Y = originalVelocity.Y + ((k2Force.Y * 0.5F * deltaT) / p.Mass);
                        p.Velocity.Z = originalVelocity.Z + ((k2Force.Z * 0.5F * deltaT) / p.Mass);
                         */
                        p.Velocity.Set(
                            ((Vector3D)this.k2Forces[i]).Times(0.5F * deltaT / p.Mass).Plus(
                                (Vector3D)this.originalVelocities[i]));
                    }
                }

                // 8) Calculate forces for all system
                // depending on new positions and velocities
                this.sys.ApplyForces();

                // 9) For each particle calculate k3 for force and velocity
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        ((Vector3D)this.k3Forces[i]).Set(p.Force);
                        ((Vector3D)this.k3Velocities[i]).Set(p.Velocity);
                    }

                    p.Force.Clear();
                }

                // 10) For each particle, set new position and velocity,
                // depending on original values and k3 values
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        // pos = originalPos + k3Vel * deltaT
                        /*
                        Vector3D originalPosition = (Vector3D)this.originalPositions[i];
                        Vector3D k3Velocity = (Vector3D)this.k3Velocities[i];
                        p.Position.X = originalPosition.X + (k3Velocity.X * deltaT);
                        p.Position.Y = originalPosition.Y + (k3Velocity.Y * deltaT);
                        p.Position.Z = originalPosition.Z + (k3Velocity.Z * deltaT);
                         */
                        p.Position.Set(
                            ((Vector3D)this.k3Velocities[i]).Times(deltaT).Plus(
                                (Vector3D)this.originalPositions[i]));

                        // vel = originalVel + (k2Force * deltaT) /  mass
                        /*
                        Vector3D originalVelocity = (Vector3D)this.originalVelocities[i];
                        Vector3D k3Force = (Vector3D)this.k3Forces[i];
                        p.Velocity.X = originalVelocity.X + ((k3Force.X * deltaT) / p.Mass);
                        p.Velocity.Y = originalVelocity.Y + ((k3Force.Y * deltaT) / p.Mass);
                        p.Velocity.Z = originalVelocity.Z + ((k3Force.Z * deltaT) / p.Mass);
                         */
                        p.Velocity.Set(
                            ((Vector3D)this.k3Forces[i]).Times(deltaT / p.Mass).Plus(
                                (Vector3D)this.originalVelocities[i]));
                    }
                }

                // 11) Calculate forces for all system
                // depending on new positions and velocities
                this.sys.ApplyForces();

                // 12) For each particle calculate k4 for force and velocity
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        ((Vector3D)this.k4Forces[i]).Set(p.Force);
                        ((Vector3D)this.k4Velocities[i]).Set(p.Velocity);
                    }
                }

                // 13) For each particle, set final position and velocity,
                // depending on original values and k1, k2, k3 and k4 values
                for (int i = 0; i < this.sys.NumberOfParticles(); i++)
                {
                    p = (Particle)this.sys.GetParticle(i);

                    if (!p.IsEnable) 
                    {
                        continue; 
                    }

                    if (p.IsFree())
                    {
                        // pos = orgPos + (k1Vel + 2*k2Vel + 2*k3Vel + k4Vel)*deltaT/6
                        /*
                        Vector3D originalPosition = (Vector3D)this.originalPositions[i];
                        Vector3D k1Velocity = (Vector3D)this.k1Velocities[i];
                        Vector3D k2Velocity = (Vector3D)this.k2Velocities[i];
                        Vector3D k3Velocity = (Vector3D)this.k3Velocities[i];
                        Vector3D k4Velocity = (Vector3D)this.k4Velocities[i];
                        p.Position.X = originalPosition.X + ((deltaT / 6F) * (k1Velocity.X + (2.0F * k2Velocity.X) + (2.0F * k3Velocity.X) + k4Velocity.X));
                        p.Position.Y = originalPosition.Y + ((deltaT / 6F) * (k1Velocity.Y + (2.0F * k2Velocity.Y) + (2.0F * k3Velocity.Y) + k4Velocity.Y));
                        p.Position.Z = originalPosition.Z + ((deltaT / 6F) * (k1Velocity.Z + (2.0F * k2Velocity.Z) + (2.0F * k3Velocity.Z) + k4Velocity.Z));
                         */
                        result = ((Vector3D)this.k1Velocities[i])
                            .Plus(((Vector3D)this.k2Velocities[i]).Times(2.0F))
                            .Plus(((Vector3D)this.k3Velocities[i]).Times(2.0F))
                            .Plus((Vector3D)this.k4Velocities[i]);
                        result.MultiplyBy(deltaT / 6F);
                        result.Add((Vector3D)this.originalPositions[i]);
                        p.Position.Set(result);

                        // vel = orgVel + (k1For + 2*k2For + 2*k3For + k4For)*deltaT/(6*mass)
                        /*
                        Vector3D originalVelocity = (Vector3D)this.originalVelocities[i];
                        Vector3D k1Force = (Vector3D)this.k1Forces[i];
                        Vector3D k2Force = (Vector3D)this.k2Forces[i];
                        Vector3D k3Force = (Vector3D)this.k3Forces[i];
                        Vector3D k4Force = (Vector3D)this.k4Forces[i];
                        p.Velocity.X = originalVelocity.X + ((deltaT / (6F * p.Mass)) * (k1Force.X + (2.0F * k2Force.X) + (2.0F * k3Force.X) + k4Force.X));
                        p.Velocity.Y = originalVelocity.Y + ((deltaT / (6F * p.Mass)) * (k1Force.Y + (2.0F * k2Force.Y) + (2.0F * k3Force.Y) + k4Force.Y));
                        p.Velocity.Z = originalVelocity.Z + ((deltaT / (6F * p.Mass)) * (k1Force.Z + (2.0F * k2Force.Z) + (2.0F * k3Force.Z) + k4Force.Z));
                         */
                        result = ((Vector3D)this.k1Forces[i])
                            .Plus(((Vector3D)this.k2Forces[i]).Times(2.0F))
                            .Plus(((Vector3D)this.k3Forces[i]).Times(2.0F))
                            .Plus((Vector3D)this.k4Forces[i]);
                        result.MultiplyBy(deltaT / (6F * p.Mass));
                        result.Add((Vector3D)this.originalVelocities[i]);
                        p.Velocity.Set(result);

                    // if (p.IsFree())
                    }

                    // In anycase, age particle
                    p.Age += deltaT;
                }

            // lock (this)
            }

        // public void Step(float deltaT)
        }

    // public class Integrator
    }

// namespace ICE.mathematics
}
