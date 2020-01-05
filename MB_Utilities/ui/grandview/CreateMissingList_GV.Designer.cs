namespace MB_Utilities.ui.grandview
{
    partial class CreateMissingList_GV
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
            this.createMissingListTitle = new System.Windows.Forms.Label();
            this.chooseLogFileBTN = new System.Windows.Forms.Button();
            this.logFilePathField = new System.Windows.Forms.RichTextBox();
            this.createMissingListBTN = new System.Windows.Forms.Button();
            this.voidedTotalLabel = new System.Windows.Forms.Label();
            this.missingTotalLabel = new System.Windows.Forms.Label();
            this.missingVoidedTabControl = new System.Windows.Forms.TabControl();
            this.missingTab = new System.Windows.Forms.TabPage();
            this.missingListOutput = new System.Windows.Forms.DataGridView();
            this.voidedTab = new System.Windows.Forms.TabPage();
            this.voidedListOutput = new System.Windows.Forms.DataGridView();
            this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyRightClickMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.missingVoidedTabControl.SuspendLayout();
            this.missingTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.missingListOutput)).BeginInit();
            this.voidedTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.voidedListOutput)).BeginInit();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // createMissingListTitle
            // 
            this.createMissingListTitle.AutoSize = true;
            this.createMissingListTitle.Font = new System.Drawing.Font("Calibri", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createMissingListTitle.Location = new System.Drawing.Point(272, 0);
            this.createMissingListTitle.Name = "createMissingListTitle";
            this.createMissingListTitle.Size = new System.Drawing.Size(272, 40);
            this.createMissingListTitle.TabIndex = 23;
            this.createMissingListTitle.Text = "Create Missing List";
            // 
            // chooseLogFileBTN
            // 
            this.chooseLogFileBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chooseLogFileBTN.Location = new System.Drawing.Point(309, 121);
            this.chooseLogFileBTN.Name = "chooseLogFileBTN";
            this.chooseLogFileBTN.Size = new System.Drawing.Size(198, 40);
            this.chooseLogFileBTN.TabIndex = 5;
            this.chooseLogFileBTN.Text = "Choose Log File";
            this.chooseLogFileBTN.UseVisualStyleBackColor = true;
            this.chooseLogFileBTN.Click += new System.EventHandler(this.chooseLogFileBTN_Click);
            // 
            // logFilePathField
            // 
            this.logFilePathField.Location = new System.Drawing.Point(112, 167);
            this.logFilePathField.Name = "logFilePathField";
            this.logFilePathField.ReadOnly = true;
            this.logFilePathField.Size = new System.Drawing.Size(592, 31);
            this.logFilePathField.TabIndex = 34;
            this.logFilePathField.TabStop = false;
            this.logFilePathField.Text = "";
            // 
            // createMissingListBTN
            // 
            this.createMissingListBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createMissingListBTN.Location = new System.Drawing.Point(309, 296);
            this.createMissingListBTN.Name = "createMissingListBTN";
            this.createMissingListBTN.Size = new System.Drawing.Size(198, 40);
            this.createMissingListBTN.TabIndex = 6;
            this.createMissingListBTN.Text = "Create Missing List";
            this.createMissingListBTN.UseVisualStyleBackColor = true;
            this.createMissingListBTN.Click += new System.EventHandler(this.createMissingListBTN_Click);
            // 
            // voidedTotalLabel
            // 
            this.voidedTotalLabel.AutoSize = true;
            this.voidedTotalLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.voidedTotalLabel.Location = new System.Drawing.Point(577, 330);
            this.voidedTotalLabel.Name = "voidedTotalLabel";
            this.voidedTotalLabel.Size = new System.Drawing.Size(100, 21);
            this.voidedTotalLabel.TabIndex = 45;
            this.voidedTotalLabel.Text = "Voided Total:";
            // 
            // missingTotalLabel
            // 
            this.missingTotalLabel.AutoSize = true;
            this.missingTotalLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.missingTotalLabel.Location = new System.Drawing.Point(571, 284);
            this.missingTotalLabel.Name = "missingTotalLabel";
            this.missingTotalLabel.Size = new System.Drawing.Size(106, 21);
            this.missingTotalLabel.TabIndex = 44;
            this.missingTotalLabel.Text = "Missing Total:";
            // 
            // missingVoidedTabControl
            // 
            this.missingVoidedTabControl.Controls.Add(this.missingTab);
            this.missingVoidedTabControl.Controls.Add(this.voidedTab);
            this.missingVoidedTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.missingVoidedTabControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.missingVoidedTabControl.ItemSize = new System.Drawing.Size(100, 30);
            this.missingVoidedTabControl.Location = new System.Drawing.Point(21, 342);
            this.missingVoidedTabControl.Multiline = true;
            this.missingVoidedTabControl.Name = "missingVoidedTabControl";
            this.missingVoidedTabControl.SelectedIndex = 0;
            this.missingVoidedTabControl.Size = new System.Drawing.Size(775, 319);
            this.missingVoidedTabControl.TabIndex = 7;
            // 
            // missingTab
            // 
            this.missingTab.BackColor = System.Drawing.Color.Transparent;
            this.missingTab.Controls.Add(this.missingListOutput);
            this.missingTab.Location = new System.Drawing.Point(4, 34);
            this.missingTab.Name = "missingTab";
            this.missingTab.Padding = new System.Windows.Forms.Padding(3);
            this.missingTab.Size = new System.Drawing.Size(767, 281);
            this.missingTab.TabIndex = 0;
            this.missingTab.Text = "Missing List";
            // 
            // missingListOutput
            // 
            this.missingListOutput.AllowUserToAddRows = false;
            this.missingListOutput.AllowUserToDeleteRows = false;
            this.missingListOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.missingListOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.missingListOutput.Location = new System.Drawing.Point(6, 6);
            this.missingListOutput.Name = "missingListOutput";
            this.missingListOutput.ReadOnly = true;
            this.missingListOutput.RowHeadersWidth = 51;
            this.missingListOutput.RowTemplate.Height = 24;
            this.missingListOutput.Size = new System.Drawing.Size(755, 263);
            this.missingListOutput.TabIndex = 8;
            this.missingListOutput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.missingListOutput_MouseClick);
            // 
            // voidedTab
            // 
            this.voidedTab.BackColor = System.Drawing.Color.Transparent;
            this.voidedTab.Controls.Add(this.voidedListOutput);
            this.voidedTab.Location = new System.Drawing.Point(4, 34);
            this.voidedTab.Name = "voidedTab";
            this.voidedTab.Padding = new System.Windows.Forms.Padding(3);
            this.voidedTab.Size = new System.Drawing.Size(767, 281);
            this.voidedTab.TabIndex = 1;
            this.voidedTab.Text = "Voided List";
            // 
            // voidedListOutput
            // 
            this.voidedListOutput.AllowUserToAddRows = false;
            this.voidedListOutput.AllowUserToDeleteRows = false;
            this.voidedListOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.voidedListOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.voidedListOutput.Location = new System.Drawing.Point(6, 6);
            this.voidedListOutput.Name = "voidedListOutput";
            this.voidedListOutput.ReadOnly = true;
            this.voidedListOutput.RowHeadersWidth = 51;
            this.voidedListOutput.RowTemplate.Height = 24;
            this.voidedListOutput.Size = new System.Drawing.Size(755, 263);
            this.voidedListOutput.TabIndex = 9;
            this.voidedListOutput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.voidedListOutput_MouseClick);
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
            // CreateMissingList_GV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.voidedTotalLabel);
            this.Controls.Add(this.missingVoidedTabControl);
            this.Controls.Add(this.missingTotalLabel);
            this.Controls.Add(this.createMissingListBTN);
            this.Controls.Add(this.logFilePathField);
            this.Controls.Add(this.chooseLogFileBTN);
            this.Controls.Add(this.createMissingListTitle);
            this.Name = "CreateMissingList_GV";
            this.Size = new System.Drawing.Size(816, 680);
            this.missingVoidedTabControl.ResumeLayout(false);
            this.missingTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.missingListOutput)).EndInit();
            this.voidedTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.voidedListOutput)).EndInit();
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label createMissingListTitle;
        private System.Windows.Forms.Button chooseLogFileBTN;
        private System.Windows.Forms.RichTextBox logFilePathField;
        private System.Windows.Forms.Button createMissingListBTN;
        private System.Windows.Forms.Label voidedTotalLabel;
        private System.Windows.Forms.Label missingTotalLabel;
        private System.Windows.Forms.TabControl missingVoidedTabControl;
        private System.Windows.Forms.TabPage missingTab;
        private System.Windows.Forms.DataGridView missingListOutput;
        private System.Windows.Forms.TabPage voidedTab;
        private System.Windows.Forms.DataGridView voidedListOutput;
        private System.Windows.Forms.ContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem copyRightClickMenuItem;
    }
}
