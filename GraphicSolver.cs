using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace GUI_Demo
{
    public class GraphicSolver //composition with Equation
    {
        private readonly Equation equation;
        public GraphicSolver(Equation equation) {
            this.equation = equation;
        }
        private double FindMaximum()
        {
            int arrSize = equation.Size * 3;
            double[] arr = new double[arrSize];
            int k = 0;
            for(int i = 0; i < equation.Size; i++)
            {
                for (int j = 0; j < equation.Size; j++)
                {
                    arr[k] = equation.Coefficients[i,j];
                    k++;
                }
                arr[k] = equation.Constants[i];
                k++;
            }
            return arr.Max();
        }
        public void SolveGraphical()
        {
            List<double> result = new List<double>();

            using (Form graphicalForm = new Form())
            {
                graphicalForm.Text = "Графіки рівнянь";
                graphicalForm.Size = new System.Drawing.Size(600, 400);

                Chart chart = new Chart();
                chart.Parent = graphicalForm;
                chart.Dock = DockStyle.Fill;
                ChartArea plot = new ChartArea("Графічний метод");
                chart.ChartAreas.Add(plot);
                plot.AxisX.Minimum = double.NaN;
                plot.AxisX.Maximum = double.NaN;
                plot.AxisY.Minimum = double.NaN;
                plot.AxisY.Maximum = double.NaN;

                Series series1 = CreateSeries(0);
                Series series2 = CreateSeries(1);
                double max = FindMaximum() * 5;
                double startX = (max > 0)? (-max) : max;
                double endX = Math.Abs(max);
                for (double x = startX; x <= endX; x += 0.1)
                {
                    double y1 = (equation.Constants[0] - equation.Coefficients[0, 0] * x) / equation.Coefficients[0, 1];
                    series1.Points.AddXY(x, y1);

                    double y2 = (equation.Constants[1] - equation.Coefficients[1, 0] * x) / equation.Coefficients[1, 1];
                    series2.Points.AddXY(x, y2);

                    if (Math.Abs(y1 - y2) < 0.001)
                    {
                        result.Add(x);
                        result.Add(y1);
                    }
                }

                chart.Series.Add(series1);
                chart.Series.Add(series2);

                graphicalForm.ShowDialog();
                equation.Result = result.ToArray();
            }
        }
        private Series CreateSeries(int index)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Line;
            series.Color = (index == 0) ? Color.Blue : Color.Red;
            series.BorderWidth = 3;
            return series;
        }
    }
}
