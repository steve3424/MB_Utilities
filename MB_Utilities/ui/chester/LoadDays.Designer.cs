namespace MB_Utilities.controls.chester
{
    partial class LoadDays
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
            this.processFilesTitle = new System.Windows.Forms.Label();
            this.saveFileToPath = new System.Windows.Forms.RichTextBox();
            this.saveFileToBTN = new System.Windows.Forms.Button();
            this.codingLogFilePath = new System.Windows.Forms.RichTextBox();
            this.chooseCodingLogFileBTN = new System.Windows.Forms.Button();
            this.createListsBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // processFilesTitle
            // 
            this.processFilesTitle.AutoSize = true;
            this.processFilesTitle.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processFilesTitle.Location = new System.Drawing.Point(330, 0);
            this.processFilesTitle.Name = "processFilesTitle";
            this.processFilesTitle.Size = new System.Drawing.Size(155, 40);
            this.processFilesTitle.TabIndex = 22;
            this.processFilesTitle.Text = "Load Days";
            // 
            // saveFileToPath
            // 
            this.saveFileToPath.Location = new System.Drawing.Point(111, 291);
            this.saveFileToPath.Name = "saveFileToPath";
            this.saveFileToPath.ReadOnly = true;
            this.saveFileToPath.Size = new System.Drawing.Size(592, 31);
            this.saveFileToPath.TabIndex = 41;
            this.saveFileToPath.TabStop = false;
            this.saveFileToPath.Text = "";
            // 
            // saveFileToBTN
            // 
            this.saveFileToBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveFileToBTN.Location = new System.Drawing.Point(308, 245);
            this.saveFileToBTN.Name = "saveFileToBTN";
            this.saveFileToBTN.Size = new System.Drawing.Size(198, 40);
            this.saveFileToBTN.TabIndex = 7;
            this.saveFileToBTN.Text = "Save File To";
            this.saveFileToBTN.UseVisualStyleBackColor = true;
            // 
            // codingLogFilePath
            // 
            this.codingLogFilePath.Location = new System.Drawing.Point(111, 162);
            this.codingLogFilePath.Name = "codingLogFilePath";
            this.codingLogFilePath.ReadOnly = true;
            this.codingLogFilePath.Size = new System.Drawing.Size(592, 31);
            this.codingLogFilePath.TabIndex = 40;
            this.codingLogFilePath.TabStop = false;
            this.codingLogFilePath.Text = "";
            // 
            // chooseCodingLogFileBTN
            // 
            this.chooseCodingLogFileBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseCodingLogFileBTN.Location = new System.Drawing.Point(308, 116);
            this.chooseCodingLogFileBTN.Name = "chooseCodingLogFileBTN";
            this.chooseCodingLogFileBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseCodingLogFileBTN.TabIndex = 6;
            this.chooseCodingLogFileBTN.Text = "Choose Coding Log File";
            this.chooseCodingLogFileBTN.UseVisualStyleBackColor = true;
            // 
            // createListsBTN
            // 
            this.createListsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createListsBTN.Location = new System.Drawing.Point(308, 420);
            this.createListsBTN.Name = "createListsBTN";
            this.createListsBTN.Size = new System.Drawing.Size(198, 40);
            this.createListsBTN.TabIndex = 8;
            this.createListsBTN.Text = "Create Lists";
            this.createListsBTN.UseVisualStyleBackColor = true;
            // 
            // LoadDays
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveFileToPath);
            this.Controls.Add(this.saveFileToBTN);
            this.Controls.Add(this.codingLogFilePath);
            this.Controls.Add(this.chooseCodingLogFileBTN);
            this.Controls.Add(this.createListsBTN);
            this.Controls.Add(this.processFilesTitle);
            this.Name = "LoadDays";
            this.Size = new System.Drawing.Size(816, 680);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label processFilesTitle;
        private System.Windows.Forms.RichTextBox saveFileToPath;
        private System.Windows.Forms.Button saveFileToBTN;
        private System.Windows.Forms.RichTextBox codingLogFilePath;
        private System.Windows.Forms.Button chooseCodingLogFileBTN;
        private System.Windows.Forms.Button createListsBTN;
    }
}
