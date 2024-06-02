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

            for (int i = 0; i < pointList.Count; i++)
            {
                var startPoint = pointList[i];
                var endPoint = pointList[(i + 1) % pointList.Count];

                if (startPoint.HasValue)
                {
                    canvas.FillCircle((float)startPoint.Value.X, (float)startPoint.Value.Y, 20f);
                }

                if (startPoint.HasValue && endPoint.HasValue)
                {
                    canvas.DrawLine((float)startPoint.Value.X, (float)startPoint.Value.Y, (float)endPoint.Value.X, (float)endPoint.Value.Y);
                }
            }
        }
    }
}
