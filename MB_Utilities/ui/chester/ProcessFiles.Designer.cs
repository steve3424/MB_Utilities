namespace MB_Utilities.controls.chester
{
    partial class ProcessFiles
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
            this.folderPathField = new System.Windows.Forms.RichTextBox();
            this.chooseFilesFolderBTN = new System.Windows.Forms.Button();
            this.outputField = new System.Windows.Forms.RichTextBox();
            this.processFilesBTN = new System.Windows.Forms.Button();
            this.Title = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // folderPathField
            // 
            this.folderPathField.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.folderPathField.Location = new System.Drawing.Point(134, 156);
            this.folderPathField.Name = "folderPathField";
            this.folderPathField.ReadOnly = true;
            this.folderPathField.Size = new System.Drawing.Size(544, 31);
            this.folderPathField.TabIndex = 16;
            this.folderPathField.TabStop = false;
            this.folderPathField.Text = "";
            // 
            // chooseFilesFolderBTN
            // 
            this.chooseFilesFolderBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseFilesFolderBTN.Location = new System.Drawing.Point(310, 110);
            this.chooseFilesFolderBTN.Name = "chooseFilesFolderBTN";
            this.chooseFilesFolderBTN.Size = new System.Drawing.Size(193, 40);
            this.chooseFilesFolderBTN.TabIndex = 6;
            this.chooseFilesFolderBTN.Text = "Choose File Folder";
            this.chooseFilesFolderBTN.UseVisualStyleBackColor = true;
            this.chooseFilesFolderBTN.Click += new System.EventHandler(this.chooseFilesFolderBTN_Click);
            // 
            // outputField
            // 
            this.outputField.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputField.Location = new System.Drawing.Point(134, 315);
            this.outputField.Name = "outputField";
            this.outputField.ReadOnly = true;
            this.outputField.Size = new System.Drawing.Size(544, 139);
            this.outputField.TabIndex = 20;
            this.outputField.TabStop = false;
            this.outputField.Text = "";
            // 
            // processFilesBTN
            // 
            this.processFilesBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processFilesBTN.Location = new System.Drawing.Point(320, 269);
            this.processFilesBTN.Name = "processFilesBTN";
            this.processFilesBTN.Size = new System.Drawing.Size(172, 40);
            this.processFilesBTN.TabIndex = 7;
            this.processFilesBTN.Text = "Process Files";
            this.processFilesBTN.UseVisualStyleBackColor = true;
            this.processFilesBTN.Click += new System.EventHandler(this.processFilesBTN_Click);
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(311, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(190, 40);
            this.Title.TabIndex = 21;
            this.Title.Text = "Process Files";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // ProcessFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Title);
            this.Controls.Add(this.outputField);
            this.Controls.Add(this.processFilesBTN);
            this.Controls.Add(this.folderPathField);
            this.Controls.Add(this.chooseFilesFolderBTN);
            this.Name = "ProcessFiles";
            this.Size = new System.Drawing.Size(816, 680);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox folderPathField;
        private System.Windows.Forms.Button chooseFilesFolderBTN;
        private System.Windows.Forms.RichTextBox outputField;
        private System.Windows.Forms.Button processFilesBTN;
        private System.Windows.Forms.Label Title;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
