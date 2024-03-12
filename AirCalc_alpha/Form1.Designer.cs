namespace AirCalc_alpha
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.systembox = new System.Windows.Forms.ComboBox();
            this.calc_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выбери систему";
            // 
            // systembox
            // 
            this.systembox.FormattingEnabled = true;
            this.systembox.Location = new System.Drawing.Point(12, 37);
            this.systembox.Name = "systembox";
            this.systembox.Size = new System.Drawing.Size(229, 21);
            this.systembox.TabIndex = 1;
            this.systembox.SelectedIndexChanged += new System.EventHandler(this.systembox_SelectedIndexChanged);
            // 
            // calc_button
            // 
            this.calc_button.Location = new System.Drawing.Point(76, 112);
            this.calc_button.Name = "calc_button";
            this.calc_button.Size = new System.Drawing.Size(103, 30);
            this.calc_button.TabIndex = 2;
            this.calc_button.Text = "Поехали!";
            this.calc_button.UseVisualStyleBackColor = true;
            this.calc_button.Click += new System.EventHandler(this.calc_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(284, 183);
            this.Controls.Add(this.calc_button);
            this.Controls.Add(this.systembox);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox systembox;
        private System.Windows.Forms.Button calc_button;
    }
}