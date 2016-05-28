﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bulkes
{
    class Indicator
    {
        private float x;
        private float y;
        private float alpha;//radians

        public void getParameters(float x0, float y0, float R, float x1, float y1)
        {
            float k;
            if (Math.Abs(x1 - x0) < 0.001f)
            {
                x = x0;
                if (y1 < y0)
                {
                    y = -R + y0;
                    alpha = (float)-Math.PI / 2.0f;//  -Pi/2
                }
                else
                {
                    y = R + y0;
                    alpha = (float)Math.PI / 2.0f;//  Pi/2
                }
            }
            else
            {
                k = (y1 - y0) / (x1 - x0);
                if (x1 - x0 < 0)
                    x = (float)Math.Sqrt(1.0f / (1f + k * k)) * (-R) + x0;
                else
                    x = (float)Math.Sqrt(1.0f / (1f + k * k)) * R + x0;

                y = k * x - k * x0 + y0;
                if (y1 - y0 < 0)
                    alpha = -(float)Math.Acos((x - x0) / R);
                else
                    alpha = (float)Math.Acos((x - x0) / R);
            }

        }

        public float getAlpha()
        {
            return alpha;
        }

        public float getX()
        {
            return x;
        }

        public float getY()
        {
            return y;
        }
        public Path getTriangle(float x0, float y0, float R, float coefficient)
        {
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.FillRule = FillRule.Nonzero;
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(x, y);
            pathFigure.IsClosed = true;
            pathGeometry.Figures.Add(pathFigure);
            float x2;
            float y2;
            float alpha2;
            float alphaDiff;
            alphaDiff = Settings.IndicatorBaseAlpha / coefficient;
            alpha2 = alpha + alphaDiff;
            x2 = (float)Math.Cos(alpha2) * R + x0;
            y2 = (float)Math.Sin(alpha2) * R + y0;
            LineSegment lineSegment1 = new LineSegment();
            lineSegment1.Point = new Point(x2, y2);
            pathFigure.Segments.Add(lineSegment1);
            float x3;
            float y3;
            float alpha3;
            alpha3 = alpha - alphaDiff;
            x3 = (float)Math.Cos(alpha3) * R + x0;
            y3 = (float)Math.Sin(alpha3) * R + y0;
            LineSegment lineSegment2 = new LineSegment();
            lineSegment2.Point = new Point(x3, y3);
            pathFigure.Segments.Add(lineSegment2);
            Path path = new Path();

            path.Stretch = Stretch.Fill;
            path.StrokeLineJoin = PenLineJoin.Round;
            path.Fill = new SolidColorBrush(Colors.Gray);
            path.Data = pathGeometry;
            return path;
        }
    }
}
