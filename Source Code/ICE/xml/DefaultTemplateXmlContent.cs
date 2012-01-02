//-----------------------------------------------------------------------
// <copyright file="DefaultTemplateXmlContent.cs" company="International Monetary Fund">
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

namespace ICE.xml
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

    /// <summary>
    /// This class contains every references to the "InformationConnectionsEngine.defaultTemplate" namespace
    /// </summary>
    /// <remarks>This class should be replaced in the future by an Xml Serialisation system.</remarks>
    public abstract class DefaultTemplateXmlContent
    {
        public const string ColorElementName = "color";
        public const string AlphaElementOfColorElementName = "alpha";
        public const string RedElementOfColorElementName = "red";
        public const string GreenElementOfColorElementName = "green";
        public const string BlueElementOfColorElementName = "blue";

        public const string TextLayoutElementName = "textLayout";
        public const string ColorElementOfTextLayoutElementName = "color";
        public const string FontElementOfTextLayoutElementName = "font";
        public const string PositionElementOfTextLayoutElementName = "position";

        public const string CenterValueOfPositionElementName = "center";
        public const string TopValueOfPositionElementName = "top";
        public const string BottomValueOfPositionElementName = "bottom";
        public const string HideValueOfPositionElementName = "hide";

        public const string SizeElementOfTextLayoutElementName = "size";

        public const string ObjectLayoutElementName = "objectLayout";
        public const string SphereElementOfObjectLayoutElementName = "sphere";
        public const string ImageElementOfObjectLayoutElementName = "image";
        public const string NoneElementOfObjectLayoutElementName = "none";

        public static readonly System.Xml.Linq.XNamespace Namespace = "InformationConnectionsEngine.defaultTemplate";

        /// <summary>
        /// This function returns the corresponding color from its xml definition
        /// </summary>
        /// <param name="colorXml">xml definition</param>
        /// <returns>The corresponding color</returns>
        public static Color GetColorFromXml(XElement colorXml)
        {
            return Color.FromArgb(
                byte.Parse(colorXml.Element(DefaultTemplateXmlContent.Namespace + DefaultTemplateXmlContent.AlphaElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber),
                byte.Parse(colorXml.Element(DefaultTemplateXmlContent.Namespace + DefaultTemplateXmlContent.RedElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber),
                byte.Parse(colorXml.Element(DefaultTemplateXmlContent.Namespace + DefaultTemplateXmlContent.GreenElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber),
                byte.Parse(colorXml.Element(DefaultTemplateXmlContent.Namespace + DefaultTemplateXmlContent.BlueElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber));
        }

        /// <summary>
        /// This funtion return the corresponding font form it's xml definition
        /// </summary>
        /// <param name="fontXml">xml definition</param>
        /// <returns>The corresponding FontFamily</returns>
        public static FontFamily GetFontFormXml(XElement fontXml)
        {
            return new FontFamily(fontXml.Value);
        }

        /// <summary>
        /// this function recover an element in the drawing information.
        /// Here, local's infromation have the priority
        /// </summary>
        /// <param name="elementName">the name of the element you seaching</param>
        /// <param name="localDrawinginformation">Xml field use to define the local look and feel. this field override the style</param>
        /// <param name="styleDrawingInformation">xml field use to define the style look and feel.</param>
        /// <returns> an xml element(return null if the element is not found)</returns>
        public static XElement GetElement(string elementName, XElement localDrawinginformation, XElement styleDrawingInformation)
        {
            XElement value = null;
            if (localDrawinginformation != null)
            {
                XElement tag = localDrawinginformation.Element(xml.DefaultTemplateXmlContent.Namespace + elementName);
                if (tag != null)
                {
                    value = tag;
                }
            }

            if ((value == null) && (styleDrawingInformation != null))
            {
                XElement tag = styleDrawingInformation.Element(xml.DefaultTemplateXmlContent.Namespace + elementName);
                if (tag != null)
                {
                    value = tag;
                }
            }

            return value;
        }
    }
}
