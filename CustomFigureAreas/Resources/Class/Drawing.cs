using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace CustomFigureAreas.Resources.Class
{
    internal class Drawing : IDrawable
    {
        public List<Point?> pointList = new List<Point?>();

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.White;
            canvas.StrokeColor = Colors.White;
            canvas.StrokeSize = 5;

            for (int i = 0; i < pointList.Count - 1; i++)
            {
                var startPoint = pointList[i];
                var endPoint = pointList[i + 1];

                if (startPoint.HasValue && endPoint.HasValue)
                {
                    canvas.FillCircle((float)startPoint.Value.X, (float)startPoint.Value.Y, 20f);
                    canvas.DrawLine((float)startPoint.Value.X, (float)startPoint.Value.Y,(float)endPoint.Value.X, (float)endPoint.Value.Y);
                }
            }

            // Draw the last point
            if (pointList.Count > 0 && pointList[pointList.Count - 1].HasValue)
            {
                canvas.FillCircle((float)pointList[pointList.Count - 1].Value.X,
                                  (float)pointList[pointList.Count - 1].Value.Y, 20f);
            }
        }
    }
}
