//-----------------------------------------------------------------------
// <copyright file="PopUp.xaml.cs" company="International Monetary Fund">
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
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Default implementation for the pop-up
    /// </summary>
    public partial class PopUp : UserControl, IPopUp
    {
        #region Fields

        /// <summary>
        /// Minimal size for the pop-up
        /// </summary>
        private Size minimalSize = new Size(40, 40);

        /// <summary>
        /// This is the pointer view use to indicate the object tracked
        /// </summary>
        private IPopUpPointer pointerView = null;

        /// <summary>
        /// This is the object view from which we want to show some information. 
        /// </summary>
        private UIElement objectTracked = null;

        /// <summary>
        /// This is the point of a object you want to track
        /// </summary>
        private Point objectTrackedPoint;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PopUp class
        /// </summary>
        public PopUp()
        {
            InitializeComponent();
            Canvas.SetZIndex(this, 1000);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event for closing the pop-up
        /// </summary>
        public event MouseButtonEventHandler Close;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the container for the pop-up data
        /// </summary>
        public Grid InfoContainer
        {
            get { return this.container; }
        }

        /// <summary>
        /// Gets or sets the color of the pop-up
        /// </summary>
        public Color Color
        {
            get
            {
                Color color = new Color();
                color.A = 0xAF;
                color.R = ((LinearGradientBrush)this.border.Background).GradientStops.ElementAt(0).Color.R;
                color.G = ((LinearGradientBrush)this.border.Background).GradientStops.ElementAt(0).Color.G;
                color.B = ((LinearGradientBrush)this.border.Background).GradientStops.ElementAt(0).Color.B;
                return color;
            }

            set
            {
                Color color = new Color();
                color.A = 0xAF;
                color.R = value.R;
                color.G = value.G;
                color.B = value.B;
                Color transparent = new Color();
                transparent.A = 0x20;
                transparent.R = value.R;
                transparent.G = value.G;
                transparent.B = value.B;
                ((LinearGradientBrush)this.border.Background).GradientStops.ElementAt(0).Color = color;
                ((LinearGradientBrush)this.border.Background).GradientStops.ElementAt(1).Color = color;
                ((LinearGradientBrush)this.border.Background).GradientStops.ElementAt(2).Color = transparent;
            }
        }

        /// <summary>
        /// Gets or sets the object view from which we want to draw information.
        /// </summary>
        public UIElement ObjectTracked
        {
            get
            {
                return this.objectTracked;
            }

            set
            {
                this.objectTracked = value;
                
                // if the new tracked object is not null and this popup don't have a pointer
                if (value != null && this.pointerView == null)
                {
                    // then we create a pointer
                    this.pointerView = new PopUpPointer();
                    Canvas.SetZIndex((UIElement)this.pointerView, 999);
                    this.UpdateObjectPointerPosition();
                }
            }
        }

        /// <summary>
        /// Gets or sets the point of the object tracked
        /// </summary>
        public Point ObjectTrackedPoint
        {
            get
            {
                return this.objectTrackedPoint;
            }

            set
            {
                this.objectTrackedPoint = value;
            }
        }

        /// <summary>
        /// Gets or sets the pointer UI of the pop-up
        /// </summary>
        public IPopUpPointer PointerView
        {
            get
            {
                return this.pointerView;
            }

            set
            {
                this.pointerView = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the pop-up
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(LayoutRoot.ActualWidth, LayoutRoot.ActualHeight);
            }

            set
            {
                this.LayoutRoot.Width = value.Width;
                this.LayoutRoot.Height = value.Height;
            }
        }

        /// <summary>
        /// Gets the minimal size of the pop-up
        /// </summary>
        public Size MinimalSize
        {
            get { return this.minimalSize; }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// this function sets the position of the PopUp's Pointer.
        /// </summary>
        /// <remarks>
        /// 1) this function must be called by a GUI thread.
        /// 2) the PopUp and its tracked object must be into the same Canvas
        /// </remarks>
        public void UpdateObjectPointerPosition()
        {
            // if there is no object attached to this popup, we can't draw the pointer
            if (this.objectTracked == null || this.objectTrackedPoint == null)
            {
                return;
            }

            // Step1 : collect information about the situation
            Point popupSituation = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            Point trackedObjectSituation = new Point(Canvas.GetLeft(this.objectTracked), Canvas.GetTop(this.objectTracked));

            // Step2 : set the head of the pointer
            this.pointerView.HeadPoint = new Point(
                this.objectTrackedPoint.X + trackedObjectSituation.X,
                this.objectTrackedPoint.Y + trackedObjectSituation.Y);

            // Step3 : set the tail of the pointer
            // Step4 : set the situation of the pointer in the Canvas

            // if the object tracked point is higher than the top of the PopUp
            if (this.pointerView.HeadPoint.Y < popupSituation.Y)
            {
                // Step3.1 : find the neerest point of the head from the top border
                this.pointerView.TailPointA = new Point(
                    System.Math.Min(
                        System.Math.Max(
                            popupSituation.X,
                            this.pointerView.HeadPoint.X), 
                            popupSituation.X + this.Size.Width - 15), 
                        popupSituation.Y);

                // Step3.2 : find a place for the other point at 25px
                // Step4.1 : set the left position
                if (this.pointerView.TailPointA.X - 25d < popupSituation.X)
                {
                    this.pointerView.TailPointB = new Point(
                        this.pointerView.TailPointA.X + 25d, popupSituation.Y);
                    Canvas.SetLeft((UIElement)this.pointerView, this.pointerView.HeadPoint.X);
                }
                else
                {
                    this.pointerView.TailPointB = new Point(
                        this.pointerView.TailPointA.X - 25d, popupSituation.Y);
                    Canvas.SetLeft((UIElement)this.pointerView, this.pointerView.TailPointB.X);
                }

                // Step4.1 : set the top position
                Canvas.SetTop((UIElement)this.pointerView, this.pointerView.HeadPoint.Y);
            }

            // if the object tracked point is between the top border and the bottom border of the PopUp
            if (this.pointerView.HeadPoint.Y <= (popupSituation.Y + this.Size.Height) &&
                this.pointerView.HeadPoint.Y >= popupSituation.Y)
            {
                // Step3.1 : find neerest border
                double borderLatitude;
                double upperValue;
                double lowerValue;
                if (this.pointerView.HeadPoint.X > popupSituation.X)
                {
                    borderLatitude = popupSituation.X + this.Size.Width;
                    upperValue = popupSituation.Y;
                    lowerValue = popupSituation.Y + this.Size.Height - 15;
                }
                else
                {
                    borderLatitude = popupSituation.X;
                    upperValue = popupSituation.Y + 15;
                    lowerValue = popupSituation.Y + this.Size.Height;
                }

                // Step3.2 : find the neerest point of the head from the selected border
                this.pointerView.TailPointA = new Point(
                    borderLatitude, 
                    System.Math.Min(System.Math.Max(upperValue, this.pointerView.HeadPoint.X), lowerValue));

                // Step3.3 : find a place for the other point at 25px
                if (this.pointerView.TailPointA.Y - 25d < upperValue)
                {
                    this.pointerView.TailPointB = new Point(
                        this.pointerView.TailPointA.X + 25d, popupSituation.Y);
                }
                else
                {
                    this.pointerView.TailPointB = new Point(
                        this.pointerView.TailPointA.X - 25d, popupSituation.Y);
                }

                // Step4.1 : set the left position
                Canvas.SetLeft((UIElement)this.pointerView, System.Math.Min(System.Math.Min(this.pointerView.HeadPoint.X, this.pointerView.TailPointA.X), this.pointerView.TailPointB.X));

                // Step4.1 : set the top position
                Canvas.SetLeft((UIElement)this.pointerView, System.Math.Min(System.Math.Min(this.pointerView.HeadPoint.Y, this.pointerView.TailPointA.Y), this.pointerView.TailPointB.Y));
            }

            // if the object tracked point is under the lower border of the popup 
            if (this.pointerView.HeadPoint.Y > (popupSituation.Y + this.Size.Height))
            {
                // Step3.1 : find neerest point of the head on the low border
                this.pointerView.TailPointA = new Point(
                    System.Math.Min(
                        System.Math.Max(
                            popupSituation.X + 15,
                            this.pointerView.HeadPoint.X), 
                        popupSituation.X + this.Size.Width), 
                    popupSituation.Y + this.Size.Height);

                // Step3.2 : find a place for the other point at 25px
                // Step4.1 : set the left position
                if (this.pointerView.TailPointA.X - 25d < (popupSituation.X + 15))
                {
                    this.pointerView.TailPointB = new Point(
                        this.pointerView.TailPointA.X + 25d, popupSituation.Y + this.Size.Height);
                    Canvas.SetLeft((UIElement)this.pointerView, this.pointerView.HeadPoint.X);
                }
                else
                {
                    this.pointerView.TailPointB = new Point(
                        this.pointerView.TailPointA.X - 25d, popupSituation.Y + this.Size.Height);
                    Canvas.SetLeft((UIElement)this.pointerView, this.pointerView.TailPointB.X);
                }

                // Step4.1 : set the top position
                Canvas.SetTop((UIElement)this.pointerView, this.pointerView.TailPointA.Y);
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// This function propagates the closing event
        /// </summary>
        /// <param name="sender">This control</param>
        /// <param name="e">The mouse event arguments</param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Close != null)
            {
                this.Close(sender, e);
            }
        }

        #endregion
    }
}
