//-----------------------------------------------------------------------
// <copyright file="CirclePanel.cs" company="International Monetary Fund">
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

namespace ICE.view
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
    /// This Panel sets the disposition of its child in a cicle
    /// </summary>
    public class CirclePanel : Panel
    {
        #region Fields

        /// <summary>
        /// DependencyProperty declaration of the diameter
        /// </summary>
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(
    "Diameter", typeof(double), typeof(CirclePanel), new PropertyMetadata(0d, new PropertyChangedCallback(DiameterChangedCallBack)));

        /// <summary>
        /// DependencyProperty declaration of the scale
        /// </summary>
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(
    "Scale", typeof(double), typeof(CirclePanel), new PropertyMetadata(0d, new PropertyChangedCallback(ScaleChangedCallBack)));

        /// <summary>
        /// DependencyProperty declaration of the angle
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(
    "StartAngle", typeof(double), typeof(CirclePanel), new PropertyMetadata(0d, new PropertyChangedCallback(StartAngleChangedCallBack)));

        /// <summary>
        /// the circle
        /// </summary>
        private Ellipse cicle;

        #endregion

        /// <summary>
        /// Initializes a new instance of the CirclePanel class.
        /// </summary>
        public CirclePanel() : base()
        {
            this.cicle = new Ellipse();
            this.cicle.Stroke = new SolidColorBrush(Colors.Orange);
            this.cicle.StrokeThickness = 3;
            this.Children.Add(this.cicle);
        }

        #region Properties

        /// <summary>
        /// Sets the color of the circle
        /// </summary>
        public Color Color
        {
            set 
            {
                this.cicle.Stroke = new SolidColorBrush(value);
            }
        }

        /// <summary>
        /// Gets or sets the diameter of the cicle
        /// </summary>
        public double Diameter
        {
            get { return (double)this.GetValue(DiameterProperty); }
            set { this.SetValue(DiameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the diameter of the cicle
        /// </summary>
        public double Scale
        {
            get { return (double)this.GetValue(ScaleProperty); }
            set { this.SetValue(ScaleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the angle of the first child
        /// The default value is 0
        /// </summary>
        public double StartAngle
        {
            get { return (double)this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }

        #endregion

        #region Panel functions

        /// <summary>
        /// this function specifies the desired size of the panel and its children
        /// </summary>
        /// <param name="availableSize">the size available from the parent component</param>
        /// <returns>the desired size</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            double maxChildWidth = 0;
            double maxChildHeight = 0;

            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);

                if (maxChildHeight < child.DesiredSize.Height)
                {
                    maxChildHeight = child.DesiredSize.Height;
                }

                if (maxChildWidth < child.DesiredSize.Width)
                {
                    maxChildWidth = child.DesiredSize.Width;
                }
            }

            this.cicle.Measure(availableSize);

            return availableSize;
        }

        /// <summary>
        /// this function sets the situation of all the children
        /// </summary>
        /// <param name="finalSize">the final size available for this panel</param>
        /// <returns>the final size of this panel</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (!this.Children.Contains(this.cicle))
            {
                this.Children.Add(this.cicle);
            }

            Point center = new Point(this.Width / 2, this.Height / 2);
            double angleBetweenTwoChild = 360d / (this.Children.Count - 1);

            for (int i = 0; i < this.Children.Count; i++)
            {
                if (this.cicle == this.Children[i])
                {
                    continue;
                }

                double childrenAngleRadian = Math.PI * ((i * angleBetweenTwoChild) + this.StartAngle) / 180;

                Point childSituation = new Point(
                    center.X + ((this.Diameter / 2) * Math.Cos(childrenAngleRadian)),
                    center.Y + ((this.Diameter / 2) * Math.Sin(childrenAngleRadian)));

                Point childRectOrigin = new Point(
                    childSituation.X - (this.Children[i].DesiredSize.Width / 2),
                    childSituation.Y - (this.Children[i].DesiredSize.Height / 2));

                Rect childPosition = new Rect(childRectOrigin, this.Children[i].DesiredSize);

                this.Children[i].Arrange(childPosition);
            }

            Rect elipsePosition = new Rect(
                new Point(center.X - (this.Diameter / 2), center.Y - (this.Diameter / 2)),
                new Point(center.X + (this.Diameter / 2), center.Y + (this.Diameter / 2)));
            this.cicle.Arrange(elipsePosition);

            return finalSize;
        }

        /// <summary>
        /// this function is called when the diameter value has changed
        /// </summary>
        /// <param name="property">diameter DependencyProperty</param>
        /// <param name="args">event arguments</param>
        private static void DiameterChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            ((CirclePanel)property).InvalidateArrange();
        }

        /// <summary>
        /// this function is called when the angle value has changed
        /// </summary>
        /// <param name="property">angle DependencyProperty</param>
        /// <param name="args">event arguments</param>
        private static void StartAngleChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            ((CirclePanel)property).InvalidateArrange();
        }

        /// <summary>
        /// this function is called when the scale value has changed
        /// </summary>
        /// <param name="property">scale DependencyProperty</param>
        /// <param name="args">event arguments</param>
        private static void ScaleChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            foreach (UIElement item in ((CirclePanel)property).Children)
            {
                if (((CirclePanel)property).cicle == item)
                {
                    continue;
                }

                if (!(item.RenderTransform is ScaleTransform))
                {
                    item.RenderTransform = new ScaleTransform();
                    item.Measure(new Size(double.MaxValue, double.MaxValue));
                    ((ScaleTransform)item.RenderTransform).CenterX = item.DesiredSize.Width / 2;
                    ((ScaleTransform)item.RenderTransform).CenterY = item.DesiredSize.Height / 2;
                }

                ((ScaleTransform)item.RenderTransform).ScaleX = ((CirclePanel)property).Scale;
                ((ScaleTransform)item.RenderTransform).ScaleY = ((CirclePanel)property).Scale;
            }

            ((CirclePanel)property).InvalidateArrange();
        }

        #endregion
    }
}
