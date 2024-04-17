using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_Demo
{
    public class ResultPanel:Panel
    {
        private double[] Results { get; set; }
        public ResultPanel(double[] results) {
            Results = results;
            this.AutoScroll = true;
        }
        public void UpdatePanelContent()
        {
            this.Controls.Clear();

            int labelY = 0;
            for (int i = 0; i < Results.Length; i++)
            {
                Label resultLabel = new Label();
                resultLabel.Text = $"x{i + 1} = {Results[i]}";
                resultLabel.AutoSize = true;
                resultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular);

                resultLabel.Location = new System.Drawing.Point(0, labelY);
                this.Controls.Add(resultLabel);

                labelY += 30;
            }
        }
        public void ClearPanel()
        {
            this.Controls.Clear();
        }
    }
}
