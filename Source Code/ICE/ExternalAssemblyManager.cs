//-----------------------------------------------------------------------
// <copyright file="ExternalAssemblyManager.cs" company="International Monetary Fund">
//
//    This file is part of "Information Connections Engine". See more information at http://ICEdotNet.codeplex.com See more information at http://ICEdotNet.codeplex.com
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

namespace ICE
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    /// <summary>
    /// This class is responssible of storing downloaded Silverlight .NET assemblies and to retrive specific kind of class in them
    /// </summary>
    public class ExternalAssemblyManager
    {
        /// <summary>
        /// this property is the list of all now external assembly
        /// </summary>
        private List<Assembly> assemblyList = new List<Assembly>();

        /// <summary>
        /// this function search a class constructor in the list of all nown assembly
        /// </summary>
        /// <param name="name">the full-name of the class</param>
        /// <returns>the empty constructor of the class</returns>
        /// <remarks>
        /// If the class does not exist in this context or the class does not have a constructor with no parameter, This function returns NULL
        /// </remarks>
        /// <example>
        ///     How to get the constructor of the class "ClassName" 
        ///     <code>
        ///         ConstructorInfo constructor = SearchClassConstructor("NameSpace.SubNameSpace.ClassName")
        ///     </code>
        /// </example>
        public ConstructorInfo SearchClassConstructor(string name)
        {
            ConstructorInfo constructor = null;

            Type[] emptyArg = new Type[0];

            // foreach now assembly
            foreach (Assembly assembly in this.assemblyList)
            {
                try
                {
                    // foreach type in the assembly
                    Type[] types = assembly.GetTypes();
                    for (int i = 0; i < types.Length; i++)
                    {
                        // if the type is the type needed
                        if (types[i].FullName.Equals(name))
                        {
                            // get the contructor or null
                            constructor = types[i].GetConstructor(emptyArg);
                        }
                    }
                }
                catch (Exception) 
                {
                    /* On error, do nothing */
                }
            }
            
            // return the founded constructor (or null)
            return constructor;
        }

        /// <summary>
        /// this function search an INodeView constructor in the list of all nown assembly
        /// </summary>
        /// <param name="name">the full-name of the class</param>
        /// <returns>the empty constructor of the class</returns>
        /// <remarks>
        /// If the class does not exist in this context or the class does not have a constructor with no parameter or does not implement INodeView interface, This function returns NULL
        /// </remarks>
        /// <example>
        ///     How to get the constructor of the class "ClassName" 
        ///     <code>
        ///         ConstructorInfo constructor = SearchNodeViewConstructor("NameSpace.SubNameSpace.ClassName")
        ///     </code>
        /// </example>
        public ConstructorInfo SearchNodeViewConstructor(string name)
        {
            ConstructorInfo constructor = this.SearchClassConstructor(name);

            // if the class of the constructor implement INodeView
            if (constructor != null && typeof(view.INodeView).IsAssignableFrom(constructor.DeclaringType))
            {
                return constructor;
            }

            // else return null
            return null;
        }

        /// <summary>
        /// this function search an ILinkView constructor in the list of all nown assembly
        /// </summary>
        /// <param name="name">the full-name of the class</param>
        /// <returns>the empty constructor of the class</returns>
        /// <remarks>
        /// If the class does not exist in this context or the class does not have a constructor with no parameter or does not implement ILinkView interface, This function returns NULL
        /// </remarks>
        /// <example>
        ///     How to get the constructor of the class "ClassName" 
        ///     <code>
        ///         ConstructorInfo constructor = SearchLinkViewConstructor("NameSpace.SubNameSpace.ClassName")
        ///     </code>
        /// </example>
        public ConstructorInfo SearchLinkViewConstructor(string name)
        {
            ConstructorInfo constructor = this.SearchClassConstructor(name);

            // if the class of the constructor exist and implement ILinkView
            if (constructor != null && typeof(view.ILinkView).IsAssignableFrom(constructor.DeclaringType))
            {
                return constructor;
            }

            // else return null
            return null;
        }

        /// <summary>
        /// This function adds an assembly to the nown assembly list
        /// </summary>
        /// <param name="assembly">an assembly</param>
        public void AddAssembly(Assembly assembly)
        {
            this.assemblyList.Add(assembly);
        }

        /// <summary>
        /// this function search an IPopUp constructor in the list of all nown assembly
        /// </summary>
        /// <param name="name">the full-name of the class</param>
        /// <returns>the empty constructor of the class</returns>
        /// <remarks>
        /// If the class does not exist in this context or the class does not have a constructor with no parameter or does not implement IPopUp interface, This function returns NULL
        /// </remarks>
        /// <example>
        ///     How to get the constructor of the class "ClassName" 
        ///     <code>
        ///         ConstructorInfo constructor = SearchLinkViewConstructor("NameSpace.SubNameSpace.ClassName")
        ///     </code>
        /// </example>
        public ConstructorInfo SearchPopUpViewConstructor(string name)
        {
            ConstructorInfo constructor = this.SearchClassConstructor(name);

            // if the class of the constructor exist and implement ILinkView
            if (constructor != null && typeof(view.IPopUp).IsAssignableFrom(constructor.DeclaringType))
            {
                return constructor;
            }

            // else return null
            return null;
        }
    }
}
