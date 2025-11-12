namespace SASXIIRPL1_AhmadDanuIlhamDin
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary> Clean up resources. </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.comboBoxMataUang = new System.Windows.Forms.ComboBox();
            this.textBoxNominal = new System.Windows.Forms.TextBox();
            this.labelInput = new System.Windows.Forms.Label();
            this.labelOutput = new System.Windows.Forms.Label();
            this.textBoxRupiah = new System.Windows.Forms.TextBox();
            this.buttonKonversi = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(210, 22);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(104, 13);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Konversi Mata Uang";
            // 
            // comboBoxMataUang
            // 
            this.comboBoxMataUang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMataUang.FormattingEnabled = true;
            this.comboBoxMataUang.Location = new System.Drawing.Point(270, 60);
            this.comboBoxMataUang.Name = "comboBoxMataUang";
            this.comboBoxMataUang.Size = new System.Drawing.Size(100, 21);
            this.comboBoxMataUang.TabIndex = 1;
            // 
            // textBoxNominal
            // 
            this.textBoxNominal.Location = new System.Drawing.Point(150, 60);
            this.textBoxNominal.Name = "textBoxNominal";
            this.textBoxNominal.Size = new System.Drawing.Size(100, 20);
            this.textBoxNominal.TabIndex = 2;
            // 
            // labelInput
            // 
            this.labelInput.AutoSize = true;
            this.labelInput.Location = new System.Drawing.Point(120, 40);
            this.labelInput.Name = "labelInput";
            this.labelInput.Size = new System.Drawing.Size(55, 13);
            this.labelInput.TabIndex = 3;
            this.labelInput.Text = "USD/JYN";
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.Location = new System.Drawing.Point(120, 100);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(90, 13);
            this.labelOutput.TabIndex = 4;
            this.labelOutput.Text = "Indonesia Rupiah";
            // 
            // textBoxRupiah
            // 
            this.textBoxRupiah.Location = new System.Drawing.Point(150, 116);
            this.textBoxRupiah.Name = "textBoxRupiah";
            this.textBoxRupiah.ReadOnly = true;
            this.textBoxRupiah.Size = new System.Drawing.Size(220, 20);
            this.textBoxRupiah.TabIndex = 5;
            // 
            // buttonKonversi
            // 
            this.buttonKonversi.Location = new System.Drawing.Point(150, 150);
            this.buttonKonversi.Name = "buttonKonversi";
            this.buttonKonversi.Size = new System.Drawing.Size(75, 23);
            this.buttonKonversi.TabIndex = 6;
            this.buttonKonversi.Text = "Konversi";
            this.buttonKonversi.UseVisualStyleBackColor = true;
            this.buttonKonversi.Click += new System.EventHandler(this.buttonKonversi_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.comboBoxMataUang);
            this.Controls.Add(this.textBoxNominal);
            this.Controls.Add(this.labelInput);
            this.Controls.Add(this.labelOutput);
            this.Controls.Add(this.textBoxRupiah);
            this.Controls.Add(this.buttonKonversi);
            this.Name = "Form1";
            this.Text = "Form Konversi";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ComboBox comboBoxMataUang;
        private System.Windows.Forms.TextBox textBoxNominal;
        private System.Windows.Forms.Label labelInput;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.TextBox textBoxRupiah;
        private System.Windows.Forms.Button buttonKonversi;
    }
}