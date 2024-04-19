using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace GUI_Demo
{
    public partial class Form1 : Form
    {
        private List<TextBox> coefficientTextBoxes = new List<TextBox>();
        private List<TextBox> constantTextBoxes = new List<TextBox>();
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DimensionInput.KeyPress += DimensionInput_KeyPress;
            SolveBtn.Enabled = false;
            this.AutoScroll = true;
        }
        private void DisplayEquations(int dimension)
        {
            EquationsContainer.Controls.Clear();
            SolveBtn.Enabled = false;
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
                    coefficientTextBoxes.Add(coefficientTextBox);
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
                constantTextBoxes.Add(constantTextBox);
                EquationsContainer.Controls.Add(constantTextBox);
            }
            EquationsContainer.Height = yOffset * dimension;
        }
        private Equation ReadEquationsValues(int dimension)
        {
            double[,] coefficients = new double[dimension, dimension];
            double[] constants = new double[dimension];
            for(int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    TextBox coefficientTextBox = (TextBox)EquationsContainer.Controls[$"textBoxCoeff{i + 1}{j + 1}"];
                    if (coefficientTextBox != null && double.TryParse(coefficientTextBox.Text, out double coefficient))
                    {
                        coefficients[i, j] = coefficient;
                    }
                }

                TextBox constantTextBox = (TextBox)EquationsContainer.Controls[$"textBoxConstant{i + 1}"];
                if (constantTextBox != null && double.TryParse(constantTextBox.Text, out double constant))
                {
                    constants[i] = constant;
                }
            }
            Equation equations = new Equation(coefficients, constants, dimension);
            return equations;
        }
        private bool IsDimensionEntered()
        {
            if (int.TryParse(DimensionInput.Text, out int dimension))
            {
                return dimension > 0 && dimension <= 10;
            }
            return false;
        }
        private bool IsMethodSelected()
        {
            return comboBoxMethods.SelectedIndex != -1;
        }
        private void UpdateSolveButtonState()
        {
            bool areCoefficientsEntered = true;
            bool areConstantsEntered = true;

            foreach (TextBox textBox in coefficientTextBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    areCoefficientsEntered = false;
                    break;
                }
            }

            foreach (TextBox textBox in constantTextBoxes)
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
        private bool IsItemInComboBox(object itemToFind, ComboBox comboBox)
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
        private void DimensionInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (int.TryParse(DimensionInput.Text, out int dimension))
                {
                    if (dimension < 2 || dimension > 10)
                    {
                        MessageBox.Show("Розмірність системи повинна бути між 2 та 10", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else
                    {
                        DisplayEquations(dimension);
                    }
                }
                else
                {
                    MessageBox.Show("Будь ласка, введіть коректну розмірність системи.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (dimension == 2)
                {
                    if(!IsItemInComboBox("Графічний метод", comboBoxMethods))
                    {
                        comboBoxMethods.Items.Add("Графічний метод");
                    }
                } else
                {
                    comboBoxMethods.Items.Remove("Графічний метод");
                }
            }
        }
        private void Create_Btn_Clear(int x, int y)
        {
            Button clearBtn = new Button();

            clearBtn.Text = "Очистити";
            clearBtn.Location = new System.Drawing.Point(x, y);
            clearBtn.Size = new System.Drawing.Size(150, 30);
            clearBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular);
            clearBtn.Click += new EventHandler(clearBtn_Click);
            clearBtn.Name = "clearBtn";

            this.Controls.Add(clearBtn);
        }
        private void Create_Btn_ChangeMethod(int x, int y)
        {
            Button clearBtn = new Button();

            clearBtn.Text = "Змінити метод розв'язання";
            clearBtn.Location = new System.Drawing.Point(x, y);
            clearBtn.Size = new System.Drawing.Size(150, 30);
            clearBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular);
            clearBtn.Click += new EventHandler(changeBtn_Click);
            clearBtn.Name = "changeBtn";

            this.Controls.Add(clearBtn);
        }
        private void Create_Btn_Export(int x, int y)
        {
            Button clearBtn = new Button();

            clearBtn.Text = "Експорт";
            clearBtn.Location = new System.Drawing.Point(x, y);
            clearBtn.Size = new System.Drawing.Size(150, 30);
            clearBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular);
            clearBtn.Click += new EventHandler(exportBtn_Click);
            clearBtn.Name = "exportBtn";

            this.Controls.Add(clearBtn);
        }
        private void SolveBtn_Click(object sender, EventArgs e)
        {
            int dimension = Convert.ToInt32(DimensionInput.Text);
            Equation equations = ReadEquationsValues(dimension);
            string selectedMethod = comboBoxMethods.SelectedItem.ToString();
            if (!equations.IsSolvable())
            {
                MessageBox.Show("Система має нуль або безліч розв'язків", "Нульовий визначник", MessageBoxButtons.OK, MessageBoxIcon.Error );
            } else
            {
                switch (selectedMethod)
                {
                    case "Метод квадратного кореня":
                        if (equations.CalculateDeterminant(equations.Coefficients) < 0 || !equations.IsSymetrical())
                        {
                            MessageBox.Show("Матриця коефіцієнтів несиметрична або має від'ємний визначник", "Систему неможливо розв'язати даним методом", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        } else
                        {
                            equations.CalculateSqrtMethod();
                        }
                        break;
                    case "Метод обертання":
                        equations.CalculateRotationMethod();
                        break;
                    case "LUP-метод":
                        equations.CalculateLUPMethod();
                        break;
                    case "Графічний метод":
                        equations.SolveGraphical();
                        break;
                    default:
                        break;
                }
                OutputResults(equations.Result);
            }
        }
        private void OutputResults(double[] result)
        {
            int panelY = EquationsContainer.Bottom + 15;
            ResultPanel resultPanel = new ResultPanel(result);
            resultPanel.Height = 30 * result.Length;
            resultPanel.Name = "resultPanel";
            resultPanel.Location = new System.Drawing.Point(18, panelY);
            this.Controls.Add(resultPanel);
            resultPanel.UpdatePanelContent();
            SolveBtn.Enabled = false;
            int clearBtnY = resultPanel.Bottom + 15;
            DisableInputs();
            Create_Btn_Clear(18, clearBtnY);
            Create_Btn_ChangeMethod(200, clearBtnY);
            Create_Btn_Export(382, clearBtnY);
        }
        private void changeBtn_Click(object sender, EventArgs e)
        {
            comboBoxMethods.Enabled = true;
            Controls.RemoveByKey("resultPanel");
            Controls.RemoveByKey("clearBtn");
            Controls.RemoveByKey("changeBtn");
            Controls.RemoveByKey("exportBtn");
            SolveBtn.Enabled = true;
        }
        private void exportBtn_Click(object sender, EventArgs e)
        {
            int dimension = Convert.ToInt32(DimensionInput.Text);
            Equation equation = ReadEquationsValues(dimension);
            Export export = new Export(equation);
            export.OpenExportFile();
        }
        private void DisableInputs()
        {
            DimensionInput.Enabled = false;
            comboBoxMethods.Enabled = false;
            foreach (Control control in EquationsContainer.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Enabled = false;
                }
            }
        }
        private void EnableInputs()
        {
            DimensionInput.Enabled = true;
            comboBoxMethods.Enabled = true;
            foreach (Control control in EquationsContainer.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Enabled = true;
                }
            }
        }
        private void DimensionInput_TextChanged(object sender, EventArgs e)
        {
            UpdateSolveButtonState(); // Оновити стан кнопки "Розв'язати"
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSolveButtonState(); // Оновити стан кнопки "Розв'язати"
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            Controls.RemoveByKey("resultPanel");  
            EquationsContainer.Controls.Clear();  
            EquationsContainer.Height = 0;
            Controls.RemoveByKey("clearBtn");
            Controls.RemoveByKey("changeBtn");
            Controls.RemoveByKey("exportBtn");
            DimensionInput.Text = "";
            comboBoxMethods.SelectedItem = null;
            comboBoxMethods.Items.Remove("Графічний метод");
            EnableInputs();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSolveButtonState();
        }
        private void textBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string text = textBox.Text;
            if (!Regex.IsMatch(text, @"^[0-9.-]*$"))
            {
                MessageBox.Show("Введено некоретктні символи!", "Помилка вводу", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true; 
            }
        }
    }
}
