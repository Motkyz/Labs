using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Ink;
using System.Windows;

namespace WPF
{
    static class PointExtensions
    {
        public static Point ToMathCoordinates(this Point point, Canvas canvas, double scale)
        {
            return new Point(
                (point.X - canvas.ActualWidth / 2) / scale,
                (canvas.ActualHeight / 2 - point.Y) / scale);
        }
        public static Point ToUICoordinates(this Point point, Canvas canvas, double scale)
        {
            return new Point(
                point.X * scale + canvas.ActualWidth / 2,
                canvas.ActualHeight / 2 - point.Y * scale);
        }
    }
    class CanvasDrawer
    {
        private readonly Canvas _canvas;
        private readonly Brush _defaultStroke = Brushes.Black;
        private readonly int _divisionLength = 6;

        private readonly Point _xAxisStart, _xAxisEnd, _yAxisStart, _yAxisEnd;

        private readonly double _xStart, _xEnd, _yStart, _yEnd;
        private readonly double _step;
        private readonly double _scale;

        public CanvasDrawer(Canvas canvas, double xStart, double xEnd, double yStart, double yEnd, double step, double scale)
        {
            _canvas = canvas;

            _xAxisStart = new Point(0, (double)_canvas.ActualHeight / 2);
            _xAxisEnd = new Point((double)_canvas.ActualWidth, (double)_canvas.ActualHeight / 2);

            _yAxisStart = new Point((double)_canvas.ActualWidth / 2, (double)_canvas.ActualHeight);
            _yAxisEnd = new Point((double)_canvas.ActualWidth / 2, 0);
            
            _xStart = xStart;
            _xEnd = xEnd;
            _yStart = yStart;
            _yEnd = yEnd;
            _step = step;
            _scale = scale;
        }

        private void DrawPoint(Point point, Brush color, double radius = 2.6)
        {
            Ellipse ellipse = new Ellipse()
            {
                Stroke = color,
                Fill = color,
                Width = radius * 2,
                Height = radius * 2,
            };

            Canvas.SetLeft(ellipse, point.X - radius);
            Canvas.SetTop(ellipse, point.Y - radius);

            _canvas.Children.Add(ellipse);
        }

        private void DrawLine(Point start, Point end, Brush color, double thickness = 1.8)
        {
            Line line = new Line()
            {
                StrokeThickness = thickness,
                Stroke = color,
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
            };

            _canvas.Children.Add(line);
        }

        private void DrawAxises()
        {   
            DrawLine(_xAxisStart, _xAxisEnd, _defaultStroke);
            DrawLine(_yAxisStart, _yAxisEnd, _defaultStroke);

            DrawDivisions();
        }

        private void DrawDivisions()
        {
            //Проставляю деления на OX
            for (double x = _xStart; x <= _xEnd; x += _step)
            {
                Point point = new Point(x, 0).ToUICoordinates(_canvas, _scale);

                Point start = new Point(point.X, point.Y - _divisionLength / 2);
                Point end = new Point(point.X, point.Y + _divisionLength / 2);

                DrawLine(start, end, _defaultStroke, 1);
            }

            //Проставляю деления на OY
            for (double y = _yStart; y <= _yEnd; y += _step)
            {
                Point point = new Point(0, y).ToUICoordinates(_canvas, _scale);

                Point start = new Point(point.X - _divisionLength / 2, point.Y);
                Point end = new Point(point.X + _divisionLength / 2, point.Y);

                DrawLine(start, end, _defaultStroke, 1);
            }
        }

        public void DrawGraphic(List<Point> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Point uiPointStart = points[i].ToUICoordinates(_canvas, _scale);
                if (i != points.Count - 1)
                {
                    Point uiPointEnd = points[i + 1].ToUICoordinates(_canvas, _scale);
                    DrawLine(uiPointStart, uiPointEnd, Brushes.Red);
                }
                DrawPoint(uiPointStart, Brushes.Black);
            }
            DrawAxises();
        }
    }
}