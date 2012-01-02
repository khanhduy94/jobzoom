//-----------------------------------------------------------------------
// <copyright file="NodeView.xaml.cs" company="International Monetary Fund">
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
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Xml.Linq;
    
    /// <summary>
    /// Default implementation for the visual of a node
    /// </summary>
    public partial class NodeView : INodeView
    {
        /// <summary>
        /// This is a xml field containing all information comming from the data definition use to represent the current node.
        /// </summary>
        private XElement nodeDrawingInformation;

        /// <summary>
        /// This is a xml field containing all information comming from the style definition use to represent the current node.
        /// </summary>
        private XElement styleDrawingInformation;

        /// <summary>
        /// Initializes a new instance of the NodeView class
        /// </summary>
        public NodeView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the node's text
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        public string Title
        {
            set { this.label.Text = value; }
        }

        /// <summary>
        /// Sets information to display in the node.
        /// </summary>
        /// <remarks>
        /// The fact that this entry point is an Xml could be usefull to draw some customized information.
        /// Setting this property can be done dynamically.
        /// Null can be set.
        /// </remarks>
        public XElement NodeDrawingInformation
        {
            set 
            {
                this.nodeDrawingInformation = value;
                this.UpdateUI();
            }
        }

        /// <summary>
        /// Sets information to display in the node. Could be use to display any additional information. E.g., text below the main title
        /// </summary>
        /// <remarks>
        /// The fact that this entry point is an Xml could be usefull to draw some customized information.
        /// Setting this property can be done dynamically.
        /// Null can be set.
        /// </remarks>
        public XElement StyleDrawingInformation
        {
            set
            {
                this.styleDrawingInformation = value;
                this.UpdateUI();
            }
        }

        /// <summary>
        /// Sets the color brush used to draw the component
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        private Color IconColor
        {
            set
            {
                Color color = value;
                color.A = 0xff;
                SolidColorBrush scb = new SolidColorBrush(color);
                this.ellipse.Fill = scb;
            }
        }

        /// <summary>
        /// Sets the font for the node's text
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        private FontFamily Font
        {
            set { this.label.FontFamily = value; }
        }

        /// <summary>
        /// this function is called when a user ask to ICE for performing a action on the node
        /// Here we realize the User Interface part of the action
        /// </summary>
        /// <param name="action">the action name to perform (E.g.; "select" or "deselect")</param>
        /// <param name="actionCaller">the xml object use to call the action. This object came from the node defnition and could contain some arguments</param>
        /// <remarks>
        /// We may add an access to some tools to call some UI services from ICE
        /// </remarks>
        public void PerformTask(string action, XElement actionCaller)
        {
            switch (action)
            {
                case "select": VisualStateManager.GoToState(this, "selected", true);
                    break;
                case "deselect": VisualStateManager.GoToState(this, "normal", true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// this function update the UI
        /// </summary>
        private void UpdateUI()
        {
            try
            {
                XElement color = xml.DefaultTemplateXmlContent.GetElement(
                    xml.DefaultTemplateXmlContent.ColorElementName,
                    this.nodeDrawingInformation,
                    this.styleDrawingInformation);
                if (color != null)
                {
                    this.IconColor = xml.DefaultTemplateXmlContent.GetColorFromXml(color);
                }

                XElement textLayoutElement = xml.DefaultTemplateXmlContent.GetElement(
                    xml.DefaultTemplateXmlContent.TextLayoutElementName,
                    this.nodeDrawingInformation,
                    this.styleDrawingInformation);
                if (textLayoutElement != null)
                {
                    XElement colorElement = textLayoutElement.Element(xml.DefaultTemplateXmlContent.Namespace + xml.DefaultTemplateXmlContent.ColorElementOfTextLayoutElementName);
                    if (colorElement != null)
                    {
                        this.label.Foreground = new SolidColorBrush(xml.DefaultTemplateXmlContent.GetColorFromXml(colorElement));
                    }

                    XElement fontElement = textLayoutElement.Element(xml.DefaultTemplateXmlContent.Namespace + xml.DefaultTemplateXmlContent.FontElementOfTextLayoutElementName);
                    if (fontElement != null)
                    {
                        this.Font = xml.DefaultTemplateXmlContent.GetFontFormXml(fontElement);
                    }

                    XElement positionElement = textLayoutElement.Element(xml.DefaultTemplateXmlContent.Namespace + xml.DefaultTemplateXmlContent.PositionElementOfTextLayoutElementName);
                    if (positionElement != null)
                    {
                        switch (positionElement.Value)
                        {
                            case xml.DefaultTemplateXmlContent.HideValueOfPositionElementName:
                                VisualStateManager.GoToState(this, "NoTextLayout", false);
                                break;
                            case xml.DefaultTemplateXmlContent.TopValueOfPositionElementName:
                                VisualStateManager.GoToState(this, "TopTextLayout", false);
                                break;
                            case xml.DefaultTemplateXmlContent.BottomValueOfPositionElementName:
                                VisualStateManager.GoToState(this, "BottomTextlayout", false);
                                break;
                            case xml.DefaultTemplateXmlContent.CenterValueOfPositionElementName:
                                VisualStateManager.GoToState(this, "CenterTextLayout", false);
                                break;
                            default:
                                break;
                        }
                    }

                    XElement sizeElement = textLayoutElement.Element(xml.DefaultTemplateXmlContent.Namespace + xml.DefaultTemplateXmlContent.SizeElementOfTextLayoutElementName);
                    if (sizeElement != null)
                    {
                        this.label.FontSize = double.Parse(sizeElement.Value);
                    }
                }

                XElement objectLayoutElement = xml.DefaultTemplateXmlContent.GetElement(
                    xml.DefaultTemplateXmlContent.ObjectLayoutElementName,
                    this.nodeDrawingInformation,
                    this.styleDrawingInformation);
                if (objectLayoutElement != null)
                {
                    XElement sphereElement = objectLayoutElement.Element(xml.DefaultTemplateXmlContent.Namespace + xml.DefaultTemplateXmlContent.SphereElementOfObjectLayoutElementName);
                    if (sphereElement != null)
                    {
                        VisualStateManager.GoToState(this, "SphereCoreLayout", false);
                    }

                    XElement imageElement = objectLayoutElement.Element(xml.DefaultTemplateXmlContent.Namespace + xml.DefaultTemplateXmlContent.ImageElementOfObjectLayoutElementName);
                    if (imageElement != null)
                    {
                        VisualStateManager.GoToState(this, "ImageCoreLayout", false);
                        this.image.Source = new BitmapImage(new Uri(imageElement.Value));
                    }

                    XElement noneElement = objectLayoutElement.Element(xml.DefaultTemplateXmlContent.Namespace + xml.DefaultTemplateXmlContent.NoneElementOfObjectLayoutElementName);
                    if (noneElement != null)
                    {
                        VisualStateManager.GoToState(this, "NoCoreLayout", false);
                    }
                }
            }
            catch (Exception)
            {
                /* do nothing */
            }
        }
    }
}
