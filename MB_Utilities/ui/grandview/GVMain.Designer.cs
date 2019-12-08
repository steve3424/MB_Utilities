namespace MB_Utilities.ui.grandview
{
    partial class GVMain
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
            this.gvNav = new System.Windows.Forms.Panel();
            this.navSelectedPanel = new System.Windows.Forms.Panel();
            this.hospitalTitlePanel = new System.Windows.Forms.Panel();
            this.hospitalTitle = new System.Windows.Forms.Label();
            this.updateMissingListNavBTN = new System.Windows.Forms.Button();
            this.createMissingListNavBTN = new System.Windows.Forms.Button();
            this.gvNav.SuspendLayout();
            this.hospitalTitlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // gvNav
            // 
            this.gvNav.BackColor = System.Drawing.SystemColors.Control;
            this.gvNav.Controls.Add(this.createMissingListNavBTN);
            this.gvNav.Controls.Add(this.navSelectedPanel);
            this.gvNav.Controls.Add(this.hospitalTitlePanel);
            this.gvNav.Controls.Add(this.updateMissingListNavBTN);
            this.gvNav.Dock = System.Windows.Forms.DockStyle.Left;
            this.gvNav.Location = new System.Drawing.Point(0, 0);
            this.gvNav.Name = "gvNav";
            this.gvNav.Size = new System.Drawing.Size(190, 692);
            this.gvNav.TabIndex = 1;
            // 
            // navSelectedPanel
            // 
            this.navSelectedPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.navSelectedPanel.Location = new System.Drawing.Point(0, 56);
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
            this.hospitalTitle.Size = new System.Drawing.Size(42, 29);
            this.hospitalTitle.TabIndex = 1;
            this.hospitalTitle.Text = "GV";
            this.hospitalTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updateMissingListNavBTN
            // 
            this.updateMissingListNavBTN.FlatAppearance.BorderSize = 0;
            this.updateMissingListNavBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.updateMissingListNavBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateMissingListNavBTN.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateMissingListNavBTN.ForeColor = System.Drawing.Color.Black;
            this.updateMissingListNavBTN.Location = new System.Drawing.Point(10, 56);
            this.updateMissingListNavBTN.Name = "updateMissingListNavBTN";
            this.updateMissingListNavBTN.Size = new System.Drawing.Size(177, 50);
            this.updateMissingListNavBTN.TabIndex = 1;
            this.updateMissingListNavBTN.Text = "Update Missing List";
            this.updateMissingListNavBTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.updateMissingListNavBTN.UseVisualStyleBackColor = true;
            // 
            // createMissingListNavBTN
            // 
            this.createMissingListNavBTN.Enabled = false;
            this.createMissingListNavBTN.FlatAppearance.BorderSize = 0;
            this.createMissingListNavBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.createMissingListNavBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createMissingListNavBTN.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createMissingListNavBTN.ForeColor = System.Drawing.Color.Black;
            this.createMissingListNavBTN.Location = new System.Drawing.Point(10, 118);
            this.createMissingListNavBTN.Name = "createMissingListNavBTN";
            this.createMissingListNavBTN.Size = new System.Drawing.Size(177, 50);
            this.createMissingListNavBTN.TabIndex = 2;
            this.createMissingListNavBTN.Text = "Create Missing List";
            this.createMissingListNavBTN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.createMissingListNavBTN.UseVisualStyleBackColor = true;
            // 
            // GVMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvNav);
            this.Name = "GVMain";
            this.Size = new System.Drawing.Size(1006, 692);
            this.gvNav.ResumeLayout(false);
            this.hospitalTitlePanel.ResumeLayout(false);
            this.hospitalTitlePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel gvNav;
        private System.Windows.Forms.Panel navSelectedPanel;
        private System.Windows.Forms.Panel hospitalTitlePanel;
        private System.Windows.Forms.Label hospitalTitle;
        private System.Windows.Forms.Button updateMissingListNavBTN;
        private System.Windows.Forms.Button createMissingListNavBTN;
    }
}
