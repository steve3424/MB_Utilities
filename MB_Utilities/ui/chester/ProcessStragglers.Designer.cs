namespace MB_Utilities.controls.chester
{
    partial class ProcessStragglers
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
            this.components = new System.ComponentModel.Container();
            this.processStragglersTitle = new System.Windows.Forms.Label();
            this.missingListPathField = new System.Windows.Forms.RichTextBox();
            this.chooseMissingListBTN = new System.Windows.Forms.Button();
            this.folderPathField = new System.Windows.Forms.RichTextBox();
            this.chooseFileFolderBTN = new System.Windows.Forms.Button();
            this.renameFilesBTN = new System.Windows.Forms.Button();
            this.stragglerListOutput = new System.Windows.Forms.DataGridView();
            this.createStragglerListBTN = new System.Windows.Forms.Button();
            this.stragglersTotalLabel = new System.Windows.Forms.Label();
            this.deleteRowsCheckBox = new System.Windows.Forms.CheckBox();
            this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyRightClickMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.stragglerListOutput)).BeginInit();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // processStragglersTitle
            // 
            this.processStragglersTitle.AutoSize = true;
            this.processStragglersTitle.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processStragglersTitle.Location = new System.Drawing.Point(272, 0);
            this.processStragglersTitle.Name = "processStragglersTitle";
            this.processStragglersTitle.Size = new System.Drawing.Size(265, 40);
            this.processStragglersTitle.TabIndex = 22;
            this.processStragglersTitle.Text = "Process Stragglers";
            // 
            // missingListPathField
            // 
            this.missingListPathField.Location = new System.Drawing.Point(112, 112);
            this.missingListPathField.Name = "missingListPathField";
            this.missingListPathField.ReadOnly = true;
            this.missingListPathField.Size = new System.Drawing.Size(592, 31);
            this.missingListPathField.TabIndex = 35;
            this.missingListPathField.TabStop = false;
            this.missingListPathField.Text = "";
            // 
            // chooseMissingListBTN
            // 
            this.chooseMissingListBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseMissingListBTN.Location = new System.Drawing.Point(309, 66);
            this.chooseMissingListBTN.Name = "chooseMissingListBTN";
            this.chooseMissingListBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseMissingListBTN.TabIndex = 6;
            this.chooseMissingListBTN.Text = "Choose Missing List";
            this.chooseMissingListBTN.UseVisualStyleBackColor = true;
            this.chooseMissingListBTN.Click += new System.EventHandler(this.chooseMissingListBTN_Click);
            // 
            // folderPathField
            // 
            this.folderPathField.Location = new System.Drawing.Point(112, 215);
            this.folderPathField.Name = "folderPathField";
            this.folderPathField.ReadOnly = true;
            this.folderPathField.Size = new System.Drawing.Size(592, 31);
            this.folderPathField.TabIndex = 37;
            this.folderPathField.TabStop = false;
            this.folderPathField.Text = "";
            // 
            // chooseFileFolderBTN
            // 
            this.chooseFileFolderBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseFileFolderBTN.Location = new System.Drawing.Point(309, 169);
            this.chooseFileFolderBTN.Name = "chooseFileFolderBTN";
            this.chooseFileFolderBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseFileFolderBTN.TabIndex = 7;
            this.chooseFileFolderBTN.Text = "Choose File Folder";
            this.chooseFileFolderBTN.UseVisualStyleBackColor = true;
            this.chooseFileFolderBTN.Click += new System.EventHandler(this.chooseFileFolderBTN_Click);
            // 
            // renameFilesBTN
            // 
            this.renameFilesBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.renameFilesBTN.Location = new System.Drawing.Point(309, 262);
            this.renameFilesBTN.Name = "renameFilesBTN";
            this.renameFilesBTN.Size = new System.Drawing.Size(198, 40);
            this.renameFilesBTN.TabIndex = 8;
            this.renameFilesBTN.Text = "Rename Files";
            this.renameFilesBTN.UseVisualStyleBackColor = true;
            this.renameFilesBTN.Click += new System.EventHandler(this.renameFilesBTN_Click);
            // 
            // stragglerListOutput
            // 
            this.stragglerListOutput.AllowUserToAddRows = false;
            this.stragglerListOutput.AllowUserToDeleteRows = false;
            this.stragglerListOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.stragglerListOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stragglerListOutput.Location = new System.Drawing.Point(31, 396);
            this.stragglerListOutput.Name = "stragglerListOutput";
            this.stragglerListOutput.ReadOnly = true;
            this.stragglerListOutput.RowHeadersWidth = 51;
            this.stragglerListOutput.RowTemplate.Height = 24;
            this.stragglerListOutput.Size = new System.Drawing.Size(755, 263);
            this.stragglerListOutput.TabIndex = 11;
            this.stragglerListOutput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.stragglerListOutput_MouseClick);
            // 
            // createStragglerListBTN
            // 
            this.createStragglerListBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createStragglerListBTN.Location = new System.Drawing.Point(309, 350);
            this.createStragglerListBTN.Name = "createStragglerListBTN";
            this.createStragglerListBTN.Size = new System.Drawing.Size(198, 40);
            this.createStragglerListBTN.TabIndex = 10;
            this.createStragglerListBTN.Text = "Create Straggler List";
            this.createStragglerListBTN.UseVisualStyleBackColor = true;
            this.createStragglerListBTN.Click += new System.EventHandler(this.createStragglerListBTN_Click);
            // 
            // stragglersTotalLabel
            // 
            this.stragglersTotalLabel.AutoSize = true;
            this.stragglersTotalLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stragglersTotalLabel.Location = new System.Drawing.Point(537, 369);
            this.stragglersTotalLabel.Name = "stragglersTotalLabel";
            this.stragglersTotalLabel.Size = new System.Drawing.Size(48, 21);
            this.stragglersTotalLabel.TabIndex = 41;
            this.stragglersTotalLabel.Text = "Total:";
            // 
            // deleteRowsCheckBox
            // 
            this.deleteRowsCheckBox.AutoSize = true;
            this.deleteRowsCheckBox.Checked = true;
            this.deleteRowsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteRowsCheckBox.Location = new System.Drawing.Point(157, 368);
            this.deleteRowsCheckBox.Name = "deleteRowsCheckBox";
            this.deleteRowsCheckBox.Size = new System.Drawing.Size(110, 21);
            this.deleteRowsCheckBox.TabIndex = 9;
            this.deleteRowsCheckBox.Text = "delete rows?";
            this.deleteRowsCheckBox.UseVisualStyleBackColor = true;
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyRightClickMenuItem});
            this.rightClickMenu.Name = "rightClickMenu";
            this.rightClickMenu.Size = new System.Drawing.Size(113, 28);
            // 
            // copyRightClickMenuItem
            // 
            this.copyRightClickMenuItem.Name = "copyRightClickMenuItem";
            this.copyRightClickMenuItem.Size = new System.Drawing.Size(112, 24);
            this.copyRightClickMenuItem.Text = "Copy";
            this.copyRightClickMenuItem.Click += new System.EventHandler(this.copyRightClickMenuItem_Click);
            // 
            // ProcessStragglers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deleteRowsCheckBox);
            this.Controls.Add(this.stragglersTotalLabel);
            this.Controls.Add(this.stragglerListOutput);
            this.Controls.Add(this.createStragglerListBTN);
            this.Controls.Add(this.renameFilesBTN);
            this.Controls.Add(this.folderPathField);
            this.Controls.Add(this.chooseFileFolderBTN);
            this.Controls.Add(this.missingListPathField);
            this.Controls.Add(this.chooseMissingListBTN);
            this.Controls.Add(this.processStragglersTitle);
            this.Name = "ProcessStragglers";
            this.Size = new System.Drawing.Size(816, 680);
            ((System.ComponentModel.ISupportInitialize)(this.stragglerListOutput)).EndInit();
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label processStragglersTitle;
        private System.Windows.Forms.RichTextBox missingListPathField;
        private System.Windows.Forms.Button chooseMissingListBTN;
        private System.Windows.Forms.RichTextBox folderPathField;
        private System.Windows.Forms.Button chooseFileFolderBTN;
        private System.Windows.Forms.Button renameFilesBTN;
        private System.Windows.Forms.DataGridView stragglerListOutput;
        private System.Windows.Forms.Button createStragglerListBTN;
        private System.Windows.Forms.Label stragglersTotalLabel;
        private System.Windows.Forms.CheckBox deleteRowsCheckBox;
        private System.Windows.Forms.ContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem copyRightClickMenuItem;
    }
}
