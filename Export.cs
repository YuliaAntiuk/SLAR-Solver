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
        public string FormatData(Equation equation)
        {
            StringBuilder sb = new StringBuilder();
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
                sb.Append("]");
                sb.Append("\t\t[");
                sb.Append($"x{i+1}");
                sb.AppendLine("]");
                sb.Append("\t\t[");
                sb.Append(equation.Constants[i]);
                sb.AppendLine("]");
            }
            sb.AppendLine("]");

            // Додаємо розв'язок у форматі [r1, r2, ..., rn]
            sb.Append("Розв'язок: [");
            for (int i = 0; i < equation.Size; i++)
            {
                sb.Append(equation.Constants[i]);
                if (i < equation.Size - 1)
                    sb.Append(", ");
            }
            sb.AppendLine("]");
            return sb.ToString();
        }
        public void ExportToFile(string exportData, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.Write(exportData);
                }
                MessageBox.Show("Дані успішно експортовано до файлу: " + fileName, "Експорт завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка при експорті даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void OpenExportFile(string data)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстові файли (*.txt)|*.txt";
            openFileDialog.Title = "Вибрати файл для експорту";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string existingFileName = openFileDialog.FileName;
                string existingFileContent;
                try
                {
                    using (StreamReader reader = new StreamReader(existingFileName))
                    {
                        existingFileContent = reader.ReadToEnd();
                    }
                    string exportData = existingFileContent + "\n" + data;
                    ExportToFile(exportData, existingFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
