using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI_Demo
{
    public class Equation
    {
        public double[,] Coefficients { get; set; }
        public double[] Constants { get; set; }
        public int Size {  get; set; }
        public double[] Result { get; set; }
        public Equation(double[,] coefficients, double[] constants, int size)
        {
            Coefficients = coefficients;
            Constants = constants;
            Size = size;
            Result = new double[size];
        }
        public double[,] CalculateMinor(double[,] matrix, int index)
        {
            int n = matrix.GetLength(0);
            double[,] minor = new double[n - 1, n - 1];

            for (int i = 1; i < n; i++)
            {
                for (int j = 0, k = 0; j < n; j++)
                {
                    if (j == index)
                        continue;

                    minor[i - 1, k++] = matrix[i, j];
                }
            }

            return minor;
        }
        public double[,] Transpose(double[,] matrix, int n)
        {
            double[,] transMatrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    transMatrix[i, j] = matrix[j, i];
                }
            }
            return transMatrix;
        }
        public double CalculateDeterminant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            if(n == 1)
            {
                return matrix[0, 0];
            }
            double determinant = 0;
            int sign = 1;
            for (int i = 0; i < n; i++)
            {
                double[,] minorMatrix = CalculateMinor(matrix, i);
                double minorDeterminant = CalculateDeterminant(minorMatrix);
                determinant += sign * matrix[0, i] * minorDeterminant;
                sign = -sign;
            }
            return determinant;
        }
        public void CalculateSqrtMethod()
        {
            double[,] S = new double[Size, Size];
            double[] y = new double[Size];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == j)
                    {
                        double sum = 0;
                        for (int k = 0; k < i; k++)
                        {
                            sum += S[i, k] * S[i, k];
                        }
                        S[i, i] = Math.Sqrt(Coefficients[i, i] - sum);
                    }
                    else if (i > j)
                    {
                        double sum = 0;
                        for (int k = 0; k < j; k++)
                        {
                            sum += S[i, k] * S[j, k];
                        }
                        S[i, j] = (Coefficients[i, j] - sum) / S[j, j];
                    }
                    else 
                    {
                        S[i, j] = 0;
                    }
                }
            }

            double[,] St = Transpose(S, Size);

            y[0] = Constants[0] / S[0, 0];
            for (int i = 1; i < Size; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                {
                    sum += S[i, j] * y[j];
                }
                y[i] = (Constants[i] - sum) / S[i, i];
            }

            Result[Size - 1] = y[Size - 1] / S[Size - 1, Size - 1];
            for (int i = Size - 2; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < Size; j++)
                {
                    sum += St[i, j] * Result[j];
                }
                Result[i] = (y[i] - sum) / S[i, i];
            }
        }
        public void CalculateRotationMethod()
        {
            for (int i = 0; i < Size - 1; i++)
            {
                for (int k = i + 1; k < Size; k++)
                {
                    double r = Math.Sqrt(Coefficients[i, i] * Coefficients[i, i] + Coefficients[k, i] * Coefficients[k, i] * Coefficients[k, i]);
                    double c = Coefficients[i, i] / r;
                    double s = -Coefficients[k, i] / r;

                    for (int j = 0; j < Size; j++)
                    {
                        double tempA1 = Coefficients[i, j];
                        double tempA2 = Coefficients[k, j];
                        Coefficients[i, j] = c * tempA1 - s * tempA2;
                        Coefficients[k, j] = s * tempA1 + c * tempA2;
                    }

                    double tempB1 = Constants[i];
                    double tempB2 = Constants[k];
                    Constants[i] = c * tempB1 - s * tempB2;
                    Constants[k] = s * tempB1 + c * tempB2;
                }
            }

            for (int i = Size - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < Size; j++)
                {
                    sum += Coefficients[i, j] * Result[j];
                }
                Result[i] = (Constants[i] - sum) / Coefficients[i, i];
            }
        }
        public void CalculateLUPMethod()
        {
            double[,] L = new double[Size, Size];
            double[,] U = new double[Size, Size];
            int[] P = new int[Size];

            for (int i = 0; i < Size; i++)
            {
                P[i] = i;
            }

            Array.Copy(Coefficients, U, Coefficients.Length);
            for (int i = 0; i < Size; i++)
            {
                L[i, i] = 1.0;
            }

            for (int k = 0; k < Size - 1; k++)
            {
                int pivotRow = k;
                double pivotValue = Math.Abs(U[k, k]);

                for (int i = k + 1; i < Size; i++)
                {
                    if (Math.Abs(U[i, k]) > pivotValue)
                    {
                        pivotRow = i;
                        pivotValue = Math.Abs(U[i, k]);
                    }
                }

                if (pivotRow != k)
                {
                    double temp;
                    for (int j = 0; j < Size; j++)
                    {
                        temp = U[k, j];
                        U[k, j] = U[pivotRow, j];
                        U[pivotRow, j] = temp;
                    }

                    int tempIndex = P[k];
                    P[k] = P[pivotRow];
                    P[pivotRow] = tempIndex;

                    for (int j = 0; j < k; j++)
                    {
                        temp = L[k, j];
                        L[k, j] = L[pivotRow, j];
                        L[pivotRow, j] = temp;
                    }
                }

                for (int i = k + 1; i < Size; i++)
                {
                    L[i, k] = U[i, k] / U[k, k];
                    for (int j = k; j < Size; j++)
                    {
                        U[i, j] -= L[i, k] * U[k, j];
                    }
                }
            }

            double[] y = new double[Size];
            for (int i = 0; i < Size; i++)
            {
                y[i] = Constants[P[i]];
                for (int j = 0; j < i; j++)
                {
                    y[i] -= L[i, j] * y[j];
                }
            }
            for (int i = Size - 1; i >= 0; i--)
            {
                Result[i] = y[i];
                for (int j = i + 1; j < Size; j++)
                {
                    Result[i] -= U[i, j] * Result[j];
                }
                Result[i] /= U[i, i];
            }
        }
        /*public void SolveGraphical()
        {
            List<double> result = new List<double>();

            Form graphicalForm = new Form();
            graphicalForm.Text = "Графік рівнянь";
            graphicalForm.Size = new System.Drawing.Size(600, 400);

            Chart chart = new Chart();
            chart.Parent = graphicalForm;
            chart.Dock = DockStyle.Fill;
            ChartArea plot = new ChartArea("Equations plot");
            chart.ChartAreas.Add(plot);
            plot.AxisX.Minimum = double.NaN;  
            plot.AxisX.Maximum = double.NaN;
            plot.AxisY.Minimum = double.NaN;
            plot.AxisY.Maximum = double.NaN;

            Series series1 = new Series();
            series1.ChartType = SeriesChartType.Line;
            series1.Color = Color.Blue;
            series1.BorderWidth = 3;

            Series series2 = new Series();
            series2.ChartType = SeriesChartType.Line;
            series2.Color = Color.Red;
            series2.BorderWidth = 3;

            for (double x = -10; x <= 10; x += 0.1)
            {
                double y1 = (Constants[0] - Coefficients[0, 0] * x) / Coefficients[0, 1];
                series1.Points.AddXY(x, y1);

                double y2 = (Constants[1] - Coefficients[1, 0] * x) / Coefficients[1, 1];
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
            Result = result.ToArray();
        }*/
    }
}
