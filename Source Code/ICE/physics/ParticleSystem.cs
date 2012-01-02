//-----------------------------------------------------------------------
// <copyright file="ParticleSystem.cs" company="International Monetary Fund">
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
    using System.Collections.Generic;
    using ICE.mathematics;

    /// <summary>
    /// Framework class for the physic engine :
    /// deals with all particles and forces
    /// and animate system by calculating paricles' movements
    /// </summary>
    public class ParticleSystem
    {
        #region Fields

        /// <summary>
        /// Reference to mathematical class used to calculate particles' movements
        /// </summary>
        private readonly Integrator integrator;

        /// <summary>
        /// List of all particles into the system
        /// </summary>
        private List<Particle> particles;

        /// <summary>
        /// List of all springs into the system
        /// </summary>
        private List<Spring> springs;

        /// <summary>
        /// List of all attraction forces into the system
        /// </summary>
        private List<Attraction> attractions;

        /// <summary>
        /// Vector representing gravity force
        /// </summary>
        private Vector3D gravity;

        /// <summary>
        /// Value of drag force
        /// </summary>
        private float drag;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ParticleSystem class
        /// </summary>
        public ParticleSystem()
        {
            this.integrator = new Integrator(this);
            this.particles = new List<Particle>();
            this.springs = new List<Spring>();
            this.attractions = new List<Attraction>();
            this.gravity = new Vector3D(0.0F, 0.0F, 0.0F);
            this.drag = 0.0F;
        }

        /// <summary>
        /// Initializes a new instance of the ParticleSystem class
        /// </summary>
        /// <param name="g">Gravity value, along the Y direction</param>
        /// <param name="somedrag">Drag value</param>
        public ParticleSystem(float g, float somedrag)
        {
            this.integrator = new Integrator(this);
            this.particles = new List<Particle>();
            this.springs = new List<Spring>();
            this.attractions = new List<Attraction>();
            this.gravity = new Vector3D(0.0F, g, 0.0F);
            this.drag = somedrag;
        }

        /// <summary>
        /// Initializes a new instance of the ParticleSystem class
        /// </summary>
        /// <param name="gx">X coordinate of gravity vector</param>
        /// <param name="gy">Y coordinate of gravity vector</param>
        /// <param name="gz">Z coordinate of gravity vector</param>
        /// <param name="somedrag">Drag value</param>
        public ParticleSystem(float gx, float gy, float gz, float somedrag)
        {
            this.integrator = new Integrator(this);
            this.particles = new List<Particle>();
            this.springs = new List<Spring>();
            this.attractions = new List<Attraction>();
            this.gravity = new Vector3D(gx, gy, gz);
            this.drag = somedrag;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Sets gravity vector
        /// </summary>
        /// <param name="x">New X value of gravity</param>
        /// <param name="y">New Y value of gravity</param>
        /// <param name="z">New Z value of gravity</param>
        public void SetGravity(float x, float y, float z)
        {
            this.gravity.Set(x, y, z);
        }

        /// <summary>
        /// Sets gravity vector along Y direction to given value
        /// </summary>
        /// <param name="g">New value of gravity</param>
        public void SetGravity(float g)
        {
            this.gravity.Set(0.0F, g, 0.0F);
        }

        /// <summary>
        /// Sets drag value
        /// </summary>
        /// <param name="d">New value of drag</param>
        public void SetDrag(float d)
        {
            this.drag = d;
        }

        /// <summary>
        /// Calculate particles' movements for one unit of time
        /// </summary>
        public void Tick()
        {
            // locking the thread to ensure no particle can be added or removed
            // while calculating next state of the system
            lock (this)
            {
                this.CleanUp();
                this.integrator.Step(1.0F);
            }
        }

        /// <summary>
        /// Calculate particles' movements for given amount of time
        /// </summary>
        /// <param name="t">Length to step in time</param>
        public void Tick(float t)
        {
            // locking the thread to ensure no particle can be added or removed
            // while calculating next state of the system
            lock (this)
            {
                this.CleanUp();
                this.integrator.Step(t);
            }
        }

        /// <summary>
        /// Creates a new particle and adds it to the system
        /// </summary>
        /// <param name="mass">Mass of the particle</param>
        /// <param name="x">Initial value of X coordinate for particle</param>
        /// <param name="y">Initial value of Y coordinate for particle</param>
        /// <param name="z">Initial value of Z coordinate for particle</param>
        /// <returns>New particle created</returns>
        public Particle MakeParticle(float mass, float x, float y, float z)
        {
            Particle p = new Particle(mass, new Vector3D(x, y, z));
            this.particles.Add(p);
            this.integrator.AllocateParticles();
            return p;
        }

        /// <summary>
        /// Creates a new particle of default mass and adds it to the system
        /// </summary>
        /// <param name="x">Initial value of X coordinate for particle</param>
        /// <param name="y">Initial value of Y coordinate for particle</param>
        /// <param name="z">Initial value of Z coordinate for particle</param>
        /// <returns>New particle created</returns>
        public Particle MakeParticle(float x, float y, float z)
        {
            Particle p = new Particle(PhysicsConstants.ParticleDefaultMass, new Vector3D(x, y, z));
            this.particles.Add(p);
            this.integrator.AllocateParticles();
            return p;
        }

        /// <summary>
        /// Creates a new particle of default mass and at center position
        /// and adds it to the system
        /// </summary>
        /// <returns>New particle created</returns>
        public Particle MakeParticle()
        {
            return this.MakeParticle(PhysicsConstants.ParticleDefaultMass, 0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Creates a new spring and adds it to the system
        /// </summary>
        /// <param name="end1">Particule at first end of the spring</param>
        /// <param name="end2">Particule at second end of the spring</param>
        /// <param name="sk">Spring strength</param>
        /// <param name="d">Damping value of spring</param>
        /// <param name="rl">Rest lenght of spring</param>
        /// <returns>New spring created</returns>
        public Spring MakeSpring(Particle end1, Particle end2, float sk, float d, float rl)
        {
            Spring s = new Spring(end1, end2, sk, d, rl);
            this.springs.Add(s);
            return s;
        }

        /// <summary>
        /// Creates a new attraction and adds it to the system
        /// </summary>
        /// <param name="end1">Particule at first end of the spring</param>
        /// <param name="end2">Particule at second end of the spring</param>
        /// <param name="k">Attraction strength</param>
        /// <param name="d">Minimal distance under which attraction is restricted</param>
        /// <returns>New attraction created</returns>
        public Attraction MakeAttraction(Particle end1, Particle end2, float k, float d)
        {
            Attraction m = new Attraction(end1, end2, k, d);
            this.attractions.Add(m);
            return m;
        }

        /// <summary>
        /// Clears all objects in particle system
        /// </summary>
        public void Clear()
        {
            this.particles.Clear();
            this.springs.Clear();
            this.attractions.Clear();
        }

        /// <summary>
        /// For all particles of the system, calculate resultant of forces
        /// (gravity, drag, springs and attractions).
        /// </summary>
        public void ApplyForces()
        {
            Particle p;
            for (int i = 0; i < this.particles.Count; i++)
            {
                p = (Particle)this.particles[i];
                p.Force.Add(this.gravity);
                p.Force.Add(p.Velocity.X * -this.drag, p.Velocity.Y * -this.drag, p.Velocity.Z * -this.drag);
            }

            Spring s;
            for (int i = 0; i < this.springs.Count; i++)
            {
                s = (Spring)this.springs[i];
                s.Apply();
            }

            Attraction a;
            for (int i = 0; i < this.attractions.Count; i++)
            {
                a = (Attraction)this.attractions[i];
                a.Apply();
            }

        // public void ApplyForces()
        }

        /// <summary>
        /// Gets number of particles of the system
        /// </summary>
        /// <returns>Number of particles</returns>
        public int NumberOfParticles()
        {
            return this.particles.Count;
        }

        /// <summary>
        /// Gets number of springs of the system
        /// </summary>
        /// <returns>Number of springs</returns>
        public int NumberOfSprings()
        {
            return this.springs.Count;
        }

        /// <summary>
        /// Gets number of attractions in the system
        /// </summary>
        /// <returns>Number of attractions</returns>
        public int NumberOfAttractions()
        {
            return this.attractions.Count;
        }

        /// <summary>
        /// Gets particle at index i
        /// </summary>
        /// <param name="i">Index of particle</param>
        /// <returns>Particle at index i</returns>
        public Particle GetParticle(int i)
        {
            return (Particle)this.particles[i];
        }

        /// <summary>
        /// Gets spring at index i
        /// </summary>
        /// <param name="i">Index of spring</param>
        /// <returns>Spring at index i</returns>
        public Spring GetSpring(int i)
        {
            return this.springs[i];
        }

        /// <summary>
        /// Get a list of all springs force
        /// </summary>
        /// <returns>A copy off the current Spring list</returns>
        public List<Spring> GetSprings()
        {
            return new List<Spring>(this.springs);
        }

        /// <summary>
        /// Gets attraction at index i
        /// </summary>
        /// <param name="i">Index of attraction</param>
        /// <returns>Attraction at index i</returns>
        public Attraction GetAttraction(int i)
        {
            return (Attraction)this.attractions[i];
        }

        /// <summary>
        /// Gets the list of all current attraction
        /// </summary>
        /// <returns>a copy of the list of all attraction</returns>
        public List<Attraction> GetAttractions()
        {
            return new List<Attraction>(this.attractions);
        }

        /// <summary>
        /// Clears all forces applied to particles
        /// </summary>
        private void ClearForces()
        {
            foreach (Particle p in this.particles)
            {
                p.Force.Clear();
            }
        }

        /// <summary>
        /// Cleans up the physic model by removing dead objects
        /// </summary>
        /// Classical approach to remove items into a list is a backward loop
        /// however it cause slowliness because of pagination issues with CPU cache
        /// That's why we use the improved method
        private void CleanUp()
        {
            Particle p;
            for (int i = 0; i < this.particles.Count; i++)
            {
                p = (Particle)this.particles[i];
                if (p.IsDisposed())
                {
                    this.particles.RemoveAt(i--);
                }
            }

            Spring s;
            for (int i = 0; i < this.springs.Count; i++)
            {
                s = (Spring)this.springs[i];
                if (s.IsDisposed())
                {
                    this.springs.RemoveAt(i--);
                }
            }

            Attraction a;
            for (int i = 0; i < this.attractions.Count; i++)
            {
                a = (Attraction)this.attractions[i];
                if (a.IsDisposed())
                {
                    this.attractions.RemoveAt(i--);
                }
            }

        // private void CleanUp()
        }
        
        #endregion

    // public class ParticleSystem
    }

// namespace ICE.physics
}
