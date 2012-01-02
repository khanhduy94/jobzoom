//-----------------------------------------------------------------------
// <copyright file="LinkView.xaml.cs" company="International Monetary Fund">
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    /// Default implementation for the visual of a link
    /// </summary>
    public partial class LinkView : ILinkView
    {
        /// <summary>
        /// an Xml field were rendrering information for this link are defined
        /// </summary>
        private XElement linkDrawinginformation;

        /// <summary>
        /// an Xml field were rendrering information for this style are defined
        /// </summary>
        private XElement styleDrawingInformation;

        /// <summary>
        /// Initializes a new instance of the LinkView class
        /// </summary>
        public LinkView()
        {
            InitializeComponent();
        }

        #region ILinkView Members

        /// <summary>
        /// Gets or sets first end of the link
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        public Point PointA
        {
            get
            {
                return new Point(this.line.X1, this.line.Y1);
            }

            set 
            {
                this.line.X1 = value.X;
                this.line.Y1 = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets second end of the link
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        public Point PointB
        {
            get
            {
                return new Point(this.line.X2, this.line.Y2);
            }

            set 
            {
                this.line.X2 = value.X;
                this.line.Y2 = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the thickness of the link
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        public double Thickness
        {
            get { return this.line.StrokeThickness; }
            set { this.line.StrokeThickness = value; }
        }

        /// <summary>
        /// Sets the meaning of the relation
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        public string Meaning
        {
            set { tooltip.Content = value; }
        }

        /// <summary>
        /// Sets the rendering information from the style
        /// </summary>
        public System.Xml.Linq.XElement StyleDrawingInformation
        {
            set
            {
                this.styleDrawingInformation = value;
                this.UpdateUI();
            }
        }

        /// <summary>
        /// Sets the rendering information from the link
        /// </summary>
        public System.Xml.Linq.XElement LinkDrawingInformation
        {
            set
            {
                this.linkDrawinginformation = value;
                this.UpdateUI();
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the color of the link
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        private Color LinkColor
        {
            get
            {
                return ((SolidColorBrush)this.line.Stroke).Color;
            }

            set
            {
                SolidColorBrush scb = new SolidColorBrush(value);
                this.line.Stroke = scb;
            }
        }

        #region ILinkView Members

        /// <summary>
        /// This function perform a task (for exemple: start or stop an animation)
        /// </summary>
        /// <param name="task">the name of the task</param>
        /// <param name="actionCaller">the parameters in XML</param>
        public void PerformTask(string task, System.Xml.Linq.XElement actionCaller)
        {
            switch (task)
            {
                default:
                    break;
            }
        }

        #endregion

        /// <summary>
        /// this function update the UI
        /// </summary>
        private void UpdateUI()
        {
            XElement color = xml.DefaultTemplateXmlContent.GetElement(
                xml.DefaultTemplateXmlContent.ColorElementName,
                this.linkDrawinginformation,
                this.styleDrawingInformation);
            if (color != null)
            {
                this.LinkColor = xml.DefaultTemplateXmlContent.GetColorFromXml(color);
            }
        } 
    }
}
