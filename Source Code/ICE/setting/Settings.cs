//-----------------------------------------------------------------------
// <copyright file="Settings.cs" company="International Monetary Fund">
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

namespace ICE.setting
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Xml.Linq;
    using ICE.view;
    using ICE.view.visualEffect;

    /// <summary>
    /// TODO
    /// This class represents the main view style and its behaviour
    /// </summary>
    public class Settings : IPhysicsSettings, ICE.setting.IViewSettings, ICE.setting.IModelSettings, ICE.setting.IFileLoadSettings
    {
        #region Fields

        #region Physics Constants

        /// <summary>
        /// Fuild resistance force value.
        /// </summary>
        private float dragForce = 0.25f;

        /// <summary>
        /// Length of a link with no constraint
        /// </summary>
        private float linkRestLength = 5f;

        /// <summary>
        /// Define default gravity;
        /// </summary>
        private int gravity = 0;

        /// <summary>
        /// Define the repultion strenght
        /// </summary>
        private float repultionForce = 1000;

        #endregion

        #region Drawing Constants

        /// <summary>
        /// Color use to draw the cicle of available actions
        /// </summary>
        private Color actionsCircleColor = Colors.Orange;

        /// <summary>
        /// Maximal thickness for a link
        /// </summary>
        private double linkMaximalThickness = 8;

        /// <summary>
        /// Minimal thickness for a link
        /// </summary>
        private double linkMinimalThickness = 0.5d;

        /// <summary>
        /// Default size ratio used to draw nodes which are not selected
        /// </summary>
        private double nodeSizeRatio = 0.25d;

        /// <summary>
        /// Default zoom ratio used to draw the graph
        /// </summary>
        private double initialZoomRatio = 4d;

        /// <summary>
        /// Default reseach depth used to draw you graph
        /// </summary>
        private int initialVisibilityGraphDepth = 1;

        /// <summary>
        /// Opacity step used to create progessive apparition
        /// </summary>
        private double opacityChangeStep = 0.1d;

        /// <summary>
        /// this is the background
        /// </summary>
        private FrameworkElement background = new Grid();

        #endregion

        #region Model Constants

        /// <summary>
        /// Maximal number of nodes displayed at once
        /// </summary>
        private int maximumNodes = 200;

        /// <summary>
        /// Define the additional depth of exploration use to know what part of the model should be suppressed
        /// </summary>
        private int cleanUpAdditionalDepth = 1;

        #endregion

        #region Download Constants

        /// <summary>
        /// This field is the time limit for all file download
        /// </summary>
        private int downloadTimeout = 100000;

        /// <summary>
        /// This field is the number of athorized download per minutes
        /// </summary>
        private int maximumDownloadPerMinute = 100;

        /// <summary>
        /// This field is the number of athorized simultaneous download
        /// </summary>
        private int maximumSimultaneousDownload = 30;

        #endregion

        #region MODES

        /// <summary>
        /// Boolean indicating whether multi-selection is enabled or disabled
        /// </summary>
        private bool multipleSelectionNodeMode = false;

        /// <summary>
        /// Boolean indicating whether debug messages are enabled or disabled
        /// </summary>
        private bool debugMode = false;

        /// <summary>
        /// Boolean indicating whether we must show or not the navigation bar
        /// </summary>
        private bool navigationBarMode = false;

        /// <summary>
        /// Boolean indicating whether we must show or not the navigation menu
        /// </summary>
        private bool navigationMenuMode = false;

        #endregion

        /// <summary>
        /// List of all known node styles
        /// </summary>
        private Dictionary<string, NodeStyle> nodeStyleList = new Dictionary<string, NodeStyle>();

        /// <summary>
        /// List of all known link styles
        /// </summary>
        private Dictionary<string, LinkStyle> linkStyleList = new Dictionary<string, LinkStyle>();

        /// <summary>
        /// List of all known action
        /// </summary>
        private Dictionary<string, action.Action> actionsList = new Dictionary<string, ICE.action.Action>();

        /// <summary>
        /// Default NodeStyle applying to all nodes with an unknow style-name
        /// </summary>
        private NodeStyle defaultNodeStyle = new NodeStyle();

        /// <summary>
        /// Default linkStyle applying to all links with an unknow style-name
        /// </summary>
        private LinkStyle defaultLinkStyle = new LinkStyle();

        #endregion

        #region Delegates

        /// <summary>
        /// This delegate contains all visual effects called when scale rate is changed
        /// </summary>
        private MainViewVisualEffectChange changeScale;

        #endregion

        /// <summary>
        /// Initializes a new instance of the Settings class.
        /// </summary>
        public Settings()
        {
            this.changeScale = delegate(ViewManager viewManager, double newValue)
            {
                return new ProportionalScaleChangeEffect(viewManager, newValue, 40d, 20);
            };
        }

        #region Events

        /// <summary>
        /// This event is raised whether the settings has changed
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the delegate that contains all visual effects called when scale rate is changed
        /// </summary>
        public MainViewVisualEffectChange ChangeScale
        {
            get { return this.changeScale; }
        }

        #region Model Constants

        /// <summary>
        /// Gets or sets the additional value of the depth use to know which part of the model should be suppressed.
        /// To know the current depth of exploration for the cleanup process, just add this value to the current visibility depth
        /// </summary>
        public int CleanUpAdditionalDepth
        {
            get { return this.cleanUpAdditionalDepth; }
            set { this.cleanUpAdditionalDepth = value; }
        }

        /// <summary>
        /// Gets or sets the maximal number of nodes displayed at once
        /// </summary>
        public int MaximumNodeLimit
        {
            get { return this.maximumNodes; }
            set { this.maximumNodes = value; }
        }

        #endregion

        #region Physics Constants

        /// <summary>
        /// Gets or sets the drag force
        /// </summary>
        public float DragForce
        {
            get { return this.dragForce; }
            set { this.dragForce = value; }
        }

        /// <summary>
        /// Gets or sets the gravity
        /// </summary>
        public int Gravity
        {
            get { return this.gravity; }
            set { this.gravity = value; }
        }

        /// <summary>
        /// Gets or sets the rest length of a link.
        /// </summary>
        public float LinkRestLength
        {
            get { return this.linkRestLength; }
            set { this.linkRestLength = value; }
        }

        /// <summary>
        /// Gets or sets replution force strength
        /// </summary>
        public float RepultionForce 
        {
            get { return this.repultionForce; }
            set { this.repultionForce = value; }
        }

        #endregion

        #region Download Constants

        /// <summary>
        /// Gets or sets the Time limit of any loading process.
        /// </summary>
        public int DownloadTimeout
        {
            get { return this.downloadTimeout; }
            set { this.downloadTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the number of authorised download per minute for an ICE item.
        /// </summary>
        public int MaximumDownloadPerMinute
        {
            get { return this.maximumDownloadPerMinute; }
            set { this.maximumDownloadPerMinute = value; }
        }

        /// <summary>
        /// Gets or sets the number of authorized simultaneous download.
        /// </summary>
        public int MaximumSimultaneousDownload
        {
            get { return this.maximumSimultaneousDownload; }
            set { this.maximumSimultaneousDownload = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets actions's circle color
        /// </summary>
        public Color ActionsCircleColor
        {
            get { return this.actionsCircleColor; }
            set { this.actionsCircleColor = value; }
        }

        /// <summary>
        /// Gets or sets the maximal thickness for a link
        /// </summary>
        public double LinkMaximalThickness
        {
            get { return this.linkMaximalThickness; }
            set { this.linkMaximalThickness = value; }
        }

        /// <summary>
        /// Gets or sets the minimal thickness for a link
        /// </summary>
        public double LinkMinimalThickness
        {
            get { return this.linkMinimalThickness; }
            set { this.linkMinimalThickness = value; }
        }

        /// <summary>
        /// Gets or sets the default size ratio used to draw nodes which are not selected
        /// </summary>
        public double NodeSizeRatio
        {
            get { return this.nodeSizeRatio; }
            set { this.nodeSizeRatio = value; }
        }

        /// <summary>
        /// Gets or sets the default zoom ratio used to draw the graph
        /// </summary>
        public double InitialZoomRatio
        {
            get { return this.initialZoomRatio; }
            set { this.initialZoomRatio = value; }
        }

        /// <summary>
        /// Gets or sets the default reseach depth used to draw you graph
        /// </summary>
        public int InitialGraphVisibilityDepth
        {
            get { return this.initialVisibilityGraphDepth; }
            set { this.initialVisibilityGraphDepth = value; }
        }

        /// <summary>
        /// Gets or sets the opacity step used to create progessive apparition
        /// </summary>
        public double OpacityChangeStep
        {
            get { return this.opacityChangeStep; }
            set { this.opacityChangeStep = value; }
        }

        /// <summary>
        /// Gets or sets the default Node Style applying to all nodes with an unknown style-name
        /// </summary>
        public NodeStyle DefaultNodeStyle
        {
            get { return this.defaultNodeStyle; }
            set { this.defaultNodeStyle = value; }
        }

        /// <summary>
        /// Gets or sets the default linkStyle applying to all links with an unknow style-name
        /// </summary>
        public LinkStyle DefaultLinkStyle
        {
            get { return this.defaultLinkStyle; }
            set { this.defaultLinkStyle = value; }
        }

        /// <summary>
        /// Gets or sets the background of the graph
        /// </summary>
        public FrameworkElement Background
        {
            get { return this.background; }
            set { this.background = value; }
        }

        #region Modes

        /// <summary>
        /// Gets or sets a value indicating whether we must show or not the navigation bar
        /// </summary>
        public bool NavigationBarMode
        {
            get { return this.navigationBarMode; }
            set { this.navigationBarMode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether we must show or not the navigation menu
        /// </summary>
        public bool NavigationMenuMode
        {
            get { return this.navigationMenuMode; }
            set { this.navigationMenuMode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether multi-selection is enabled or disabled
        /// </summary>
        public bool MultiSelectionNodeMode
        {
            get { return this.multipleSelectionNodeMode; }
            set { this.multipleSelectionNodeMode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether debug messages are enabled or disabled
        /// </summary>
        public bool DebugMode
        {
            get { return this.debugMode; }
            set { this.debugMode = value; }
        }

        #endregion

        #endregion

        #region Function

        /// <summary>
        /// This function adds a node style to the settings
        /// </summary>
        /// <param name="name">The name of style to add</param>
        /// <param name="nodeStyle">The style implementation</param>
        public void AddNodeStyle(string name, NodeStyle nodeStyle)
        {
            if (this.nodeStyleList.ContainsKey(name))
            {
                NodeStyle old = this.nodeStyleList[name];
                this.nodeStyleList.Remove(name);
                this.nodeStyleList.Add(name, nodeStyle);
                old.RaiseChangeEvent();
            }
            else
            {
                this.nodeStyleList.Add(name, nodeStyle);
                this.defaultNodeStyle.RaiseChangeEvent();
            }
        }

        /// <summary>
        /// This function adds a link style to the settings
        /// </summary>
        /// <param name="name">The name of the style to add</param>
        /// <param name="linkStyle">The style implementation</param>
        public void AddLinkStyle(string name, LinkStyle linkStyle)
        {
            if (this.linkStyleList.ContainsKey(name))
            {
                LinkStyle old = this.linkStyleList[name];
                this.linkStyleList.Remove(name);
                this.linkStyleList.Add(name, linkStyle);
                old.RaiseChangeEvent();
            }
            else
            {
                this.linkStyleList.Add(name, linkStyle);
                this.defaultLinkStyle.RaiseChangeEvent();
            }
        }

        /// <summary>
        /// This function adds a action to the settings
        /// </summary>
        /// <param name="name">The name of the action to add</param>
        /// <param name="action">The action implementation</param>
        public void AddAction(string name, action.Action action)
        {
            if (this.actionsList.ContainsKey(name))
            {
                this.actionsList.Remove(name);   
            }

            this.actionsList.Add(name, action);
        }

        /// <summary>
        /// Gets a Action from its name
        /// </summary>
        /// <param name="name">The name of the action</param>
        /// <returns>
        /// The Action corresponding to the name
        /// If no Action matches the name, null is returned
        /// </returns>
        public action.Action GetAction(string name)
        {
            try
            {
                return this.actionsList[name];
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a NodeStyle from its name
        /// </summary>
        /// <param name="name">The name of the style</param>
        /// <returns>
        /// The NodeStyle corresponding to the name
        /// If no NodeStyle matches the name, the default node style is returned
        /// </returns>
        public NodeStyle GetNodeStyle(string name)
        {
            if (this.nodeStyleList.ContainsKey(name))
            {
                return this.nodeStyleList[name];
            }
            else
            {
                return this.defaultNodeStyle;
            }
        }

        /// <summary>
        /// Gets the Linkstyle from its name
        /// </summary>
        /// <param name="name">The name of the style</param>
        /// <returns>
        /// The LinkStyle corresponding to the name
        /// If no NodeStyle matches the name, the default node style is returned
        /// </returns>
        public LinkStyle GetLinkStyle(string name)
        {
            if (this.linkStyleList.ContainsKey(name))
            {
                return this.linkStyleList[name];
            }
            else
            {
                return this.defaultLinkStyle;
            }  
        }

        /// <summary>
        /// This function apply the settings in the Xml document to the current instance
        /// </summary>
        /// <param name="document">The xml document</param>
        /// <param name="assemblyManager">the assmbly manager (use to find all custom component)</param>
        public void ApplyXmlStyleFile(XDocument document, ExternalAssemblyManager assemblyManager)
        {
            XElement iceSettings = document.Root;

            // <nodeStyles>
            if (iceSettings.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.NodeStylesElementName) != null)
            {
                XElement nodeStylesElement = iceSettings.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.NodeStylesElementName);

                foreach (XElement nodeStyleElement in nodeStylesElement.Elements(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.NodeStyleElementName))
                {
                    this.AddNodeStyle(
                        nodeStyleElement.Attribute(xml.SettingsXmlContent.IDAttributeOfNodeStyleElementName).Value,
                        new NodeStyle(nodeStyleElement, assemblyManager));
                }

                XAttribute defaultNodeStyleAttribute = nodeStylesElement.Attribute(xml.SettingsXmlContent.DefaultAttributeOfNodeStylesElementName);
                if (defaultNodeStyleAttribute != null)
                {
                    NodeStyle old = this.DefaultNodeStyle;
                    this.DefaultNodeStyle = this.nodeStyleList[defaultNodeStyleAttribute.Value];
                    old.RaiseChangeEvent();
                }
            }

            // <linkStyles>
            if (iceSettings.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.LinkStylesElementName) != null)
            {
                XElement linkStylesElement = iceSettings.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.LinkStylesElementName);

                foreach (XElement linkStyleElement in linkStylesElement.Elements(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.LinkStyleElementName))
                {
                    this.AddLinkStyle(
                        linkStyleElement.Attribute(xml.SettingsXmlContent.IDAttributeOfLinkStyleElementName).Value,
                        new LinkStyle(linkStyleElement, assemblyManager));
                }

                XAttribute defaultLinkStyle = linkStylesElement.Attribute(xml.SettingsXmlContent.DefaultAttributeOfLinkStylesElementName);
                if (defaultLinkStyle != null)
                {
                    LinkStyle old = this.DefaultLinkStyle;
                    this.DefaultLinkStyle = this.linkStyleList[defaultLinkStyle.Value];
                    old.RaiseChangeEvent();
                }
            }

            XElement actionsElement = iceSettings.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.ActionsElementName);
            if (actionsElement != null)
            {
                foreach (XElement actionElement in actionsElement.Elements())
                {
                    this.AddAction(actionElement.Attribute(xml.SettingsXmlContent.IDAttributeOfActionElementName).Value, new action.Action(actionElement));
                }
            }

            // <mods>
            XElement mods = iceSettings.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.ModesElementOfConstantsElementName);
            if (mods != null)
            {
                XElement multiNodeSelection = mods.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.MultiSelectionElementOfModesElementName);
                this.MultiSelectionNodeMode = multiNodeSelection != null;

                XElement debugModeElement = mods.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.DebugElementOfModesElementName);
                this.DebugMode = debugModeElement != null;

                XElement navigationBarModeElement = mods.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.NavigationBarElementOfModesElementName);
                this.NavigationBarMode = navigationBarModeElement != null;

                XElement navigationMenuModeElement = mods.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.NavigationBarElementOfModesElementName);
                this.NavigationMenuMode = navigationMenuModeElement != null;
            }

            // <constants>
            XElement constantsElement = iceSettings.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.ConstantsElementName);
            if (constantsElement != null)
            {
                // <drawingConstant>
                XElement drawingConstantsElement = constantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.DrawingContantsElementOfConstantsElementName);
                if (drawingConstantsElement != null)
                {
                    XElement actionsCircleColorElement = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.ActionCircleColorElementOfDrawingContantsElementName);
                    if (actionsCircleColorElement != null)
                    {
                        this.actionsCircleColor = xml.SettingsXmlContent.ParseColor(actionsCircleColorElement);
                    }

                    XElement initialZoomRatioElement = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.InitialZoomRatioElementOfDrawingContantsElementName);
                    if (initialZoomRatioElement != null)
                    {
                        this.InitialZoomRatio = xml.SettingsXmlContent.ParseToDouble(initialZoomRatioElement.Value);
                    }

                    XElement initialGraphVisibilityDepthElement = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.InitialGraphVisibilityDepthElementOfDrawingContantsElementName);
                    if (initialGraphVisibilityDepthElement != null)
                    {
                        this.InitialGraphVisibilityDepth = int.Parse(initialGraphVisibilityDepthElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement opacityChangeStepElement = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.OpacityChangeStepElementOfDrawingContantsElementName);
                    if (opacityChangeStepElement != null)
                    {
                        this.OpacityChangeStep = xml.SettingsXmlContent.ParseToDouble(opacityChangeStepElement.Value);
                    }

                    XElement nodeSizeElement = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.NodeSizeElementOfDrawingContantsElementName);
                    if (nodeSizeElement != null)
                    {
                        this.NodeSizeRatio = xml.SettingsXmlContent.ParseToDouble(nodeSizeElement.Value);
                    }

                    XElement linkMaximalThickness = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.LinkMaximalThicknessElementOfDrawingContantsElementName);
                    if (linkMaximalThickness != null)
                    {
                        this.LinkMaximalThickness = double.Parse(linkMaximalThickness.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement linkMinimalThickness = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.LinkMinimalThicknessElementOfDrawingContantsElementName);
                    if (linkMinimalThickness != null)
                    {
                        this.LinkMinimalThickness = double.Parse(linkMinimalThickness.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement backgroundElement = drawingConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.BackgroundElementOfDrawingContantsElementName);
                    if (backgroundElement != null)
                    {
                        this.background = new Grid();
                        ViewManager.SetXamlIntoGrid(0, backgroundElement.Value, (Grid)this.background);
                    }
                }

                // <physicsConstants>
                XElement physicsConstantsElement = constantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.PhysicsContantsElementOfConstantsElementName);
                if (physicsConstantsElement != null)
                {
                    XElement gravityElement = physicsConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.GravityElementOfPhysicsContantsElementName);
                    if (gravityElement != null)
                    {
                        this.Gravity = int.Parse(gravityElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement springRestLenghtElement = physicsConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.SpringRestLenghtElementOfPhysicsContantsElementName);
                    if (springRestLenghtElement != null)
                    {
                        this.LinkRestLength = Single.Parse(springRestLenghtElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement repultionForceElement = physicsConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.RepultionStrengthElementOfPhysicsContantsElementName);
                    if (repultionForceElement != null)
                    {
                        this.RepultionForce = Single.Parse(repultionForceElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement dragForceElement = physicsConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.DragForceElementOfPhysicsContantsElementName);
                    if (dragForceElement != null)
                    {
                        this.dragForce = Single.Parse(dragForceElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                // <modelConstants>
                XElement modelConstantsElement = constantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.ModelContantsElementOfConstantsElementName);
                if (modelConstantsElement != null)
                {
                    XElement maximumNodesElement = modelConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.MaximumNodeElementOfModelContantsElementName);
                    if (maximumNodesElement != null)
                    {
                        this.maximumNodes = int.Parse(maximumNodesElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement cleanUpAdditionalDepthElement = modelConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.CleanUpadditionalDepthElementOfModelContantsElementName);
                    if (cleanUpAdditionalDepthElement != null)
                    {
                        this.CleanUpAdditionalDepth = int.Parse(cleanUpAdditionalDepthElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                // <download>
                XElement downloadConstantsElement = constantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.DownloadConstantsElementOfConstantsElementName);
                if (downloadConstantsElement != null)
                {
                    XElement timeoutElement = downloadConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.TimeoutElementOfDownloadConstantsElementName);
                    if (timeoutElement != null)
                    {
                        this.downloadTimeout = int.Parse(timeoutElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement maximumDownloadPerMinuteElement = downloadConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.MaximumDownloadPerMinuteElementOfDownloadConstantsElementName);
                    if (maximumDownloadPerMinuteElement != null)
                    {
                        this.maximumDownloadPerMinute = int.Parse(maximumDownloadPerMinuteElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }

                    XElement maximumSimultaneousDownloadElement = downloadConstantsElement.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.MaximumSimultaneousDownloadElementOfDownloadConstantsElementName);
                    if (maximumSimultaneousDownloadElement != null)
                    {
                        this.downloadTimeout = int.Parse(maximumSimultaneousDownloadElement.Value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }

            if (this.Changed != null)
            {
                this.Changed(this, new EventArgs());
            }
        }

        #endregion
    }
}
