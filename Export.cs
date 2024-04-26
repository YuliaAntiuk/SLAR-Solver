using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_Demo
{
    public class Export
    {
        private string Data { get; set; }
        public Export (Equation equation)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Матриця коефіцієнтів:");
            sb.AppendLine("[");
            for (int i = 0; i < equation.Size; i++)
            {
                sb.Append("[");
                for (int j = 0; j < equation.Size; j++)
                {
                    sb.Append(equation.Coefficients[i, j]);
                    if (j < equation.Size - 1)
                        sb.Append(", ");
                }
                sb.AppendLine("]");
            }
            sb.AppendLine("]\nВектор вільних членів:");
            sb.Append("[");
            for (int i = 0; i < equation.Size; i++)
            {
                sb.Append(equation.Constants[i]);
                if (i < equation.Size - 1)
                    sb.Append(", ");
            }
            sb.AppendLine("]");
            sb.Append("Розв'язок: [");
            for (int i = 0; i < equation.Size; i++)
            {
                sb.Append(equation.Result[i]);
                if (i < equation.Size - 1)
                    sb.Append(", ");
            }
            sb.AppendLine("]");
            Data = sb.ToString();
        }
        private void ExportToFile(string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(Data);
                }
                MessageBox.Show("Дані успішно експортовано до файлу: " + fileName, "Експорт завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка при експорті даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void OpenExportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстові файли (*.txt)|*.txt";
            openFileDialog.Title = "Вибрати файл для експорту";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                string existingFileContent;
                try
                {
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        existingFileContent = reader.ReadToEnd();
                    }
                    Data = existingFileContent + "\n\n" + Data;
                    ExportToFile(fileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
