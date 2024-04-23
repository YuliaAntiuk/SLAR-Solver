using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_Demo
{
    public class Displayer
    {
        private List<TextBox> CoefficientTextBoxes { get; set; }
        private List<TextBox> ConstantTextBoxes { get; set; }
        private Panel EquationsContainer { get; set; }
        public Displayer(Panel equationContainer)
        {
            CoefficientTextBoxes = new List<TextBox>();
            ConstantTextBoxes = new List<TextBox>();
            EquationsContainer = equationContainer;
        }
        private void DisplayEquations(int dimension)
        {
            EquationsContainer.Controls.Clear();
            //SolveBtn.Enabled = false;
            const int textBoxWidth = 50;
            const int textBoxSpacing = 5;
            int yOffset = 30;

            for (int i = 0; i < dimension; i++)
            {
                int x = 0;
                for (int j = 0; j < dimension; j++)
                {
                    TextBox coefficientTextBox = new TextBox();
                    coefficientTextBox.Name = $"textBoxCoeff{i + 1}{j + 1}";
                    coefficientTextBox.Width = textBoxWidth;
                    coefficientTextBox.Location = new Point(x, yOffset * i);
                    coefficientTextBox.TextChanged += TextBox_TextChanged;
                    coefficientTextBox.Validating += textBox_Validating;
                    CoefficientTextBoxes.Add(coefficientTextBox);
                    EquationsContainer.Controls.Add(coefficientTextBox);
                    x = coefficientTextBox.Right + textBoxSpacing;
                }
                Label variableLabel = new Label();
                variableLabel.Text = $"x{i + 1}";
                variableLabel.AutoSize = true;
                variableLabel.Location = new Point(x, yOffset * i);
                EquationsContainer.Controls.Add(variableLabel);
                x = variableLabel.Right + textBoxSpacing;
                Label equalsLabel = new Label();
                equalsLabel.Text = " = ";
                equalsLabel.AutoSize = true;
                equalsLabel.Location = new Point(x, yOffset * i);
                EquationsContainer.Controls.Add(equalsLabel);
                x = equalsLabel.Right + textBoxSpacing;

                TextBox constantTextBox = new TextBox();
                constantTextBox.Name = $"textBoxConstant{i + 1}";
                constantTextBox.Width = textBoxWidth;
                constantTextBox.Location = new Point(x, yOffset * i);
                constantTextBox.TextChanged += TextBox_TextChanged;
                constantTextBox.Validating += textBox_Validating;
                ConstantTextBoxes.Add(constantTextBox);
                EquationsContainer.Controls.Add(constantTextBox);
            }
            EquationsContainer.Height = yOffset * dimension;
        }
        private void UpdateSolveButtonState()
        {
            bool areCoefficientsEntered = true;
            bool areConstantsEntered = true;

            foreach (TextBox textBox in CoefficientTextBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    areCoefficientsEntered = false;
                    break;
                }
            }

            foreach (TextBox textBox in ConstantTextBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    areConstantsEntered = false;
                    break;
                }
            }
            // Оновлення стану кнопки "Розв'язати"
            SolveBtn.Enabled = IsDimensionEntered() && IsMethodSelected() && areCoefficientsEntered && areConstantsEntered;
        }
    }
}
