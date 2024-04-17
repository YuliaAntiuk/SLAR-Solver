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
using System.Windows.Forms.DataVisualization.Charting;

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
            // Очищаємо контейнер перед додаванням нових елементів
            EquationsContainer.Controls.Clear();
            const int textBoxWidth = 50;
            const int textBoxSpacing = 5;
            int yOffset = 30;

            for (int i = 0; i < dimension; i++)
            {
                int x = 0;
                for (int j = 0; j < dimension; j++)
                {
                    // Текстове поле для коефіцієнта невідомої
                    TextBox coefficientTextBox = new TextBox();
                    coefficientTextBox.Name = $"textBoxCoeff{i + 1}{j + 1}";
                    coefficientTextBox.Width = textBoxWidth;
                    coefficientTextBox.Location = new Point(x, yOffset * i);
                    coefficientTextBox.TextChanged += TextBox_TextChanged;
                    coefficientTextBoxes.Add(coefficientTextBox);
                    EquationsContainer.Controls.Add(coefficientTextBox);
                    x = coefficientTextBox.Right + textBoxSpacing;
                }
                // Назва невідомої (наприклад, x1, x2, ..., xn)
                Label variableLabel = new Label();
                variableLabel.Text = $"x{i + 1}";
                variableLabel.AutoSize = true;
                variableLabel.Location = new Point(x, yOffset * i);
                EquationsContainer.Controls.Add(variableLabel);
                x = variableLabel.Right + textBoxSpacing;
                // Додавання знака "=" до контейнера
                Label equalsLabel = new Label();
                equalsLabel.Text = " = ";
                equalsLabel.AutoSize = true;
                equalsLabel.Location = new Point(x, yOffset * i);
                EquationsContainer.Controls.Add(equalsLabel);
                x = equalsLabel.Right + textBoxSpacing;

                // Додавання текстового поля для введення вільного члена рівняння
                TextBox constantTextBox = new TextBox();
                constantTextBox.Name = $"textBoxConstant{i + 1}";
                constantTextBox.Width = textBoxWidth;
                constantTextBox.Location = new Point(x, yOffset * i);
                constantTextBox.TextChanged += TextBox_TextChanged;
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
            int dimension;
            if (int.TryParse(DimensionInput.Text, out dimension))
            {
                // Перевірити, чи розмірність є позитивним цілим числом
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
            bool isDimensionEntered = IsDimensionEntered();
            bool isMethodSelected = IsMethodSelected();
            bool areCoefficientsEntered = true;
            bool areConstantsEntered = true;

            // Перевірка текстових полів для коефіцієнтів
            foreach (TextBox textBox in coefficientTextBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    areCoefficientsEntered = false;
                    break;
                }
            }
            // Перевірка текстових полів для вільних членів
            foreach (TextBox textBox in constantTextBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    areConstantsEntered = false;
                    break;
                }
            }
            // Оновлення стану кнопки "Розв'язати"
            SolveBtn.Enabled = isDimensionEntered && isMethodSelected && areCoefficientsEntered && areConstantsEntered;
        }
        private void DimensionInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо чи натиснута клавіша Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Отримуємо розмірність системи з textBoxDimension
                if (int.TryParse(DimensionInput.Text, out int dimension))
                {
                    if (dimension < 2 || dimension > 10)
                    {
                        MessageBox.Show("Розмірність системи повинна бути між 2 та 10", "Помилка введення", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else
                    {
                        // Відображаємо рівняння з відповідною кількістю невідомих
                        DisplayEquations(dimension);
                    }
                }
                else
                {
                    MessageBox.Show("Будь ласка, введіть коректну розмірність системи.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (dimension == 2)
                {
                    comboBoxMethods.Items.Add("Графічний метод");
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
        private void SolveBtn_Click(object sender, EventArgs e)
        {
            int dimension = Convert.ToInt32(DimensionInput.Text);
            Equation equations = ReadEquationsValues(dimension);
            string selectedMethod = comboBoxMethods.SelectedItem.ToString();
            double[] result = new double[dimension];
            switch (selectedMethod)
            {
                case "Метод квадратного кореня":
                    result = equations.CalculateSqrtMethod();
                    break;
                case "Метод обертання":
                    result = equations.CalculateRotationMethod();
                    break;
                case "LUP-метод":
                    result = equations.CalculateLUPMethod();
                    break;
                case "Графічний метод":
                    result = equations.SolveGraphical();
                    break;
                default:
                    break;
            }
            OutputResults(result);
        }
        private void OutputResults(double[] result)
        {
            int panelY = EquationsContainer.Bottom + 15;
            Panel resultPanel = new Panel();
            resultPanel.Height = 30 * result.Length;
            resultPanel.Name = "resultPanel";
            resultPanel.Location = new System.Drawing.Point(18, panelY);
            this.Controls.Add(resultPanel);
            int labelY = 0;
            for (int i = 0; i < result.Length; i++)
            {
                Label resultLabel = new Label();
                resultLabel.Text = $"x{i + 1} = {result[i]}";
                resultLabel.AutoSize = true;
                resultLabel.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular);

                resultLabel.Location = new Point(0, labelY);
                resultPanel.Controls.Add(resultLabel); 

                labelY += 30; 
            }
            SolveBtn.Enabled = false;
            int clearBtnY = resultPanel.Bottom + 15;
            DisableInputs();
            Create_Btn_Clear(18, clearBtnY);
            Create_Btn_ChangeMethod(200, clearBtnY);
        }
        private void changeBtn_Click(object sender, EventArgs e)
        {
            comboBoxMethods.Enabled = true;
            Controls.RemoveByKey("resultPanel");
            Controls.RemoveByKey("clearBtn");
            Controls.RemoveByKey("changeBtn");
            SolveBtn.Enabled = true;
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
        private void TextBox_Validating(object sender, CancelEventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            Controls.RemoveByKey("resultPanel");  
            EquationsContainer.Controls.Clear();  
            EquationsContainer.Height = 0;
            Controls.RemoveByKey("clearBtn");
            Controls.RemoveByKey("changeBtn");
            DimensionInput.Text = "";
            comboBoxMethods.SelectedItem = null;
            comboBoxMethods.Items.Remove("Графічний метод");
            EnableInputs();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSolveButtonState();
        }
        private void label3_Click_1(object sender, EventArgs e)
        {

        }
    }
}
