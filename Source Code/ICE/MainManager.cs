//-----------------------------------------------------------------------
// <copyright file="MainManager.cs" company="International Monetary Fund">
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

namespace ICE
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using System.Xml.Linq;

    /// <summary>
    /// this classe is the principale manager of the graph
    /// </summary>
    public class MainManager
    {
        #region Fields

        /// <summary>
        /// this is the settings of the application
        /// </summary>
        private setting.Settings settings;

        /// <summary>
        /// if the initialization process failed
        /// </summary>
        private bool initializationFailed;
        
        /// <summary>
        /// the random number generator
        /// </summary>
        private Random random;

        /// <summary>
        /// the javascript manager
        /// </summary>
        private JavaScriptManager javaScriptManager;

        /// <summary>
        /// this field is the key management system
        /// </summary>
        private KeyManager keyManager;

        /// <summary>
        /// the physics system
        /// </summary>
        private physics.PhysicsManager physicsManager;

        /// <summary>
        /// the model management system
        /// </summary>
        private model.ModelManager modelManager;

        /// <summary>
        /// the view management system
        /// </summary>
        private view.ViewManager viewManager;

        /// <summary>
        /// the assembly management system
        /// </summary>
        private ExternalAssemblyManager assemblyManager;
        
        /// <summary>
        /// this field is the loading management system
        /// </summary>
        private download.FileLoadManager fileDownloadManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainManager class.
        /// </summary>
        /// <param name="initParams">List of all initialisation parameters</param>
        /// <param name="loaderType">type of loader use to access to the file describes in initParams</param>
        public MainManager(IDictionary<string, string> initParams, Type loaderType)
        {
            // creating the assembly manager
            this.assemblyManager = new ExternalAssemblyManager();

            // creating the model
            this.modelManager = new model.ModelManager();

            // Initiate the download manager
            this.InitalizeFileDownloadManager(loaderType);

            // creating the view from model and style
            this.InitializeViewManager();

            // set Splash Screen
            this.viewManager.ShowSplachScreen();

            // set the Javascript incomming event
            this.javaScriptManager = new JavaScriptManager(this);
   
            // creating the random engine
            this.random = new Random(DateTime.Now.Millisecond);
            
            // creating the physic system
            this.physicsManager = new physics.PhysicsManager(this);

            // creating the key manager
            this.keyManager = new KeyManager(this);

            // create and set the default settings
            this.Settings = new setting.Settings();

            // add Init URI
            this.AddInitUrlToDownloadManager(initParams);

            // start the initialization process
            this.initializationFailed = false;

            // run sub-tasks
            this.modelManager.Start();
            this.physicsManager.Start();
            this.viewManager.Start();
            this.fileDownloadManager.Start();
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the downloading management system
        /// </summary>
        public download.FileLoadManager FileDownloadManager
        {
            get { return this.fileDownloadManager; }
        }

        /// <summary>
        /// Gets the assembly management system
        /// </summary>
        public ExternalAssemblyManager AssemblyManager
        {
            get { return this.assemblyManager; }
        }

        /// <summary>
        /// Gets the view
        /// </summary>
        public view.ViewManager ViewManager
        {
            get { return this.viewManager; }
        }

        /// <summary>
        /// Gets the model
        /// </summary>
        public model.ModelManager ModelManager
        {
            get { return this.modelManager; }
        }

        /// <summary>
        /// Gets or sets the key management system
        /// </summary>
        public KeyManager KeyManager
        {
            get { return this.keyManager; }
            set { this.keyManager = value; }
        }

        /// <summary>
        /// Gets or sets the current settings of the application
        /// </summary>
        public setting.Settings Settings
        {
            get
            {
                return this.settings;
            }

            set
            {
                this.settings = value;
                this.viewManager.Settings = value;
                this.physicsManager.Settings = value;
                this.modelManager.Settings = value;
                this.fileDownloadManager.Settings = value;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// this function createsa node, it's view and it's physics representation
        /// </summary>
        /// <remarks>if the node already exist, the function return the existing one</remarks>
        /// <param name="id">the ID of the node</param>
        /// <param name="style">the Style name</param>
        /// <param name="type">the type of the node</param>
        /// <param name="title">the title of the node</param>
        /// <param name="actions">the list of actions and respective parameters we can use on the node (XML)</param>
        /// <param name="drawingInformation">the drawing information (XML)</param>
        /// <param name="refreshRate">the refresh rate in millisecond</param>
        /// <param name="url">the url use to download this node</param>
        /// <param name="isDownloaded">true if the node has been downloaded. This variable is use to avoid to redownload the node definition during runtime</param>
        /// <param name="x">X coordonate</param>
        /// <param name="y">Y coordonate</param>
        /// <param name="z">Z coordonate</param>
        /// <param name="relativeSize">the double value use to change the size of the current node on the graph</param>
        /// <returns>the created node</returns>
        public model.Node CreateNode(
                string id,
                string style,
                string type,
                string title,
                List<string> actions,
                string drawingInformation,
                int refreshRate,
                string url,
                bool isDownloaded,
                float x,
                float y,
                float z,
                double relativeSize) 
        {
            model.Node node = this.modelManager.CreateNode(id, style, type, title, actions, drawingInformation, refreshRate, relativeSize, url, isDownloaded);

            // set the physic representation of our link
            this.physicsManager.AddPhysicRepresentation(x, y, z, node, this.modelManager);

            // set view to node
            this.viewManager.SetViewToNode(node);

            return node;
        }

        /// <summary>
        /// this function createsa node next to an over node.
        /// </summary>
        /// <remarks>if the node already exist, the function return the existing one</remarks>
        /// <param name="id">the ID of the node</param>
        /// <param name="style">the Style name</param>
        /// <param name="type">the type of the node</param>
        /// <param name="title">the title of the node</param>
        /// <param name="actions">the list of actions and respective parameters we can use on the node (XML)</param>
        /// <param name="drawingInformation">the drawing information (XML)</param>
        /// <param name="refreshRate">the refresh rate in millisecond</param>
        /// <param name="url">the url use to download this node</param>
        /// <param name="isDownloaded">true if the node has been downloaded. This variable is use to avoid to redownload the node definition during runtime</param>
        /// <param name="relativeSize">the size multiplicator defined in the node Xml definition</param>
        /// <param name="neighbor">node's neighbor</param>
        /// <returns>return new node</returns>
        public model.Node CreateNode(
                string id,
                string style,
                string type,
                string title,
                List<string> actions,
                string drawingInformation,
                int refreshRate,
                string url,
                bool isDownloaded,
                double relativeSize,
                model.Node neighbor)
        {
            float x = neighbor.PhysicRepresentation.Position.X + ((((float)this.random.NextDouble()) * 0.2f) - 0.1f);
            float y = neighbor.PhysicRepresentation.Position.Y + ((((float)this.random.NextDouble()) * 0.2f) - 0.1f);
            float z = neighbor.PhysicRepresentation.Position.Z;
            return this.CreateNode(id, style, type, title, actions, drawingInformation, refreshRate, url, isDownloaded, x, y, z, relativeSize);
        }

        /// <summary>
        /// this function creates the model representation of a link, the physic model of the link and the view of the link
        /// It also create the repulsion between the Node 2 in argument and all the others
        /// </summary>
        /// <remarks>the repulsion implementation could be extract from this methode in the future</remarks>
        /// <param name="id">ID of the link</param>
        /// <param name="nodeFrom">ID of the first node relative to the link</param>
        /// <param name="nodeTo">ID of the second node relative to the link</param>
        /// <param name="actions">List of all action that could be performed on the link (XML)</param>
        /// <param name="drawingInformation">An Xml representation of all information needed to draw a link on the graph</param>
        /// <param name="strength">Strength between the two nodes</param>
        /// <param name="style">Style of the link</param>
        /// <param name="verb">Verb used in the link description</param>
        /// <param name="complement">Complement used in the link description</param>
        /// <returns>the new link model representation</returns>
        public model.Link CreateLink(
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
            model.Link link = null;

            try
            {
                // link creation
                link = this.modelManager.CreateLink(id, nodeFrom, nodeTo, actions, drawingInformation, strength, style, verb, complement);

                // create the link in the physics engine
                this.physicsManager.AddPhysicRepresentation(link);

                // create link in the graph
                this.viewManager.SetViewToLink(link);

                return link;
            }
            catch (Exception error)
            {
                if (link != null)
                {
                    link.Dispose();
                }

                this.viewManager.AddDebugMessage("Create link: " + error.Message);
                return null;
            }
        }

        /// <summary>
        /// This function returns the main visual component of ICE
        /// </summary>
        /// <returns>the container (Grid) in whitch we draw the graph</returns>
        public UIElement GetICEView()
        {
            var phuc = this.viewManager.GetView();
            return this.viewManager.GetView();
        }

        /// <summary>
        /// This function adds listening of a page in argument
        /// </summary>
        /// <param name="page">the page to listen</param>
        public void ListenKeyPress(Page page)
        {
            this.keyManager.ListenKeyPress(page);
        }

        #endregion 

        #region Private Functions

        /// <summary>
        /// this function apply the Xml content to the model
        /// </summary>
        /// <param name="document">the Xml document</param>
        /// <param name="selectTheCurrentNode">if you want to select the main node</param>
        private void ApplyXmlRelationsFile(XDocument document, bool selectTheCurrentNode)
        {
            XElement element = document.Root.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.CurrentNodeElementName).Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.NodeElementName);
            
            // creating the main node
            model.Node currentNode;
            if (this.ModelManager.NodeList.Count == 0)
            {
                // creating the node on 000
                currentNode = this.CreateNode(element, true, 0, 0, 0);
            }
            else
            {
                // creating the node next to another node
                currentNode = this.CreateNode(element, true, this.ModelManager.NodeList[0]);
            }

            if (selectTheCurrentNode)
            {
                currentNode.IsSelected = true;
            }

            // creating neighbor nodes
            foreach (XElement nodeElement in document.Root.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.NeighborsElementName).
                Elements(xml.DataXmlContent.Namespace + xml.DataXmlContent.NodeElementName))
            {
                model.Node node = this.CreateNode(nodeElement, false, currentNode);
                foreach (XElement linkElement in document.Root.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.CurrentNodeElementName).
                    Elements(xml.DataXmlContent.Namespace + xml.DataXmlContent.LinkElementName))
                {
                    string fieldFrom = linkElement.Attribute(xml.DataXmlContent.FromAttributeOfLinkElementName).Value;
                    string fieldTo = linkElement.Attribute(xml.DataXmlContent.ToAttributeOfLinkElementName).Value;

                    if (fieldFrom != node.ID && fieldTo != node.ID)
                    {
                        continue;
                    }

                    if (fieldFrom == fieldTo)
                    {
                        this.viewManager.AddDebugMessage("This engine can't process a node that points to itself (node \"" + node.ID + "\")");
                        continue;
                    }

                    this.CreateLink(linkElement);
                }
            }
        }

        /// <summary>
        /// this function createsa link from its xml representation
        /// </summary>
        /// <param name="linkElement">the xml representation of a link</param>
        /// <returns>The link model representation (created or updated according to the xml argument)</returns>
        private model.Link CreateLink(XElement linkElement)
        {
            string id = linkElement.Attribute(xml.DataXmlContent.IDAttributeOfLinkElementName).Value;
            string from = linkElement.Attribute(xml.DataXmlContent.FromAttributeOfLinkElementName).Value;
            string to = linkElement.Attribute(xml.DataXmlContent.ToAttributeOfLinkElementName).Value;

            float strength = 50;
            XElement strengthElement = linkElement.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.StrengthElementOfLinkElementName);
            if (strengthElement != null)
            {
                strength = Single.Parse(strengthElement.Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            string style = string.Empty;
            XElement styleElement = linkElement.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.StyleElementOfLinkElementName);
            if (styleElement != null)
            {
                style = styleElement.Value;
            }

            XElement grammarElement = linkElement.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.GrammarElementOfLinkElementName);
            string verb = grammarElement.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.VerbElementOfGrammarElementName).Value;
            string complement = grammarElement.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.ComplementElementOfGrammarElementName).Value;

            List<string> actions = new List<string>();
            XElement actionsElement = linkElement.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.ActionsElementOfLinkElementName);
            if (actionsElement != null)
            {
                foreach (XElement action in actionsElement.Elements(xml.DataXmlContent.Namespace + xml.DataXmlContent.ActionElementName))
                {
                    actions.Add(action.ToString());
                } 
            }

            string drawingInformation = "<" + xml.DataXmlContent.DrawingInformationElementOfNodeElementName + " xmlns=\"" + xml.DataXmlContent.Namespace + "\"/>";
            XElement drawingInformationElement = linkElement.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.DrawingInformationElementOfLinkElementName);
            if (drawingInformationElement != null)
            {
                drawingInformation = drawingInformationElement.ToString();
            }

            return this.CreateLink(id, from, to, actions, drawingInformation, strength, style, verb, complement);
        }

        /// <summary>
        /// this function createsa node from its Xml representation
        /// </summary>
        /// <param name="element">the XElement</param>
        /// <param name="isDownloaded">true if the node has been downloaded. This variable is use to avoid to redownload the node definition during runtime</param>
        /// <param name="x">the X coordonate</param>
        /// <param name="y">the Y coordonate</param>
        /// <param name="z">the Z coordonate</param>
        /// <returns>the node created</returns>
        private model.Node CreateNode(XElement element, bool isDownloaded, float x, float y, float z)
        {
            string id = element.Attribute(xml.DataXmlContent.IDAttributeOfNodeElementName).Value;

            string title = null;
            XElement titleElement = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.TitleElementOfNodeElementName);
            if (titleElement != null)
            {
                title = titleElement.Value;
            }

            string style = null;
            XElement styleElement = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.StyleElementOfNodeElementName);
            if (styleElement != null)
            {
                style = styleElement.Value;
            }

            string type = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.TypeElementOfNodeElementName).Value;
            string url = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.URLElementOfNodeElementName).Value;

            List<string> actions = null;
            XElement actionsElement = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.ActionsElementOfNodeElementName);
            if (actionsElement != null)
            {
                actions = new List<string>();
                foreach (XElement action in actionsElement.Elements(xml.DataXmlContent.Namespace + xml.DataXmlContent.ActionElementName))
                {
                    actions.Add(action.ToString());
                }
            }

            string drawingInformation = null;
            XElement drawingInformationElement = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.DrawingInformationElementOfNodeElementName);
            if (drawingInformationElement != null)
            {
                drawingInformation = drawingInformationElement.ToString();
            }

            int refreshRate = -1;
            XElement refreshRateElement = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.RefreshRateElementOfNodeElementName);
            if (refreshRateElement != null)
            {
                refreshRate = int.Parse(refreshRateElement.Value, System.Globalization.CultureInfo.InvariantCulture);
            }

            double relativeSize = 1d;
            XElement relativeSizeElement = element.Element(xml.DataXmlContent.Namespace + xml.DataXmlContent.RelativeSizeElementOfNodeElementName);
            if (relativeSizeElement != null)
            {
                relativeSize = xml.SettingsXmlContent.ParseToDouble(relativeSizeElement.Value);
            }

            return this.CreateNode(id, style, type, title, actions, drawingInformation, refreshRate, url, isDownloaded, x, y, z, relativeSize);
        }

        /// <summary>
        /// this function createsa node from its xml representation and from its neighbor
        /// </summary>
        /// <param name="element">the xml element</param>
        /// <param name="isDownloaded">true if the node has been downloaded. This variable is use to avoid to redownload the node definition during runtime</param>
        /// <param name="neighbor">the neighbor</param>
        /// <returns>the created (or updated) node</returns>
        private model.Node CreateNode(XElement element, bool isDownloaded, model.Node neighbor)
        {
            float x = neighbor.PhysicRepresentation.Position.X + ((((float)this.random.NextDouble()) * 30f) - 15f);
            float y = neighbor.PhysicRepresentation.Position.Y + ((((float)this.random.NextDouble()) * 30f) - 15f);
            float z = neighbor.PhysicRepresentation.Position.Z;
            return this.CreateNode(element, isDownloaded, x, y, z);
        }

        /// <summary>
        /// this function creates the view manager and set the events listening
        /// </summary>
        private void InitializeViewManager()
        {
            this.viewManager = new view.ViewManager(this.modelManager);

            this.viewManager.DecreaseDepth += delegate(object sender, EventArgs e)
            {
                this.modelManager.DecreaseDepth();
            };
            this.viewManager.IncreaseDepth += delegate(object sender, EventArgs e)
            {
                this.modelManager.IncreaseDepth();
            };
        }

        /// <summary>
        /// this function creates the file downloader management system and set the event listening
        /// </summary>
        /// <param name="loaderType">The type of loader our download manager must use</param>
        private void InitalizeFileDownloadManager(Type loaderType)
        {
            // creating the download manager
            this.fileDownloadManager = new download.FileLoadManager();

            // set the loading type
            this.fileDownloadManager.LoaderType = loaderType;

            this.fileDownloadManager.ICEXmlRelationFileLoaded += delegate(XDocument e)
            {
                this.ApplyXmlRelationsFile(e, this.fileDownloadManager.IsInInitialisationPhase);
            };

            this.fileDownloadManager.ICEXmlSettingsFileLoaded += delegate(XDocument e)
            {
                this.settings.ApplyXmlStyleFile(e, this.assemblyManager);
            };

            this.fileDownloadManager.ICEErrorFileLoaded += delegate(XDocument e)
            {
                this.ApplyXmlErrorFile(e);
            };

            this.fileDownloadManager.InitialisationFinished += new EventHandler(this.EndOfInitialization);

            this.fileDownloadManager.FileLoadingFailded += delegate(string message)
            {
                this.ViewManager.AddDebugMessage(message);
                if (this.fileDownloadManager.IsInInitialisationPhase)
                {
                    this.initializationFailed = true;
                }
            };

            this.fileDownloadManager.AssemblyFileLoaded += delegate(Assembly e)
            {
                this.assemblyManager.AddAssembly(e);
            };

            // we call the file downloader if we need to update a node
            this.modelManager.NeedUpdate += delegate(object sender, model.Node node, download.Priority priority)
            {
               this.fileDownloadManager.LoadFile(node.Url, priority); 
            };
        }

        /// <summary>
        /// This function end the initialisation process by creating the settings and adding object to the model form the downloaded files
        /// </summary>
        /// <param name="sender">download manager</param>
        /// <param name="args">event arguments</param>
        private void EndOfInitialization(object sender, EventArgs args)
        {
            if (!this.initializationFailed)
            {
                // Hide splash screen
                this.viewManager.HideSplachScreen();
            }
        }

        /// <summary>
        /// This function adds all URI in argument to the download manager
        /// </summary>
        /// <param name="initParams">List of all initialisation parameters</param>
        private void AddInitUrlToDownloadManager(IDictionary<string, string> initParams)
        {
            foreach (string stringURL in initParams.Values)
            {
                this.fileDownloadManager.LoadFile(stringURL, download.Priority.Absolute);
            }
        }

        /// <summary>
        /// This function draw the error 
        /// </summary>
        /// <param name="e">an external error message</param>
        private void ApplyXmlErrorFile(XDocument e)
        {
            XElement rootElement = e.Element(xml.ErrorXmlContent.Namespace + xml.ErrorXmlContent.RootElementName);
            XElement messageElement = rootElement.Element(xml.ErrorXmlContent.Namespace + xml.ErrorXmlContent.MessageElementOfRootElementName);

            switch (messageElement.Attribute(xml.ErrorXmlContent.ShowOnAttributeOfMessageElementName).Value)
            {
                case xml.ErrorXmlContent.NeverValueOfShowOnAttributeName:
                    /* do nothing */
                    break;
                case xml.ErrorXmlContent.DebugValueOfShowOnAttributeName:
                    /* draw if on debug */
                    this.viewManager.AddDebugMessage(messageElement.Value);
                    break;
                case xml.ErrorXmlContent.EverytimeValueOfShowOnAttributeName:
                    /* draw message */
                    this.viewManager.AddMessage(messageElement.Value);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
