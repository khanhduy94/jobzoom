//-----------------------------------------------------------------------
// <copyright file="ModelManager.cs" company="International Monetary Fund">
//   This file is part of "Information Connections Engine". See more information at http://ICEdotNet.codeplex.com
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
    using System.Windows.Threading;
    using System.Xml.Linq;
    using ICE.setting;

    /// <summary>
    /// This delegate is use to call every object that could provide an update to the model about a specific node.
    /// </summary>
    /// <param name="sender">sender of the event</param>
    /// <param name="node">node that should be updated</param>
    /// <param name="priority">the priority of the update</param>
    public delegate void NeedUpdateEventHandler(object sender, Node node, download.Priority priority);

    /// <summary>
    /// Representation of the relationnal graph (nodes and links)
    /// </summary>
    public class ModelManager
    {
        /// <summary>
        /// List of all nodes
        /// </summary>
        private List<Node> nodeList;

        /// <summary>
        /// List of all type of node in the model
        /// </summary>
        private Dictionary<string, model.NodeType> nodeTypeList;

        /// <summary>
        /// List of all links
        /// </summary>
        private List<Link> linkList;

        /// <summary>
        /// List of all type of link in the model
        /// </summary>
        private Dictionary<string, model.LinkType> linkTypeList;

        /// <summary>
        /// this property is the depth of exploration use to make difference between visible node and invisible node
        /// </summary>
        private int visibilityExplorationDepth = 0;

        /// <summary>
        /// This is the settings of the current model management.
        /// </summary>
        private setting.IModelSettings settings;

        /// <summary>
        /// the timer that update the model management system;
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// Initializes a new instance of the ModelManager class
        /// </summary>
        public ModelManager()
        {
            this.nodeList = new List<Node>();
            this.linkList = new List<Link>();
            this.nodeTypeList = new Dictionary<string, model.NodeType>();
            this.linkTypeList = new Dictionary<string, model.LinkType>();

            // create the timer
            this.timer = new DispatcherTimer();
        }

        #region Events & Delegates

        /// <summary>
        /// This event is raised whether the model need to update a node information
        /// </summary>
        public event NeedUpdateEventHandler NeedUpdate;

        /// <summary>
        /// This event is raised whether the list of node types changed
        /// </summary>
        public event EventHandler NodeTypesChanged;

        /// <summary>
        /// This event is raised whether the list of link types changed
        /// </summary>
        public event EventHandler LinkTypesChanged;

        #endregion

        /// <summary>
        /// Gets or sets the settings of the current model manager
        /// </summary>
        public setting.IModelSettings Settings
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

        /// <summary>
        /// Gets or sets the depth of exploration use to make difference between visible node and invisible node
        /// </summary>
        public int VisibilityExplorationDepth
        {
            get { return this.visibilityExplorationDepth; }
            set { this.visibilityExplorationDepth = value; }
        }

        /// <summary>
        /// Gets a list containing all node in the model
        /// </summary>
        public List<Node> NodeList
        {
            get { return new List<model.Node>(this.nodeList); }
        }

        /// <summary>
        /// Gets a list containing all known types of node
        /// </summary>
        public List<NodeType> NodeTypeList
        {
            get { return new List<NodeType>(this.nodeTypeList.Values); }
        }

        /// <summary>
        /// Gets the list of all links
        /// </summary>
        public List<Link> LinkList
        {
            get { return new List<model.Link>(this.linkList); }
        }

        /// <summary>
        /// Gets a list containing all known types of link
        /// </summary>
        public List<LinkType> LinkTypeList
        {
            get { return new List<LinkType>(this.linkTypeList.Values); }
        }

        /// <summary>
        /// Creates and returns a node
        /// </summary>
        /// <remarks>
        /// If a node with same ID already exist, modifies existing node with the given values and returns it
        /// </remarks>
        /// <param name="id">ID of the node</param>
        /// <param name="style">Style name</param>
        /// <param name="type">type of the node</param>
        /// <param name="title">title of the node</param>
        /// <param name="actions">list of actions and respective parameters we can use on the node (XML)</param>
        /// <param name="drawingInformation">drawing information (XML)</param>
        /// <param name="refreshRate">refresh rate in millisecond</param>
        /// <param name="relativeSize">size coeficient to applied the node</param>
        /// <param name="url">url use to download this node</param>
        /// <param name="isDownloaded">true if the node has been downloaded</param>
        /// <returns>the model representation of our node</returns>
        public Node CreateNode(
            string id,
            string style,
            string type,
            string title,
            List<string> actions,
            string drawingInformation,
            int refreshRate,
            double relativeSize,
            string url,
            bool isDownloaded)
        {
            bool nodeExist = false;
            Node node = null;

            // unicity constrain
            foreach (Node n in new List<Node>(this.nodeList))
            {
                if (id == n.ID) 
                {
                    nodeExist = true;
                    node = n;
                }
            }

            if (! nodeExist)
            {
                // creating node
                node = new Node(id);
            }

            if (actions != null)
            {
                node.Actions = actions;
            }

            if (drawingInformation != null)
            {
                node.DrawingInformation = drawingInformation;
            }
            
            node.RefreshRate = refreshRate;

            if (style != null)
            {
                node.StyleName = style;
            }
            
            node.TypeName = type;
            this.GetNodeType(node.TypeName);

            if (title != null)
            {
                node.Title = title;
            }

            node.Url = url;

            if (relativeSize >= 0)
            {
                node.RelativeSize = relativeSize;
            }
            
            node.IsDeployed |= isDownloaded;

            if (! nodeExist)
            {
                // we add the node to the model
                node.SelectionChanged += new EventHandler(this.Node_SelectionChanged);
                this.nodeList.Add(node);
            }

            return node;
        }

        /// <summary>
        /// Creates and returns a link but doesn't add it to the current model
        /// </summary>
        /// <remarks>
        /// If a link with same ID already exist, modifies existing link with the given values and returns it
        /// You must add the link to the model after creation.
        /// </remarks>
        /// <param name="id">ID of the link</param>
        /// <param name="nodeFrom">ID of the first node relative to the link</param>
        /// <param name="nodeTo">ID of the second node relative to the link</param>
        /// <param name="actions">List of all action that could be performed on the link (XML)</param>
        /// <param name="drawingInformation">An Xml representation of all information needed to draw a link on the graph</param>
        /// <param name="strength">Strength between the two nodes</param>
        /// <param name="style">Style of the link</param>
        /// <param name="verb">Verb used in the link description</param>
        /// <param name="complement">Complement used in the link description</param>
        /// <returns>Created link</returns>
        public Link CreateLink(
                string id,
                string nodeFrom,
                string nodeTo,
                List<string> actions,
                string drawingInformation,
                float strength,
                string style,
                string verb,
                string complement)
        {
            bool linkExist = false;
            Link link = null;

            // make sure this link is unique
            foreach (Link otherLink in new List<Link>(this.linkList))
            {
                if (otherLink.ID.Equals(id))
                {
                    // if the link already exist
                    linkExist = true;
                    link = otherLink;
                }
            }

            if (!linkExist)
            {
                // get related nodes
                Node node1 = null;
                Node node2 = null;
                foreach (Node node in new List<Node>(this.nodeList))
                {
                    if (node.ID.Equals(nodeFrom))
                    {
                        node1 = node;
                    }

                    if (node.ID.Equals(nodeTo))
                    {
                        node2 = node;
                    }
                }

                if (node1 == null)
                {
                    throw new ArgumentException("the link \"" + id + "\" cannot be create. (the value of property \"From\" isn't a valid node ID)");
                }
                
                if (node2 == null)
                {
                    throw new ArgumentException("the link \"" + id + "\" cannot be create. (the value of property \"To\" isn't a valid node ID)");
                }

                // creating the link
                link = new Link(id, node1, node2);

                // we add it to the model
                this.linkList.Add(link);
            }

            link.Actions = actions;
            link.DrawingInformation = drawingInformation;
            link.Strength = strength;
            link.StyleName = style;
            link.Verb = verb;
            this.GetLinkType(link);
            link.Complement = complement;

            return link;
        }

        /// <summary>
        /// this function increase the depth of the graph
        /// </summary>
        public void IncreaseDepth()
        {
            this.visibilityExplorationDepth++;
        }

        /// <summary>
        /// this function decrease the depth of the graph
        /// </summary>
        public void DecreaseDepth()
        {
            this.visibilityExplorationDepth--;

            if (this.visibilityExplorationDepth < 0)
            {
                this.visibilityExplorationDepth = 0;
            }
        }

        /// <summary>
        /// This function start the model management thread
        /// </summary>
        public void Start()
        {
            // initilising the update timer
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            this.timer.Tick += new EventHandler(this.Update);
            this.timer.Start();
        }

        /// <summary>
        /// This function pause the model management thread
        /// </summary>
        public void Pause()
        {
            this.timer.Stop();
        }

        /// <summary>
        /// This function resume the model management thread
        /// </summary>
        public void Resume()
        {
            this.timer.Start();
        }

        /// <summary>
        /// This function dipose the object in argument
        /// </summary>
        /// <param name="id">ID reference to the object to dispose</param>
        public void DisposeObject(string id)
        {
            foreach (Node node in this.NodeList)
            {
                if (node.ID == id)
                {
                    node.Dispose();
                }
            }

            foreach (Link link in this.LinkList)
            {
                if (link.ID == id)
                {
                    link.Dispose();
                }
            }
        }

        /// <summary>
        /// this function is called when a node selection status change
        /// </summary>
        /// <param name="sender">the node itself</param>
        /// <param name="e"> the event argument</param>
        private void Node_SelectionChanged(object sender, EventArgs e)
        {
            Node node = (Node)sender;
            if (this.settings.MultiSelectionNodeMode)
            {
                /* there is no restriction at all */
            }
            else
            {
                // only one node can be selected at once
                if (node.IsSelected)
                {
                    List<Node> copylist = this.NodeList;

                    copylist.Remove(node);
                    foreach (Node otherNode in copylist)
                    {
                        otherNode.IsSelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// this function is called when the settings has changed
        /// </summary>
        /// <param name="sender">the settings</param>
        /// <param name="e">event argument</param>
        private void Settings_Changed(object sender, EventArgs e)
        {
            this.visibilityExplorationDepth = this.settings.InitialGraphVisibilityDepth;
        }

        /// <summary>
        /// this function manage the current model
        /// </summary>
        /// <param name="sender">the timer of the physics engine</param>
        /// <param name="e">the event argument</param>
        private void Update(object sender, EventArgs e)
        {
            // Step1 : set visibility of all node and link
            this.SetVisibility();

            // Step2 : dispose useless information from the model
            this.CleanUp();

            // Step3 : preload all neighbor node & update node if necessary
            this.UpdateModelInformation();
        }

        /// <summary>
        /// This function clean the model from useless nodes
        /// </summary>
        private void CleanUp()
        {
            this.Explore(this.visibilityExplorationDepth + this.settings.CleanUpAdditionalDepth, false);

            List<Node> toDispose = new List<Node>();

            foreach (Node node in new List<Node>(this.nodeList))
            {
                if (!node.IsExplored)
                {
                    this.nodeList.Remove(node);
                    toDispose.Add(node);
                }
            }

            foreach (Link link in new List<Link>(this.linkList))
            {
                if (!link.IsExplored)
                {
                    this.linkList.Remove(link);
                }
            }

            foreach (Node node in toDispose)
            {
                node.Dispose();
            }
        }

        /// <summary>
        /// this function is a part of the update process.
        /// This function ask each node if it's information is up to date.
        /// If not, the function ask for un update with the "NeedUpdate" event
        /// </summary>
        private void UpdateModelInformation()
        {
            foreach (Node node in new List<Node>(this.nodeList))
            {  
                if (node.IsVisible)
                {
                    if (this.nodeList.Count <= this.settings.MaximumNodeLimit)
                    {
                        if (!node.IsDeployed)
                        {
                            // we ask for an update of "node"
                            if (this.NeedUpdate != null)
                            {
                                this.NeedUpdate(this, node, download.Priority.Required);
                            }
                        }
                    }
                    if (!node.IsUpToDate)
                    {
                        // we ask for an update of "node"
                        if (this.NeedUpdate != null)
                        {
                            this.NeedUpdate(this, node, download.Priority.Desired);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// this function is a part of the update process.
        /// this function sets the visibility flag to true or false by exploring the model from the selected node with the visibility depth.
        /// </summary>
        private void SetVisibility()
        {
            this.Explore(this.visibilityExplorationDepth, true);

            foreach (Node node in new List<Node>(this.nodeList))
            {
                node.IsVisible = node.IsExplored && this.GetNodeType(node.TypeName).IsVisible;

                // set node physic model enable or disable (Less CPU ressource used)
                node.PhysicRepresentation.IsEnable = node.IsExplored;
            }

            foreach (Link link in new List<Link>(this.linkList))
            {
                link.IsVisible = link.IsExplored && this.GetLinkType(link).IsVisible;

                // set node physic model enable or disable (Less CPU ressource used)
                link.PhysicRepresentation.IsEnable = link.IsExplored;
            }
        }

        /// <summary>
        /// this function explore the model from the selected nodes and update the explore flags of all node and link
        /// </summary>
        /// <param name="depth">the depth of the exploration</param>
        /// <param name="useFilter">True if you this exploration use the type filter</param>
        private void Explore(int depth, bool useFilter)
        {
            // this is the a copy of the list of all node in the model at the begining of the function.
            // we use a copy because we don't want to have bad interactions with another thread.
            List<Node> nodeList = new List<Node>(this.nodeList);

            // this is the a copy of the list of all node in the model at the begining of the function.
            // we use a copy because we don't want to have bad interactions with another thread.
            List<Link> linkList = new List<Link>(this.linkList);

            // First, we initializes the exploration by set all exploration flags to false
            foreach (Node node in nodeList)
            {
                node.IsExplored = false;
            }

            foreach (Link link in linkList)
            {
                link.IsExplored = false;
            }

            // List of all nodes already explored
            List<Node> exploredNodeList = new List<Node>();

            // List of all links already explored
            List<Link> exploredLinkList = new List<Link>();

            // Initialization
            if (depth >= 0)
            {
                // Adds all central nodes to the list of already explored nodes
                foreach (Node node in nodeList)
                {
                    if (node.IsSelected)
                    {
                        exploredNodeList.Add(node);
                    }
                }
            }

            // Iteratively adding nodes by increasing depth
            for (int currentDepth = 1; currentDepth <= depth; currentDepth++)
            {
                // List of all nodes currently being explored
                List<Node> nextStepNodeList = new List<Node>();

                // For each node that was already explored, we get the neighbours
                foreach (Node currentNode in exploredNodeList)
                {
                    // Ensures we don't get back in the model exploration
                    // by adding all already explored nodes to current list of nodes in exploration
                    if (!nextStepNodeList.Contains(currentNode))
                    {
                        nextStepNodeList.Add(currentNode);
                    }

                    // Explores current node neighbors
                    List<Link> linkToExploreList = new List<Link>(currentNode.LinkList);

                    foreach (Link currentLink in linkToExploreList)
                    {
                        // If the type of the link is hide, then continue
                        if (useFilter && (!this.GetLinkType(currentLink).IsEnable))
                        {
                            continue;
                        }

                        // Gets the other node at the end of the link
                        Node n = currentLink.GetTheOppositeNode(currentNode);

                        // If the type of the related node is hide, then continue
                        if (useFilter && (!this.GetNodeType(n.TypeName).IsEnable))
                        {
                            continue;
                        }

                        // Ensures we don't get back in the model exploration
                        // If neighbour node was already explored, continue the loop
                        if (n == null || nextStepNodeList.Contains(n)) 
                        {
                            continue; 
                        }

                        // Add neighbour node to the list of explored nodes
                        nextStepNodeList.Add(n);

                        // foreach (Link currentLink in LinkList)
                    }

                    // foreach (Node currentNode in currentNodeList.Values)
                }

                // when all previous list of nodes is explored,
                // update the list with newly explored nodes
                exploredNodeList = nextStepNodeList;

                // for (int currentDepth = 1; currentDepth < depth; currentDepth++)
            }

            // when final depth is reached, returns explored nodes and links
            foreach (Node node in exploredNodeList)
            {
                node.IsExplored = true;

                foreach (Link link in node.LinkList)
                {
                    // If the type of the link is hide, then continue
                    if (useFilter && (!this.GetLinkType(link).IsEnable))
                    {
                        continue;
                    }

                    if (link.RelatedNode1.IsExplored && link.RelatedNode2.IsExplored)
                    {
                        link.IsExplored = true;
                    }
                }
            }
        }

        /// <summary>
        /// This function returns the node type state from the name in argument
        /// </summary>
        /// <param name="typeName">name of the type you need</param>
        /// <returns>the type of node you asked for</returns>
        private NodeType GetNodeType(string typeName)
        {
            if (!this.nodeTypeList.ContainsKey(typeName))
            {
                this.nodeTypeList.Add(typeName, new NodeType(typeName));

                if (this.NodeTypesChanged != null)
                {
                    this.NodeTypesChanged(this, new EventArgs());
                }
            }

            return this.nodeTypeList[typeName];
        }

        /// <summary>
        /// This function returns the link type state from the name in argument
        /// </summary>
        /// <param name="link">name of the type you need</param>
        /// <returns>the type of link you asked for</returns>
        private model.LinkType GetLinkType(Link link)
        {
            if (!this.linkTypeList.ContainsKey(link.TypeName))
            {
                this.linkTypeList.Add(link.TypeName, new LinkType(link.Verb, link.RelatedNode1.TypeName, link.RelatedNode2.TypeName));

                if (this.LinkTypesChanged != null)
                {
                    this.LinkTypesChanged(this, new EventArgs());
                }
            }

            return this.linkTypeList[link.TypeName];
        }
    }
}
