//-----------------------------------------------------------------------
// <copyright file="LinkViewManager.cs" company="International Monetary Fund">
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
namespace ICE.view
{
    using System;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    using System.Xml.Linq;
    using ICE.action;
    using ICE.setting;

    /// <summary>
    /// This class manages the view of a link. This component is responsible of keeping the view up to date and to manage the human interaction with the link.
    /// </summary>
    public class LinkViewManager : IActionable
    {
        #region Fields

        /// <summary>
        /// Pointer to view manager
        /// </summary>
        /// TODO : move away ?
        private ViewManager viewManager;

        /// <summary>
        /// the model we want to represent in  the graph
        /// </summary>
        private model.Link link;

        /// <summary>
        /// The corresponding GUI component that represent the node
        /// </summary>
        private ILinkView guiComponent;

        /// <summary>
        /// Links's style
        /// </summary>
        private LinkStyle style;

        /// <summary>
        /// The current calculated thickness of the link
        /// </summary>
        /// <remarks>
        /// this value is updated by the TryUpdateGUIData function
        /// </remarks>
        private double thickness;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the LinkViewManager class.
        /// </summary>
        /// <param name="link">the node you want to manage</param>
        /// <param name="viewManager">the view management system</param>
        public LinkViewManager(model.Link link, ViewManager viewManager)
        {
            // set the node field
            this.link = link;
            this.link.GUIDataChanged += new EventHandler(this.Link_GUIDataChanged);

            // set the view manager pointer
            this.viewManager = viewManager;
            this.viewManager.ZoomRatioChanged += new EventHandler(this.ViewManager_ZoomRatioChanged);

            // set the style
            this.SetStyle();

            // create the corresponding view
            this.SetGUIComponent();

            this.TryUpdateGUIData();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the model of the current link
        /// </summary>
        public model.Link Link
        {
            get { return this.link; }
            set { this.link = value; }
        }

        /// <summary>
        /// Gets or sets the UI view of the current link
        /// </summary>
        public ILinkView GUIComponent
        {
            get { return this.guiComponent; }
            set { this.guiComponent = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the link is entirely disposed
        /// </summary>
        public bool IsDisposed
        {
            get 
            {
                return this.link.IsDisposed && !this.viewManager.IsInGraph((UserControl)this.guiComponent);
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Set all event functions related to the main system
        /// </summary>
        public void SetViewManagement()
        {
            ////this.viewManager.GetView().Dispatcher.BeginInvoke(delegate
            ////{
            ////    // here we take care of Java Script events
            ////    ((UIElement)this.guiComponent).MouseEnter += delegate(object sender, MouseEventArgs e)
            ////    {
            ////       // JavaScriptManager.Invoke(this, JavaScriptEvent.MouseEnter);
            ////    };
            ////    ((UIElement)this.guiComponent).MouseLeave += delegate(object sender, MouseEventArgs e)
            ////    {
            ////        //JavaScriptManager.Invoke(this, JavaScriptEvent.MouseLeave);
            ////    };
            ////    ((UIElement)this.guiComponent).MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
            ////    {
            ////        //JavaScriptManager.Invoke(this, JavaScriptEvent.Click);
            ////    };
            ////});
        }

        /// <summary>
        /// This function update the position of the GUI acording to the link's physics representation
        /// This function is a part of the view management update.
        /// </summary>
        public void Update()
        {
            // perform a cast
            UserControl guiComponent = (UserControl)this.guiComponent;

            if (this.link.IsVisible)
            {
                this.viewManager.AddOrMaintainUserControlInCanvas(guiComponent);

                // set XYZ² situation
                this.guiComponent.PointA = this.viewManager.GetCanvasCoordonate(this.link.RelatedNode1.PhysicRepresentation.Position);
                this.guiComponent.PointB = this.viewManager.GetCanvasCoordonate(this.link.RelatedNode2.PhysicRepresentation.Position);

                this.UpdateGUIComponentSize();
            }
            else
            {
                this.viewManager.RemoveUserControlFromCanvas(guiComponent);
            }
        }

        #region IActionable Members

        /// <summary>
        /// This function ask the UI component to execute a task
        /// </summary>
        /// <param name="name">name of the task</param>
        /// <param name="callDefinition">the call definition</param>
        public void PerformGUITask(string name, XElement callDefinition)
        {
            this.guiComponent.PerformTask(name, callDefinition);
        }

        /// <summary>
        /// This funtion return the part of xml use as a parameter
        /// </summary>
        /// <param name="actionId">the current id</param>
        /// <returns>an xml action reference</returns>
        public XElement GetCallStatement(string actionId)
        {
            foreach (string actionXml in this.link.Actions)
            {
                try
                {
                    XElement actionElement = XDocument.Parse(actionXml).Root;
                    string actionName = actionElement.Attribute(xml.DataXmlContent.IDRefAttributeOfActionElementName).Value;
                    if (actionName == actionId)
                    {
                        return actionElement;
                    }
                }
                catch 
                { 
                    /* do nothing */ 
                }
            }

            return null;
        }
        #endregion

        #endregion

        #region Private Functions

        /// <summary>
        /// Create a new UserControl for the current link and add it to the graph view (and replace the old one if nessessary)
        /// </summary>
        private void SetGUIComponent()
        {
            ILinkView gui = this.style.GetNewLinkView();

            if (this.guiComponent != null && this.viewManager.IsInGraph((UserControl)this.guiComponent))
            {
                this.viewManager.ReplaceGraphComponent((UserControl)this.guiComponent, (UserControl)gui);
            }

            this.guiComponent = gui;

            // set the event on the node view
            this.SetViewManagement();
        }

        /// <summary>
        /// this function sets the style from the node's infromation
        /// </summary>
        private void SetStyle()
        {
            this.style = this.viewManager.Settings.GetLinkStyle(this.link.StyleName);
            this.style.Changed += new EventHandler(this.Style_Changed);
        }

        /// <summary>
        /// this function is called when the style change
        /// </summary>
        /// <param name="sender">the old style</param>
        /// <param name="e">the event's argument</param>
        private void Style_Changed(object sender, EventArgs e)
        {
            this.SetStyle();
            this.SetGUIComponent();
            this.TryUpdateGUIData();
        }

        /// <summary>
        /// This function is called when the data visible data of the link change
        /// </summary>
        /// <param name="sender">the link iself</param>
        /// <param name="e">the event argument</param>
        private void Link_GUIDataChanged(object sender, EventArgs e)
        {
            this.TryUpdateGUIData();
        }

        /// <summary>
        /// this function update the information provided to the view ( E.g.; thickness )
        /// </summary>
        private void TryUpdateGUIData()
        {
            // set the thickness field value
            double deltaThickness = this.viewManager.Settings.LinkMaximalThickness - this.viewManager.Settings.LinkMinimalThickness;
            double strengthRatio = this.link.Strength / 100;
            this.thickness = this.viewManager.Settings.LinkMinimalThickness + (deltaThickness * strengthRatio);

            // set the meaning sentence
            try
            {
                string meaning = this.link.RelatedNode1.Title + " " + this.link.Verb + " " + this.link.RelatedNode2.Title + " " + this.link.Complement;
                this.guiComponent.Meaning = meaning;
            }
            catch (Exception)
            {
                /* Create an error message */
                this.viewManager.AddDebugMessage("An Error occured in UserControl \"" +
                this.guiComponent.GetType().FullName +
                "\" when the program tried to change \"Meaning\" property." +
                "(Link ID: \"" + this.link.ID + "\")");
            }
            
            // set the Xml drawing information
            try
            {
                XDocument doc = XDocument.Parse(this.link.DrawingInformation);
                this.guiComponent.LinkDrawingInformation = doc.Root;
            }
            catch (Exception)
            {
                /* Create an error message */
                this.viewManager.AddDebugMessage("An Error occured in UserControl \"" +
                this.guiComponent.GetType().FullName +
                "\" when the program tried to change the \"DrawingInformation\" property." +
                "(Link ID: \"" + this.link.ID + "\")");
            }
        }

        /// <summary>
        /// This function is called when the zoom of the view change
        /// </summary>
        /// <param name="sender">the view management system</param>
        /// <param name="e">the event argument</param>
        private void ViewManager_ZoomRatioChanged(object sender, EventArgs e)
        {
            this.UpdateGUIComponentSize();
        }

        /// <summary>
        /// This function update the size of the link's visual representation
        /// </summary>
        private void UpdateGUIComponentSize()
        {
            // Change the thickness
            this.guiComponent.Thickness = this.viewManager.CurrentZoomRatio * this.thickness;
        }

        #endregion
    }
}
