//-----------------------------------------------------------------------
// <copyright file="Action.cs" company="International Monetary Fund">
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

namespace ICE.action
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Xml.Linq;

    /// <summary>
    /// This class represents an action that could be used on a link or a node.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// this is the id of the current action
        /// </summary>
        private string name;

        /// <summary>
        /// This is the list of all tasks you need to perform to execute the action
        /// </summary>
        private List<Task> tasks = new List<Task>();

        /// <summary>
        /// This is the image used in the icon field of the action view
        /// </summary>
        private ImageSource iconSource;

        /// <summary>
        /// This is the description of the action
        /// </summary>
        private string description;

        /// <summary>
        /// This field indicates whether the current instance is a groupAction or not
        /// </summary>
        private bool isGroupAction;

        /// <summary>
        /// Initializes a new instance of the Action class
        /// </summary>
        /// <param name="xmlAction">the Xml definition of t</param>
        public Action(XElement xmlAction)
        {
            this.isGroupAction = xmlAction.Name.LocalName == xml.SettingsXmlContent.GroupableActionElementName;
            this.name = xmlAction.Attribute(xml.SettingsXmlContent.IDAttributeOfActionElementName).Value;
            string url = xmlAction.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.IconURLElementOfActionElementName).Value;
            this.iconSource = new BitmapImage(new Uri(url));
            this.description = xmlAction.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.DescriptionElementOfActionElementName).Value;
            XElement xmlTasks = xmlAction.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.TasksElementOfActionElementName);
            foreach (XElement xmlElement in xmlTasks.Elements())
            {
                Task task = Task.CreateTask(xmlElement);
                if (task != null)
                {
                    this.tasks.Add(task);
                }
            }
        }

        /// <summary>
        /// Prevents a default instance of the Action class from being created
        /// Initializes a new instance of the Action class. But this class is empty
        /// </summary>
        private Action()
        {
            /*do nothing*/
        }

        /// <summary>
        /// Gets or sets the name of the current action
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current instance is a group action or not.
        /// </summary>
        public bool IsGroupAction
        {
            get { return this.isGroupAction; }
            set { this.isGroupAction = value; }
        }

        /// <summary>
        /// Gets or sets the Icon of the action
        /// </summary>
        public ImageSource IconSource
        {
            get { return this.iconSource; }
            set { this.iconSource = value; }
        }

        /// <summary>
        /// Gets or sets description of the action
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        /// <summary>
        /// this function creates the special action: Expand (a plus sign icon)
        /// </summary>
        /// <returns>an instance of the Expand action</returns>
        public static Action GetExpandAction()
        {
            Action action = new Action();

            action.description = "Expand node";
            Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ICE.embeddedImages.Expand.png");
            BitmapImage iconSource = new BitmapImage();
            iconSource.SetSource(imageStream);
            action.iconSource = iconSource;
            action.tasks.Add(new Select());
            action.tasks.Add(new GUITask("select"));

            return action;
        }

        /// <summary>
        /// this function creates the special action: Collapse (a minus sign icon)
        /// </summary>
        /// <returns>an instance of the collapse action</returns>
        public static Action GetCollapseAction()
        {
            Action action = new Action();

            action.description = "Collapse node";
            Stream imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ICE.embeddedImages.Collapse.png");
            BitmapImage iconSource = new BitmapImage();
            iconSource.SetSource(imageStream);
            action.iconSource = iconSource;
            action.tasks.Add(new Deselect());
            action.tasks.Add(new GUITask("deselect"));

            return action;
        }

        /// <summary>
        /// this function must execute the task on the selected target, the caller could hold some parameter
        /// </summary>
        /// <param name="targets">the objects on which we want to execute the task</param>
        public void PerformAction(IEnumerable<IActionable> targets)
        {
            foreach (Task task in this.tasks)
            {
                task.PerformTask(targets, this.name);
            }
        }
    }
}
