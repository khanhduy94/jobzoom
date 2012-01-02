//-----------------------------------------------------------------------
// <copyright file="LinkStyle.cs" company="International Monetary Fund">
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
    using System.Windows.Media;
    using System.Xml.Linq;
    using ICE.view;
    
    /// <summary>
    /// This class represents the style of a link and its behaviour
    /// </summary>
    public class LinkStyle
    {
        #region Fields

        /// <summary>
        /// Constructor of the link view related to the style
        /// </summary>
        private ConstructorInfo viewConstructor;

        /// <summary>
        /// This is the Xml definition of all useful information used to apply the style on the graph
        /// </summary>
        private XElement drawingInformation;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the LinkStyle class
        /// </summary>
        public LinkStyle()
        {
            this.SetDefaultFieldValues();
        }

        /// <summary>
        /// Initializes a new instance of the LinkStyle class
        /// </summary>
        /// <param name="xmlStyle">Xml representation of the link style</param>
        /// <param name="assemblyManager">Manager for assembly/name resolution</param>
        public LinkStyle(XElement xmlStyle, ExternalAssemblyManager assemblyManager)
        {
            this.SetDefaultFieldValues();

            // sets the constructor of the LinkView
            XElement constructorXml = xmlStyle.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.ClassNameElementOfLinkStyleElementName);
            if (constructorXml != null)
            {
                ConstructorInfo constructor = assemblyManager.SearchLinkViewConstructor(constructorXml.Value);
                if (constructor != null)
                {
                    this.viewConstructor = constructor;
                }
            }

            // set the drawing information to the style
            XElement drawingInformationElement = xmlStyle.Element(xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.DrawingInformationElementOfLinkStyleElementName);
            if (drawingInformationElement != null)
            {
                this.drawingInformation = drawingInformationElement;
            }
        }

        #endregion

        #region Events & Delegates

        /// <summary>
        /// This event occur when the link style change
        /// </summary>
        public event EventHandler Changed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the constructor of the node view related to the style
        /// </summary>
        public ConstructorInfo ViewConstructor
        {
            get { return this.viewConstructor; }
            set { this.viewConstructor = value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets the Xml definition of all useful information used to apply the style on the graph
        /// </summary>
        public XElement DrawingInformation
        {
            get { return this.drawingInformation; }
            set { this.drawingInformation = value; }
        }

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
        /// Gives a new link view according to the style definition
        /// </summary>
        /// <returns>
        /// Instance of the choosen link view class inheriting from ILinkView
        /// </returns>
        public ILinkView GetNewLinkView()
        {
            object[] arguments = new object[0];
            ILinkView linkView = (ILinkView)this.viewConstructor.Invoke(arguments);
            linkView.StyleDrawingInformation = this.drawingInformation;
            return linkView;
        }

        /// <summary>
        /// Sets all fields with default values 
        /// </summary>
        private void SetDefaultFieldValues()
        {
            this.viewConstructor = typeof(LinkView).GetConstructor(new Type[0]);
            XDocument doc = XDocument.Parse("<" + xml.SettingsXmlContent.DrawingInformationElementOfNodeStyleElementName + " xmlns=\"" + xml.SettingsXmlContent.Namespace + "\"/>");
            this.DrawingInformation = doc.Root;
        }
    }
}
