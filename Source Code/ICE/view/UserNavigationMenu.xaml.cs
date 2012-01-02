//-----------------------------------------------------------------------
// <copyright file="UserNavigationMenu.xaml.cs" company="International Monetary Fund">
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
    using System.Linq;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// This class is the menu in the user's interface.
    /// </summary>
    public partial class UserNavigationMenu : UserControl
    {
        /// <summary>
        /// this field indicates whether a node is open or not
        /// </summary>
        private bool isOpen = false;

        /// <summary>
        /// this is the index of the collapsable tab
        /// </summary>
        private int dropTabIndex = 2;

        /// <summary>
        /// the reference type of each checkbox in the Hide / Show node submenu
        /// </summary>
        private Dictionary<model.NodeType, ObjectFilterListItem> nodeTypeListObjectReferences;

        /// <summary>
        /// the reference type of each checkbox in the Hide / Show node submenu
        /// </summary>
        private Dictionary<model.LinkType, ObjectFilterListItem> linkTypeListObjectReferences;

        /// <summary>
        /// This is the list of all nodes dropped in the group panel
        /// </summary>
        private Dictionary<NodeViewManager, ObjectGroupListItem> nodeGroupList;

        /// <summary>
        /// This is the list of all actions you can perform on the group
        /// </summary>
        private Dictionary<action.Action, ActionView> actionOnGroupViewList;

        /// <summary>
        /// this is the list of all actions and their respective targets
        /// </summary>
        private Dictionary<action.Action, List<action.IActionable>> actionTargetsTable;

        /// <summary>
        /// Initializes a new instance of the UserNavigationMenu class.
        /// </summary>
        public UserNavigationMenu()
        {
            InitializeComponent();
            this.nodeTypeListObjectReferences = new Dictionary<ICE.model.NodeType, ObjectFilterListItem>();
            this.linkTypeListObjectReferences = new Dictionary<ICE.model.LinkType, ObjectFilterListItem>();
            this.nodeGroupList = new Dictionary<NodeViewManager, ObjectGroupListItem>();
            this.actionOnGroupViewList = new Dictionary<ICE.action.Action, ActionView>();
            VisualStateManager.GoToState(this, "close", false);
        }

        /// <summary>
        /// this event is raised when the mouse clicks on the component
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        /// update the checkbox list in the Show/Hide nodes submenu
        /// </summary>
        /// <param name="list">list of all types of node in the current model</param>
        public void UpdateNodeTypeList(List<model.NodeType> list)
        {
            List<ObjectFilterListItem> unusedListObjects = new List<ObjectFilterListItem>(this.nodeTypeListObjectReferences.Values);

            foreach (model.NodeType item in list)
            {
                if (this.nodeTypeListObjectReferences.Keys.Contains(item))
                {
                    unusedListObjects.Remove(this.nodeTypeListObjectReferences[item]);
                }
                else
                {
                    // create a new item 
                    ObjectFilterListItem newListObject = new ObjectFilterListItem();
                    newListObject.enableDisable.IsChecked = item.IsEnable;
                    newListObject.showHide.IsChecked = item.IsVisible;
                    newListObject.TargetType = item;
                    newListObject.label.Text = item.Name;

                    // subscribe to Enable/Disable checkbox events
                    newListObject.enableDisable.Checked += delegate(object sender, RoutedEventArgs e) 
                    {
                        ((model.NodeType)newListObject.TargetType).IsEnable = true;
                        this.DisableUselessLinkTypeCheckbox();
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    newListObject.enableDisable.Unchecked += delegate(object sender, RoutedEventArgs e) 
                    {
                        ((model.NodeType)newListObject.TargetType).IsEnable = false;
                        this.DisableUselessLinkTypeCheckbox();
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    // subscribe to Show/Hide checkbox events
                    newListObject.showHide.Checked += delegate(object sender, RoutedEventArgs e)
                    {
                        ((model.NodeType)newListObject.TargetType).IsVisible = true;
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    newListObject.showHide.Unchecked += delegate(object sender, RoutedEventArgs e)
                    {
                        ((model.NodeType)newListObject.TargetType).IsVisible = false;
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    this.nodeTypeListObjectReferences.Add(item, newListObject);
                    this.ShowHideNodeStackPanel.Children.Add(newListObject);
                }
            }

            foreach (ObjectFilterListItem listObject in unusedListObjects)
            {
                this.ShowHideNodeStackPanel.Children.Remove(listObject);
            }

            this.DisableUselessLinkTypeCheckbox();
        }

        /// <summary>
        /// This function returns true if the mouse is over the visible part of the menu
        /// </summary>
        /// <param name="args">mouse situation</param>
        /// <returns>true or false</returns>
        public bool IsMouseOver(MouseEventArgs args)
        {
            Point situation = args.GetPosition(this);
            bool isOver = true;
            isOver &= situation.Y >= 0;
            isOver &= situation.Y <= this.ActualHeight;
            isOver &= situation.X >= 0;
            isOver &= situation.X <= this.ActualWidth;
            return isOver;
        }

        /// <summary>
        /// This function highlights the group menu
        /// </summary>
        public void BeginHighlight()
        {
            border.Opacity = 0.3;
            accordeonGroupMenu.Background = new SolidColorBrush(Color.FromArgb(0x0, 255, 255, 255));
        }

        /// <summary>
        /// This function stops highlighting the group menu
        /// </summary>
        public void EndHighlight()
        {
            border.Opacity = 0.8;
            accordeonGroupMenu.Background = new SolidColorBrush(Color.FromArgb(0x6E, 255, 255, 255));
        }

        /// <summary>
        /// this function openss the tab used to drop a node
        /// </summary>
        public void OpenDropTab()
        {
            if (!this.isOpen)
            {
                this.OpenMenu();
            }

            if (this.menu.SelectedIndex != this.dropTabIndex)
            {
                this.menu.SelectedIndex = this.dropTabIndex;
            }
        }

        /// <summary>
        /// This function is called when the user drops a node in the menu
        /// </summary>
        /// <param name="node">the node instance you want to add to the current group</param>
        public void DropNode(NodeViewManager node)
        {
            if (!this.nodeGroupList.ContainsKey(node))
            {
                node.Node.IsDisposable = false;

                ObjectGroupListItem item = new ObjectGroupListItem();
                node.Node.GUIDataChanged += delegate(object sender, EventArgs args)
                {
                    item.objectTitle.Text = node.Node.Title;
                };
                
                item.objectTitle.Text = node.Node.Title;
                this.nodeGroupList.Add(node, item);

                item.suppressButtonImage.MouseLeftButtonUp += 
                    delegate(object sender, MouseButtonEventArgs args)
                {
                    this.nodeGroupList.Remove(node);
                    node.Node.IsDisposable = true;
                    this.UpdateFromGroupList();
                };

                this.UpdateFromGroupList();
            }
        }

        /// <summary>
        /// update the list of checkboxes in the Show/Hide link submenu
        /// </summary>
        /// <param name="list">the list of all types of links you can find in the model</param>
        public void UpdateLinkTypeList(List<ICE.model.LinkType> list)
        {
            List<ObjectFilterListItem> unusedListObject = new List<ObjectFilterListItem>(this.linkTypeListObjectReferences.Values);

            foreach (model.LinkType item in list)
            {
                if (this.linkTypeListObjectReferences.Keys.Contains(item))
                {
                    unusedListObject.Remove(this.linkTypeListObjectReferences[item]);
                }
                else
                {
                    ObjectFilterListItem newListObject = new ObjectFilterListItem();
                    newListObject.enableDisable.IsChecked = item.IsEnable;
                    newListObject.showHide.IsChecked = item.IsVisible;
                    newListObject.TargetType = item;
                    newListObject.label.Text = item.Name;

                    // subscribe to Enable/Disable checkbox events
                    newListObject.enableDisable.Checked += delegate(object sender, RoutedEventArgs e)
                    {
                        ((model.LinkType)newListObject.TargetType).IsEnable = true;
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    newListObject.enableDisable.Unchecked += delegate(object sender, RoutedEventArgs e)
                    {
                        ((model.LinkType)newListObject.TargetType).IsEnable = false;
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    // subscribe to Show/Hide checkbox events
                    newListObject.showHide.Checked += delegate(object sender, RoutedEventArgs e)
                    {
                        ((model.LinkType)newListObject.TargetType).IsVisible = true;
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    newListObject.showHide.Unchecked += delegate(object sender, RoutedEventArgs e)
                    {
                        ((model.LinkType)newListObject.TargetType).IsVisible = false;
                        if (this.Click != null)
                        {
                            this.Click(sender, e);
                        }
                    };

                    this.linkTypeListObjectReferences.Add(item, newListObject);
                    this.ShowHideLinkStackPanel.Children.Add(newListObject);
                }
            }

            foreach (ObjectFilterListItem listObject in unusedListObject)
            {
                this.ShowHideLinkStackPanel.Children.Remove(listObject);
            }

            this.DisableUselessLinkTypeCheckbox();
        }

        /// <summary>
        /// this function changes the color of linkType Item that are disabled because of the current nodeType hiding situation.
        /// </summary>
        private void DisableUselessLinkTypeCheckbox()
        {
            foreach (model.LinkType linkType in this.linkTypeListObjectReferences.Keys)
            {
                bool isEnable = true;

                foreach (model.NodeType nodetype in this.nodeTypeListObjectReferences.Keys)
                {
                    if ((!nodetype.IsEnable) && linkType.IsRelatedTo(nodetype))
                    {
                        isEnable = false;
                        break;
                    }
                }

                this.linkTypeListObjectReferences[linkType].IsVisualyEnable = isEnable;
            }
        }
        
        /// <summary>
        /// this function opens the menu
        /// </summary>
        private void OpenMenu()
        {
            VisualStateManager.GoToState(this, "open", true);
            this.isOpen = true;
        }

        /// <summary>
        /// this function closes the menu
        /// </summary>
        private void CloseMenu()
        {
            VisualStateManager.GoToState(this, "close", true);
            this.isOpen = false;
        }

        /// <summary>
        /// this function is called when the user asks to clear the group list
        /// </summary>
        /// <param name="sender">the clear button</param>
        /// <param name="e">the event argument</param>
        private void ClearGroupList(object sender, RoutedEventArgs e)
        {
            this.nodeGroupList.Clear();
            this.UpdateFromGroupList();
        }

        /// <summary>
        /// This function updates the available actions from the current group of nodes
        /// </summary>
        private void UpdateFromGroupList()
        {
            foreach (UIElement item in new List<UIElement>(this.nodeListPanel.Children))
            {
                if (!this.nodeGroupList.ContainsValue((ObjectGroupListItem)item))
                {
                    this.nodeListPanel.Children.Remove(item);
                }
            }

            foreach (UIElement item in this.nodeGroupList.Values)
            {
                if (!this.nodeListPanel.Children.Contains(item))
                {
                    this.nodeListPanel.Children.Add(item);
                }
            }

            this.actionTargetsTable = new Dictionary<ICE.action.Action, List<action.IActionable>>();
            foreach (NodeViewManager node in this.nodeGroupList.Keys)
            {
                foreach (action.Action action in node.GetGroupableActions())
                {
                    if (!this.actionTargetsTable.ContainsKey(action))
                    {
                        this.actionTargetsTable.Add(action, new List<action.IActionable>());
                    }

                    this.actionTargetsTable[action].Add(node);
                }
            }

            List<action.Action> sortedActions = new List<ICE.action.Action>(from act in this.actionTargetsTable.Keys orderby this.actionTargetsTable[act].Count descending select act);

            this.ActionListPanel.Children.Clear();

            foreach (action.Action action in sortedActions)
            {
                ActionView view = new ActionView(this.actionTargetsTable[action], action);

                view.MouseEnter += new MouseEventHandler(this.ActionView_MouseEnter);
                view.MouseLeave += new MouseEventHandler(this.ActionView_MouseLeave);

                this.ActionListPanel.Children.Add(view);
            }
        }

        /// <summary>
        /// this function is called when the mouse leaves an action button in the group panel
        /// </summary>
        /// <param name="sender">the action button</param>
        /// <param name="e">the event argument</param>
        private void ActionView_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach (ObjectGroupListItem item in this.nodeGroupList.Values)
            {
                item.objectTitle.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// this function is called when the mouse enters an action button in the group panel
        /// </summary>
        /// <param name="sender">the action button</param>
        /// <param name="e">the event argument</param>
        private void ActionView_MouseEnter(object sender, MouseEventArgs e)
        {
            ActionView view = (ActionView)sender;
            foreach (NodeViewManager node in this.nodeGroupList.Keys)
            {  
                if (!this.actionTargetsTable[view.Action].Contains(node))
                {
                    this.nodeGroupList[node].objectTitle.Foreground = new SolidColorBrush(Colors.Gray);
                }
                else
                {
                    this.nodeGroupList[node].objectTitle.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }

        /// <summary>
        /// This function is called when the user clicks on the menu "Open - Close" button
        /// </summary>
        /// <param name="sender">the button in the interface</param>
        /// <param name="e">the click event</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.isOpen)
            {
                this.CloseMenu();

                if (this.Click != null)
                {
                    this.Click(sender, e);
                }
            }
            else
            {
                this.OpenMenu();

                if (this.Click != null)
                {
                    this.Click(sender, e);
                }
            }
        }
    }
}
