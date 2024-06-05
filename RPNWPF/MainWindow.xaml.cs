using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RPNLogic;

namespace WPF
{
    class Point
    {
        public readonly double X;
        public readonly double Y;
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnMain_Click(object sender, RoutedEventArgs e)
        {
            CanvasField.Children.Clear();
            DrawCanvas();
        }

        private void DrawCanvas()
        {
            string expression = tbInput.Text;
            double xStart = double.Parse(tbXStart.Text);
            double xEnd = double.Parse(tbXEnd.Text);
            double yStart = 0, yEnd = 0;
            double step = double.Parse(tbStep.Text);
            double scale = double.Parse(tbScale.Text);

            RPNCalculator calculator = new RPNCalculator(expression);
            List<Point> points = new List<Point>();

            for (double x = xStart; x <= xEnd; x+=step)
            {
                double y = calculator.Calculate(x);
                points.Add(new Point(x, y));

                yStart = (yStart > y) ? y : yStart;
                yEnd = (yEnd < y) ? y : yEnd;
            }

            CanvasDrawer canvasDrawer = new CanvasDrawer(CanvasField, xStart, xEnd, yStart, yEnd, step, scale);
            canvasDrawer.DrawGraphic(points);
        }
    }
}
