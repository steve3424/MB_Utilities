namespace MB_Utilities
{
    partial class FormMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.navMain = new System.Windows.Forms.MenuStrip();
            this.chooseHospitalNavBTN = new System.Windows.Forms.ToolStripMenuItem();
            this.chesterCountyNavBTN = new System.Windows.Forms.ToolStripMenuItem();
            this.grandviewNavBTN = new System.Windows.Forms.ToolStripMenuItem();
            this.titleMain = new System.Windows.Forms.Label();
            this.iconMain = new System.Windows.Forms.PictureBox();
            this.gvMain1 = new MB_Utilities.ui.grandview.GVMain();
            this.ctMain1 = new MB_Utilities.ui.chester.CTMain();
            this.navMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconMain)).BeginInit();
            this.SuspendLayout();
            // 
            // navMain
            // 
            this.navMain.BackColor = System.Drawing.Color.WhiteSmoke;
            this.navMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.navMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chooseHospitalNavBTN});
            this.navMain.Location = new System.Drawing.Point(0, 0);
            this.navMain.Name = "navMain";
            this.navMain.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.navMain.Size = new System.Drawing.Size(1006, 29);
            this.navMain.TabIndex = 0;
            // 
            // chooseHospitalNavBTN
            // 
            this.chooseHospitalNavBTN.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chesterCountyNavBTN,
            this.grandviewNavBTN});
            this.chooseHospitalNavBTN.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.chooseHospitalNavBTN.Name = "chooseHospitalNavBTN";
            this.chooseHospitalNavBTN.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.chooseHospitalNavBTN.Size = new System.Drawing.Size(132, 29);
            this.chooseHospitalNavBTN.Text = "Choose Hospital";
            // 
            // chesterCountyNavBTN
            // 
            this.chesterCountyNavBTN.Name = "chesterCountyNavBTN";
            this.chesterCountyNavBTN.Size = new System.Drawing.Size(224, 26);
            this.chesterCountyNavBTN.Text = "Chester County";
            this.chesterCountyNavBTN.Click += new System.EventHandler(this.chesterCountyNavBTN_Click);
            // 
            // grandviewNavBTN
            // 
            this.grandviewNavBTN.Name = "grandviewNavBTN";
            this.grandviewNavBTN.Size = new System.Drawing.Size(224, 26);
            this.grandviewNavBTN.Text = "Grandview";
            this.grandviewNavBTN.Click += new System.EventHandler(this.grandviewNavBTN_Click);
            // 
            // titleMain
            // 
            this.titleMain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.titleMain.AutoSize = true;
            this.titleMain.Font = new System.Drawing.Font("Calibri", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleMain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.titleMain.Location = new System.Drawing.Point(386, 486);
            this.titleMain.Name = "titleMain";
            this.titleMain.Size = new System.Drawing.Size(253, 59);
            this.titleMain.TabIndex = 1;
            this.titleMain.Text = "MB Utilities";
            // 
            // iconMain
            // 
            this.iconMain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.iconMain.Image = ((System.Drawing.Image)(resources.GetObject("iconMain.Image")));
            this.iconMain.Location = new System.Drawing.Point(307, 128);
            this.iconMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconMain.Name = "iconMain";
            this.iconMain.Size = new System.Drawing.Size(410, 307);
            this.iconMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconMain.TabIndex = 2;
            this.iconMain.TabStop = false;
            // 
            // gvMain1
            // 
            this.gvMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMain1.Location = new System.Drawing.Point(0, 29);
            this.gvMain1.Name = "gvMain1";
            this.gvMain1.Size = new System.Drawing.Size(1006, 692);
            this.gvMain1.TabIndex = 4;
            this.gvMain1.Visible = false;
            // 
            // ctMain1
            // 
            this.ctMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctMain1.Location = new System.Drawing.Point(0, 29);
            this.ctMain1.Name = "ctMain1";
            this.ctMain1.Size = new System.Drawing.Size(1006, 692);
            this.ctMain1.TabIndex = 3;
            this.ctMain1.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.gvMain1);
            this.Controls.Add(this.ctMain1);
            this.Controls.Add(this.iconMain);
            this.Controls.Add(this.titleMain);
            this.Controls.Add(this.navMain);
            this.MainMenuStrip = this.navMain;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMain";
            this.Text = "MB Utilities";
            this.navMain.ResumeLayout(false);
            this.navMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip navMain;
        private System.Windows.Forms.ToolStripMenuItem chooseHospitalNavBTN;
        private System.Windows.Forms.ToolStripMenuItem chesterCountyNavBTN;
        private System.Windows.Forms.ToolStripMenuItem grandviewNavBTN;
        private System.Windows.Forms.Label titleMain;
        private System.Windows.Forms.PictureBox iconMain;
        private ui.chester.CTMain ctMain1;
        private ui.grandview.GVMain gvMain1;
    }
}

