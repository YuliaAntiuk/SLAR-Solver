using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI_Demo
{
    public class ControlBtnPanel:Panel
    {
        private List<Button> buttons = new List<Button>();
        private ComboBox comboBoxMethods;
        private Button SolveBtn;
        private Equation equation;
        private Panel EquationsContainer;
        private TextBox DimensionInput;
        private Form form;
        public ControlBtnPanel(Form form, Equation equation)
        {
            this.form = form;
            comboBoxMethods = form.Controls.Find("comboBoxMethods", true).FirstOrDefault() as ComboBox;
            SolveBtn = form.Controls.Find("SolveBtn", true).FirstOrDefault() as Button;
            EquationsContainer = form.Controls.Find("EquationsContainer", true).FirstOrDefault() as Panel;
            DimensionInput = form.Controls.Find("DimensionInput", true).FirstOrDefault() as TextBox;
            this.equation = equation;
        }
        private void CreateBtn(int x, int y, string btnText, string btnName, EventHandler eventHandler) {
            Button btn = new Button();
            btn.Text = btnText;
            btn.Location = new Point(x, y);
            btn.Size = new System.Drawing.Size(150, 30);
            btn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular);
            btn.Click += new EventHandler(eventHandler);
            btn.Name = btnName;
            buttons.Add(btn);
        }
        public void CreateControlButtons(int x, int y)
        {
            string[] textxsForBtn = { "Очистити", "Змінити метод", "Експорт", "Складність" };
            string[] btnNames = { "clearBtn", "changeBtn", "exportBtn", "complexityBtn" };
            EventHandler[] eventHandlers = { ClearBtn_Click, ChangeBtn_Click, ExportBtn_Click, ComplexityBtn_Click };
            for(int i = 0; i < textxsForBtn.Length; i++)
            {
                CreateBtn(x, y, textxsForBtn[i], btnNames[i], eventHandlers[i]);
                x = buttons[i].Right + 50;
            }
            foreach (Button button in buttons)
            {
                this.Controls.Add(button);
            }
        }
        private void ChangeBtn_Click(object sender, EventArgs e)
        {
            comboBoxMethods.Enabled = true;
            Controls.RemoveByKey("resultPanel");
            Controls.RemoveByKey("clearBtn");
            Controls.RemoveByKey("changeBtn");
            Controls.RemoveByKey("exportBtn");
            SolveBtn.Enabled = true;
        }
        private void ExportBtn_Click(object sender, EventArgs e)
        {
            Export export = new Export(equation);
            export.OpenExportFile();
        }
        private void ClearBtn_Click(object sender, EventArgs e)
        {
            form.Controls.RemoveByKey("resultPanel");
            EquationsContainer.Controls.Clear();
            EquationsContainer.Height = 0;
            Controls.RemoveByKey("clearBtn");
            Controls.RemoveByKey("changeBtn");
            Controls.RemoveByKey("exportBtn");
            DimensionInput.Text = "";
            comboBoxMethods.Items.Remove("Графічний метод");
            comboBoxMethods.SelectedItem = null;
            EnableInputs();
        }
        private void ComplexityBtn_Click(object sender, EventArgs e)
        {

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
    }
}
