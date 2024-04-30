using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI_Demo
{
    public class Equation:GraphicSolver
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
                        if (Math.Abs(S[j, j]) < double.Epsilon)
                        {
                            throw new InvalidOperationException("Ділення на число, близьке за модулем до 0.");
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
            double[,] A = new double[Size, Size];
            double[] B = new double[Size];
            Array.Copy(Coefficients, A, Coefficients.Length);
            Array.Copy(Constants, B, Constants.Length);
            for (int i = 0; i < Size - 1; i++)
            {
                for (int k = i + 1; k < Size; k++)
                {
                    double r = Math.Sqrt(A[i, i] * A[i, i] + A[k, i] * A[k, i]);
                    if(Math.Abs(r) < double.Epsilon)
                    {
                        throw new InvalidOperationException("Ділення на число, близьке за модулем до 0.");
                    }
                    double c = A[i, i] / r;
                    double s = -A[k, i] / r;

                    for (int j = 0; j < Size; j++)
                    {
                        double tempA1 = A[i, j];
                        double tempA2 = A[k, j];
                        A[i, j] = c * tempA1 - s * tempA2;
                        A[k, j] = s * tempA1 + c * tempA2;
                    }

                    double tempB1 = B[i];
                    double tempB2 = B[k];
                    B[i] = c * tempB1 - s * tempB2;
                    B[k] = s * tempB1 + c * tempB2;
                }
            }

            for (int i = Size - 1; i >= 0; i--)
            {
                if (Math.Abs(A[i,i]) < double.Epsilon)
                {
                    throw new InvalidOperationException("Ділення на число, близьке за модулем до 0.");
                }
                double sum = 0;
                for (int j = i + 1; j < Size; j++)
                {
                    sum += A[i, j] * Result[j];
                }
                Result[i] = (B[i] - sum) / A[i, i];
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
                    if (Math.Abs(U[k,k]) < double.Epsilon)
                    {
                        throw new InvalidOperationException("Ділення на число, близьке за модулем до 0.");
                    }
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
        public double FindMaximum()
        {
            int arrSize = Size * 3;
            double[] arr = new double[arrSize];
            int k = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    arr[k] = Coefficients[i, j];
                    k++;
                }
                arr[k] = Constants[i];
                k++;
            }
            return arr.Max();
        }
        public void CreateGraphic(Form graphicalForm)
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
            double startX = (max > 0) ? (-max) : max;
            double endX = Math.Abs(max);
            for (double x = startX; x <= endX; x += 0.1)
            {
                double y1 = (Constants[0] - Coefficients[0, 0] * x) / Coefficients[0, 1];
                series1.Points.AddXY(x, y1);

                double y2 = (Constants[1] - Coefficients[1, 0] * x) / Coefficients[1, 1];
                series2.Points.AddXY(x, y2);
                CalculateIntersectionPoints();
            }

            chart.Series.Add(series1);
            chart.Series.Add(series2);

            graphicalForm.Show();
            graphicalForm.FormClosed += (sender, e) =>
            {
                graphicalForm.Dispose();
            };
        }
        public Series CreateSeries(int index)
        {
            Series series = new Series();
            series.ChartType = SeriesChartType.Line;
            series.Color = (index == 0) ? Color.Blue : Color.Red;
            series.BorderWidth = 3;
            return series;
        }
        public void CalculateIntersectionPoints()
        {
            Result[0] = (Coefficients[1, 1] * Constants[0] - Coefficients[0, 1] * Constants[1]) / CalculateDeterminant(Coefficients);
            Result[1] = (Coefficients[0, 0] * Constants[1] - Coefficients[1, 0] * Constants[0]) / CalculateDeterminant(Coefficients);
        }

    }
}
