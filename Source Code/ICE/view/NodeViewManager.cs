//-----------------------------------------------------------------------
// <copyright file="NodeViewManager.cs" company="International Monetary Fund">
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
    using System.Xml.Linq;
    using ICE.action;
    using ICE.setting;

    /// <summary>
    /// This class represents the management of the node visualisation
    /// it's make sure the view is the representation of the node model.
    /// It's also manage the IHM controls of the node.
    /// </summary>
    public class NodeViewManager : IActionable
    {
        #region Fields

        /// <summary>
        /// Pointer to view manager
        /// </summary>
        private ViewManager viewManager;

        /// <summary>
        /// the model we want to represent in  the graph
        /// </summary>
        private model.Node node;

        /// <summary>
        /// The corresponding GUI component that represent the node
        /// </summary>
        private INodeView guiComponent;

        /// <summary>
        /// Mouse manager for the view
        /// </summary>
        private MouseManager mouseManager;

        /// <summary>
        /// Node's style
        /// </summary>
        private NodeStyle style;

        /// <summary>
        /// the current size multiplicator
        /// </summary>
        private double relativeSize = 1;

        /// <summary>
        /// this is the menu of action related to the node
        /// </summary>
        private ActionMenu actionMenu;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the NodeViewManager class.
        /// </summary>
        /// <param name="node">node you want to represent</param>
        /// <param name="viewManager">parent management system</param>
        public NodeViewManager(model.Node node, ViewManager viewManager)
        {
            // set the node field
            this.node = node;
            this.node.GUIDataChanged += new EventHandler(this.Node_GUIDataChanged);
            this.node.SelectionChanged += new EventHandler(this.Node_SelectionChanged);
            
            // set the view manager pointer
            this.viewManager = viewManager;
            this.viewManager.ZoomRatioChanged += new EventHandler(this.ViewManager_ZoomRatioChanged);

            // set the style
            this.SetStyle();

            // create the corresponding view
            this.SetGUIComponent();

            // update information from the node
            // we ask for an update of "node"
            this.TryUpdateGUIData();

            this.TryUpdateSelectionState();
        }

        #endregion

        #region Events & Delegates

        /// <summary>
        /// this event is raised whether the user drag the node
        /// </summary>
        public event MouseEventHandler Drag;

        /// <summary>
        /// this event is raised whether the user drop the node
        /// </summary>
        public event MouseEventHandler Drop;

        /// <summary>
        /// this event is raised whether the user has draged the node and move it to another position
        /// </summary>
        public event MouseEventHandler DragedAndMove;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the node is entirely disposed
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.node.IsDisposed && !this.viewManager.IsInGraph((UserControl)this.guiComponent);
            }
        }

        /// <summary>
        /// Gets or sets the model of the current node
        /// </summary>
        public model.Node Node
        {
            get { return this.node; }
            set { this.node = value; }
        }

        /// <summary>
        /// Gets or sets the Ui view of the current node
        /// </summary>
        public INodeView GUIComponent
        {
            get { return this.guiComponent; }
            set { this.guiComponent = value; }
        }

        /// <summary>
        /// Gets or sets the Size multiplicator of the current node
        /// </summary>
        public double SizeMultiplicatorCoeficient
        {
            get { return this.relativeSize; }
            set { this.relativeSize = value; }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Set all event functions related to the main system
        /// </summary>
        public void SetViewManagement()
        {
            this.viewManager.GetView().Dispatcher.BeginInvoke(delegate
            {
                // set the actions
                this.mouseManager = new MouseManager((UIElement)this.guiComponent);
                this.mouseManager.DragOnLeftButtonDown += new MouseEventHandler(this.MouseManager_DragOnLeftButtonDown);
                this.mouseManager.DropOnLeftButtonDown += new MouseButtonEventHandler(this.MouseManager_DropOnLeftButtonDown);
                this.mouseManager.LeftButtonDown += new MouseButtonEventHandler(this.MouseManager_LeftButtonDown);
                this.mouseManager.MouseMovedOnLeftButtonDown += new MouseEventHandler(this.MouseManager_MouseMovedOnLeftButtonDown);
                ((UIElement)this.guiComponent).MouseEnter += new MouseEventHandler(this.NodeViewManager_MouseEnter);
            });
        }

        /// <summary>
        /// This function update the position of the GUI acording to the node's physics representation
        /// This function is a part of the view management update.
        /// </summary>
        public void Update()
        {
            // perform a cast
            UserControl guiComponent = (UserControl)this.guiComponent;
            if (this.node.IsVisible)
            {
                this.viewManager.AddOrMaintainUserControlInCanvas(guiComponent);

                // set XYZ situation
                Point canvasCoordonate = this.viewManager.GetCanvasCoordonate(this.node.PhysicRepresentation.Position);
                Canvas.SetLeft(guiComponent, canvasCoordonate.X - (guiComponent.DesiredSize.Width / 2));
                Canvas.SetTop(guiComponent, canvasCoordonate.Y - (guiComponent.DesiredSize.Height / 2));

                if (this.actionMenu != null)
                {
                    Canvas.SetLeft(this.actionMenu, canvasCoordonate.X - (this.actionMenu.Width / 2));
                    Canvas.SetTop(this.actionMenu, canvasCoordonate.Y - (this.actionMenu.Height / 2));
                }
            }
            else
            {
                this.viewManager.RemoveUserControlFromCanvas(guiComponent);
            }
        }

        /// <summary>
        /// This funtion create the list of all action compatible with this node and that coulb be use on a group
        /// </summary>
        /// <returns>A list of all "groupable action" compatible with this node</returns>
        public List<action.Action> GetGroupableActions()
        {
            List<action.Action> actionList = new List<action.Action>();
            foreach (string actionXml in this.node.Actions)
            {
                try
                {
                    XElement actionElement = XDocument.Parse(actionXml).Root;

                    string actionName = actionElement.Attribute(xml.DataXmlContent.IDRefAttributeOfActionElementName).Value;
                    action.Action action = this.viewManager.Settings.GetAction(actionName);
                    if (action != null && action.IsGroupAction)
                    {
                        actionList.Add(action);
                    }
                }
                catch (Exception) 
                {
                    /* do nothing*/
                }
            }

            return actionList;
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
            foreach (string actionXml in this.node.Actions)
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
        /// This function is called when the user drag the node view
        /// </summary>
        /// <param name="sender">the current node view</param>
        /// <param name="e">the mouse event argument</param>
        private void MouseManager_DragOnLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (this.Drag != null)
            {
                this.Drag(this, e);
            }
        }

        /// <summary>
        /// This function is called when the node selection statue changed
        /// </summary>
        /// <param name="sender">the node itself</param>
        /// <param name="e">the event argument</param>
        private void Node_SelectionChanged(object sender, EventArgs e)
        {
            this.TryUpdateSelectionState();
        }

        /// <summary>
        /// this function sets the state of the UI component.
        /// </summary>
        private void TryUpdateSelectionState()
        {
            if (this.node.IsSelected)
            {
                try
                {
                    this.guiComponent.PerformTask("select", null);
                }
                catch (Exception) 
                {
                    /* do nothing */
                }
            }
            else
            {
                try
                {
                    this.guiComponent.PerformTask("deselect", null);
                }
                catch (Exception) 
                {
                    /* do nothing */
                }
            }
        }

        /// <summary>
        /// this function sets the menu of actions
        /// </summary>
        private void SetActionMenu()
        {
            // step1: get all actions
            Dictionary<action.Action, XElement> actionList = new Dictionary<ICE.action.Action, XElement>();
            foreach (string actionXml in this.node.Actions)
            {
                try
                {
                    XElement actionElement = XDocument.Parse(actionXml).Root;
                    string actionName = actionElement.Attribute(xml.DataXmlContent.IDRefAttributeOfActionElementName).Value;
                    action.Action action = this.viewManager.Settings.GetAction(actionName);
                    if (action != null && (!action.IsGroupAction))
                    {
                        actionList.Add(action, actionElement);
                    }
                }
                catch (Exception) 
                {
                    /* do nothing*/
                }                
            }

            if (this.node.IsSelected)
            {
                action.Action collapse = action.Action.GetCollapseAction();
                actionList.Add(collapse, null);
            }
            else
            {
                action.Action expand = action.Action.GetExpandAction();
                actionList.Add(expand, null);
            }

            ActionMenu menu = new ActionMenu();
            menu.Color = this.viewManager.Settings.ActionsCircleColor;

            foreach (action.Action action in actionList.Keys)
            {
                // step2: create an actionView for each of them
                List<action.IActionable> targets = new List<IActionable>();
                targets.Add(this);
                ActionView actionView = new ActionView(targets, action);

                // step3: add the action to a action menu
                menu.AddActionView(actionView);
            }

            // step4: subscribe to the close event
            menu.Closed += delegate(object sender, EventArgs args)
            {
                this.actionMenu = null;
            };

            // step5: display
            this.actionMenu = menu;
            this.viewManager.DisplayActionMenu(menu);
            menu.Open();
        }

        /// <summary>
        /// this function sets the GUI component from the node and is's style
        /// </summary>
        private void SetGUIComponent()
        {
            INodeView gui;
            try
            {
                gui = this.style.GetNewNodeView();
            }
            catch (Exception)
            {
                this.viewManager.AddDebugMessage("The imported Theme \"" + style.ViewConstructor.DeclaringType.FullName + "\" cannot be used. You should rebuild the corresponding library with the current version of ICE XAP file, and try again.");
                gui = new view.NodeView();
            }

            this.SetStyleInfo(gui);

            if (this.guiComponent != null && this.viewManager.IsInGraph((UserControl)this.guiComponent))
            {
                this.viewManager.ReplaceGraphComponent((UserControl)this.guiComponent, (UserControl)gui);
            }

            this.guiComponent = gui;

            // set the event on the node view
            this.SetViewManagement();

            // set the scale tool
            ScaleTransform scaleTransform = new ScaleTransform();
            ((FrameworkElement)this.guiComponent).RenderTransform = scaleTransform;

            // set the center 
            ((FrameworkElement)this.guiComponent).Measure(new Size(double.MaxValue, double.MaxValue));
            scaleTransform.CenterX = ((FrameworkElement)this.guiComponent).DesiredSize.Width / 2;
            scaleTransform.CenterY = ((FrameworkElement)this.guiComponent).DesiredSize.Height / 2;

            this.UpdateGUIComponentSize();
        }

        /// <summary>
        /// this function sets the style information of the node GUI
        /// </summary>
        /// <param name="gui">the Gui item you want to setup</param>
        private void SetStyleInfo(INodeView gui)
        {
            try
            {
                gui.StyleDrawingInformation = this.style.DrawingInformation;
            }
            catch (System.Exception)
            {
                /* Create an error message */
                this.viewManager.AddDebugMessage("An Error occured in the UserControl \"" +
                gui.GetType().FullName +
                "\" when the program tried to change the \"StyleDrawingInformation\" property.");
            }
        }

        /// <summary>
        /// this function sets or reset the style from the node
        /// </summary>
        private void SetStyle()
        {
            this.style = this.viewManager.Settings.GetNodeStyle(this.node.StyleName);
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
        /// this function is called when the zoom change.
        /// </summary>
        /// <param name="sender">the view management system</param>
        /// <param name="e">the event argument</param>
        private void ViewManager_ZoomRatioChanged(object sender, EventArgs e)
        {
            this.UpdateGUIComponentSize();
        }

        /// <summary>
        /// this function is called when the visual data (E.g.; The title, the image, the xml of the node ...) from the node change
        /// </summary>
        /// <param name="sender">the node itself</param>
        /// <param name="e">the event arguments</param>
        private void Node_GUIDataChanged(object sender, EventArgs e)
        {
            this.TryUpdateGUIData();
        }

        /// <summary>
        /// this function update the size of the node's visual representation
        /// </summary>
        private void UpdateGUIComponentSize()
        {
            setting.NodeStyle style = this.viewManager.Settings.GetNodeStyle(this.node.StyleName);

            // first we calculate the new size
            double scaleX = this.viewManager.CurrentZoomRatio *
                this.viewManager.Settings.NodeSizeRatio *
                style.RelativeSize *
                this.node.RelativeSize;
            double scaleY = this.viewManager.CurrentZoomRatio * 
                this.viewManager.Settings.NodeSizeRatio *
                style.RelativeSize *
                this.node.RelativeSize;

            // and finaly we set the size
            ScaleTransform scaleTransform = (ScaleTransform)((FrameworkElement)this.guiComponent).RenderTransform;
            scaleTransform.ScaleX = scaleX;
            scaleTransform.ScaleY = scaleY;
        }

        /// <summary>
        /// This function is called when the user mouve the view during a drag and drop
        /// Move the view to the new coordinates
        /// </summary>
        /// <param name="sender">The node view</param>
        /// <param name="e">The mouse event arguments</param>
        private void MouseManager_MouseMovedOnLeftButtonDown(object sender, MouseEventArgs e)
        {
            this.viewManager.SetVectorFromCanvasCoodonate(
                this.node.PhysicRepresentation.Position,
                e.GetPosition(this.viewManager.GetView()));
            
            if (this.DragedAndMove != null)
            {
                this.DragedAndMove(this, e);
            }
        }

        /// <summary>
        /// This function is called when the user push the left button down.
        /// It is either to select the node or to begin a drag'n'drop :
        /// in both case we need to turn of all forces applied to the node
        /// </summary>
        /// <param name="sender">The node view</param>
        /// <param name="e">The mouse event arguments</param>
        private void MouseManager_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.node.PhysicRepresentation.MakeFixed();
        }

        /// <summary>
        /// This function is called at the end of the drag'n'drop.
        /// If the node was not selected previously, we need to apply forces on it again.
        /// </summary>
        /// <param name="sender">The node view</param>
        /// <param name="e">The mouse event arguments</param>
        private void MouseManager_DropOnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.node.IsSelected)
            {
                this.node.PhysicRepresentation.MakeFree();
            }

            if (this.Drop != null)
            {
                this.Drop(this, e);
            }
        }

        /// <summary>
        /// this is the function called when the mouse is over the node GUI
        /// </summary>
        /// <param name="sender">the node GUI</param>
        /// <param name="e">the event arguments</param>
        private void NodeViewManager_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.actionMenu == null)
            {
                System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500); 
                myDispatcherTimer.Tick += delegate(object s, EventArgs args)
                {
                    myDispatcherTimer.Stop();
                    this.SetActionMenu();
                };
                ((UIElement)this.guiComponent).MouseLeave += delegate(object s, MouseEventArgs args)
                {
                    myDispatcherTimer.Stop();
                };
                myDispatcherTimer.Start();
            }
        }

        /// <summary>
        /// this function update the information provided to the view ( e.g. display-name )
        /// </summary>
        private void TryUpdateGUIData()
        {
            // set the title of the node
            try
            {
                this.guiComponent.Title = this.node.Title;
            }
            catch (System.Exception) 
            {
                /* Create an error message */
                this.viewManager.AddDebugMessage("An Error occured in the UserControl \"" +
                this.guiComponent.GetType().FullName +
                "\" when the program tried to change the \"Title\" property to \"" +
                this.node.Title + "\".");
            }

            // set the title of the node
            try
            {
                XDocument doc = XDocument.Parse(this.node.DrawingInformation);
                this.guiComponent.NodeDrawingInformation = doc.Root;
            }
            catch (System.Exception)
            {
                /* Create an error message */
                this.viewManager.AddDebugMessage("An Error occured in the UserControl \"" +
                this.guiComponent.GetType().FullName +
                "\" when the program tried to change the \"NodeDrawingInformation\" property.");
            }
        }

        #endregion
    }
}
