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
        public static Point ToMathCoordinates(this Point point, Canvas canvas, double zoom)
        {
            return new Point(
                (point.X - canvas.ActualWidth / 2) / zoom,
                (canvas.ActualHeight / 2 - point.Y) / zoom);
        }
        public static Point ToUICoordinates(this Point point, Canvas canvas, double zoom)
        {
            return new Point(
                point.X * zoom + canvas.ActualWidth / 2,
                canvas.ActualHeight / 2 - point.Y * zoom);
        }
    }
    class CanvasDrawer
    {
        private readonly Canvas _canvas;
        private readonly Brush _defaultStroke = Brushes.Black;
        private readonly int _divisionLength = 6;

        private readonly Point _xAxisStart, _xAxisEnd, _yAxisStart, _yAxisEnd;

        private readonly double _xStart,  _xEnd;
        private readonly double _step;
        private readonly double _zoom;
        private readonly double _yStart, _yEnd;

        public CanvasDrawer(Canvas canvas, double xStart, double xEnd, double yStart, double yEnd, double step, double zoom)
        {
            _canvas = canvas;
            _xAxisStart = new Point((double)_canvas.ActualWidth / 2, 0);
            _xAxisEnd = new Point((double)_canvas.ActualWidth / 2, (double)_canvas.ActualHeight);

            _yAxisStart = new Point(0, (double)_canvas.ActualHeight / 2);
            _yAxisEnd = new Point((double)_canvas.ActualWidth, (double)_canvas.ActualHeight / 2);

            _xStart = xStart;
            _xEnd = xEnd;
            _yStart = yStart;
            _yEnd = yEnd;
            _step = step;
            _zoom = zoom;
        }

        private void DrawAxises()
        {   
            DrawLine(_xAxisStart, _xAxisEnd, _defaultStroke);
            DrawLine(_yAxisStart, _yAxisEnd, _defaultStroke);

            DrawDivisions();
        }

        private void DrawPoint(Point point, Brush color, int radius = 3)
        {
            Ellipse dot = new Ellipse()
            { 
                Stroke = color,
                Fill = color,
                Width = radius * 2,
                Height = radius * 2,
            };

            Canvas.SetLeft(dot, point.X - radius);
            Canvas.SetTop(dot, point.Y - radius);

            _canvas.Children.Add(dot);
        }
        private void DrawLine(Point start, Point end, Brush color, double thickness = 2)
        {
            Line line = new Line()
            {
                StrokeThickness = 2,
                Stroke = color,
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
            };

            _canvas.Children.Add(line);
        }

        private void DrawDivisions()
        {
            //Проставляю деления на OX
            for (double x = _xStart; x <= _xEnd; x += _step)
            {
                Point point = new Point(x, 0).ToUICoordinates(_canvas, _zoom);

                Point p1 = new Point(point.X, point.Y - _divisionLength / 2);
                Point p2 = new Point(point.X, point.Y + _divisionLength / 2);

                DrawLine(p1, p2, _defaultStroke);
            }

            //Проставляю деления на OY
            for (double y = _yStart; y <= _yEnd; y += _step)
            {
                Point point = new Point(0, y).ToUICoordinates(_canvas, _zoom);

                Point p1 = new Point(point.X - _divisionLength / 2, point.Y);
                Point p2 = new Point(point.X + _divisionLength / 2, point.Y);

                DrawLine(p1, p2, _defaultStroke);
            }
        }

        public void DrawGraphic(List<Point> points)
        {
            for (int i = 0; i < points.Count - 2; i++)
            {
                Point uiPoint1 = points[i].ToUICoordinates(_canvas, _zoom);
                Point uiPoint2 = points[i + 1].ToUICoordinates(_canvas, _zoom);

                DrawLine(uiPoint1, uiPoint2, Brushes.Red);
                DrawPoint(uiPoint1, Brushes.Black);
            }
            DrawAxises();
        }
    }
}