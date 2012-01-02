//-----------------------------------------------------------------------
// <copyright file="KeyManager.cs" company="International Monetary Fund">
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
//-----------------------------------------------------------------------

namespace ICE
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

    /// <summary>
    /// This class manages all keyboard entries
    /// </summary>
    public class KeyManager
    {
        /// <summary>
        /// this property is the page pointer
        /// </summary>
        private Page page;

        /// <summary>
        /// this property is the main system manager
        /// </summary>
        private MainManager mainManager;

        /// <summary>
        /// Value indicating whether the SHIFT key is down or not.
        /// </summary>
        private bool shiftDown = false;

        /// <summary>
        /// Value indicating whether the CTRL key is down or not.
        /// </summary>
        private bool ctrlDown = false;

        /// <summary>
        /// Initializes a new instance of the KeyManager class.
        /// </summary>
        /// <param name="mainManager">the main manager to intercat with</param>
        public KeyManager(MainManager mainManager)
        {
            this.mainManager = mainManager;
        }

        /// <summary>
        /// Gets a value indicating whether the SHIFT key is down or not.
        /// </summary>
        public bool ShiftDown
        {
            get { return this.shiftDown; }
        }

        /// <summary>
        /// Gets a value indicating whether the CTRL key is down or not.
        /// </summary>
        public bool CtrlDown
        {
            get { return this.ctrlDown; }
        }

        /// <summary>
        /// This function adds the key-event listening to the page
        /// </summary>
        /// <param name="page">the page to listen</param>
        public void ListenKeyPress(Page page)
        {
            this.page = page;
            this.page.KeyDown += new KeyEventHandler(this.Page_KeyDown);
            this.page.KeyUp += new KeyEventHandler(this.Page_KeyUp);
        }

        /// <summary>
        /// this function manage all key press event
        /// </summary>
        /// <param name="sender">the page you register</param>
        /// <param name="e">the key event arguments</param>
        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    break;
                case Key.Add:
                    this.mainManager.ViewManager.ZoomIn();
                    break;
                case Key.Alt:
                    break;
                case Key.B:
                    break;
                case Key.Back:
                    break;
                case Key.C:
                    break;
                case Key.CapsLock:
                    break;
                case Key.Ctrl:
                    // CTRL is pressed
                    this.ctrlDown = true;
                    break;
                case Key.D:
                    break;
                case Key.D0:
                    break;
                case Key.D1:
                    break;
                case Key.D2:
                    break;
                case Key.D3:
                    break;
                case Key.D4:
                    break;
                case Key.D5:
                    break;
                case Key.D6:
                    break;
                case Key.D7:
                    break;
                case Key.D8:
                    break;
                case Key.D9:
                    break;
                case Key.Decimal:
                    break;
                case Key.Delete:
                    break;
                case Key.Divide:
                    break;
                case Key.Down:
                    this.mainManager.ViewManager.GoUp();
                    break;
                case Key.E:
                    break;
                case Key.End:
                    break;
                case Key.Enter:
                    break;
                case Key.Escape:
                    break;
                case Key.F:
                    break;
                case Key.F1:
                    break;
                case Key.F10:
                    break;
                case Key.F11:
                    break;
                case Key.F12:
                    break;
                case Key.F2:
                    break;
                case Key.F3:
                    break;
                case Key.F4:
                    break;
                case Key.F5:
                    break;
                case Key.F6:
                    break;
                case Key.F7:
                    break;
                case Key.F8:
                    break;
                case Key.F9:
                    break;
                case Key.G:
                    break;
                case Key.H:
                    break;
                case Key.Home:
                    break;
                case Key.I:
                    break;
                case Key.Insert:
                    break;
                case Key.J:
                    break;
                case Key.K:
                    break;
                case Key.L:
                    break;
                case Key.Left:
                    this.mainManager.ViewManager.GoRight();
                    break;
                case Key.M:
                    break;
                case Key.Multiply:
                    break;
                case Key.N:
                    break;
                case Key.None:
                    break;
                case Key.NumPad0:
                    break;
                case Key.NumPad1:
                    break;
                case Key.NumPad2:
                    break;
                case Key.NumPad3:
                    break;
                case Key.NumPad4:
                    break;
                case Key.NumPad5:
                    break;
                case Key.NumPad6:
                    break;
                case Key.NumPad7:
                    break;
                case Key.NumPad8:
                    break;
                case Key.NumPad9:
                    break;
                case Key.O:
                    break;
                case Key.P:
                    break;
                case Key.PageDown:

                    if (this.shiftDown) 
                    {
                        // on SHIFT + PgDOWN we increase the graphDepth
                        this.mainManager.ViewManager.RaiseDecreaseDepth(); 
                    }
                    else 
                    {
                        // on PgDOWN we increase the scale rate
                        this.mainManager.ViewManager.ZoomIn(); 
                    }

                    break;
                case Key.PageUp:

                    if (this.shiftDown) 
                    {
                        // on SHIFT +PgUP we decrease the graphDepth
                        this.mainManager.ViewManager.RaiseIncreaseDepth(); 
                    }
                    else 
                    {
                        // on PgUP we decrease the scale rate
                        this.mainManager.ViewManager.ZoomOut(); 
                    }

                    break;
                case Key.Q:
                    break;
                case Key.R:
                    break;
                case Key.Right:
                    this.mainManager.ViewManager.GoLeft();
                    break;
                case Key.S:
                    if (this.shiftDown)
                    {
                        if (this.mainManager.ViewManager.CheckSplachScreen() == true)
                        {
                            this.mainManager.ViewManager.HideSplachScreen();
                        }
                        else
                        {
                            this.mainManager.ViewManager.ShowSplachScreen();
                        }
                    }

                    break;
                case Key.Shift:
                    // SHIFT is pressed
                    this.shiftDown = true;
                    break;
                case Key.Space:
                    // on CTRL + SPACE we center the view on the first selected node
                    if (this.ctrlDown)
                    {
                        this.mainManager.ViewManager.GoToSelectedNode();
                    }

                    break;
                case Key.Subtract:
                    this.mainManager.ViewManager.ZoomOut();
                    break;
                case Key.T:
                    break;
                case Key.Tab:
                    break;
                case Key.U:
                    break;
                case Key.Unknown:
                    break;
                case Key.Up:
                    this.mainManager.ViewManager.GoDown();
                    break;
                case Key.V:
                    break;
                case Key.W:
                    break;
                case Key.X:
                    break;
                case Key.Y:
                    break;
                case Key.Z:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// this function is called when a key is pressed down
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the key event</param>
        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Ctrl) 
            {
                this.ctrlDown = false; 
            }

            if (e.Key == Key.Shift) 
            {
                this.shiftDown = false; 
            }
        }
    }
}
