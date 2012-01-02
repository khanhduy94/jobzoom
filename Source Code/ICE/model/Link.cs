//-----------------------------------------------------------------------
// <copyright file="Link.cs" company="International Monetary Fund">
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

namespace ICE.model
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using ICE.physics;
    using ICE.setting;
    using ICE.view;

    /// <summary>
    /// Data for a link
    /// </summary>
    public class Link
    {
        #region Fields

        /// <summary>
        /// The link's ID
        /// </summary>
        private string id;

        /// <summary>
        /// First node relative to the link
        /// </summary>
        private Node relatedNode1;

        /// <summary>
        /// Second node relative to the link
        /// </summary>
        private Node relatedNode2;

        /// <summary>
        /// Strength of the link
        /// </summary>
        /// <remarks>Strength is a percentage, are between 1 and 100</remarks>
        private float strength;

        /// <summary>
        /// Links's style
        /// </summary>
        private string styleName;

        /// <summary>
        /// Verb used in the link description
        /// </summary>
        private string verb;

        /// <summary>
        /// Complement used in the link description
        /// </summary>
        private string complement;

        /// <summary>
        /// Link's physical representation
        /// </summary>
        /// TODO : move away ?
        private Spring physicRepresentation;

        /// <summary>
        /// Define whether the node is visible on the graph
        /// </summary>
        private bool isVisible = false;

        /// <summary>
        /// Define whether the model manager explored this node during the exploration process.
        /// </summary>
        /// <remarks>
        /// this field is set to false after the initialiation of the exploration process.
        /// </remarks>
        private bool isExplored = false;

        /// <summary>
        /// Define whether the link has been diposed or not
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// This is the list of all actions the user can perform on the link and there respectives arguments
        /// Each action is represented in XML (@see the ICE Data file)
        /// </summary>
        private List<string> actions = new List<string>();

        /// <summary>
        /// This is the Xml definition of all useful information used to draw the link on the graph
        /// </summary>
        private string drawingInformation;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Link class.
        /// </summary>
        /// <param name="id">ID of the new link</param>
        /// <param name="node1">First node relative to the link</param>
        /// <param name="node2">Second node relative to the link</param>
        public Link(
                string id,
                Node node1,
                Node node2)
        {
            this.id = id;
            this.relatedNode1 = node1;
            this.relatedNode1.LinkList.Add(this);
            this.relatedNode1.SetRelativeMass();
            this.relatedNode2 = node2;
            this.relatedNode2.LinkList.Add(this);
            this.relatedNode2.SetRelativeMass();
        }

        #endregion

        #region Events & Delegates

        /// <summary>
        /// this event is raised whether the data related to the GUI change
        /// </summary>
        public event EventHandler GUIDataChanged;

        #endregion

        #region Propreties

        /// <summary>
        /// Gets the name of type of link (For example: "myVerb (myFromNodeType -> myToNodeType)" )
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.verb + " (" + this.relatedNode1.TypeName + " -> " + this.relatedNode2.TypeName + ")"; 
            }
        }

        /// <summary>
        /// Gets or sets the list of all actions the user can perform on the link and there respectives arguments
        /// Each action is represented in XML (@see the ICE Data file)
        /// </summary>
        public List<string> Actions
        {
            get { return this.actions; }
            set { this.actions = value; }
        }

        /// <summary>
        /// Gets or sets the Xml definition of all useful information used to draw the link on the graph
        /// </summary>
        public string DrawingInformation
        {
            get 
            {
                return this.drawingInformation; 
            }

            set 
            {
                this.drawingInformation = value;
                if (this.GUIDataChanged != null)
                {
                    this.GUIDataChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the node has been disposed or not.
        /// </summary>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the model manager explored this node during the exploration process.
        /// </summary>
        /// <remarks>
        /// this property is return always false after the initialiation of the exploration process.
        /// </remarks>
        public bool IsExplored
        {
            get { return this.isExplored; }
            set { this.isExplored = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the node is visible on the graph or not.
        /// </summary>
        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.isVisible = value; }
        }

        /// <summary>
        /// Gets the link's ID
        /// </summary>
        public string ID
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets first node relative to the link
        /// </summary>
        public Node RelatedNode1
        {
            get { return this.relatedNode1; }
        }

        /// <summary>
        /// Gets second node relative to the link
        /// </summary>
        public Node RelatedNode2
        {
            get { return this.relatedNode2; }
        }

        /// <summary>
        /// Gets or sets the strength of the link
        /// </summary>
        /// <remarks>Strength is a percentage, values must be between 1 and 100</remarks>
        public float Strength
        {
            get
            {
                return this.strength;
            }

            set
            {
                this.strength = value;
            }
        }

        /// <summary>
        /// Gets or sets the links's style
        /// </summary>
        public string StyleName
        {
            get { return this.styleName; }
            set { this.styleName = value; }
        }

        /// <summary>
        /// Gets or sets the verb used in the link description
        /// </summary>
        public string Verb
        {
            get 
            {
                return this.verb; 
            }

            set
            { 
                this.verb = value;
                if (this.GUIDataChanged != null)
                {
                    this.GUIDataChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the complement used in the link description
        /// </summary>
        public string Complement
        {
            get 
            { 
                return this.complement; 
            }

            set 
            { 
                this.complement = value;
                if (this.GUIDataChanged != null)
                {
                    this.GUIDataChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the link's physical representation
        /// </summary>
        public Spring PhysicRepresentation
        {
            get { return this.physicRepresentation; }
            set { this.physicRepresentation = value; }
        }

        /// <summary>
        /// Gets the second node at end of the link
        /// </summary>
        /// <param name="node">Node at one end of the link</param>
        /// <returns>
        /// Node at the other end of the link
        /// If given node is not related to the link, returns null
        /// </returns>
        public Node GetTheOppositeNode(Node node)
        {
            if (node == this.relatedNode1)
            {
                return this.relatedNode2;
            }

            if (node == this.relatedNode2)
            {
                return this.relatedNode1;
            }

            return null;
        }

        /// <summary>
        /// Dispose of the link
        /// </summary>
        public void Dispose()
        {
            // Remove current link from link list of both nodes
            this.isDisposed = true;
            this.relatedNode1.LinkList.Remove(this);
            this.relatedNode2.LinkList.Remove(this);
            this.relatedNode1.IsDeployed = false;
            this.relatedNode2.IsDeployed = false;
            this.relatedNode1.SetRelativeMass();
            this.relatedNode2.SetRelativeMass();
            this.relatedNode1 = null;
            this.relatedNode2 = null;
            this.physicRepresentation.Dispose();
        }

        #endregion
    }
}
