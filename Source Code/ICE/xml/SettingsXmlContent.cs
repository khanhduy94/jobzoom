//-----------------------------------------------------------------------
// <copyright file="SettingsXmlContent.cs" company="International Monetary Fund">
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

namespace ICE.xml
{
    using System;
    using System.Globalization;
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
    /// This class contains every reference names you may find in a settings xml file
    /// </summary>
    /// <remarks>This class should be replaced in the future by an Xml Serialisation system.</remarks>
    public abstract class SettingsXmlContent
    {
        public const string RootElementName = "iceSettings";

        public const string ConstantsElementName = "constants";

        public const string ModesElementOfConstantsElementName = "modes";
        public const string MultiSelectionElementOfModesElementName = "multiNodeSelection";
        public const string DebugElementOfModesElementName = "debug";
        public const string NavigationBarElementOfModesElementName = "navigationBar";
        public const string NavigationMenuElementOfModesElementName = "navigationMenu";

        public const string NodeStylesElementName = "nodeStyles";
        public const string DefaultAttributeOfNodeStylesElementName = "default";

        public const string LinkStylesElementName = "linkStyles";
        public const string DefaultAttributeOfLinkStylesElementName = "default";

        public const string ActionsElementName = "actions";

        public const string DrawingContantsElementOfConstantsElementName = "drawingConstants";
        public const string ActionCircleColorElementOfDrawingContantsElementName = "actionsCircleColor";
        public const string InitialZoomRatioElementOfDrawingContantsElementName = "initialZoomRatio";
        public const string InitialGraphVisibilityDepthElementOfDrawingContantsElementName = "initialGraphVisibilityDepth";
        public const string OpacityChangeStepElementOfDrawingContantsElementName = "opacityChangeStep";
        public const string NodeSizeElementOfDrawingContantsElementName = "nodeSize";
        public const string LinkMaximalThicknessElementOfDrawingContantsElementName = "linkMaximalThickness";
        public const string LinkMinimalThicknessElementOfDrawingContantsElementName = "linkMinimalThickness";
        public const string BackgroundElementOfDrawingContantsElementName = "background";

        public const string PhysicsContantsElementOfConstantsElementName = "physicsConstants";
        public const string GravityElementOfPhysicsContantsElementName = "gravity";
        public const string SpringRestLenghtElementOfPhysicsContantsElementName = "springRestLenght";
        public const string RepultionStrengthElementOfPhysicsContantsElementName = "repultionStrength";
        public const string DragForceElementOfPhysicsContantsElementName = "fluidResistance";

        public const string ModelContantsElementOfConstantsElementName = "modelConstants";
        public const string MaximumNodeElementOfModelContantsElementName = "maximumNodes";
        public const string CleanUpadditionalDepthElementOfModelContantsElementName = "cleanUpAdditionalDepth";

        public const string DownloadConstantsElementOfConstantsElementName = "downloadConstants";
        public const string TimeoutElementOfDownloadConstantsElementName = "timeout";
        public const string MaximumDownloadPerMinuteElementOfDownloadConstantsElementName = "maximumDownloadPerMinute";
        public const string MaximumSimultaneousDownloadElementOfDownloadConstantsElementName = "maximumSimultaneousDownload";

        public const string NodeStyleElementName = "nodeStyle";
        public const string IDAttributeOfNodeStyleElementName = "id";
        public const string ClassNameElementOfNodeStyleElementName = "className";
        public const string DrawingInformationElementOfNodeStyleElementName = "drawingInformation";
        public const string RelativeSizeElementOfNodeStyleElementName = "relativeSize";
        
        public const string LinkStyleElementName = "linkStyle";
        public const string IDAttributeOfLinkStyleElementName = "id";
        public const string ClassNameElementOfLinkStyleElementName = "className";
        public const string DrawingInformationElementOfLinkStyleElementName = "drawingInformation";

        public const string ActionElementName = "action";
        public const string GroupableActionElementName = "groupAction";
        public const string IDAttributeOfActionElementName = "id";
        public const string DescriptionElementOfActionElementName = "description";
        public const string IconURLElementOfActionElementName = "iconURL";
        public const string TasksElementOfActionElementName = "tasks";

        public const string GUITaskElementName = "guiTask";
        public const string NameAttributeOfGUITaskElementName = "name";

        public const string JavascriptTaskElementName = "javascriptTask";
        public const string NameAttributeOfJavascriptTaskElementName = "name";

        public const string DrawPopupTaskElementName = "drawPopupTask";
        public const string NameAttributeOfDrawPopupTaskElementName = "name";

        public const string AlphaElementOfColorElementName = "alpha";
        public const string RedElementOfColorElementName = "red";
        public const string GreenElementOfColorElementName = "green";
        public const string BlueElementOfColorElementName = "blue";

        public static readonly XNamespace Namespace = "InformationConnectionsEngine.settings";

        /// <summary>
        /// This fonction create a point from its xml representation
        /// </summary>
        /// <param name="pointXml">you point describe in Xml</param>
        /// <returns>an instance of point corresponding to the XML definition</returns>
        public static Point GetPointFromXml(XElement pointXml)
        {
            return new Point(
                double.Parse(pointXml.Element(DefaultTemplateXmlContent.Namespace + "X").Value, System.Globalization.CultureInfo.InvariantCulture),
                double.Parse(pointXml.Element(DefaultTemplateXmlContent.Namespace + "Y").Value, System.Globalization.CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// This function parse a double value from a string value, If the string in argument is in percent, this function made the convertion
        /// </summary>
        /// <param name="value"> the string to parse</param>
        /// <returns>corresponding double value</returns>
        public static double ParseToDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("formattedNumber");
            }

            double result;

            if (value.Contains(CultureInfo.InvariantCulture.NumberFormat.PercentSymbol))
            {
                // value is in percent
                string str = value.Replace(CultureInfo.InvariantCulture.NumberFormat.PercentSymbol, string.Empty);
                result = double.Parse(str, NumberStyles.Any, CultureInfo.InvariantCulture);
                result /= 100.0;
            }
            else
            {
                // value just a double
                result = double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
            }

            return result;
        }

        /// <summary>
        /// This function returns the corresponding color from its xml definition
        /// </summary>
        /// <param name="colorXml">xml definition</param>
        /// <returns>The corresponding color</returns>
        public static Color ParseColor(XElement colorXml)
        {
            return Color.FromArgb(
                byte.Parse(colorXml.Element(SettingsXmlContent.Namespace + SettingsXmlContent.AlphaElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber),
                byte.Parse(colorXml.Element(SettingsXmlContent.Namespace + SettingsXmlContent.RedElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber),
                byte.Parse(colorXml.Element(SettingsXmlContent.Namespace + SettingsXmlContent.GreenElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber),
                byte.Parse(colorXml.Element(SettingsXmlContent.Namespace + SettingsXmlContent.BlueElementOfColorElementName).Value, System.Globalization.NumberStyles.HexNumber));
        }
    }
}
