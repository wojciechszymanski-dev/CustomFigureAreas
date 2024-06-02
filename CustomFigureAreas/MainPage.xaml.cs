using CustomFigureAreas.Resources.Class;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace CustomFigureAreas
{
    public partial class MainPage : ContentPage
    {
        readonly Drawing drawInstance;
        private DateTime lastTapTime = DateTime.MinValue;
        private Point? lastTapLocation = null;

        public MainPage()
        {
            InitializeComponent();
            drawInstance = new Drawing();
            Canvas.Drawable = drawInstance;
        }

        public void CanvasTapped(object sender, TappedEventArgs e)
        {
            var tapLocation = e.GetPosition(Canvas);
            if (tapLocation is null) return;

            var newPoint = new Point(tapLocation.Value.X, tapLocation.Value.Y);
            var timeSinceLastTap = DateTime.Now - lastTapTime;
            lastTapTime = DateTime.Now;

            if (timeSinceLastTap < TimeSpan.FromMilliseconds(500) && lastTapLocation == newPoint) 
                RemovePointAtLocation(newPoint, 20f);
            else
            {
                if (drawInstance.pointList.Count > 2 && drawInstance.pointList[0].HasValue)
                {
                    var firstPoint = drawInstance.pointList[0].Value;
                    var distance = Math.Sqrt(Math.Pow(newPoint.X - firstPoint.X, 2) + Math.Pow(newPoint.Y - firstPoint.Y, 2));
                    if (distance < 20 * 2) drawInstance.pointList.Add(firstPoint);
                    else if (!IsPointIntersecting(newPoint, 20f)) drawInstance.pointList.Add(newPoint);
                }
                else if (!IsPointIntersecting(newPoint, 20f)) drawInstance.pointList.Add(newPoint);
            }

            lastTapLocation = newPoint; 
            lastTapLocation = newPoint; 
            Canvas.Invalidate();
        }

        private bool IsPointIntersecting(Point newPoint, float radius)
        {
            foreach (var point in drawInstance.pointList)
            {
                if (point.HasValue)
                {
                    var distance = Math.Sqrt(Math.Pow(newPoint.X - point.Value.X, 2) + Math.Pow(newPoint.Y - point.Value.Y, 2));
                    if (distance < radius * 2) return true;
                }
            }
            return false;
        }

        private void RemovePointAtLocation(Point location, float radius)
        {
            for (int i = 0; i < drawInstance.pointList.Count; i++)
            {
                var point = drawInstance.pointList[i];
                if (point.HasValue)
                {
                    var distance = Math.Sqrt(Math.Pow(location.X - point.Value.X, 2) + Math.Pow(location.Y - point.Value.Y, 2));
                    if (distance < radius * 2)
                    {
                        drawInstance.pointList.RemoveAt(i);
                        return;
                    }
                }
            }
        }
    }
}
