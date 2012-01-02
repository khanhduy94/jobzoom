//-----------------------------------------------------------------------
// <copyright file="MouseManager.cs" company="International Monetary Fund">
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

namespace ICE
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;
    
    /// <summary>
    /// this class is a tool use to help silverlight user to use drag and drop and the mouse's weel
    /// </summary>
    public class MouseManager
    {
        /// <summary>
        /// this property is the element we listen
        /// </summary>
        private UIElement element;

        /// <summary>
        /// this property is true if the mouse is captured and if the mouse has moved
        /// </summary>
        private bool hasmoved;

        /// <summary>
        /// Initializes a new instance of the MouseManager class.
        /// </summary>
        /// <param name="element">the element to listen</param>
        public MouseManager(UIElement element)
        {
            this.element = element;
            this.element.MouseLeftButtonUp += new MouseButtonEventHandler(this.Element_MouseLeftButtonUp);
            this.element.MouseLeftButtonDown += new MouseButtonEventHandler(this.Element_MouseLeftButtonDown);

            HtmlPage.Window.AttachEvent("DOMMouseScroll", this.OnMouseWheel);
            HtmlPage.Window.AttachEvent("onmousewheel", this.OnMouseWheel);
            HtmlPage.Document.AttachEvent("onmousewheel", this.OnMouseWheel);
        }

        /// <summary>
        /// this event occur when a simple click occur on the element
        /// </summary>
        public event MouseButtonEventHandler LeftButtonClick;

        /// <summary>
        /// this event occur when the mouse wheel is used forward
        /// </summary>
        public event MouseEventHandler WheelMouseUp;

        /// <summary>
        /// this event occur when the mouse wheel is used backward
        /// </summary>
        public event MouseEventHandler WheelMouseDown;

        /// <summary>
        /// this event occur at the end of a left click and move
        /// </summary>
        public event MouseButtonEventHandler DropOnLeftButtonDown;

        /// <summary>
        /// this event occur at the begining of a left click and move
        /// </summary>
        public event MouseEventHandler DragOnLeftButtonDown;

        /// <summary>
        /// this event occur when a the left button is pressed down on the element
        /// </summary>
        public event MouseButtonEventHandler LeftButtonDown;

        /// <summary>
        /// this event occur when the mouse move and the left button is down
        /// </summary>
        public event MouseEventHandler MouseMovedOnLeftButtonDown;

        /// <summary>
        /// Gets or sets the element we listen
        /// </summary>
        public UIElement Element
        {
            get { return this.element; }
            set { this.element = value; }
        }

        /// <summary>
        /// this function is called when the browser receive an event from the mouse's weel
        /// </summary>
        /// <param name="sender">the browser's page</param>
        /// <param name="args">the event argument</param>
        private void OnMouseWheel(object sender, HtmlEventArgs args)
        {
            double mouseDelta = 0;
            ScriptObject e = args.EventObject;

            if (e.GetProperty("detail") != null)
            {
                // Mozilla and Safari
                mouseDelta = (double)e.GetProperty("detail");
                mouseDelta = Math.Sign(mouseDelta);
                if (mouseDelta < 0)
                {
                    if (this.WheelMouseUp != null)
                    {
                        this.WheelMouseUp(sender, null);
                    }
                }
                else
                {
                    if (this.WheelMouseDown != null) 
                    {
                        this.WheelMouseDown(sender, null); 
                    }
                }
            }
            else if (e.GetProperty("wheelDelta") != null)
            {
                // IE and Opera
                mouseDelta = (double)e.GetProperty("wheelDelta");
                mouseDelta = Math.Sign(mouseDelta);
                if (mouseDelta >= 0)
                {
                    if (this.WheelMouseUp != null) 
                    {
                        this.WheelMouseUp(sender, null); 
                    }
                }
                else
                {
                    if (this.WheelMouseDown != null) 
                    {
                        this.WheelMouseDown(sender, null); 
                    }
                }
            }
        }

        /// <summary>
        /// this function sets the left button down action
        /// </summary>
        /// <param name="sender">the element</param>
        /// <param name="e">the mouse event args</param>
        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.hasmoved = false;
            this.element.CaptureMouse();
            this.element.MouseMove += new MouseEventHandler(this.Element_MouseMove);
            if (this.LeftButtonDown != null) 
            { 
                this.LeftButtonDown(sender, e); 
            }
        }

        /// <summary>
        /// this function sets the left button down and move action
        /// </summary>
        /// <param name="sender">the element</param>
        /// <param name="e">the mouse event args</param>
        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
          ((FrameworkElement)sender).Cursor = Cursors.Hand;

            if (!this.hasmoved)
            {
                this.hasmoved = true;
                if (this.DragOnLeftButtonDown != null) 
                {
                    this.DragOnLeftButtonDown(sender, e); 
                }
            }

            if (this.MouseMovedOnLeftButtonDown != null) 
            {
                this.MouseMovedOnLeftButtonDown(sender, e); 
            }
        }

        /// <summary>
        /// this function trasform  the button down to a simple or a double click
        /// </summary>
        /// <param name="sender">the element</param>
        /// <param name="e"> the mouse event parameter</param>
        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.element.ReleaseMouseCapture();
            this.element.MouseMove -= new MouseEventHandler(this.Element_MouseMove);
            ((FrameworkElement)sender).Cursor = Cursors.Arrow;

            if (this.hasmoved)
            {
                if (this.DropOnLeftButtonDown != null)
                {
                    this.DropOnLeftButtonDown(sender, e); 
                }
            }
            else
            {
                if (this.LeftButtonClick != null) 
                {
                    this.LeftButtonClick(this.element, e); 
                }
            }
        }
    }
}
