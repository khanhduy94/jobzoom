//-----------------------------------------------------------------------
// <copyright file="NodeStyle.cs" company="International Monetary Fund">
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
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;
    using System.Xml.Linq;
    using ICE.view;
    using ICE.view.visualEffect;

    /// <summary>
    /// This class represents the style of a node and its behaviour
    /// </summary>
    public class NodeStyle
    {
        #region Fields

        /// <summary>
        /// Constructor of the node view related to the style
        /// </summary>
        private ConstructorInfo viewConstructor;

        /// <summary>
        /// This is the Xml definition of all useful information used to apply the style on the graph
        /// </summary>
        private XElement drawingInformation;

        /// <summary>
        /// This is the size multiplicator for the style
        /// </summary>
        private double relativeSize;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the NodeStyle class
        /// </summary>
        public NodeStyle()
        {
            this.SetDefaultFieldValues();
        }

        /// <summary>
        /// Initializes a new instance of the NodeStyle class
        /// </summary>
        /// <param name="xmlStyle">Xml representation of the node style</param>
        /// <param name="assemblyManager">Manager for assembly/name resolution</param>
        public NodeStyle(XElement xmlStyle, ExternalAssemblyManager assemblyManager)
        {
            this.SetDefaultFieldValues();

            // set the constructor of the LinkView
            XElement constructorXml = xmlStyle.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.ClassNameElementOfNodeStyleElementName);
            if (constructorXml != null)
            {
                ConstructorInfo constructor = assemblyManager.SearchNodeViewConstructor(constructorXml.Value);
                if (constructor != null)
                {
                    this.viewConstructor = constructor;
                }
            }

            // set the drawing information to the style
            XElement drawingInformationElement = xmlStyle.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.DrawingInformationElementOfNodeStyleElementName);
            if (drawingInformationElement != null)
            {
                this.drawingInformation = drawingInformationElement;
            }

            // set the size coeficient
            XElement relativeSizeElement = xmlStyle.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.RelativeSizeElementOfNodeStyleElementName);
            if (relativeSizeElement != null)
            {
                this.relativeSize = xml.SettingsXmlContent.ParseToDouble(relativeSizeElement.Value);
            }
        }

        #endregion

        #region Events & Delegates

        /// <summary>
        /// this event occur when the current node style change
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets multiplicator coeficient for this style
        /// </summary>
        public double RelativeSize
        {
            get { return this.relativeSize; }
        }

        /// <summary>
        /// Gets the Xml definition of all useful information used to apply the style on the graph
        /// </summary>
        public XElement DrawingInformation
        {
            get { return this.drawingInformation; }
        }

        /// <summary>
        /// Gets the constructor of the node view related to the style
        /// </summary>
        public ConstructorInfo ViewConstructor
        {
            get { return this.viewConstructor; }
        }

        #endregion

        /// <summary>
        /// This function raise the event Change
        /// </summary>
        public void RaiseChangeEvent()
        {
            if (this.Changed != null)
            {
                this.Changed(this, new EventArgs());
            }
        }

        /// <summary>
        /// Factory :
        /// Gives a new node view according to the style definition
        /// </summary>
        /// <returns>
        /// Instance of the choosen node view class inheriting from INodeView
        /// </returns>
        public INodeView GetNewNodeView()
        {
            object[] arguments = new object[0];
            INodeView nodeView = (INodeView) this.viewConstructor.Invoke(arguments);

            return nodeView;
        }

        /// <summary>
        /// this function sets all fields to the default value
        /// </summary>
        private void SetDefaultFieldValues()
        {
            // sets the default values
            this.viewConstructor = typeof(NodeView).GetConstructor(new Type[0]);
            XDocument doc = XDocument.Parse("<" + xml.SettingsXmlContent.DrawingInformationElementOfNodeStyleElementName + " xmlns=\"" + xml.SettingsXmlContent.Namespace + "\"/>");
            this.drawingInformation = doc.Root;
            this.relativeSize = 1d;
        }
    }
}
