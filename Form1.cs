using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI_Demo
{
    public partial class InterfaceForm : Form
    {
        private readonly List<TextBox> coefficientTextBoxes = new List<TextBox>();
        private readonly List<TextBox> constantTextBoxes = new List<TextBox>();
        private Equation equation;
        private List<Button> controlbuttons = new List<Button>();
        private Form graphicalForm = new Form();
        public InterfaceForm()
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
            coefficientTextBoxes.Clear();
            constantTextBoxes.Clear();
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
                    coefficientTextBox.Validating += Validation.TextBox_Validating;
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
                constantTextBox.Validating += Validation.TextBox_Validating;
                constantTextBoxes.Add(constantTextBox);
                EquationsContainer.Controls.Add(constantTextBox);
            }
            EquationsContainer.Height = yOffset * dimension;
        }
        private void ReadEquationsValues(int dimension)
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
            this.equation = new Equation(coefficients, constants, dimension);
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
            SolveBtn.Enabled = Validation.IsDimensionEntered(DimensionInput) && Validation.IsMethodSelected(comboBoxMethods) && areCoefficientsEntered && areConstantsEntered;
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
                    if(!Validation.IsItemInComboBox("Графічний метод", comboBoxMethods))
                    {
                        comboBoxMethods.Items.Add("Графічний метод");
                    }
                } else
                {
                    comboBoxMethods.Items.Remove("Графічний метод");
                }
            }
        }
        private void CreateBtn(int x, int y, string btnText, string btnName, EventHandler eventHandler)
        {
            Button btn = new Button();
            btn.Text = btnText;
            btn.Location = new Point(x, y);
            btn.Size = new System.Drawing.Size(150, 30);
            btn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular);
            btn.Click += new EventHandler(eventHandler);
            btn.Name = btnName;
            controlbuttons.Add(btn);
        }
        private void CreateControlButtons(int x, int y)
        {
            string[] textxsForBtn = { "Очистити", "Змінити метод", "Експорт", "Складність" };
            string[] btnNames = { "clearBtn", "changeBtn", "exportBtn", "complexityBtn" };
            EventHandler[] eventHandlers = { ClearBtn_Click, ChangeBtn_Click, ExportBtn_Click, ComplexityBtn_Click };
            int buttonNumber = 0;
            if (comboBoxMethods.SelectedItem == "Графічний метод") {
                buttonNumber = btnNames.Length - 1;
            } else
            {
                buttonNumber = btnNames.Length;
            }
            for (int i = 0; i < buttonNumber; i++)
            {
                CreateBtn(x, y, textxsForBtn[i], btnNames[i], eventHandlers[i]);
                x = controlbuttons[i].Right + 30;
            }
            foreach (Button button in controlbuttons)
            {
                this.Controls.Add(button);
            }
        }
        private void SolveBtn_Click(object sender, EventArgs e)
        {
            int dimension = Convert.ToInt32(DimensionInput.Text);
            ReadEquationsValues(dimension);
            string selectedMethod = comboBoxMethods.SelectedItem.ToString();
            if (!Validation.IsEquationValid(equation))
            {
                MessageBox.Show("Коефіцієнти виходять за межі обмежень", "Невалідні коефіцієнти", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if (!Validation.IsSolvable(equation))
            {
                MessageBox.Show("Система має нуль або безліч розв'язків", "Нульовий визначник", MessageBoxButtons.OK, MessageBoxIcon.Error );
            } else
            {
                try
                {
                    switch (selectedMethod)
                    {
                        case "Метод квадратного кореня":
                            if (equation.CalculateDeterminant(equation.Coefficients) < 0 || !Validation.IsSymetrical(equation))
                            {
                                MessageBox.Show("Матриця коефіцієнтів несиметрична або має від'ємний визначник", "Систему неможливо розв'язати даним методом", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                equation.CalculateSqrtMethod();
                            }
                            break;
                        case "Метод обертання":
                            equation.CalculateRotationMethod();
                            break;
                        case "LUP-метод":
                            equation.CalculateLUPMethod();
                            break;
                        case "Графічний метод":
                            if (graphicalForm == null || graphicalForm.IsDisposed)
                            {
                                graphicalForm = new Form();
                            }
                            equation.CreateGraphic(graphicalForm);
                            break;
                        default:
                            break;
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Помилка в обчисленнях", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                OutputResults(equation.Result);
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
            int controlPanelY = resultPanel.Bottom + 15;
            DisableInputs();
            CreateControlButtons(18, controlPanelY);
        }
        private void RemoveBtns()
        {
            graphicalForm.Dispose();
            Controls.RemoveByKey("resultPanel");
            Controls.RemoveByKey("clearBtn");
            Controls.RemoveByKey("changeBtn");
            Controls.RemoveByKey("exportBtn");
            Controls.RemoveByKey("complexityBtn");
            Controls.RemoveByKey("complexityLabel");
            controlbuttons.Clear();
        }
        private void ChangeBtn_Click(object sender, EventArgs e)
        {
            RemoveBtns();
            comboBoxMethods.Enabled = true;
            SolveBtn.Enabled = true;
        }
        private void ExportBtn_Click(object sender, EventArgs e)
        {
            Export export = new Export(equation);
            export.OpenExportFile();
        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            EquationsContainer.Controls.Clear();  
            EquationsContainer.Height = 0;
            RemoveBtns();
            DimensionInput.Text = "";
            comboBoxMethods.SelectedItem = null;
            comboBoxMethods.Items.Remove("Графічний метод");
            EnableInputs();
        }
        private void ComplexityBtn_Click(object sender, EventArgs e)
        {
            Label complexityLabel = new Label();
            complexityLabel.Text = $"Практична складність - {equation.IterationCounter}";
            complexityLabel.AutoSize = true;
            complexityLabel.Name = "complexityLabel";
            complexityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular);
            complexityLabel.Location = new System.Drawing.Point(18, controlbuttons[0].Bottom + 15);
            this.Controls.Add(complexityLabel);
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
        public void TextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSolveButtonState(); // Оновити стан кнопки "Розв'язати"
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSolveButtonState();
        }
    }
}
