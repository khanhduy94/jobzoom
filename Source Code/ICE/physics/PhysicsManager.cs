//-----------------------------------------------------------------------
// <copyright file="PhysicsManager.cs" company="International Monetary Fund">
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
//      Poirot Clément (Project Officer)
// </authors>
// <context>
//      for the benefit of the the Open Source Community
// </context>
// <supervisors>
//      Hervé Tourpe (Team Leader)
//      Jeffrey Hatton (Project Manager)
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
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// This class manages all interactions with the physics engine
    /// </summary>
    public class PhysicsManager
    {
        #region Fields

        /// <summary>
        /// The physics system
        /// </summary>
        private physics.ParticleSystem particleSystem;

        /// <summary>
        /// The settings of our physics engine
        /// </summary>
        private setting.IPhysicsSettings settings;

        /// <summary>
        /// the timer that update the physic system;
        /// </summary>
        private DispatcherTimer timer;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PhysicsManager class.
        /// </summary>
        /// <param name="mainManager">the main manager reference</param>
        public PhysicsManager(MainManager mainManager)
        {
            // create the particle system
            this.particleSystem = new ParticleSystem();

            // create the timer
            this.timer = new DispatcherTimer();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the settings of the physics engine
        /// </summary>
        public setting.IPhysicsSettings Settings
        {
            get 
            { 
                return this.settings; 
            }

            set 
            {
                this.settings = value;
                this.settings.Changed += new EventHandler(this.Settings_Changed);
                this.Settings_Changed(null, null);
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// This function adds (if necessary) a particle to the physics engine as a representation of the node passed in argument
        /// </summary>
        /// <param name="x">the x coordinate</param>
        /// <param name="y">the y coordinate</param>
        /// <param name="z">the z coordinate</param>
        /// <param name="node">the node you want to represent in the physics engine</param>
        /// <param name="model">the model manager. It's used to create interactions</param>
        public void AddPhysicRepresentation(float x, float y, float z, model.Node node, model.ModelManager model)
        {
            // if the node already exists and already has a physic representation then don't do anything, go back to where you come from.
            if (node.PhysicRepresentation != null) 
            { 
                return; 
            }

            // else we create a physic representation
            Particle particle = this.particleSystem.MakeParticle(x, y, z);

            // create some space between the nodes 
            foreach (model.Node otherNode in model.NodeList)
            {
                if (otherNode.PhysicRepresentation != null)
                {
                    physics.Attraction repulsion = this.particleSystem.MakeAttraction(
                         otherNode.PhysicRepresentation,
                         particle,
                         -1 * this.settings.RepultionForce,
                         PhysicsConstants.AttractionEffectMinimalDistance);
                    node.SetRepulsion(otherNode, repulsion);
                    otherNode.SetRepulsion(node, repulsion);
                }
            }

            // finally set the particle
            node.PhysicRepresentation = particle;
        }

        /// <summary>
        /// This function adds (if necessary) a spring to the physics engine as a representation of the link passed in argument
        /// </summary>
        /// <param name="link">the link you want to represent in the physics engine</param>
        public void AddPhysicRepresentation(model.Link link)
        {
            // if the link physic representation already exists then don't do anything
            if (link.PhysicRepresentation != null) 
            {
                // we update the strength of the physics representation
                link.PhysicRepresentation.Strength = this.GetSpringStrength(link);
                return; 
            }

            // else we calculate the spring strength
            float springStrength = this.GetSpringStrength(link);

            // create the physic representation
            link.PhysicRepresentation = this.particleSystem.MakeSpring(
                link.RelatedNode1.PhysicRepresentation,
                link.RelatedNode2.PhysicRepresentation,
                springStrength,
                PhysicsConstants.SpringDamping,
                this.settings.LinkRestLength);

            // delete useless repulsion
            link.RelatedNode1.GetRepulsion(link.RelatedNode2).Dispose();
        }

        /// <summary>
        /// This function starts the physic management thread
        /// </summary>
        public void Start()
        {
            // initialising the update timer
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            this.timer.Tick += new EventHandler(this.Update);
            this.timer.Start();
        }

        /// <summary>
        /// This function pauses the physic management thread
        /// </summary>
        public void Pause()
        {
            this.timer.Stop();
        }

        /// <summary>
        /// This function resumes the physic management thread
        /// </summary>
        public void Resume()
        {
            this.timer.Start();
        }

        #endregion

        /// <summary>
        /// this function is called when the settings have changed
        /// </summary>
        /// <param name="sender">the settings</param>
        /// <param name="e">event argument</param>
        private void Settings_Changed(object sender, EventArgs e)
        {
            // Set gravity as defined in user settings
            this.particleSystem.SetGravity(this.settings.Gravity);

            // Set drag force
            this.particleSystem.SetDrag(this.settings.DragForce);

            // change spring length at rest
            foreach (Spring spring in this.particleSystem.GetSprings())
            {
                spring.RestLength = this.settings.LinkRestLength;
            }

            // change repulsion strength
            foreach (Attraction repultion in this.particleSystem.GetAttractions())
            {
                repultion.Strength = -1 * this.settings.RepultionForce;
            }
        }

        /// <summary>
        /// This function updates the current physics engine
        /// </summary>
        /// <param name="sender">the timer of the physics engine</param>
        /// <param name="e">the event argument</param>
        private void Update(object sender, EventArgs e)
        {
            this.particleSystem.Tick();
        }

        /// <summary>
        /// This function converts the strength in the XML into a usable strength for our springs
        /// </summary>
        /// <param name="link">the link from which we extract the Strength parameter</param>
        /// <returns>the strength of the spring</returns>
        private float GetSpringStrength(model.Link link)
        {
            float springStrength;
            springStrength = link.Strength / 100;
            springStrength *= PhysicsConstants.MaximalSpringStrength - PhysicsConstants.MinimalSpringStrength;
            springStrength += PhysicsConstants.MinimalSpringStrength;
            return springStrength;
        }
    }
}
