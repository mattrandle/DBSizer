namespace DatabaseSizer
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
            this.btnProduceSpreadsheet = new System.Windows.Forms.Button();
            this.edDatabaseName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edServerName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAuthenticationType = new System.Windows.Forms.ComboBox();
            this.lbUsername = new System.Windows.Forms.Label();
            this.edUsername = new System.Windows.Forms.TextBox();
            this.edPassword = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbHundred = new System.Windows.Forms.RadioButton();
            this.rbAverages = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server name:";
            // 
            // btnProduceSpreadsheet
            // 
            this.btnProduceSpreadsheet.Location = new System.Drawing.Point(163, 263);
            this.btnProduceSpreadsheet.Name = "btnProduceSpreadsheet";
            this.btnProduceSpreadsheet.Size = new System.Drawing.Size(135, 23);
            this.btnProduceSpreadsheet.TabIndex = 9;
            this.btnProduceSpreadsheet.Text = "Produce Spreadsheet";
            this.btnProduceSpreadsheet.UseVisualStyleBackColor = true;
            this.btnProduceSpreadsheet.Click += new System.EventHandler(this.btnProduceSpreadsheet_Click);
            // 
            // edDatabaseName
            // 
            this.edDatabaseName.Location = new System.Drawing.Point(112, 117);
            this.edDatabaseName.Name = "edDatabaseName";
            this.edDatabaseName.Size = new System.Drawing.Size(267, 20);
            this.edDatabaseName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Database name:";
            // 
            // edServerName
            // 
            this.edServerName.Location = new System.Drawing.Point(112, 12);
            this.edServerName.Name = "edServerName";
            this.edServerName.Size = new System.Drawing.Size(267, 20);
            this.edServerName.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Authentication:";
            // 
            // cbAuthenticationType
            // 
            this.cbAuthenticationType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuthenticationType.FormattingEnabled = true;
            this.cbAuthenticationType.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cbAuthenticationType.Location = new System.Drawing.Point(112, 38);
            this.cbAuthenticationType.Name = "cbAuthenticationType";
            this.cbAuthenticationType.Size = new System.Drawing.Size(267, 21);
            this.cbAuthenticationType.TabIndex = 1;
            this.cbAuthenticationType.SelectedIndexChanged += new System.EventHandler(this.cbAuthenticationType_SelectedIndexChanged);
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Enabled = false;
            this.lbUsername.Location = new System.Drawing.Point(55, 68);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(58, 13);
            this.lbUsername.TabIndex = 8;
            this.lbUsername.Text = "Username:";
            // 
            // edUsername
            // 
            this.edUsername.Enabled = false;
            this.edUsername.Location = new System.Drawing.Point(144, 65);
            this.edUsername.Name = "edUsername";
            this.edUsername.Size = new System.Drawing.Size(235, 20);
            this.edUsername.TabIndex = 2;
            // 
            // edPassword
            // 
            this.edPassword.Enabled = false;
            this.edPassword.Location = new System.Drawing.Point(144, 91);
            this.edPassword.Name = "edPassword";
            this.edPassword.PasswordChar = '*';
            this.edPassword.Size = new System.Drawing.Size(235, 20);
            this.edPassword.TabIndex = 3;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Enabled = false;
            this.lbPassword.Location = new System.Drawing.Point(55, 94);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(56, 13);
            this.lbPassword.TabIndex = 10;
            this.lbPassword.Text = "Password:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbHundred);
            this.groupBox1.Controls.Add(this.rbAverages);
            this.groupBox1.Location = new System.Drawing.Point(12, 154);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(367, 91);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Variable column sizes";
            // 
            // rbHundred
            // 
            this.rbHundred.AutoSize = true;
            this.rbHundred.Location = new System.Drawing.Point(24, 51);
            this.rbHundred.Name = "rbHundred";
            this.rbHundred.Size = new System.Drawing.Size(207, 17);
            this.rbHundred.TabIndex = 7;
            this.rbHundred.Text = "100% - can be modified in spreadsheet";
            this.rbHundred.UseVisualStyleBackColor = true;
            // 
            // rbAverages
            // 
            this.rbAverages.AutoSize = true;
            this.rbAverages.Checked = true;
            this.rbAverages.Location = new System.Drawing.Point(24, 28);
            this.rbAverages.Name = "rbAverages";
            this.rbAverages.Size = new System.Drawing.Size(254, 17);
            this.rbAverages.TabIndex = 6;
            this.rbAverages.TabStop = true;
            this.rbAverages.Text = "Averages from databaes - slow on large datasets";
            this.rbAverages.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(304, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 298);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.edPassword);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.edUsername);
            this.Controls.Add(this.lbUsername);
            this.Controls.Add(this.cbAuthenticationType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edServerName);
            this.Controls.Add(this.edDatabaseName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnProduceSpreadsheet);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "SQL Server Database Size Estimator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProduceSpreadsheet;
        private System.Windows.Forms.TextBox edDatabaseName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edServerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbAuthenticationType;
        private System.Windows.Forms.Label lbUsername;
        private System.Windows.Forms.TextBox edUsername;
        private System.Windows.Forms.TextBox edPassword;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbAverages;
        private System.Windows.Forms.RadioButton rbHundred;
        private System.Windows.Forms.Button button1;

    }
}

