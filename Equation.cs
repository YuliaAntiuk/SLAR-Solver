﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GUI_Demo
{
    public class Equation
    {
        public double[,] Coefficients { get; set; }
        public double[] Constants { get; set; }
        private double[,] Transpose(double[,] matrix, int n)
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
        public double[] CalculateSqrtMethod(double[,] AMatrix, int n, double[] BMatrix)
        {
            double[,] S = new double[n, n];
            double[] y = new double[n];
            double[] x = new double[n];

            // Calculate S matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        double sum = 0;
                        for (int k = 0; k < i; k++)
                        {
                            sum += S[i, k] * S[i, k];
                        }
                        S[i, i] = Math.Sqrt(AMatrix[i, i] - sum);
                    }
                    else if (i > j)
                    {
                        double sum = 0;
                        for (int k = 0; k < j; k++)
                        {
                            sum += S[i, k] * S[j, k];
                        }
                        S[i, j] = (AMatrix[i, j] - sum) / S[j, j];
                    }
                    else // i < j
                    {
                        S[i, j] = 0;
                    }
                }
            }

            // Calculate transpose of S matrix
            double[,] St = Transpose(S, n);

            // Solve Ly = B using forward substitution
            y[0] = BMatrix[0] / S[0, 0];
            for (int i = 1; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < i; j++)
                {
                    sum += S[i, j] * y[j];
                }
                y[i] = (BMatrix[i] - sum) / S[i, i];
            }

            // Solve Ux = y using backward substitution
            x[n - 1] = y[n - 1] / S[n - 1, n - 1];
            for (int i = n - 2; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += St[i, j] * x[j];
                }
                x[i] = (y[i] - sum) / S[i, i];
            }

            return x;
        }
        public double[] CalculateRotationMethod(double[,] AMatrix, int n, double[] BMatrix)
        {
            double[] x = new double[n];

            for (int i = 0; i < n - 1; i++)
            {
                for (int k = i + 1; k < n; k++)
                {
                    double r = Math.Sqrt(AMatrix[i, i] * AMatrix[i, i] + AMatrix[k, i] * AMatrix[k, i]);
                    double c = AMatrix[i, i] / r;
                    double s = -AMatrix[k, i] / r;

                    for (int j = 0; j < n; j++)
                    {
                        double tempA1 = AMatrix[i, j];
                        double tempA2 = AMatrix[k, j];
                        AMatrix[i, j] = c * tempA1 - s * tempA2;
                        AMatrix[k, j] = s * tempA1 + c * tempA2;
                    }

                    double tempB1 = BMatrix[i];
                    double tempB2 = BMatrix[k];
                    BMatrix[i] = c * tempB1 - s * tempB2;
                    BMatrix[k] = s * tempB1 + c * tempB2;
                }
            }

            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += AMatrix[i, j] * x[j];
                }
                x[i] = (BMatrix[i] - sum) / AMatrix[i, i];
            }

            return x;
        }
    }
}
