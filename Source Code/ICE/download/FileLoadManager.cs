﻿//-----------------------------------------------------------------------
// <copyright file="FileLoadManager.cs" company="International Monetary Fund">
//   This file is part of "Information Connections Engine". See more information at http://ICEdotNet.codeplex.com
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

namespace ICE.download
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;
    using System.Xml;
    using System.Xml.Linq;
    
    /// <summary>
    /// Event Handler used whether an Xml file has been loaded.
    /// </summary>
    /// <param name="document">the loaded XML file</param>
    public delegate void XmlFileLoadedHandler(XDocument document);

    /// <summary>
    /// Event Handler used whether a file loading failed.
    /// </summary>
    /// <param name="errorMessage">an error message generated by the loader</param>
    public delegate void LoadingFailedHandler(string errorMessage);

    /// <summary>
    /// Event Handler used whether an assembly file has been loaded.
    /// </summary>
    /// <param name="document">the loaded assembly file</param>
    public delegate void AssemblyLoadedHandler(Assembly document);

    /// <summary>
    /// Event Handler used whether an assembly file has been loaded.
    /// </summary>
    /// <param name="document">the loaded stream</param>
    public delegate void UndeterminedFileLoadedHandler(Stream document);

    /// <summary>
    /// This class manages all download of all external file
    /// </summary>
    public class FileLoadManager
    {
        #region Fields

        /// <summary>
        /// this is the argument use to invoke any loader constructor
        /// </summary>
        private static Type[] empty = new Type[0];

        /// <summary>
        /// this field is the queue use to store all waiting loading process
        /// </summary>
        private PriorityQueue<Loader> queue = new PriorityQueue<Loader>();

        /// <summary>
        /// This field is the list of all active loading process
        /// </summary>
        private List<Loader> activeLoaders = new List<Loader>();

        /// <summary>
        /// This field is the list of all initialisation loader
        /// </summary>
        private List<Loader> initialisationLoaders = new List<Loader>();

        /// <summary>
        /// This field is the constructor used to create a loader
        /// </summary>
        private ConstructorInfo loaderConstructor = null;

        /// <summary>
        /// this property is the current style applied on the loading process
        /// </summary>
        private setting.IFileLoadSettings settings;

        /// <summary>
        /// this is the amount of free timetoken 
        /// </summary>
        private int timeTokens = 0;

        /// <summary>
        /// this is the amout of free loading slot
        /// </summary>
        private int countTokens = 0;

        /// <summary>
        /// this is the partial timetokens left
        /// </summary>
        private double timeTokenParts = 0d;

        /// <summary>
        /// refresh rate in milliscond
        /// </summary>
        private int refreshRate = 200;

        /// <summary>
        /// the timer that update the download system;
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region Constructor

        #endregion
        
        #region Events

        /// <summary>
        /// This event is raised whether a relation file is loaded
        /// </summary>
        public event XmlFileLoadedHandler ICEXmlRelationFileLoaded;

        /// <summary>
        /// This event is raised whether a setting file is loaded
        /// </summary>
        public event XmlFileLoadedHandler ICEXmlSettingsFileLoaded;

        /// <summary>
        /// This event is raised whether a error file is loaded
        /// </summary>
        public event XmlFileLoadedHandler ICEErrorFileLoaded;

        /// <summary>
        /// This event is raised whether a assembly file is loaded
        /// </summary>
        public event AssemblyLoadedHandler AssemblyFileLoaded;

        /// <summary>
        /// This event is raised whether a undetermined file is loaded
        /// </summary>
        public event UndeterminedFileLoadedHandler UndeterminedFileFileLoaded;

        /// <summary>
        /// This event is raised whether a all init file are loaded (successfully or not)
        /// </summary>
        public event EventHandler InitialisationFinished;

        /// <summary>
        /// This event is raised whether a download failded during the initialisation process
        /// </summary>
        public event LoadingFailedHandler FileLoadingFailded;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the loader type you want to use.
        /// </summary>
        /// <exception cref="ArgumentException">The value set must be a valid implementation of the Loader class.</exception>
        public Type LoaderType
        {
            get
            {
                if (this.loaderConstructor != null)
                {
                    return this.loaderConstructor.ReflectedType;
                }

                return null;
            }

            set
            {
                if (value.IsSubclassOf(typeof(Loader)))
                {
                    ConstructorInfo ctor = value.GetConstructor(empty);

                    if (value.IsAbstract)
                    {
                        throw new ArgumentException("the type " + value.Name + " must not be abstract");
                    }

                    if (ctor != null)
                    {
                        this.loaderConstructor = ctor;
                        return;
                    }

                    throw new ArgumentException("the type " + value.Name + " must have a valid constructor with no arguments");
                }

                throw new ArgumentException("the type " + value.Name + " is not a subclass of " + typeof(Loader).Name + ".");
            }
        }

        /// <summary>
        /// Gets or sets the current settings applied on the loading process
        /// </summary>
        public setting.IFileLoadSettings Settings
        {
            get
            {
                return this.settings;
            }

            set
            {
                if (this.settings != null)
                {
                    this.settings.Changed -= new EventHandler(this.Settings_Changed);
                }

                this.settings = value;
                this.settings.Changed += new EventHandler(this.Settings_Changed);
                this.Settings_Changed(null, null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the manager still be in the initialisation phase
        /// </summary>
        public bool IsInInitialisationPhase
        {
            get { return !this.timer.IsEnabled; }
        }

        #endregion

        #region Public functions

        /// <summary>
        /// This function adds a loading process
        /// </summary>
        /// <param name="url">path to reach the designated file</param>
        /// <param name="priority">priority of the new loading process</param>
        /// <returns>The component use to load the designated file</returns>
        public Loader LoadFile(string url, Priority priority)
        {
            Loader loader = this.CreateLoader(url, priority);

            if (this.IsInInitialisationPhase)
            {
                // the manager isn't started yet    
                this.activeLoaders.Add(loader);   
            }
            else
            {
                this.queue.Enqueue(loader);
            }

            return loader;
        }

        /// <summary>
        /// This function start the internal thread of the current downloader
        /// </summary>
        public void Start()
        {
            // initilising the update timer
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, this.refreshRate);
            this.timer.Tick += new EventHandler(this.MainLoop);

            foreach (Loader loader in new List<Loader>(this.activeLoaders))
            {
                this.initialisationLoaders.Add(loader);
            }

            foreach (Loader loader in new List<Loader>(this.activeLoaders))
            {
                loader.StartLoading();
            }
        }

        /// <summary>
        /// This function returns a complete report on the current situation of the downloader
        /// </summary>
        /// <returns>the report as a string</returns>
        public string GetReport()
        {
            StringBuilder report = new StringBuilder("Download Manager Report :: " + DateTime.Now.ToString());
            report.AppendLine();

            if (this.IsInInitialisationPhase)
            {
                report.AppendLine("=== Initialisation Phase ====");
                report.AppendLine();
            }

            report.AppendLine("-------- Queue Status --------");
            report.AppendLine("--> Count :" + this.queue.Count);
            report.AppendLine("------------------------------");
            report.AppendLine();

            foreach (Loader loader in new List<Loader>(this.activeLoaders))
            {
                report.AppendLine("==============================");
                report.AppendLine("--> URL :" + loader.Url);
                report.AppendLine("--> Priority :" + loader.Priority.ToString());
            }

            return report.ToString();
        }

        #endregion

        #region Private function

        /// <summary>
        /// this function try to convert the stream into a ICE setting file
        /// </summary>
        /// <param name="stream">the input stream</param>
        /// <returns>a valid ICE Setting XML File or null</returns>
        /// <remarks>
        /// - this function MUST NOT alter in any way the stream
        /// - this function Must be enhance when Silverlight will support Xml Schema Validation
        /// </remarks>
        private static XDocument TryGetSettingsFile(Stream stream)
        {
            try
            {
                // Creation of the Xml file
                XDocument document = XDocument.Load(stream);
                stream.Position = 0;

                // the root node's name must be "iceSettings"
                if (document.Root.Name == xml.SettingsXmlContent.Namespace + xml.SettingsXmlContent.RootElementName)
                {
                    return document;
                }
                else
                {
                    return null;
                }
            }
            catch (XmlException)
            {
                // an exception has occured, so the file is not a valid ice data xml file.
                return null;
            }
        }

        /// <summary>
        /// this function try to convert the stream into a ICE data file
        /// </summary>
        /// <param name="stream">the stream of the file downloaded</param>
        /// <returns>a valid ICE Data XML File or null</returns>
        /// <remarks>
        /// - this function MUST NOT alter in any way the stream
        /// - this function Must be enhance when Silverlight will support Xml Schema Validation
        /// </remarks>
        private static XDocument TryGetRelationFile(Stream stream)
        {
            try
            {
                // Creation of the Xml file
                XDocument document = XDocument.Load(stream);
                stream.Position = 0;

                // the root node's name must be "iceData"
                if (document.Root.Name == xml.DataXmlContent.Namespace + xml.DataXmlContent.RootElementName)
                {
                    return document;
                }
                else
                {
                    return null;
                }
            }
            catch (XmlException)
            {
                // an exception has occured, so the file is not a valid ice data xml file.
                return null;
            }
        }

        /// <summary>
        /// this function try to convert the stream into a ICE error file
        /// </summary>
        /// <param name="stream">the stream of the file downloaded</param>
        /// <returns>a valid ICE Data XML File or null</returns>
        /// <remarks>
        /// - this function MUST NOT alter in any way the stream
        /// - this function Must be enhance when Silverlight will support Xml Schema Validation
        /// </remarks>
        private static XDocument TryGetErrorFile(Stream stream)
        {
            try
            {
                // Creation of the Xml file
                XDocument document = XDocument.Load(stream);
                stream.Position = 0;

                // the root node's name must be "iceData"
                if (document.Root.Name == xml.ErrorXmlContent.Namespace + xml.ErrorXmlContent.RootElementName)
                {
                    return document;
                }
                else
                {
                    return null;
                }
            }
            catch (XmlException)
            {
                // an exception has occured, so the file is not a valid ice data xml file.
                return null;
            }
        }

        /// <summary>
        /// this function try to load an assembly from the stream in argument
        /// </summary>
        /// <param name="stream">a stream to load</param>
        /// <returns>an instance of a .NET assembly or null</returns>
        /// <remarks>
        /// - this function MUST NOT alter in any way the stream
        /// </remarks>
        private static Assembly TryGetAssembly(Stream stream)
        {
            try
            {
                AssemblyPart assemblyLoader = new AssemblyPart();
                Assembly assembly = assemblyLoader.Load(stream);
                stream.Position = 0;
                return assembly;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// This function is called when the stettings has changed
        /// </summary>
        /// <param name="sender">the settings</param>
        /// <param name="e">the event argument</param>
        private void Settings_Changed(object sender, EventArgs e)
        {
            /* do nothing */
        }

        /// <summary>
        /// This function initiate a loader from the loader class
        /// </summary>
        /// <param name="url">the path to the file</param>
        /// <param name="priority">the priority of the download</param>
        /// <returns>a new instance of the selected loader</returns>
        private Loader CreateLoader(string url, Priority priority)
        {
            if (this.loaderConstructor == null)
            {
                throw new InvalidOperationException("A valid loader type must be set before creating any loader");
            }

            Loader loader = (Loader)this.loaderConstructor.Invoke(empty);
            loader.Url = url;
            loader.Priority = priority;
            loader.Timeout = this.settings.DownloadTimeout;
            loader.Finished += new EventHandler(this.LoaderFinished);
            loader.Failed += new EventHandler(this.LoaderFailed);
            return loader;
        }

        /// <summary>
        /// This function is the main loop of the management process
        /// </summary>
        /// <param name="obj">timer dispacher</param>
        /// <param name="args">tick arguments</param>
        private void MainLoop(object obj, EventArgs args)
        {
            // update the timetoken amout
            this.timeTokenParts += this.settings.MaximumDownloadPerMinute * this.refreshRate / 60000d;
            this.timeTokens += (int)this.timeTokenParts;
            this.timeTokenParts %= 1;
            this.timeTokens %= this.settings.MaximumDownloadPerMinute;
            this.countTokens = this.settings.MaximumSimultaneousDownload - this.activeLoaders.Count;

            // use as many token as possible
            while (this.timeTokens != 0 && this.countTokens != 0 && this.queue.Count != 0)
            {
                Loader loader = this.queue.Dequeue();

                lock (this)
                {
                    this.activeLoaders.Add(loader);
                }

                this.timeTokens--;
                this.countTokens--;
                loader.StartLoading();
            }
        }

        /// <summary>
        /// this function is called when a loader finished successfully to load a file.
        /// </summary>
        /// <param name="sender">the loader that just terminate</param>
        /// <param name="e">event arguments</param>
        private void LoaderFinished(object sender, EventArgs e)
        {
            Loader loader = (Loader)sender;

            // Find out what kind of file it is.
            this.ParseStream(loader);

            // release the process
            loader.Dispose();
            lock (this)
            {
                this.activeLoaders.Remove(loader);
                if (this.IsInInitialisationPhase)
                {
                    this.initialisationLoaders.Remove(loader);
                    this.TryEndInitialisation(sender, e);
                }
            }
        }

        /// <summary>
        /// This function parse the loader and raise the corresponding event.
        /// </summary>
        /// <param name="loader">the loader to parse</param>
        private void ParseStream(Loader loader)
        {
            if (loader.Result == null || !loader.Result.CanRead || loader.Result.Length == 0 || loader.Result.Position != 0)
            {
                // throw the error in the application
                if (this.FileLoadingFailded != null)
                {
                    this.FileLoadingFailded("An undetected error occured during the loading of the \"" + loader.Url + "\" file");
                }

                return;
            }

            XDocument dataXDoc = TryGetRelationFile(loader.Result);
            if (dataXDoc != null)
            {
                if (this.ICEXmlRelationFileLoaded != null)
                {
                    this.ICEXmlRelationFileLoaded(dataXDoc);
                }

                return;
            }

            XDocument settingsXDoc = TryGetSettingsFile(loader.Result);
            if (settingsXDoc != null)
            {
                if (this.ICEXmlSettingsFileLoaded != null)
                {
                    this.ICEXmlSettingsFileLoaded(settingsXDoc);
                }

                return;
            }

            XDocument errorXDoc = TryGetErrorFile(loader.Result);
            if (errorXDoc != null)
            {
                if (this.ICEErrorFileLoaded != null)
                {
                    this.ICEErrorFileLoaded(errorXDoc);
                }

                return;
            }

            Assembly assembly = TryGetAssembly(loader.Result);
            if (assembly != null)
            {
                if (this.AssemblyFileLoaded != null)
                {
                    this.AssemblyFileLoaded(assembly);
                }

                return;
            }

            if (this.UndeterminedFileFileLoaded != null)
            {
                this.UndeterminedFileFileLoaded(loader.Result);
            }
        }

        /// <summary>
        /// this function is called when a loader finished unsuccessfully to load a file.
        /// </summary>
        /// <param name="sender">the loader that just terminate</param>
        /// <param name="e">event arguments</param>
        private void LoaderFailed(object sender, EventArgs e)
        {
            Loader loader = (Loader)sender;

            // throw the error in the application
            if (this.FileLoadingFailded != null)
            {
                if (loader.Error != null)
                {
                    this.FileLoadingFailded(loader.Error);
                }
                else
                {
                    this.FileLoadingFailded("An unexpected error occured while the program try to load \"" + loader.Url + "\".");
                }                
            }

            // release the process
            loader.Dispose();
            lock (this)
            {
                this.activeLoaders.Remove(loader);
                if (this.IsInInitialisationPhase)
                {
                    this.TryEndInitialisation(sender, e);
                }
            }
        }

        /// <summary>
        /// This function try to end the initialisation phase
        /// </summary>
        /// <param name="sender">the loader that just terminate</param>
        /// <param name="e">event arguments</param>
        private void TryEndInitialisation(object sender, EventArgs e)
        {
            if (this.initialisationLoaders.Count == 0)
            {
                // there is no file to download
                if (this.InitialisationFinished != null)
                {
                    this.InitialisationFinished(this, new EventArgs());
                }

                // begin the main loop
                this.timer.Start();
            }
        }

        #endregion
    }
}