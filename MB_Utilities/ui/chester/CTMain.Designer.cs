namespace MB_Utilities.ui.chester
{
    partial class CTMain
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
            this.ctNav = new System.Windows.Forms.Panel();
            this.createListsNavBTN = new System.Windows.Forms.Button();
            this.navSelectedPanel = new System.Windows.Forms.Panel();
            this.hospitalTitlePanel = new System.Windows.Forms.Panel();
            this.hospitalTitle = new System.Windows.Forms.Label();
            this.loadDaysNavBTN = new System.Windows.Forms.Button();
            this.processStragglersNavBTN = new System.Windows.Forms.Button();
            this.processFilesNavBTN = new System.Windows.Forms.Button();
            this.processStragglers1 = new MB_Utilities.controls.chester.ProcessStragglers();
            this.processFiles1 = new MB_Utilities.controls.chester.ProcessFiles();
            this.loadDays1 = new MB_Utilities.controls.chester.LoadDays();
            this.createLists1 = new MB_Utilities.ui.chester.CreateLists();
            this.ctNav.SuspendLayout();
            this.hospitalTitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctNav
            // 
            this.ctNav.BackColor = System.Drawing.SystemColors.Control;
            this.ctNav.Controls.Add(this.createListsNavBTN);
            this.ctNav.Controls.Add(this.navSelectedPanel);
            this.ctNav.Controls.Add(this.hospitalTitlePanel);
            this.ctNav.Controls.Add(this.loadDaysNavBTN);
            this.ctNav.Controls.Add(this.processStragglersNavBTN);
            this.ctNav.Controls.Add(this.processFilesNavBTN);
            this.ctNav.Dock = System.Windows.Forms.DockStyle.Left;
            this.ctNav.Location = new System.Drawing.Point(0, 0);
            this.ctNav.Name = "ctNav";
            this.ctNav.Size = new System.Drawing.Size(190, 692);
            this.ctNav.TabIndex = 0;
            // 
            // createListsNavBTN
            // 
            this.createListsNavBTN.FlatAppearance.BorderSize = 0;
            this.createListsNavBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.createListsNavBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createListsNavBTN.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createListsNavBTN.ForeColor = System.Drawing.Color.Black;
            this.createListsNavBTN.Location = new System.Drawing.Point(3, 183);
            this.createListsNavBTN.Name = "createListsNavBTN";
            this.createListsNavBTN.Size = new System.Drawing.Size(177, 50);
            this.createListsNavBTN.TabIndex = 3;
            this.createListsNavBTN.Text = "Create Lists";
            this.createListsNavBTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.createListsNavBTN.UseVisualStyleBackColor = true;
            this.createListsNavBTN.Click += new System.EventHandler(this.createListsNavBTN_Click);
            // 
            // navSelectedPanel
            // 
            this.navSelectedPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.navSelectedPanel.Location = new System.Drawing.Point(180, 56);
            this.navSelectedPanel.Name = "navSelectedPanel";
            this.navSelectedPanel.Size = new System.Drawing.Size(10, 50);
            this.navSelectedPanel.TabIndex = 1;
            // 
            // hospitalTitlePanel
            // 
            this.hospitalTitlePanel.BackColor = System.Drawing.Color.LightGray;
            this.hospitalTitlePanel.Controls.Add(this.hospitalTitle);
            this.hospitalTitlePanel.Location = new System.Drawing.Point(0, 0);
            this.hospitalTitlePanel.Name = "hospitalTitlePanel";
            this.hospitalTitlePanel.Size = new System.Drawing.Size(190, 50);
            this.hospitalTitlePanel.TabIndex = 0;
            // 
            // hospitalTitle
            // 
            this.hospitalTitle.AutoSize = true;
            this.hospitalTitle.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hospitalTitle.Location = new System.Drawing.Point(76, 10);
            this.hospitalTitle.Name = "hospitalTitle";
            this.hospitalTitle.Size = new System.Drawing.Size(38, 29);
            this.hospitalTitle.TabIndex = 1;
            this.hospitalTitle.Text = "CT";
            this.hospitalTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loadDaysNavBTN
            // 
            this.loadDaysNavBTN.FlatAppearance.BorderSize = 0;
            this.loadDaysNavBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.loadDaysNavBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadDaysNavBTN.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadDaysNavBTN.ForeColor = System.Drawing.Color.Black;
            this.loadDaysNavBTN.Location = new System.Drawing.Point(3, 248);
            this.loadDaysNavBTN.Name = "loadDaysNavBTN";
            this.loadDaysNavBTN.Size = new System.Drawing.Size(177, 50);
            this.loadDaysNavBTN.TabIndex = 5;
            this.loadDaysNavBTN.Text = "Load Days";
            this.loadDaysNavBTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.loadDaysNavBTN.UseVisualStyleBackColor = true;
            this.loadDaysNavBTN.Click += new System.EventHandler(this.loadDaysNavBTN_Click);
            // 
            // processStragglersNavBTN
            // 
            this.processStragglersNavBTN.FlatAppearance.BorderSize = 0;
            this.processStragglersNavBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.processStragglersNavBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.processStragglersNavBTN.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processStragglersNavBTN.ForeColor = System.Drawing.Color.Black;
            this.processStragglersNavBTN.Location = new System.Drawing.Point(3, 118);
            this.processStragglersNavBTN.Name = "processStragglersNavBTN";
            this.processStragglersNavBTN.Size = new System.Drawing.Size(177, 50);
            this.processStragglersNavBTN.TabIndex = 2;
            this.processStragglersNavBTN.Text = "Process Stragglers";
            this.processStragglersNavBTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.processStragglersNavBTN.UseVisualStyleBackColor = true;
            this.processStragglersNavBTN.Click += new System.EventHandler(this.processStragglersNavBTN_Click);
            // 
            // processFilesNavBTN
            // 
            this.processFilesNavBTN.FlatAppearance.BorderSize = 0;
            this.processFilesNavBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.processFilesNavBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.processFilesNavBTN.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processFilesNavBTN.ForeColor = System.Drawing.Color.Black;
            this.processFilesNavBTN.Location = new System.Drawing.Point(3, 56);
            this.processFilesNavBTN.Name = "processFilesNavBTN";
            this.processFilesNavBTN.Size = new System.Drawing.Size(177, 50);
            this.processFilesNavBTN.TabIndex = 1;
            this.processFilesNavBTN.Text = "Process Files";
            this.processFilesNavBTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.processFilesNavBTN.UseVisualStyleBackColor = true;
            this.processFilesNavBTN.Click += new System.EventHandler(this.processFilesNavBTN_Click);
            // 
            // processStragglers1
            // 
            this.processStragglers1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processStragglers1.Location = new System.Drawing.Point(190, 0);
            this.processStragglers1.Name = "processStragglers1";
            this.processStragglers1.Size = new System.Drawing.Size(816, 692);
            this.processStragglers1.TabIndex = 2;
            // 
            // processFiles1
            // 
            this.processFiles1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processFiles1.Location = new System.Drawing.Point(190, 0);
            this.processFiles1.Name = "processFiles1";
            this.processFiles1.Size = new System.Drawing.Size(816, 692);
            this.processFiles1.TabIndex = 1;
            // 
            // loadDays1
            // 
            this.loadDays1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadDays1.Location = new System.Drawing.Point(190, 0);
            this.loadDays1.Name = "loadDays1";
            this.loadDays1.Size = new System.Drawing.Size(816, 692);
            this.loadDays1.TabIndex = 5;
            // 
            // createLists1
            // 
            this.createLists1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.createLists1.Location = new System.Drawing.Point(190, 0);
            this.createLists1.Name = "createLists1";
            this.createLists1.Size = new System.Drawing.Size(816, 692);
            this.createLists1.TabIndex = 6;
            // 
            // CTMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.createLists1);
            this.Controls.Add(this.loadDays1);
            this.Controls.Add(this.processStragglers1);
            this.Controls.Add(this.processFiles1);
            this.Controls.Add(this.ctNav);
            this.Name = "CTMain";
            this.Size = new System.Drawing.Size(1006, 692);
            this.Load += new System.EventHandler(this.CTMain_Load);
            this.ctNav.ResumeLayout(false);
            this.hospitalTitlePanel.ResumeLayout(false);
            this.hospitalTitlePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ctNav;
        private System.Windows.Forms.Button processFilesNavBTN;
        private System.Windows.Forms.Button processStragglersNavBTN;
        private System.Windows.Forms.Button loadDaysNavBTN;
        private System.Windows.Forms.Label hospitalTitle;
        private System.Windows.Forms.Panel hospitalTitlePanel;
        private System.Windows.Forms.Panel navSelectedPanel;
        private controls.chester.ProcessFiles processFiles1;
        private controls.chester.ProcessStragglers processStragglers1;
        private controls.chester.LoadDays loadDays1;
        private CreateLists createLists1;
        private System.Windows.Forms.Button createListsNavBTN;
    }
}
