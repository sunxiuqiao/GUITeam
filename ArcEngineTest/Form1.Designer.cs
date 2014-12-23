namespace ArcEngineTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axMapControl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axTOCControl = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.AddData = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMapControl
            // 
            this.axMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl.Location = new System.Drawing.Point(151, 49);
            this.axMapControl.Name = "axMapControl";
            this.axMapControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl.OcxState")));
            this.axMapControl.Size = new System.Drawing.Size(490, 445);
            this.axMapControl.TabIndex = 1;
            // 
            // axTOCControl
            // 
            this.axTOCControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl.Location = new System.Drawing.Point(0, 49);
            this.axTOCControl.Name = "axTOCControl";
            this.axTOCControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl.OcxState")));
            this.axTOCControl.Size = new System.Drawing.Size(151, 445);
            this.axTOCControl.TabIndex = 2;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(306, 198);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddData});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(641, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(641, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // AddData
            // 
            this.AddData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddData.Image = ((System.Drawing.Image)(resources.GetObject("AddData.Image")));
            this.AddData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddData.Name = "AddData";
            this.AddData.Size = new System.Drawing.Size(23, 22);
            this.AddData.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 494);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapControl);
            this.Controls.Add(this.axTOCControl);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripButton AddData;
    }
}

