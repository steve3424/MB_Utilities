namespace MB_Utilities.ui.grandview
{
    partial class SearchStragglers_GV
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
            this.searchStragglersTitle = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.missingListPathField = new System.Windows.Forms.RichTextBox();
            this.chooseMissingListBTN = new System.Windows.Forms.Button();
            this.saveFileToPathField = new System.Windows.Forms.RichTextBox();
            this.saveFileToBTN = new System.Windows.Forms.Button();
            this.chooseListsLabel = new System.Windows.Forms.Label();
            this.createListsBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // searchStragglersTitle
            // 
            this.searchStragglersTitle.AutoSize = true;
            this.searchStragglersTitle.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchStragglersTitle.Location = new System.Drawing.Point(281, 0);
            this.searchStragglersTitle.Name = "searchStragglersTitle";
            this.searchStragglersTitle.Size = new System.Drawing.Size(253, 40);
            this.searchStragglersTitle.TabIndex = 48;
            this.searchStragglersTitle.Text = "Search Stragglers";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedListBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "ME",
            "PM",
            "SC",
            "WR",
            "TD"});
            this.checkedListBox1.Location = new System.Drawing.Point(378, 389);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(110, 154);
            this.checkedListBox1.TabIndex = 51;
            // 
            // missingListPathField
            // 
            this.missingListPathField.Location = new System.Drawing.Point(112, 149);
            this.missingListPathField.Name = "missingListPathField";
            this.missingListPathField.ReadOnly = true;
            this.missingListPathField.Size = new System.Drawing.Size(592, 31);
            this.missingListPathField.TabIndex = 57;
            this.missingListPathField.TabStop = false;
            this.missingListPathField.Text = "";
            // 
            // chooseMissingListBTN
            // 
            this.chooseMissingListBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseMissingListBTN.Location = new System.Drawing.Point(309, 103);
            this.chooseMissingListBTN.Name = "chooseMissingListBTN";
            this.chooseMissingListBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseMissingListBTN.TabIndex = 56;
            this.chooseMissingListBTN.Text = "Choose Missing List";
            this.chooseMissingListBTN.UseVisualStyleBackColor = true;
            this.chooseMissingListBTN.Click += new System.EventHandler(this.chooseMissingListBTN_Click);
            // 
            // saveFileToPathField
            // 
            this.saveFileToPathField.Location = new System.Drawing.Point(112, 275);
            this.saveFileToPathField.Name = "saveFileToPathField";
            this.saveFileToPathField.ReadOnly = true;
            this.saveFileToPathField.Size = new System.Drawing.Size(592, 31);
            this.saveFileToPathField.TabIndex = 59;
            this.saveFileToPathField.TabStop = false;
            this.saveFileToPathField.Text = "";
            // 
            // saveFileToBTN
            // 
            this.saveFileToBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveFileToBTN.Location = new System.Drawing.Point(309, 226);
            this.saveFileToBTN.Name = "saveFileToBTN";
            this.saveFileToBTN.Size = new System.Drawing.Size(198, 40);
            this.saveFileToBTN.TabIndex = 58;
            this.saveFileToBTN.Text = "Save File To";
            this.saveFileToBTN.UseVisualStyleBackColor = true;
            this.saveFileToBTN.Click += new System.EventHandler(this.saveFileToBTN_Click);
            // 
            // chooseListsLabel
            // 
            this.chooseListsLabel.AutoSize = true;
            this.chooseListsLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseListsLabel.Location = new System.Drawing.Point(305, 347);
            this.chooseListsLabel.Name = "chooseListsLabel";
            this.chooseListsLabel.Size = new System.Drawing.Size(202, 24);
            this.chooseListsLabel.TabIndex = 60;
            this.chooseListsLabel.Text = "Choose Lists to Search:";
            // 
            // createListsBTN
            // 
            this.createListsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createListsBTN.Location = new System.Drawing.Point(309, 549);
            this.createListsBTN.Name = "createListsBTN";
            this.createListsBTN.Size = new System.Drawing.Size(198, 40);
            this.createListsBTN.TabIndex = 61;
            this.createListsBTN.Text = "Create Lists";
            this.createListsBTN.UseVisualStyleBackColor = true;
            this.createListsBTN.Click += new System.EventHandler(this.createListsBTN_Click);
            // 
            // SearchStragglers_GV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.createListsBTN);
            this.Controls.Add(this.chooseListsLabel);
            this.Controls.Add(this.saveFileToPathField);
            this.Controls.Add(this.saveFileToBTN);
            this.Controls.Add(this.missingListPathField);
            this.Controls.Add(this.chooseMissingListBTN);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.searchStragglersTitle);
            this.Name = "SearchStragglers_GV";
            this.Size = new System.Drawing.Size(816, 680);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label searchStragglersTitle;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.RichTextBox missingListPathField;
        private System.Windows.Forms.Button chooseMissingListBTN;
        private System.Windows.Forms.RichTextBox saveFileToPathField;
        private System.Windows.Forms.Button saveFileToBTN;
        private System.Windows.Forms.Label chooseListsLabel;
        private System.Windows.Forms.Button createListsBTN;
    }
}
