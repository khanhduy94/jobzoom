//-----------------------------------------------------------------------
// <copyright file="ViewManager.cs" company="International Monetary Fund">
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

namespace ICE.view
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Threading;

    /// <summary>
    /// This class creates and manages the visual part of an ICE Instance.
    /// </summary>
    public class ViewManager
    {
        #region Fields

        /// <summary>
        /// this Grid contain all visual component of ICE
        /// </summary>
        private Grid iceVisualComponentContainer;

        /// <summary>
        /// this Canvas is use to draw the graph components
        /// </summary>
        private Canvas graphCanvas;

        /// <summary>
        /// this Canves contain all controler such has the navigation bar
        /// </summary>
        private Grid controlerGrid;

        /// <summary>
        /// this property is the area where we can use the drag and drop to move
        /// </summary>
        private Rectangle gripArea;

        /// <summary>
        /// this property is the background of ice
        /// </summary>
        private Grid background;

        /// <summary>
        /// the navigation bar in the user's interface
        /// </summary>
        private UserNavigationBar navigationBar;

        /// <summary>
        /// navigation menu in the user's interface
        /// </summary>
        private UserNavigationMenu navigationMenu;

        /// <summary>
        /// the message bar in the view
        /// </summary>
        private ErrorBar messageBar;

        /// <summary>
        /// this is the current scale change effects
        /// </summary>
        private visualEffect.IVisualEffect currentScaleChangeEffect;

        /// <summary>
        /// this property is the curent scale rate.
        /// Default value is 1.
        /// </summary>
        private double currentZoomRatio;

        /// <summary>
        /// this property is the mouse management for the main canvas
        /// </summary>
        private MouseManager mouseManager;

        /// <summary>
        /// this property is the center of our screen in the physic representation
        /// </summary>
        private mathematics.Vector3D center;

        /// <summary>
        /// this property is the last situation of the mouse (in the physic representation) during a drag and drop
        /// </summary>
        private mathematics.Vector3D mouseSituationFromLastMove;

        /// <summary>
        /// this is the model manager
        /// </summary>
        private model.ModelManager modelManager;

        /// <summary>
        /// this property is the node that is on focus
        /// </summary>
        /// TODO
        private List<model.Node> selectedNodes = new List<ICE.model.Node>();

        /// <summary>
        /// this is the list of all node view
        /// </summary>
        private List<NodeViewManager> nodeViewList = new List<NodeViewManager>();

        /// <summary>
        /// this is the list of all link view
        /// </summary>
        private List<LinkViewManager> linkViewList = new List<LinkViewManager>();

        /// <summary>
        /// this property is the list of all popup drawn on the graph and their respective node.
        /// </summary>
        private Dictionary<object, IPopUp> activePopup = new Dictionary<object, IPopUp>();

        /// <summary>
        /// this is the list off all active visual effect
        /// </summary>
        private List<visualEffect.IVisualEffect> visualEffectList;

        /// <summary>
        /// this property is the current style applied on the view
        /// </summary>
        private setting.IViewSettings settings;

        /// <summary>
        /// this property is the array of zoom which select the good zoom in function of the 
        /// </summary>
        private double[] arrayZoom = new double[41];

        /// <summary>
        /// this property is the value of in array of zoom
        /// </summary>
        private int currentZoom = 20;

        /// <summary>
        /// this field is the splach screen
        /// </summary>
        private SplashScreen splachScreen;

        /// <summary>
        /// the timer that update the visual system;
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// This field is the current menu of action on the screen
        /// </summary>
        /// <remarks>If there is no menu on the screen, this field should be null</remarks>
        private ActionMenu currentActionMenu;

        #endregion
        
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ViewManager class.
        /// </summary>
        /// <param name="model">the model to observe</param>
        public ViewManager(model.ModelManager model)
        {
            this.InitializeComponent();
            
            this.modelManager = model;
            this.modelManager.NodeTypesChanged += new EventHandler(this.ModelManager_NodeTypesChanged);
            this.modelManager.LinkTypesChanged += new EventHandler(this.ModelManager_LinkTypesChanged);

            this.center = new mathematics.Vector3D(0, 0, 0);
            this.visualEffectList = new List<ICE.view.visualEffect.IVisualEffect>();
            
            this.Settings = new setting.Settings();
            this.splachScreen = new SplashScreen();
            
            this.mouseManager = new MouseManager(this.gripArea);
            this.mouseManager.DragOnLeftButtonDown += new MouseEventHandler(this.BeginDragAction);
            this.mouseManager.MouseMovedOnLeftButtonDown += new MouseEventHandler(this.MoveFromMouseMovement);
            this.mouseManager.WheelMouseDown += new MouseEventHandler(this.WheelMouseDown);
            this.mouseManager.WheelMouseUp += new MouseEventHandler(this.WheelMouseUp);

            this.gripArea.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs arg)
            {
                if (this.currentActionMenu != null)
                {
                    this.currentActionMenu.Close();
                    this.currentActionMenu = null;
                }
            };

            // create the timer
            this.timer = new DispatcherTimer();
        }

        #endregion

        #region Events & Delegates

        /// <summary>
        /// this event is raised whether the user asks for increasing by one the depth of exploration
        /// </summary>
        public event EventHandler IncreaseDepth;

        /// <summary>
        /// this event is raised whether the user asks for decreasing by one the depth of exploration
        /// </summary>
        public event EventHandler DecreaseDepth;

        /// <summary>
        /// This event is raised whether the zoomRatio has changed
        /// </summary>
        public event EventHandler ZoomRatioChanged;

        #endregion
        
        #region Properties
        
        /// <summary>
        /// Gets the current splach screen
        /// </summary>
        public SplashScreen SplachScreen
        {
            get { return this.splachScreen; }
        }

        /// <summary>
        /// Gets the message bar
        /// </summary>
        public ErrorBar MessageBar
        {
            get { return this.messageBar; }
        }

        /// <summary>
        /// Gets or sets the current settings applied on the view
        /// </summary>
        public setting.IViewSettings Settings
        {
            get
            {
                return this.settings;
            }

            set
            {
                if (this.settings != null)
                {
                    this.settings.Changed -= new EventHandler(this.Settings_Changed);
                }

                this.settings = value;
                this.settings.Changed += new EventHandler(this.Settings_Changed);
                this.Settings_Changed(null, null);
            }
        }

        /// <summary>
        /// Gets the list off all active visual effect ( get only )
        /// </summary>
        public List<visualEffect.IVisualEffect> VisualEffectList
        {
            get { return this.visualEffectList; }
        }

        /// <summary>
        /// Gets the model manager
        /// </summary>
        public model.ModelManager ModelManager
        {
            get { return this.modelManager; }
        }

        /// <summary>
        /// Gets or sets the curent scale rate.
        /// Default value is 1.
        /// </summary>
        public double CurrentZoomRatio
        {
            get 
            {
                return this.currentZoomRatio; 
            }
            
            set 
            {
                this.currentZoomRatio = value;
                this.navigationBar.slidezoom.Value = this.currentZoom - 20;
                if (this.ZoomRatioChanged != null)
                {
                    this.ZoomRatioChanged(this, new EventArgs());
                }
            }
        }

        #endregion
        
        #region Public Functions

        /// <summary>
        /// This function adds the actionDrawer in argument to the graph canvas and ask the older one to close. 
        /// </summary>
        /// <param name="menu">the action menu to display</param>
        public void DisplayActionMenu(ActionMenu menu)
        {
            if (this.currentActionMenu != null && (this.currentActionMenu.IsOpen || this.currentActionMenu.IsOpening))
            {
                this.currentActionMenu.Close();
            }

            this.currentActionMenu = menu;
            menu.Closed += new EventHandler(this.ActionMenu_Closed);
            this.graphCanvas.Children.Add(menu);
        }

        /// <summary>
        /// This function remove the menu in argument
        /// </summary>
        /// <param name="menu">the action drawer</param>
        public void RemoveActionMenu(ActionMenu menu)
        {
            this.graphCanvas.Children.Remove(menu);
        }

        /// <summary>
        /// This function replace a component with another
        /// </summary>
        /// <param name="userControlToReplace">the component that will BE replace</param>
        /// <param name="userControlThatRelpace">the component that will replace the first one</param>
        public void ReplaceGraphComponent(UserControl userControlToReplace, UserControl userControlThatRelpace) 
        {
            double left = Canvas.GetLeft(userControlToReplace);
            double top = Canvas.GetTop(userControlToReplace);
            this.graphCanvas.Children.Remove(userControlToReplace);
            this.graphCanvas.Children.Add(userControlThatRelpace);
            Canvas.SetLeft(userControlThatRelpace, left);
            Canvas.SetTop(userControlThatRelpace, top);
        }

        /// <summary>
        /// This function raise the event IncreaseDepth
        /// </summary>
        public void RaiseIncreaseDepth()
        {
            if (this.IncreaseDepth != null)
            {
                this.IncreaseDepth(this, new EventArgs());
            }
        }

        /// <summary>
        /// this function raise the event DecreaseDepth
        /// </summary>
        public void RaiseDecreaseDepth()
        {
            if (this.IncreaseDepth != null)
            {
                this.DecreaseDepth(this, new EventArgs());
            }
        }

        /// <summary>
        /// This function start the physic management thread
        /// </summary>
        public void Start()
        {
            // initilising the update timer
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            this.timer.Tick += new EventHandler(this.Update);
            this.timer.Start();
        }

        /// <summary>
        /// This function pause the physic management thread
        /// </summary>
        public void Pause()
        {
            this.timer.Stop();
        }

        /// <summary>
        /// This function resume the physic management thread
        /// </summary>
        public void Resume()
        {
            this.timer.Start();
        }

        /// <summary>
        /// this function show the splach screen on the screen
        /// </summary>
        public void ShowSplachScreen()
        {
            this.iceVisualComponentContainer.Children.Add(this.splachScreen);
        }

        /// <summary>
        /// this function check if the splach screen is on the screen
        /// </summary>
        /// <returns>return a value indicating whether the splash screen is displayed or not</returns>
        public bool CheckSplachScreen()
        {
            return this.iceVisualComponentContainer.Children.Contains(this.splachScreen);
        }

        /// <summary>
        /// this function hide the splach screen
        /// </summary>
        public void HideSplachScreen()
        {
            if (this.CheckSplachScreen() == true)
            {
                this.iceVisualComponentContainer.Children.Remove(this.splachScreen);
            }
        }

        /// <summary>
        /// This function adds an debug message to the Debug GUI
        /// </summary>
        /// <param name="message">an error message</param>
        public void AddDebugMessage(string message)
        {
            if (this.settings.DebugMode)
            {
                this.GetView().Dispatcher.BeginInvoke(delegate
                {
                    this.SplachScreen.AddMessage(message);
                    this.MessageBar.AddMessage(message);
                });
            }           
        }

        /// <summary>
        /// This function adds a message to the GUI
        /// </summary>
        /// <param name="message">a message to draw</param>
        public void AddMessage(string message)
        {
            this.GetView().Dispatcher.BeginInvoke(delegate
            {
                this.SplachScreen.AddMessage(message);
                this.MessageBar.AddMessage(message);
            });
        }

        /// <summary>
        /// This function returns the drawing canvas
        /// </summary>
        /// <returns>the canvas</returns>
        public UIElement GetView()
        {
            return this.iceVisualComponentContainer;
        }

        /// <summary>
        /// This function transform physic engine coordonate in the current canvas coordonate
        /// </summary>
        /// <param name="vector">the physics engine coodonate</param>
        /// <returns>the equivalent coordonate in the canvas</returns>
        public Point GetCanvasCoordonate(mathematics.Vector3D vector)
        {
            Point point = new Point();
            point.X = ((vector.X - this.center.X) * this.CurrentZoomRatio) + (this.graphCanvas.ActualWidth / 2d);
            point.Y = ((vector.Y - this.center.Y) * this.CurrentZoomRatio) + (this.graphCanvas.ActualHeight / 2d);
            return point;
        }

        /// <summary>
        /// This function tranform the current canvas coordonate to physic engine coordonate
        /// </summary>
        /// <param name="vector">the 3D vector to set</param>
        /// <param name="canvasCoordonate">the point in canvas coordonate</param>
        public void SetVectorFromCanvasCoodonate(mathematics.Vector3D vector, Point canvasCoordonate)
        {
            vector.X = Convert.ToSingle(((canvasCoordonate.X - (this.graphCanvas.ActualWidth / 2d)) / this.CurrentZoomRatio) + this.center.X);
            vector.Y = Convert.ToSingle(((canvasCoordonate.Y - (this.graphCanvas.ActualHeight / 2d)) / this.CurrentZoomRatio) + this.center.Y);
        }

        /// <summary>
        /// this function sets a visual component to the node in argument
        /// the visual content depend of the node style reference and the current style configuration.
        /// </summary>
        /// <param name="node">the node model</param>
        public void SetViewToNode(model.Node node)
        {
            bool exist = false;
            foreach (NodeViewManager item in this.nodeViewList)
            {
                exist |= item.Node == node;
            }

            if (exist)
            {
                // if the node already have a view manager, then return>
                return;
            }

            // create the class
            NodeViewManager nodeViewManager = new NodeViewManager(node, this);
            nodeViewManager.DragedAndMove += new MouseEventHandler(this.NodeViewManager_DragedAndMove);
            this.nodeViewList.Add(nodeViewManager);
        }

        /// <summary>
        /// this function sets a visual component to the node in argument
        /// the visual content depend of the node style reference and the current style configuration.
        /// </summary>
        /// <param name="link">the link model</param>
        public void SetViewToLink(model.Link link)
        {
            bool exist = false;

            foreach (LinkViewManager item in this.linkViewList)
            {
                exist |= item.Link == link;
            }

            if (exist)
            {
                // if the node already have a view manager, then return>
                return;
            }

            // we create the view
            LinkViewManager linkManager = new LinkViewManager(link, this);
            this.linkViewList.Add(linkManager);
        }

        /// <summary>
        /// this function make a zoom out action
        /// </summary>
        public void ZoomOut()
        {
            this.currentZoom = this.currentZoom - 3;

            if (this.currentZoom < 0)
            {
                this.currentZoom = 0;
            }

            if (this.currentScaleChangeEffect != null)
            {
                this.currentScaleChangeEffect.End();
            }

            this.currentScaleChangeEffect = this.settings.ChangeScale(this, this.arrayZoom[this.currentZoom]);
            this.currentScaleChangeEffect.Begin();
        }

        /// <summary>
        /// this function make a zoom in action
        /// </summary>
        public void ZoomIn()
        {
            this.currentZoom = this.currentZoom + 3;

            if (this.currentZoom > 40)
            {
                this.currentZoom = 40;
            }

            if (this.currentScaleChangeEffect != null)
            {
                this.currentScaleChangeEffect.End();
            }

            this.currentScaleChangeEffect = this.settings.ChangeScale(this, this.arrayZoom[this.currentZoom]);
            this.currentScaleChangeEffect.Begin();
        }

        /// <summary>
        /// this function changes the zoom value by creating an animation
        /// </summary>
        /// <param name="ratio">the final zoom ratio</param>
        public void Zoom(int ratio)
        {
            lock (this)
            {
                this.currentZoom = ratio;

                if (this.currentScaleChangeEffect != null)
                {
                    this.currentScaleChangeEffect.End();
                }

                this.currentScaleChangeEffect = this.settings.ChangeScale(this, this.arrayZoom[this.currentZoom]);
                this.currentScaleChangeEffect.Begin();
            }
        }

        /// <summary>
        /// this function go to left for 12 units
        /// </summary>
        public void GoLeft()
        {
            this.center.X -= (int)(12 * this.CurrentZoomRatio);
        }

        /// <summary>
        /// this function go to right for 12 units
        /// </summary>
        public void GoRight()
        {
            this.center.X += (int)(12 * this.CurrentZoomRatio);
        }

        /// <summary>
        /// this function center the screen to the selected node
        /// </summary>
        /// <remarks>DEPRECATED</remarks>
        public void GoToSelectedNode()
        {
            if (this.selectedNodes.Count >= 1) 
            {
                this.center.Set(this.selectedNodes.ToArray()[0].PhysicRepresentation.Position); 
            }
            else 
            { 
                this.center.Set(0, 0, 0); 
            }
        }

        /// <summary>
        /// this function go to up for 12 units
        /// </summary>
        public void GoUp()
        {
            this.center.Y -= (int)(12 * this.CurrentZoomRatio);
        }

        /// <summary>
        /// this function go to down for 12 units
        /// </summary>
        public void GoDown()
        {
            this.center.Y += (int)(12 * this.CurrentZoomRatio);
        }

        /// <summary>
        /// This function remove progressively a view from the graph.
        /// </summary>
        /// <param name="userControl">the UserControl you need to remove</param>
        public void RemoveUserControlFromCanvas(UserControl userControl)
        {
            userControl.Opacity -= this.Settings.OpacityChangeStep;

            if (userControl.Opacity <= 0d)
            {
                this.graphCanvas.Children.Remove(userControl);
            }
        }

        /// <summary>
        /// This function adds or maintain a user controle in the Canvas.
        /// </summary>
        /// <param name="userControl">the userControl you want to add or maintain in the graph</param>
        public void AddOrMaintainUserControlInCanvas(UserControl userControl)
        {
            // if the userControl isn't in the graph, but must be in the graph.
            if (!this.IsInGraph(userControl))
            {
                // set basic opacity
                userControl.Opacity = this.Settings.OpacityChangeStep;

                // add userControl to the graph
                this.graphCanvas.Children.Add(userControl);

                if (userControl is INodeView)
                {
                    Canvas.SetZIndex(userControl, 0);
                }

                if (userControl is ILinkView)
                {
                    Canvas.SetZIndex(userControl, -1000);
                }

                if (userControl is IPopUp)
                {
                    Canvas.SetZIndex(userControl, 1000);
                }
            }
            else
            {
                // if the userControle isn't in the graph
                // if the the opacity is not to 100%, and in the graph, we progressively set it to 100 percent of opacity
                if (userControl.Opacity < 1d)
                {
                    userControl.Opacity += this.Settings.OpacityChangeStep;
                }
            }
        }

        /// <summary>
        /// This function returns if yes or not the usercontrol in argument is in the graph
        /// </summary>
        /// <param name="userControl">the user control that could be in the graph panel</param>
        /// <returns>True if it's in, False if not</returns>
        public bool IsInGraph(UserControl userControl)
        {
            return this.graphCanvas.Children.Contains(userControl);
        }

        #endregion

        /// <summary>
        /// this function creates the xaml component in argument and adds it in the grid in argument and in the row in argument.
        /// </summary>
        /// <param name="xamlRow">the row number</param>
        /// <param name="xaml">the xaml code in a string</param>
        /// <param name="grid">the grid to fill</param>
        /// <remarks>
        /// To run this function in debug mode you MUST delete the XamlParseException from the debug-exception manager
        /// [Debuguer/Exceptions/Common Language Runtime Exception/System.Windows.Markup/XamlParseException]
        /// (you must uncheck all checkboxes)
        /// </remarks>
        internal static void SetXamlIntoGrid(int xamlRow, string xaml, Grid grid)
        {
            // xaml reading
            try
            {
                FrameworkElement info = (FrameworkElement)XamlReader.Load(xaml);
                grid.Children.Add(info);
                Grid.SetRow(info, xamlRow);
            }
            catch (XamlParseException exception)
            {
                // if the xaml is invalid
                TextBlock error = new TextBlock();
                error.HorizontalAlignment = HorizontalAlignment.Center;
                error.VerticalAlignment = VerticalAlignment.Center;
                error.TextWrapping = TextWrapping.Wrap;
                error.Text = "Error: The information provided is not in a valid XAML format\n\n" + exception.Message;
                grid.Children.Add(error);
                Grid.SetRow(error, xamlRow);
            }
            catch (InvalidCastException exception)
            {
                // if the root is not a FrameworkElement
                TextBlock error = new TextBlock();
                error.HorizontalAlignment = HorizontalAlignment.Center;
                error.VerticalAlignment = VerticalAlignment.Center;
                error.TextWrapping = TextWrapping.Wrap;
                error.Text = "Error: Invalid root type in the provided XAML file\n\n" + exception.Message;
                grid.Children.Add(error);
                Grid.SetRow(error, xamlRow);
            }
            catch (Exception exception)
            {
                // if any other exception occurs
                TextBlock error = new TextBlock();
                error.HorizontalAlignment = HorizontalAlignment.Center;
                error.VerticalAlignment = VerticalAlignment.Center;
                error.TextWrapping = TextWrapping.Wrap;
                error.Text = "Error: An exception has occured while loading the popup\n\n" + exception.Message;
                grid.Children.Add(error);
                Grid.SetRow(error, xamlRow);
            }
        }

        #region Private Functions

        /// <summary>
        /// This funtion open the menu if you want to drop something in it
        /// </summary>
        /// <param name="sender">a node view management system</param>
        /// <param name="e">last mouse position</param>
        private void NodeViewManager_DragedAndMove(object sender, MouseEventArgs e)
        {
            if (this.navigationMenu.IsMouseOver(e))
            {
                // open the menu panel
                this.navigationMenu.OpenDropTab();

                this.navigationMenu.BeginHighlight();

                // subscribe to drop event
                ((NodeViewManager)sender).Drop += new MouseEventHandler(this.ViewManager_Drop);
                ((NodeViewManager)sender).DragedAndMove -= new MouseEventHandler(this.NodeViewManager_DragedAndMove);
                ((NodeViewManager)sender).DragedAndMove += new MouseEventHandler(this.NodeViewManager_DragedAndMoveFromMenu);
            }
        }

        /// <summary>
        /// This funtion observe if you go out of the menu without droping
        /// </summary>
        /// <param name="sender">a node view management system</param>
        /// <param name="e">last mouse position</param>
        private void NodeViewManager_DragedAndMoveFromMenu(object sender, MouseEventArgs e)
        {
            if (!this.navigationMenu.IsMouseOver(e))
            {
                this.navigationMenu.EndHighlight();

                // unsubscribe to drop event
                ((NodeViewManager)sender).Drop -= new MouseEventHandler(this.ViewManager_Drop);
                ((NodeViewManager)sender).DragedAndMove -= new MouseEventHandler(this.NodeViewManager_DragedAndMoveFromMenu);
                ((NodeViewManager)sender).DragedAndMove += new MouseEventHandler(this.NodeViewManager_DragedAndMove);
            }
        }

        /// <summary>
        /// this function send the node to the drop menu
        /// </summary>
        /// <param name="sender">the node management system</param>
        /// <param name="e">mouse event arguments</param>
        private void ViewManager_Drop(object sender, MouseEventArgs e)
        {
            this.navigationMenu.EndHighlight();

            // un suscribe do drop event
            ((NodeViewManager)sender).Drop -= new MouseEventHandler(this.ViewManager_Drop);
            ((NodeViewManager)sender).DragedAndMove -= new MouseEventHandler(this.NodeViewManager_DragedAndMoveFromMenu);
            ((NodeViewManager)sender).DragedAndMove += new MouseEventHandler(this.NodeViewManager_DragedAndMove);

            // send the node in the menu
            this.navigationMenu.DropNode((NodeViewManager)sender);
        }

        /// <summary>
        /// The update function
        /// Each modification in the drawing part must be updated from this function
        /// </summary>
        /// <param name="sender">the timer of the physics engine</param>
        /// <param name="e">the event argument</param>
        private void Update(object sender, EventArgs e)
        {
            // Step1 : update node view
            foreach (NodeViewManager nodeView in new List<NodeViewManager>(this.nodeViewList))
            {
                nodeView.Update();

                if (nodeView.IsDisposed)
                {
                    this.nodeViewList.Remove(nodeView);
                }
            }

            // Step2 : update link view
            foreach (LinkViewManager linkView in new List<LinkViewManager>(this.linkViewList))
            {
                linkView.Update();

                if (linkView.IsDisposed)
                {
                    this.linkViewList.Remove(linkView);
                }
            }

            // Step3 : execute visual effects
            foreach (visualEffect.IVisualEffect effect in new List<visualEffect.IVisualEffect>(this.visualEffectList))
            {
                effect.Execute();
            }
        }

        /// <summary>
        /// this function is called when an action menu has finished closing
        /// </summary>
        /// <param name="sender">the action menu</param>
        /// <param name="e">event arguments</param>
        private void ActionMenu_Closed(object sender, EventArgs e)
        {
            this.RemoveActionMenu((ActionMenu)sender);
        }

        /// <summary>
        /// This function is called when the stettings has changed
        /// </summary>
        /// <param name="sender">the settings</param>
        /// <param name="e">the event argument</param>
        private void Settings_Changed(object sender, EventArgs e)
        {
            this.CurrentZoomRatio = this.settings.InitialZoomRatio;
            this.BuildZoomArray();
            this.background.Children.Clear();
            this.background.Children.Add(this.settings.Background);
            this.SetNavigationBar();
            this.SetNavigationMenu();
        }

        /// <summary>
        /// this function createsor recreate the array of zoom we use from the default zoom
        /// </summary>
        private void BuildZoomArray()
        {
            int i = 0;
            this.arrayZoom[20] = this.Settings.InitialZoomRatio;
            for (i = 21; i <= 40; i++)
            {
                this.arrayZoom[i] = this.arrayZoom[i - 1] * 1.2;
            }

            for (i = 19; i >= 0; i--)
            {
                this.arrayZoom[i] = this.arrayZoom[i + 1] / 1.2;
            }
        }

        /// <summary>
        /// this function initialize the visual components
        /// </summary>
        private void InitializeComponent()
        {
            this.iceVisualComponentContainer = new Grid();
            this.graphCanvas = new Canvas();

            this.controlerGrid = new Grid();
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            this.controlerGrid.RowDefinitions.Add(row);
            row = new RowDefinition();
            row.Height = new GridLength(1.3, GridUnitType.Star);
            this.controlerGrid.RowDefinitions.Add(row);

            this.background = new Grid();

            this.gripArea = new Rectangle();
            this.gripArea.Stretch = Stretch.Fill;
            Canvas.SetZIndex(this.gripArea, -10000);
            this.gripArea.Fill = new SolidColorBrush(Colors.Transparent);

            Canvas.SetZIndex(this.background, -10001);

            this.navigationBar = new UserNavigationBar(this);
            this.navigationMenu = new UserNavigationMenu();
            this.navigationMenu.Click += new RoutedEventHandler(this.NavigationMenu_Click);

            this.messageBar = new ErrorBar();

            this.iceVisualComponentContainer.Children.Add(this.background);
            this.iceVisualComponentContainer.Children.Add(this.messageBar);
            this.iceVisualComponentContainer.Children.Add(this.graphCanvas);
            this.iceVisualComponentContainer.Children.Add(this.controlerGrid);
            this.iceVisualComponentContainer.Children.Add(this.gripArea);
        }

        /// <summary>
        /// this function Apply all the Visual Effect for this frame
        /// </summary>
        private void ApplyVisualEffect()
        {
            List<visualEffect.IVisualEffect> list = new List<ICE.view.visualEffect.IVisualEffect>(this.visualEffectList);
            foreach (visualEffect.IVisualEffect visualEffect in list)
            {
                visualEffect.Execute();
            }
        }

        /// <summary>
        /// this funtion use the wheel mouse up to zoom in
        /// </summary>
        /// <param name="sender">the grip area</param>
        /// <param name="e">the arguments</param>
        private void WheelMouseUp(object sender, MouseEventArgs e)
        {
            this.ZoomIn();
        }

        /// <summary>
        /// this funtion use the wheel mouse down to zoom out
        /// </summary>
        /// <param name="sender">the grip area</param>
        /// <param name="e">the arguments</param>
        private void WheelMouseDown(object sender, MouseEventArgs e)
        {
            this.ZoomOut();
        }

        /// <summary>
        /// this funtion use the grip area to move the center of the graph
        /// </summary>
        /// <param name="sender">the grip area</param>
        /// <param name="e">the arguments</param>
        private void MoveFromMouseMovement(object sender, MouseEventArgs e)
        {
            mathematics.Vector3D currentMousePosition = new mathematics.Vector3D();
            this.SetVectorFromCanvasCoodonate(currentMousePosition, e.GetPosition(this.graphCanvas));

            // we create an opposit vector of the mouse movement by making the substraction between the two vector
            this.mouseSituationFromLastMove.Subtract(currentMousePosition);

            // we add the move vector to the center of the view
            this.center.Add(this.mouseSituationFromLastMove);

            // we get the new situation of the mouse after moving the center
            this.SetVectorFromCanvasCoodonate(this.mouseSituationFromLastMove, e.GetPosition(this.graphCanvas));
        }

        /// <summary>
        /// this funtion initialize the drag and drop movement
        /// </summary>
        /// <param name="sender">the grip area</param>
        /// <param name="e">the arguments</param>
        private void BeginDragAction(object sender, MouseEventArgs e)
        {
            this.mouseSituationFromLastMove = new mathematics.Vector3D();

            // we get the new situation of the mouse before any movement
            this.SetVectorFromCanvasCoodonate(this.mouseSituationFromLastMove, e.GetPosition(this.graphCanvas));
        }

        /// <summary>
        /// this function sets the state of the navigation bar
        /// </summary>
        private void SetNavigationBar()
        {
            if (this.settings.NavigationBarMode)
            {
                if (!this.controlerGrid.Children.Contains(this.navigationBar))
                {
                    this.navigationBar.VerticalAlignment = VerticalAlignment.Top;
                    this.navigationBar.HorizontalAlignment = HorizontalAlignment.Left;
                    Grid.SetRow(this.navigationMenu, 0);
                    this.controlerGrid.Children.Add(this.navigationBar);
                }
            }
            else
            {
                if (this.controlerGrid.Children.Contains(this.navigationBar))
                {
                    this.controlerGrid.Children.Remove(this.navigationBar);
                }
            }
        }

        /// <summary>
        /// this function sets the state of the navigation menu
        /// </summary>
        private void SetNavigationMenu()
        {
            if (this.settings.NavigationMenuMode)
            {
                if (!this.controlerGrid.Children.Contains(this.navigationMenu))
                {
                    this.navigationMenu.VerticalAlignment = VerticalAlignment.Stretch;
                    this.navigationMenu.HorizontalAlignment = HorizontalAlignment.Left;
                    Grid.SetRow(this.navigationMenu, 1);
                    this.controlerGrid.Children.Add(this.navigationMenu);
                }
            }
            else
            {
                if (this.controlerGrid.Children.Contains(this.navigationMenu))
                {
                    this.controlerGrid.Children.Remove(this.navigationMenu);
                }
            }
        }

        /// <summary>
        /// This function is called when the list of node types change
        /// </summary>
        /// <param name="sender">the model manager</param>
        /// <param name="e">the event arguments</param>
        private void ModelManager_NodeTypesChanged(object sender, EventArgs e)
        {
            this.GetView().Dispatcher.BeginInvoke(delegate
            {
                this.navigationMenu.UpdateNodeTypeList(this.modelManager.NodeTypeList);
            });
        }

        /// <summary>
        /// This function is called when the list of link types change
        /// </summary>
        /// <param name="sender">the model manager</param>
        /// <param name="e">the event arguments</param>
        private void ModelManager_LinkTypesChanged(object sender, EventArgs e)
        {
            this.navigationMenu.UpdateLinkTypeList(this.modelManager.LinkTypeList);
        }

        /// <summary>
        /// This function is called when the navigation menu recieve a click
        /// </summary>
        /// <param name="sender">the navigation menu</param>
        /// <param name="e">the event arguments</param>
        private void NavigationMenu_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentActionMenu != null)
            {
                this.currentActionMenu.Close();
            }
        }

        #endregion
    }
}
