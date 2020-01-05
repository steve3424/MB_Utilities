namespace MB_Utilities.ui.grandview
{
    partial class LoadDays_GV
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveFileToPathField = new System.Windows.Forms.RichTextBox();
            this.saveFileToBTN = new System.Windows.Forms.Button();
            this.codingLogFilePathField = new System.Windows.Forms.RichTextBox();
            this.chooseCodingLogFileBTN = new System.Windows.Forms.Button();
            this.createListsBTN = new System.Windows.Forms.Button();
            this.processFilesTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // saveFileToPathField
            // 
            this.saveFileToPathField.Location = new System.Drawing.Point(111, 291);
            this.saveFileToPathField.Name = "saveFileToPathField";
            this.saveFileToPathField.ReadOnly = true;
            this.saveFileToPathField.Size = new System.Drawing.Size(592, 31);
            this.saveFileToPathField.TabIndex = 46;
            this.saveFileToPathField.TabStop = false;
            this.saveFileToPathField.Text = "";
            // 
            // saveFileToBTN
            // 
            this.saveFileToBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveFileToBTN.Location = new System.Drawing.Point(308, 245);
            this.saveFileToBTN.Name = "saveFileToBTN";
            this.saveFileToBTN.Size = new System.Drawing.Size(198, 40);
            this.saveFileToBTN.TabIndex = 6;
            this.saveFileToBTN.Text = "Save File To";
            this.saveFileToBTN.UseVisualStyleBackColor = true;
            this.saveFileToBTN.Click += new System.EventHandler(this.saveFileToBTN_Click);
            // 
            // codingLogFilePathField
            // 
            this.codingLogFilePathField.Location = new System.Drawing.Point(111, 162);
            this.codingLogFilePathField.Name = "codingLogFilePathField";
            this.codingLogFilePathField.ReadOnly = true;
            this.codingLogFilePathField.Size = new System.Drawing.Size(592, 31);
            this.codingLogFilePathField.TabIndex = 45;
            this.codingLogFilePathField.TabStop = false;
            this.codingLogFilePathField.Text = "";
            // 
            // chooseCodingLogFileBTN
            // 
            this.chooseCodingLogFileBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseCodingLogFileBTN.Location = new System.Drawing.Point(308, 116);
            this.chooseCodingLogFileBTN.Name = "chooseCodingLogFileBTN";
            this.chooseCodingLogFileBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseCodingLogFileBTN.TabIndex = 5;
            this.chooseCodingLogFileBTN.Text = "Choose Coding Log File";
            this.chooseCodingLogFileBTN.UseVisualStyleBackColor = true;
            this.chooseCodingLogFileBTN.Click += new System.EventHandler(this.chooseCodingLogFileBTN_Click);
            // 
            // createListsBTN
            // 
            this.createListsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createListsBTN.Location = new System.Drawing.Point(308, 420);
            this.createListsBTN.Name = "createListsBTN";
            this.createListsBTN.Size = new System.Drawing.Size(198, 40);
            this.createListsBTN.TabIndex = 7;
            this.createListsBTN.Text = "Create Lists";
            this.createListsBTN.UseVisualStyleBackColor = true;
            this.createListsBTN.Click += new System.EventHandler(this.createListsBTN_Click);
            // 
            // processFilesTitle
            // 
            this.processFilesTitle.AutoSize = true;
            this.processFilesTitle.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processFilesTitle.Location = new System.Drawing.Point(330, 0);
            this.processFilesTitle.Name = "processFilesTitle";
            this.processFilesTitle.Size = new System.Drawing.Size(155, 40);
            this.processFilesTitle.TabIndex = 47;
            this.processFilesTitle.Text = "Load Days";
            // 
            // LoadDays_GV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.processFilesTitle);
            this.Controls.Add(this.saveFileToPathField);
            this.Controls.Add(this.saveFileToBTN);
            this.Controls.Add(this.codingLogFilePathField);
            this.Controls.Add(this.chooseCodingLogFileBTN);
            this.Controls.Add(this.createListsBTN);
            this.Name = "LoadDays_GV";
            this.Size = new System.Drawing.Size(816, 680);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox saveFileToPathField;
        private System.Windows.Forms.Button saveFileToBTN;
        private System.Windows.Forms.RichTextBox codingLogFilePathField;
        private System.Windows.Forms.Button chooseCodingLogFileBTN;
        private System.Windows.Forms.Button createListsBTN;
        private System.Windows.Forms.Label processFilesTitle;
    }
}
