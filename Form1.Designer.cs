﻿namespace GUI_Demo
{
    partial class InterfaceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SolveBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DimensionInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMethods = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EquationsContainer = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SolveBtn
            // 
            this.SolveBtn.BackColor = System.Drawing.SystemColors.HighlightText;
            this.SolveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.SolveBtn.Location = new System.Drawing.Point(779, 11);
            this.SolveBtn.Name = "SolveBtn";
            this.SolveBtn.Size = new System.Drawing.Size(163, 34);
            this.SolveBtn.TabIndex = 0;
            this.SolveBtn.Text = "Розв\'язати";
            this.SolveBtn.UseVisualStyleBackColor = false;
            this.SolveBtn.Click += new System.EventHandler(this.SolveBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(0, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Вкажіть кількість рівнянь в системі";
            // 
            // DimensionInput
            // 
            this.DimensionInput.Location = new System.Drawing.Point(395, 19);
            this.DimensionInput.Name = "DimensionInput";
            this.DimensionInput.Size = new System.Drawing.Size(100, 22);
            this.DimensionInput.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(0, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(230, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Заповніть коефіцієнти:";
            // 
            // comboBoxMethods
            // 
            this.comboBoxMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMethods.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxMethods.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.comboBoxMethods.FormattingEnabled = true;
            this.comboBoxMethods.Items.AddRange(new object[] {
            "LUP-метод",
            "Метод обертання",
            "Метод квадратного кореня"});
            this.comboBoxMethods.Location = new System.Drawing.Point(302, 52);
            this.comboBoxMethods.Name = "comboBoxMethods";
            this.comboBoxMethods.Size = new System.Drawing.Size(268, 28);
            this.comboBoxMethods.TabIndex = 16;
            this.comboBoxMethods.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(0, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(271, 25);
            this.label3.TabIndex = 18;
            this.label3.Text = "Оберіть метод розв\'язання";
            // 
            // EquationsContainer
            // 
            this.EquationsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EquationsContainer.AutoScroll = true;
            this.EquationsContainer.Location = new System.Drawing.Point(18, 149);
            this.EquationsContainer.Name = "EquationsContainer";
            this.EquationsContainer.Size = new System.Drawing.Size(943, 368);
            this.EquationsContainer.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.SolveBtn);
            this.panel1.Controls.Add(this.comboBoxMethods);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DimensionInput);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(18, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(943, 127);
            this.panel1.TabIndex = 19;
            // 
            // InterfaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(982, 753);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.EquationsContainer);
            this.Name = "InterfaceForm";
            this.Text = "Розв\'язання СЛАР точними методами";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SolveBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DimensionInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMethods;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel EquationsContainer;
        private System.Windows.Forms.Panel panel1;
    }
}

