using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Demo
{
    public class Equation
    {
        public double[,] Coefficients { get; set; }
        public double[] Constants { get; set; }
        public double[] CalculateSqrtMethod(double[,] coefficients, double[] constants)
        {
            int n = constants.Length;  // Assuming constants.Length equals the size of the matrix
            double[,] AMatrix = coefficients;
            double[] BMatrix = constants;
            double[,] L = new double[n, n]; // Lower triangular matrix L
            double[] y = new double[n];     // Intermediate result vector y
            double[] x = new double[n];     // Result vector x

            // Perform Cholesky decomposition
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    double sum = 0;

                    if (j == i)
                    {
                        // Diagonal elements
                        for (int k = 0; k < j; k++)
                        {
                            sum += L[j, k] * L[j, k];
                        }
                        L[j, j] = Math.Sqrt(AMatrix[j, j] - sum);
                    }
                    else
                    {
                        // Non-diagonal elements
                        for (int k = 0; k < j; k++)
                        {
                            sum += L[i, k] * L[j, k];
                        }
                        L[i, j] = (AMatrix[i, j] - sum) / L[j, j];
                    }
                }
            }

            // Solve Ly = B using forward substitution
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                {
                    sum += L[i, j] * y[j];
                }
                y[i] = (BMatrix[i] - sum) / L[i, i];
            }

            // Solve L^T x = y using backward substitution
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += L[j, i] * x[j];
                }
                x[i] = (y[i] - sum) / L[i, i];
            }

            return x;
        }
    }
}
