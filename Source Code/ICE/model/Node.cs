//-----------------------------------------------------------------------
// <copyright file="Node.cs" company="International Monetary Fund">
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
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Xml.Linq;
    using ICE.physics;
    using ICE.setting;
    using ICE.view;
    
    /// <summary>
    /// Data for a node
    /// </summary>
    public class Node
    {
        #region Fields

        /// <summary>
        /// List of all links to other nodes
        /// </summary>
        private List<Link> linkList = new List<Link>();

        /// <summary>
        /// Node's style
        /// </summary>
        private string styleName = string.Empty;

        /// <summary>
        /// Node's physical representation
        /// </summary>
        /// TODO : move away ?
        private physics.Particle physicRepresentation;

        /// <summary>
        /// The node's ID
        /// </summary>
        private string id;

        /// <summary>
        /// Type of the node (used with links to explain relations between nodes)
        /// </summary>
        private string type;

        /// <summary>
        /// Name of the node
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// List off all repulsion forces appied to the node
        /// </summary>
        private Dictionary<Node, Attraction> repulsionsList = new Dictionary<Node, Attraction>();
        
        /// <summary>
        /// Define whether the node is selected or not
        /// </summary>
        private bool isSelected = false;

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
        /// URL use to download node informations
        /// </summary>
        private string url;

        /// <summary>
        /// this date value is true if the node file is downloaded and all related links and node are présent in the model.
        /// </summary>
        private bool isDeployed = false;

        /// <summary>
        /// Define whether the node has been disposed or not
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Define whether the node can be disposed or not
        /// </summary>
        private bool isDisposable = true;

        /// <summary>
        /// This is the list of all actions the user can perform on the node and there respectives arguments
        /// Each action is represented in XML (@see the ICE Data file)
        /// </summary>
        private List<string> actions = new List<string>();

        /// <summary>
        /// This is the refresh rate (in millisecond)
        /// </summary>
        /// <remarks>
        /// if the refresh rate is a negatif number the refreshing mode is disabled
        /// </remarks>
        private int refreshRate = -1;

        /// <summary>
        /// This is the last time we doznload the file of the node
        /// </summary>
        private DateTime downloadTime = DateTime.MinValue;

        /// <summary>
        /// This is the Xml definition of all useful information used to draw the node on the graph
        /// </summary>
        private string drawingInformation = "<" + xml.DataXmlContent.DrawingInformationElementOfNodeElementName + " xmlns=\"" + xml.DataXmlContent.Namespace.NamespaceName + "\" />";

        /// <summary>
        /// this is the size multiplicator set by the data provider
        /// </summary>
        private double relativeSize = 1;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the Node class
        /// </summary>
        /// <param name="id">ID of the new node</param>
        public Node(string id) 
        {
            this.id = id;
        }

        #endregion

        #region Events & Delegates

        /// <summary>
        /// This event is raised whether the data related to the GUI interface change
        /// </summary>
        public event EventHandler GUIDataChanged;

        /// <summary>
        /// This event is raised whether the selection flag change
        /// </summary>
        public event EventHandler SelectionChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the size multiplicator
        /// </summary>
        public double RelativeSize
        {
            get 
            { 
                return this.relativeSize; 
            }

            set 
            {
                this.relativeSize = value;
                if (this.GUIDataChanged != null)
                {
                    this.GUIDataChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets or sets the Xml definition of all useful information used to draw the node on the graph
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
        /// Gets or sets the refresh rate (in millisecond)
        /// </summary>
        /// <remarks>
        /// if the refresh rate is a negatif number the refreshing mode is disabled
        /// </remarks>
        public int RefreshRate
        {
            get { return this.refreshRate; }
            set { this.refreshRate = value; }
        }

        /// <summary>
        /// Gets or sets the list of all actions the user can perform on the node and there respectives arguments
        /// Each action is represented in XML (@see the ICE Data file)
        /// </summary>
        public List<string> Actions
        {
            get { return this.actions; }
            set { this.actions = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the node has been disposed or not.
        /// </summary>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the node can be disposed or not.
        /// </summary>
        public bool IsDisposable
        {
            get { return this.isDisposable; }
            set { this.isDisposable = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the node information are up to date or not.
        /// </summary>
        public bool IsUpToDate
        {
            get
            {
                if (this.refreshRate >= 0)
                {
                    int cmp = DateTime.Compare(this.downloadTime.AddMilliseconds(this.refreshRate), DateTime.Now);
                    return cmp >= 0;
                }
                else
                {
                    return true;
                }
            }
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
        /// Gets or sets a value indicating whether the node is selected or not.
        /// </summary>
        public bool IsSelected
        {
            get 
            {
                return this.isSelected; 
            }

            set 
            {
                bool oldValue = this.isSelected;
                this.isSelected = value;

                if (oldValue != this.isSelected)
                {
                    if (this.physicRepresentation != null)
                    {
                        // we set the pin situation according to the selection situation
                        if (this.isSelected)
                        {
                            this.physicRepresentation.MakeFixed();
                        }
                        else
                        {
                            this.physicRepresentation.MakeFree();
                        }
                    }

                    if (this.SelectionChanged != null)
                    {
                        this.SelectionChanged(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets list of links to other nodes
        /// </summary>
        public List<Link> LinkList
        {
            get 
            { 
                return this.linkList; 
            }

            set 
            { 
                this.linkList = value;
            }
        }

        /// <summary>
        /// Gets or sets the node's style
        /// </summary>
        public string StyleName
        {
            get { return this.styleName; }
            set { this.styleName = value; }
        }

        /// <summary>
        /// Gets or sets the node's physical representation
        /// </summary>
        public physics.Particle PhysicRepresentation
        {
            get { return this.physicRepresentation; }
            set { this.physicRepresentation = value; }
        }

        /// <summary>
        /// Gets the node's ID
        /// </summary>
        public string ID
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets or sets the type of the node
        /// </summary>
        public string TypeName
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// Gets or sets the name of the node (shown on the graph)
        /// </summary>
        public string Title 
        {
            get 
            { 
                return this.title; 
            }

            set 
            { 
                this.title = value;
                if (this.GUIDataChanged != null)
                {
                    this.GUIDataChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets the list of all nodes linked to the current node
        /// </summary>
        public List<Node> RelatedNodeList
        {
            get 
            {
                List<Node> nodes = new List<Node>();
                foreach (Link link in this.linkList)
                {
                    nodes.Add(link.GetTheOppositeNode(this));
                }

                return nodes;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the node is fully deployed in the model
        /// </summary>
        public bool IsDeployed
        {
            get 
            {
                return this.isDeployed;
            }

            set 
            {
                this.isDeployed = value;
                if (value)
                {
                    this.downloadTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Gets or sets the url use to download the node's information
        /// </summary>
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// This function dispose the node and it's physic representation
        /// </summary>
        public void Dispose()
        {
            if (this.isDisposable)
            {
                this.isDisposed = true;

                foreach (Link link in new List<Link>(this.linkList))
                {
                    link.Dispose();
                }

                this.physicRepresentation.Dispose();
            }
        }

        /// <summary>
        /// Sets the given repulsion force between current node and given node
        /// </summary>
        /// <param name="node">Node with which the repulsion with apply</param>
        /// <param name="repulsion">Repulsion force to apply</param>
        /// TODO : move away ?
        public void SetRepulsion(Node node, Attraction repulsion)
        {
            // if there already was a repulsion force between the two nodes,
            // we need to turn it off.
            if (this.repulsionsList.ContainsKey(node))
            {
                this.repulsionsList[node].Dispose();
            }

            // sets the repulsion force
            // (as repulsionList is a dictionnary, the entry will be added if it doesn't already exist)
            this.repulsionsList[node] = repulsion;
        }

        /// <summary>
        /// Gets the repulsion force between current node and given node
        /// </summary>
        /// <param name="node">Second node for repulsion force</param>
        /// <returns>Repulsion force between current node and given node</returns>
        /// TODO : move away ?
        public Attraction GetRepulsion(Node node)
        {
            // the method is not protected because
            // we assume here that there will always be an entry for the node
            // (as all nodes repulse each other)
            return this.repulsionsList[node];
        }

        /// <summary>
        /// Sets the mass of the node relatively to the number of links
        /// </summary>
        /// <remarks>
        /// Node mass increases with its number of links to assure system stability
        /// </remarks>
        /// TODO : move away ?
        public void SetRelativeMass()
        {
            this.physicRepresentation.Mass =
                PhysicsConstants.ParticleDefaultMass
                + (float)System.Math.Log(PhysicsConstants.ParticleDefaultMass * this.linkList.Count);
        }

        #endregion    
    }
}
