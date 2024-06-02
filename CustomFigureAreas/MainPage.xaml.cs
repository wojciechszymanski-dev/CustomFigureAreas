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
                if (!IsPointIntersecting(newPoint, 20f) && !DoesNewPointCauseIntersection(newPoint))
                {
                    drawInstance.pointList.Add(newPoint);
                    CalculateAndDisplayArea();
                }
            }

            lastTapLocation = newPoint;
            Canvas.Invalidate();
        }

        private void CalculateAndDisplayArea()
        {
            if (drawInstance.pointList.Count < 3)
            {
                area.Text = "0.00 cm²";
                return;
            }

            double areaValue = CalculatePolygonArea(drawInstance.pointList);
            area.Text = areaValue.ToString("0.00 cm²");
        }

        private double CalculatePolygonArea(List<Point?> vertices)
        {
            double area = 0;

            for (int i = 0; i < vertices.Count; i++)
            {
                var startPoint = vertices[i].Value;
                var endPoint = vertices[(i + 1) % vertices.Count].Value;

                area += (((double)startPoint.X/37.8) * ((double)endPoint.Y)/37.8) - (((double)endPoint.X)/37.8) * ((double)startPoint.Y/37.8);
            }

            return Math.Abs(area) / 2.0;
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
                        CalculateAndDisplayArea();
                        return;
                    }
                }
            }
        }

        private bool DoesNewPointCauseIntersection(Point newPoint)
        {
            int count = drawInstance.pointList.Count;
            if (count < 2)
                return false;

            var lastPoint = drawInstance.pointList[count - 1].Value;
            var newLine = (lastPoint, newPoint);

            for (int i = 0; i < count - 1; i++)
            {
                var line1 = (drawInstance.pointList[i].Value, drawInstance.pointList[i + 1].Value);
                if (DoLinesIntersect(line1, newLine))
                    return true;
            }

            var firstPoint = drawInstance.pointList[0].Value;
            var closingLine = (newPoint, firstPoint);
            for (int i = 0; i < count - 1; i++)
            {
                var line1 = (drawInstance.pointList[i].Value, drawInstance.pointList[i + 1].Value);
                if (DoLinesIntersect(line1, closingLine))
                    return true;
            }

            return false;
        }

        private bool DoLinesIntersect((Point A, Point B) line1, (Point C, Point D) line2)
        {
            double ccw(Point A, Point B, Point C)
            {
                return (C.Y - A.Y) * (B.X - A.X) - (C.X - A.X) * (B.Y - A.Y);
            }

            var A = line1.A;
            var B = line1.B;
            var C = line2.C;
            var D = line2.D;

            return (ccw(A, C, D) * ccw(B, C, D) < 0) && (ccw(A, B, C) * ccw(A, B, D) < 0);
        }
    }
}
