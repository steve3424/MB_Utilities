namespace MB_Utilities.ui.grandview
{
    partial class UpdateMissingList_GV
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
            this.updateMissingListTitle = new System.Windows.Forms.Label();
            this.updateMissingListBTN = new System.Windows.Forms.Button();
            this.chooseDateLabel = new System.Windows.Forms.Label();
            this.chooseDatePicker = new System.Windows.Forms.DateTimePicker();
            this.missingListPathField = new System.Windows.Forms.RichTextBox();
            this.chooseMissingListBTN = new System.Windows.Forms.Button();
            this.unbilledReportPathField = new System.Windows.Forms.RichTextBox();
            this.chooseUnbilledReportBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // updateMissingListTitle
            // 
            this.updateMissingListTitle.AutoSize = true;
            this.updateMissingListTitle.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateMissingListTitle.Location = new System.Drawing.Point(266, 0);
            this.updateMissingListTitle.Name = "updateMissingListTitle";
            this.updateMissingListTitle.Size = new System.Drawing.Size(284, 40);
            this.updateMissingListTitle.TabIndex = 24;
            this.updateMissingListTitle.Text = "Update Missing List";
            // 
            // updateMissingListBTN
            // 
            this.updateMissingListBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateMissingListBTN.Location = new System.Drawing.Point(309, 481);
            this.updateMissingListBTN.Name = "updateMissingListBTN";
            this.updateMissingListBTN.Size = new System.Drawing.Size(198, 40);
            this.updateMissingListBTN.TabIndex = 8;
            this.updateMissingListBTN.Text = "Update Missing List";
            this.updateMissingListBTN.UseVisualStyleBackColor = true;
            this.updateMissingListBTN.Click += new System.EventHandler(this.updateMissingListBTN_Click);
            // 
            // chooseDateLabel
            // 
            this.chooseDateLabel.AutoSize = true;
            this.chooseDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseDateLabel.Location = new System.Drawing.Point(332, 364);
            this.chooseDateLabel.Name = "chooseDateLabel";
            this.chooseDateLabel.Size = new System.Drawing.Size(153, 29);
            this.chooseDateLabel.TabIndex = 56;
            this.chooseDateLabel.Text = "Choose Date";
            // 
            // chooseDatePicker
            // 
            this.chooseDatePicker.Location = new System.Drawing.Point(277, 407);
            this.chooseDatePicker.Name = "chooseDatePicker";
            this.chooseDatePicker.Size = new System.Drawing.Size(262, 22);
            this.chooseDatePicker.TabIndex = 7;
            // 
            // missingListPathField
            // 
            this.missingListPathField.Location = new System.Drawing.Point(112, 149);
            this.missingListPathField.Name = "missingListPathField";
            this.missingListPathField.ReadOnly = true;
            this.missingListPathField.Size = new System.Drawing.Size(592, 31);
            this.missingListPathField.TabIndex = 55;
            this.missingListPathField.TabStop = false;
            this.missingListPathField.Text = "";
            // 
            // chooseMissingListBTN
            // 
            this.chooseMissingListBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseMissingListBTN.Location = new System.Drawing.Point(309, 103);
            this.chooseMissingListBTN.Name = "chooseMissingListBTN";
            this.chooseMissingListBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseMissingListBTN.TabIndex = 5;
            this.chooseMissingListBTN.Text = "Choose Missing List";
            this.chooseMissingListBTN.UseVisualStyleBackColor = true;
            this.chooseMissingListBTN.Click += new System.EventHandler(this.chooseMissingListBTN_Click);
            // 
            // unbilledReportPathField
            // 
            this.unbilledReportPathField.Location = new System.Drawing.Point(112, 275);
            this.unbilledReportPathField.Name = "unbilledReportPathField";
            this.unbilledReportPathField.ReadOnly = true;
            this.unbilledReportPathField.Size = new System.Drawing.Size(592, 31);
            this.unbilledReportPathField.TabIndex = 54;
            this.unbilledReportPathField.TabStop = false;
            this.unbilledReportPathField.Text = "";
            // 
            // chooseUnbilledReportBTN
            // 
            this.chooseUnbilledReportBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseUnbilledReportBTN.Location = new System.Drawing.Point(298, 229);
            this.chooseUnbilledReportBTN.Name = "chooseUnbilledReportBTN";
            this.chooseUnbilledReportBTN.Size = new System.Drawing.Size(221, 40);
            this.chooseUnbilledReportBTN.TabIndex = 6;
            this.chooseUnbilledReportBTN.Text = "Choose Unbilled Report";
            this.chooseUnbilledReportBTN.UseVisualStyleBackColor = true;
            this.chooseUnbilledReportBTN.Click += new System.EventHandler(this.chooseUnbilledReportBTN_Click);
            // 
            // UpdateMissingList_GV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.updateMissingListBTN);
            this.Controls.Add(this.chooseDateLabel);
            this.Controls.Add(this.chooseDatePicker);
            this.Controls.Add(this.missingListPathField);
            this.Controls.Add(this.chooseMissingListBTN);
            this.Controls.Add(this.unbilledReportPathField);
            this.Controls.Add(this.chooseUnbilledReportBTN);
            this.Controls.Add(this.updateMissingListTitle);
            this.Name = "UpdateMissingList_GV";
            this.Size = new System.Drawing.Size(816, 680);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label updateMissingListTitle;
        private System.Windows.Forms.Button updateMissingListBTN;
        private System.Windows.Forms.Label chooseDateLabel;
        private System.Windows.Forms.DateTimePicker chooseDatePicker;
        private System.Windows.Forms.RichTextBox missingListPathField;
        private System.Windows.Forms.Button chooseMissingListBTN;
        private System.Windows.Forms.RichTextBox unbilledReportPathField;
        private System.Windows.Forms.Button chooseUnbilledReportBTN;
    }
}
