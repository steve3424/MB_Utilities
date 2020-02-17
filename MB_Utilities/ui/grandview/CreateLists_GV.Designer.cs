namespace MB_Utilities.ui.grandview
{
    partial class CreateLists_GV
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
            this.createListsTitle = new System.Windows.Forms.Label();
            this.missingListPathField = new System.Windows.Forms.RichTextBox();
            this.chooseMissingListBTN = new System.Windows.Forms.Button();
            this.logFilePathField = new System.Windows.Forms.RichTextBox();
            this.chooseLogFileBTN = new System.Windows.Forms.Button();
            this.createListsBTN = new System.Windows.Forms.Button();
            this.deleteRowsCheckBox = new System.Windows.Forms.CheckBox();
            this.stragglersTotalLabel = new System.Windows.Forms.Label();
            this.voidedTotalLabel = new System.Windows.Forms.Label();
            this.missingTotalLabel = new System.Windows.Forms.Label();
            this.saveFileToBTN = new System.Windows.Forms.Button();
            this.saveFileToPathField = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // createListsTitle
            // 
            this.createListsTitle.AutoSize = true;
            this.createListsTitle.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createListsTitle.Location = new System.Drawing.Point(321, 0);
            this.createListsTitle.Name = "createListsTitle";
            this.createListsTitle.Size = new System.Drawing.Size(173, 40);
            this.createListsTitle.TabIndex = 24;
            this.createListsTitle.Text = "Create Lists";
            // 
            // missingListPathField
            // 
            this.missingListPathField.Location = new System.Drawing.Point(111, 115);
            this.missingListPathField.Name = "missingListPathField";
            this.missingListPathField.ReadOnly = true;
            this.missingListPathField.Size = new System.Drawing.Size(592, 31);
            this.missingListPathField.TabIndex = 39;
            this.missingListPathField.TabStop = false;
            this.missingListPathField.Text = "";
            // 
            // chooseMissingListBTN
            // 
            this.chooseMissingListBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseMissingListBTN.Location = new System.Drawing.Point(308, 69);
            this.chooseMissingListBTN.Name = "chooseMissingListBTN";
            this.chooseMissingListBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseMissingListBTN.TabIndex = 38;
            this.chooseMissingListBTN.Text = "Choose Missing List";
            this.chooseMissingListBTN.UseVisualStyleBackColor = true;
            this.chooseMissingListBTN.Click += new System.EventHandler(this.chooseMissingListBTN_Click);
            // 
            // logFilePathField
            // 
            this.logFilePathField.Location = new System.Drawing.Point(111, 219);
            this.logFilePathField.Name = "logFilePathField";
            this.logFilePathField.ReadOnly = true;
            this.logFilePathField.Size = new System.Drawing.Size(592, 31);
            this.logFilePathField.TabIndex = 41;
            this.logFilePathField.TabStop = false;
            this.logFilePathField.Text = "";
            // 
            // chooseLogFileBTN
            // 
            this.chooseLogFileBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseLogFileBTN.Location = new System.Drawing.Point(308, 173);
            this.chooseLogFileBTN.Name = "chooseLogFileBTN";
            this.chooseLogFileBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseLogFileBTN.TabIndex = 40;
            this.chooseLogFileBTN.Text = "Choose Log File";
            this.chooseLogFileBTN.UseVisualStyleBackColor = true;
            this.chooseLogFileBTN.Click += new System.EventHandler(this.chooseLogFileBTN_Click);
            // 
            // createListsBTN
            // 
            this.createListsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createListsBTN.Location = new System.Drawing.Point(308, 440);
            this.createListsBTN.Name = "createListsBTN";
            this.createListsBTN.Size = new System.Drawing.Size(198, 40);
            this.createListsBTN.TabIndex = 44;
            this.createListsBTN.Text = "Create Lists";
            this.createListsBTN.UseVisualStyleBackColor = true;
            this.createListsBTN.Click += new System.EventHandler(this.createListsBTN_Click);
            // 
            // deleteRowsCheckBox
            // 
            this.deleteRowsCheckBox.AutoSize = true;
            this.deleteRowsCheckBox.Checked = true;
            this.deleteRowsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteRowsCheckBox.Location = new System.Drawing.Point(151, 459);
            this.deleteRowsCheckBox.Name = "deleteRowsCheckBox";
            this.deleteRowsCheckBox.Size = new System.Drawing.Size(102, 21);
            this.deleteRowsCheckBox.TabIndex = 45;
            this.deleteRowsCheckBox.Text = "delete rows";
            this.deleteRowsCheckBox.UseVisualStyleBackColor = true;
            // 
            // stragglersTotalLabel
            // 
            this.stragglersTotalLabel.AutoSize = true;
            this.stragglersTotalLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stragglersTotalLabel.Location = new System.Drawing.Point(567, 532);
            this.stragglersTotalLabel.Name = "stragglersTotalLabel";
            this.stragglersTotalLabel.Size = new System.Drawing.Size(121, 21);
            this.stragglersTotalLabel.TabIndex = 49;
            this.stragglersTotalLabel.Text = "Stragglers Total:";
            // 
            // voidedTotalLabel
            // 
            this.voidedTotalLabel.AutoSize = true;
            this.voidedTotalLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voidedTotalLabel.Location = new System.Drawing.Point(588, 486);
            this.voidedTotalLabel.Name = "voidedTotalLabel";
            this.voidedTotalLabel.Size = new System.Drawing.Size(100, 21);
            this.voidedTotalLabel.TabIndex = 48;
            this.voidedTotalLabel.Text = "Voided Total:";
            // 
            // missingTotalLabel
            // 
            this.missingTotalLabel.AutoSize = true;
            this.missingTotalLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.missingTotalLabel.Location = new System.Drawing.Point(582, 440);
            this.missingTotalLabel.Name = "missingTotalLabel";
            this.missingTotalLabel.Size = new System.Drawing.Size(106, 21);
            this.missingTotalLabel.TabIndex = 47;
            this.missingTotalLabel.Text = "Missing Total:";
            // 
            // saveFileToBTN
            // 
            this.saveFileToBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveFileToBTN.Location = new System.Drawing.Point(308, 282);
            this.saveFileToBTN.Name = "saveFileToBTN";
            this.saveFileToBTN.Size = new System.Drawing.Size(198, 40);
            this.saveFileToBTN.TabIndex = 50;
            this.saveFileToBTN.Text = "Save File To";
            this.saveFileToBTN.UseVisualStyleBackColor = true;
            this.saveFileToBTN.Click += new System.EventHandler(this.saveFileToBTN_Click);
            // 
            // saveFileToPathField
            // 
            this.saveFileToPathField.Location = new System.Drawing.Point(111, 328);
            this.saveFileToPathField.Name = "saveFileToPathField";
            this.saveFileToPathField.ReadOnly = true;
            this.saveFileToPathField.Size = new System.Drawing.Size(592, 31);
            this.saveFileToPathField.TabIndex = 51;
            this.saveFileToPathField.TabStop = false;
            this.saveFileToPathField.Text = "";
            // 
            // CreateLists_GV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveFileToPathField);
            this.Controls.Add(this.saveFileToBTN);
            this.Controls.Add(this.stragglersTotalLabel);
            this.Controls.Add(this.voidedTotalLabel);
            this.Controls.Add(this.missingTotalLabel);
            this.Controls.Add(this.deleteRowsCheckBox);
            this.Controls.Add(this.createListsBTN);
            this.Controls.Add(this.logFilePathField);
            this.Controls.Add(this.chooseLogFileBTN);
            this.Controls.Add(this.missingListPathField);
            this.Controls.Add(this.chooseMissingListBTN);
            this.Controls.Add(this.createListsTitle);
            this.Name = "CreateLists_GV";
            this.Size = new System.Drawing.Size(816, 680);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label createListsTitle;
        private System.Windows.Forms.RichTextBox missingListPathField;
        private System.Windows.Forms.Button chooseMissingListBTN;
        private System.Windows.Forms.RichTextBox logFilePathField;
        private System.Windows.Forms.Button chooseLogFileBTN;
        private System.Windows.Forms.Button createListsBTN;
        private System.Windows.Forms.CheckBox deleteRowsCheckBox;
        private System.Windows.Forms.Label stragglersTotalLabel;
        private System.Windows.Forms.Label voidedTotalLabel;
        private System.Windows.Forms.Label missingTotalLabel;
        private System.Windows.Forms.Button saveFileToBTN;
        private System.Windows.Forms.RichTextBox saveFileToPathField;
    }
}
