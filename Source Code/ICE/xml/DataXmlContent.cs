//-----------------------------------------------------------------------
// <copyright file="DataXmlContent.cs" company="International Monetary Fund">
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
    /// <summary>
    /// This class contains every reference names you may find in a relation xml file
    /// </summary>
    /// <remarks>This class should be replaced in the future by an Xml Serialisation system.</remarks>
    public abstract class DataXmlContent
    {
        public const string RootElementName = "iceData";
        public const string CurrentNodeElementName = "currentNode";
        public const string NeighborsElementName = "neighbors";
        
        public const string NodeElementName = "node";
        public const string IDAttributeOfNodeElementName = "id";
        public const string ActionsElementOfNodeElementName = "actions";
        public const string RefreshRateElementOfNodeElementName = "refreshRate";
        public const string RelativeSizeElementOfNodeElementName = "relativeSize";
        public const string DrawingInformationElementOfNodeElementName = "drawingInformation";
        public const string TitleElementOfNodeElementName = "title";        
        public const string TypeElementOfNodeElementName = "type";
        public const string StyleElementOfNodeElementName = "style";
        public const string URLElementOfNodeElementName = "url";

        public const string LinkElementName = "link";
        public const string IDAttributeOfLinkElementName = "id";
        public const string FromAttributeOfLinkElementName = "from";
        public const string ToAttributeOfLinkElementName = "to";
        public const string ActionsElementOfLinkElementName = "actions";
        public const string DrawingInformationElementOfLinkElementName = "drawingInformation";
        public const string GrammarElementOfLinkElementName = "grammar";
        public const string VerbElementOfGrammarElementName = "verb";
        public const string ComplementElementOfGrammarElementName = "complement";
        public const string StrengthElementOfLinkElementName = "strength";
        public const string StyleElementOfLinkElementName = "style";

        public const string ActionElementName = "action";
        public const string IDRefAttributeOfActionElementName = "idref";

        public const string JSParametersElementName = "parameters";

        public static readonly System.Xml.Linq.XNamespace Namespace = "InformationConnectionsEngine.data";
    }
}
