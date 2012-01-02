//-----------------------------------------------------------------------
// <copyright file="Task.cs" company="International Monetary Fund">
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

    /// <summary>
    /// this interface represent a task
    /// </summary>
    public abstract class Task
    {
        /// <summary>
        /// this function createsa task instance from its Xml representation
        /// </summary>
        /// <param name="xmlTask">the xml representation of your task</param>
        /// <returns>the task instance (return null on an invalid argument)</returns>
        public static Task CreateTask(XElement xmlTask)
        {
            try
            {
                switch (xmlTask.Name.LocalName)
                {
                    case xml.SettingsXmlContent.GUITaskElementName:
                        return new GUITask(xmlTask.Attribute(xml.SettingsXmlContent.NameAttributeOfGUITaskElementName).Value);

                    case xml.SettingsXmlContent.JavascriptTaskElementName:
                        return new JavascriptTask(xmlTask.Attribute(xml.SettingsXmlContent.NameAttributeOfJavascriptTaskElementName).Value);

                    case xml.SettingsXmlContent.DrawPopupTaskElementName:
                        return new DrawPopupTask(xmlTask.Attribute(xml.SettingsXmlContent.NameAttributeOfDrawPopupTaskElementName).Value);

                    default:
                        break;
                }
            }
            catch 
            {
                /* do nothing */
            }

            // if any error occur, return null
            return null;
        }

        /// <summary>
        /// this function must execute the task on the selected target, the caller could hold some parameter
        /// </summary>
        /// <param name="targets">the object on which we want to execute the task</param>
        /// <param name="callStatementId">the action caller ID</param>
        public abstract void PerformTask(IEnumerable<IActionable> targets, string callStatementId);
    }

    /// <summary>
    /// This class represente a task execute on the GUI of the target
    /// </summary>
    public class GUITask : Task
    {
        /// <summary>
        /// this is the name of the GUITask
        /// </summary>
        private string name;

        /// <summary>
        /// Initializes a new instance of the GUITask class.
        /// </summary>
        /// <param name="name">the name of the GUITask</param>
        public GUITask(string name)
        {
            this.name = name;
        }

        #region ITask Members

        /// <summary>
        /// this function must execute the task on the selected target, the caller could hold some parameter
        /// </summary>
        /// <param name="targets">the object on which we want to execute the task</param>
        /// <param name="callStatementId">the action caller ID</param>
        public override void PerformTask(IEnumerable<IActionable> targets, string callStatementId)
        {
            foreach (IActionable target in targets)
            {
                target.PerformGUITask(this.name, target.GetCallStatement(callStatementId));
            }
        }

        #endregion
    }

    /// <summary>
    /// This class represents a javascript call
    /// </summary>
    public class JavascriptTask : Task
    {
        /// <summary>
        /// this is the name of the function you may call by executing this task.
        /// </summary>
        private string functionName;

        /// <summary>
        /// Initializes a new instance of the JavascriptTask class.
        /// </summary>
        /// <param name="functionName">the name of the corresponding javascript function</param>
        public JavascriptTask(string functionName)
        {
            this.functionName = functionName;
        }

        #region ITask Members

        /// <summary>
        /// this function must execute the task on the selected target, the caller could hold some parameter
        /// </summary>
        /// <param name="targets">the object on which we want to execute the task</param>
        /// <param name="callStatementId">the action caller ID</param>
        public override void PerformTask(IEnumerable<IActionable> targets, string callStatementId)
        {
            XElement caller = new XElement(xml.DataXmlContent.Namespace + xml.DataXmlContent.JSParametersElementName);
            foreach (IActionable target in targets)
            {
                caller.Add(target.GetCallStatement(callStatementId));
            }

            JavaScriptManager.Invoke(targets, this.functionName, caller.ToString());
        }

        #endregion
    }

    /// <summary>
    /// This class represents a selection task
    /// </summary>
    public class Select : Task
    {
        #region ITask Members

        /// <summary>
        /// this function must execute the task on target, the caller could hold some parameter
        /// </summary>
        /// <param name="targets">the object on which we want to execute the task</param>
        /// <param name="callStatementId">the action caller ID</param>
        public override void PerformTask(IEnumerable<IActionable> targets, string callStatementId)
        {
            foreach (IActionable target in targets)
            {
                if (target is view.NodeViewManager)
                {
                    ((view.NodeViewManager)target).Node.IsSelected = true;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// This class represents a deselection task
    /// </summary>
    public class Deselect : Task
    {
        #region ITask Members

        /// <summary>
        /// this function must execute the task on target, the caller could hold some parameter
        /// </summary>
        /// <param name="targets">the object on which we want to execute the task</param>
        /// <param name="callStatementId">the action caller ID</param>
        public override void PerformTask(IEnumerable<IActionable> targets, string callStatementId)
        {
            foreach (IActionable target in targets)
            {
                if (target is view.NodeViewManager)
                {
                    ((view.NodeViewManager)target).Node.IsSelected = false;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// This class represents the instanciation of a popup on the target
    /// </summary>
    /// NOT YET IMPLEMENTED
    public class DrawPopupTask : Task
    {
        /// <summary>
        /// This is the name of the XmlElement that contain the Xaml information in the popup
        /// </summary>
        private string xmlElementName;

        /// <summary>
        /// Initializes a new instance of the DrawPopupTask class.
        /// </summary>
        /// <param name="xmlElementName">name of the sub element where we supose to find the Xaml to draw</param>
        public DrawPopupTask(string xmlElementName)
        {
            this.xmlElementName = xmlElementName;
        }

        #region ITask Members

        /// <summary>
        /// this function must execute the task on the selected target, the caller could hold some parameter
        /// </summary>
        /// <param name="targets">the object on which we want to execute the task</param>
        /// <param name="callStatementId">the action caller ID</param>
        public override void PerformTask(IEnumerable<IActionable> targets, string callStatementId)
        {
            // TODO: invoke popup
        }

        #endregion
    }
}
