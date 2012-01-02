//-----------------------------------------------------------------------
// <copyright file="ILinkView.cs" company="International Monetary Fund">
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
    using System.Windows.Media;
    using System.Xml.Linq;

    /// <summary>
    /// Interface for the visual of a link
    /// </summary>
    /// <remarks>
    /// Any custom link implementation must comply to this interface to be used in ICE
    /// </remarks>
    public interface ILinkView
    {
        /// <summary>
        /// Sets information to display in the link. Could be use to display any additional information. E.g., text below the main title
        /// </summary>
        /// <remarks>
        /// The fact that this entry point is an Xml could be usefull to draw some customized information.
        /// Setting this property can be done dynamically.
        /// Null can be set.
        /// </remarks>
        XElement StyleDrawingInformation
        {
            set;
        }

        /// <summary>
        /// Sets information to display in the link. Could be use to display any additional information. E.g., text below the main title
        /// </summary>
        /// <remarks>
        /// The fact that this entry point is an Xml could be usefull to draw some customized information.
        /// Setting this property can be done dynamically.
        /// Null can be set.
        /// </remarks>
        XElement LinkDrawingInformation
        {
            set;
        }

        /// <summary>
        /// Gets or sets first end of the link
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        Point PointA
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets second end of the link
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        Point PointB
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the thickness of the link
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        double Thickness
        {
            set;
        }

        /// <summary>
        /// Sets the sentence that should represent the meaning of the relation
        /// </summary>
        /// <remarks>
        /// Setting this property can be done dynamically
        /// </remarks>
        string Meaning
        {
            set;
        }

        /// <summary>
        /// this function is called when a user ask to ICE for performing a action on the link
        /// Here we realize the User Interface part of the action
        /// </summary>
        /// <param name="task">the task name to perform</param>
        /// <param name="actionCaller">the xml object use to call the action. This object came from the link defnition and could contain some arguments</param>
        /// <remarks>
        /// We may add an access to some tools to call some UI services from ICE
        /// </remarks>
        void PerformTask(string task, XElement actionCaller);
    }
}
