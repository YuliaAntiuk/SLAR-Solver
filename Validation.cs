using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_Demo
{
    public static class Validation
    {
        public static bool IsDimensionEntered(TextBox DimensionInput)
        {
            if (int.TryParse(DimensionInput.Text, out int dimension))
            {
                return dimension > 0 && dimension <= 10;
            }
            return false;
        }
        public static bool IsMethodSelected(ComboBox comboBox)
        {
            return comboBox.SelectedIndex != -1;
        }
        public static bool IsItemInComboBox(object itemToFind, ComboBox comboBox)
        {
            foreach (object item in comboBox.Items)
            {
                if (item.Equals(itemToFind))
                {
                    return true;
                }
            }
            return false;
        }
        public static void TextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string text = textBox.Text;
            if (!Regex.IsMatch(text, @"^[0-9.-]*$"))
            {
                MessageBox.Show("Введено некоретктні символи!", "Помилка вводу", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }
        public static bool IsSolvable(Equation equation)
        {
            return (equation.CalculateDeterminant(equation.Coefficients) != 0);
        }
        public static bool IsSymetrical(Equation equation)
        {
            double[,] transposed = equation.Transpose(equation.Coefficients, equation.Size);
            for (int i = 0; i < equation.Size; i++)
            {
                for (int j = 0; j < equation.Size; j++)
                {
                    if (equation.Coefficients[i, j] != transposed[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
