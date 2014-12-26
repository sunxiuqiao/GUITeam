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
            this.AddData = new System.Windows.Forms.ToolStripButton();
            this.openMapDoc = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.editTS = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.开始编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存编辑内容ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.分割ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.构造点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平行复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.合并ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缓冲ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.联合ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.裁剪ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.捕捉ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.editTS.SuspendLayout();
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
            this.axMapControl.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl_OnMouseDown);
            // 
            // axTOCControl
            // 
            this.axTOCControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.axTOCControl.Location = new System.Drawing.Point(0, 49);
            this.axTOCControl.Name = "axTOCControl";
            this.axTOCControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl.OcxState")));
            this.axTOCControl.Size = new System.Drawing.Size(151, 445);
            this.axTOCControl.TabIndex = 2;
            this.axTOCControl.OnMouseDown += new ESRI.ArcGIS.Controls.ITOCControlEvents_Ax_OnMouseDownEventHandler(this.axTOCControl_OnMouseDown);
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
            this.AddData,
            this.openMapDoc});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(641, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
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
            // openMapDoc
            // 
            this.openMapDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openMapDoc.Image = global::ArcEngineTest.Properties.Resources.GenericOpen_B_16;
            this.openMapDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openMapDoc.Name = "openMapDoc";
            this.openMapDoc.Size = new System.Drawing.Size(23, 22);
            this.openMapDoc.Text = "toolStripButton5";
            this.openMapDoc.Click += new System.EventHandler(this.openMapDoc_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(641, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // editTS
            // 
            this.editTS.AllowMerge = false;
            this.editTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton2,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator3,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton6,
            this.toolStripSeparator2,
            this.toolStripSplitButton1,
            this.toolStripButton7,
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripButton10,
            this.toolStripButton11,
            this.toolStripSeparator4,
            this.toolStripButton12,
            this.toolStripButton13,
            this.toolStripButton14});
            this.editTS.Location = new System.Drawing.Point(151, 49);
            this.editTS.Name = "editTS";
            this.editTS.Size = new System.Drawing.Size(490, 25);
            this.editTS.TabIndex = 6;
            this.editTS.Text = "toolStrip2";
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始编辑ToolStripMenuItem,
            this.停止编辑ToolStripMenuItem,
            this.保存编辑内容ToolStripMenuItem,
            this.移动ToolStripMenuItem,
            this.分割ToolStripMenuItem,
            this.构造点ToolStripMenuItem,
            this.平行复制ToolStripMenuItem,
            this.合并ToolStripMenuItem,
            this.缓冲ToolStripMenuItem,
            this.联合ToolStripMenuItem,
            this.裁剪ToolStripMenuItem,
            this.捕捉ToolStripMenuItem});
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(60, 22);
            this.toolStripSplitButton2.Text = "编辑器";
            // 
            // 开始编辑ToolStripMenuItem
            // 
            this.开始编辑ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.点ToolStripMenuItem,
            this.线ToolStripMenuItem,
            this.面ToolStripMenuItem});
            this.开始编辑ToolStripMenuItem.Image = global::ArcEngineTest.Properties.Resources.EditingStartEditing16;
            this.开始编辑ToolStripMenuItem.Name = "开始编辑ToolStripMenuItem";
            this.开始编辑ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.开始编辑ToolStripMenuItem.Text = "开始编辑";
            this.开始编辑ToolStripMenuItem.Click += new System.EventHandler(this.开始编辑ToolStripMenuItem_Click_1);
            // 
            // 点ToolStripMenuItem
            // 
            this.点ToolStripMenuItem.Name = "点ToolStripMenuItem";
            this.点ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.点ToolStripMenuItem.Text = "点";
            this.点ToolStripMenuItem.Click += new System.EventHandler(this.点ToolStripMenuItem_Click);
            // 
            // 线ToolStripMenuItem
            // 
            this.线ToolStripMenuItem.Name = "线ToolStripMenuItem";
            this.线ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.线ToolStripMenuItem.Text = "线";
            this.线ToolStripMenuItem.Click += new System.EventHandler(this.线ToolStripMenuItem_Click);
            // 
            // 面ToolStripMenuItem
            // 
            this.面ToolStripMenuItem.Name = "面ToolStripMenuItem";
            this.面ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.面ToolStripMenuItem.Text = "面";
            this.面ToolStripMenuItem.Click += new System.EventHandler(this.面ToolStripMenuItem_Click);
            // 
            // 停止编辑ToolStripMenuItem
            // 
            this.停止编辑ToolStripMenuItem.Image = global::ArcEngineTest.Properties.Resources.EditingStopEditing16;
            this.停止编辑ToolStripMenuItem.Name = "停止编辑ToolStripMenuItem";
            this.停止编辑ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.停止编辑ToolStripMenuItem.Text = "停止编辑";
            // 
            // 保存编辑内容ToolStripMenuItem
            // 
            this.保存编辑内容ToolStripMenuItem.Image = global::ArcEngineTest.Properties.Resources.EditingSaveEdits16;
            this.保存编辑内容ToolStripMenuItem.Name = "保存编辑内容ToolStripMenuItem";
            this.保存编辑内容ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.保存编辑内容ToolStripMenuItem.Text = "保存编辑内容";
            // 
            // 移动ToolStripMenuItem
            // 
            this.移动ToolStripMenuItem.Name = "移动ToolStripMenuItem";
            this.移动ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.移动ToolStripMenuItem.Text = "移动";
            // 
            // 分割ToolStripMenuItem
            // 
            this.分割ToolStripMenuItem.Name = "分割ToolStripMenuItem";
            this.分割ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.分割ToolStripMenuItem.Text = "分割";
            // 
            // 构造点ToolStripMenuItem
            // 
            this.构造点ToolStripMenuItem.Name = "构造点ToolStripMenuItem";
            this.构造点ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.构造点ToolStripMenuItem.Text = "构造点";
            // 
            // 平行复制ToolStripMenuItem
            // 
            this.平行复制ToolStripMenuItem.Image = global::ArcEngineTest.Properties.Resources.EditingCopyParallel16;
            this.平行复制ToolStripMenuItem.Name = "平行复制ToolStripMenuItem";
            this.平行复制ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.平行复制ToolStripMenuItem.Text = "平行复制";
            // 
            // 合并ToolStripMenuItem
            // 
            this.合并ToolStripMenuItem.Name = "合并ToolStripMenuItem";
            this.合并ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.合并ToolStripMenuItem.Text = "合并";
            // 
            // 缓冲ToolStripMenuItem
            // 
            this.缓冲ToolStripMenuItem.Name = "缓冲ToolStripMenuItem";
            this.缓冲ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.缓冲ToolStripMenuItem.Text = "缓冲";
            // 
            // 联合ToolStripMenuItem
            // 
            this.联合ToolStripMenuItem.Name = "联合ToolStripMenuItem";
            this.联合ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.联合ToolStripMenuItem.Text = "联合";
            // 
            // 裁剪ToolStripMenuItem
            // 
            this.裁剪ToolStripMenuItem.Image = global::ArcEngineTest.Properties.Resources.EditingSaveEdits16;
            this.裁剪ToolStripMenuItem.Name = "裁剪ToolStripMenuItem";
            this.裁剪ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.裁剪ToolStripMenuItem.Text = "裁剪";
            // 
            // 捕捉ToolStripMenuItem
            // 
            this.捕捉ToolStripMenuItem.Name = "捕捉ToolStripMenuItem";
            this.捕捉ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.捕捉ToolStripMenuItem.Text = "捕捉";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::ArcEngineTest.Properties.Resources.EditingEditTool16;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::ArcEngineTest.Properties.Resources.EditingEditAnnotationTool16;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::ArcEngineTest.Properties.Resources.EditingAddStraightSegmentTool16;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::ArcEngineTest.Properties.Resources.EditingMidpointArcSegmentTool16;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "toolStripButton4";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = global::ArcEngineTest.Properties.Resources.EditingPointConstructor16;
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "toolStripButton6";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.Image = global::ArcEngineTest.Properties.Resources.EditingOffset16;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::ArcEngineTest.Properties.Resources.EditingEditVertices16;
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton7.Text = "toolStripButton7";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = global::ArcEngineTest.Properties.Resources.EditingReshapeFeatureTool16;
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton8.Text = "toolStripButton8";
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::ArcEngineTest.Properties.Resources.EditingSplitPolygonTool16;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton9.Text = "toolStripButton9";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::ArcEngineTest.Properties.Resources.EditingSplitLineTool16;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton10.Text = "toolStripButton10";
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Image = global::ArcEngineTest.Properties.Resources.EditingRotateTool16;
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton11.Text = "toolStripButton11";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton12.Text = "toolStripButton12";
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton13.Image = global::ArcEngineTest.Properties.Resources.EditingSketchPropertiesWindowShow16;
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton13.Text = "toolStripButton13";
            // 
            // toolStripButton14
            // 
            this.toolStripButton14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton14.Image = global::ArcEngineTest.Properties.Resources.EditingCreateFeaturesWindowShow16;
            this.toolStripButton14.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton14.Name = "toolStripButton14";
            this.toolStripButton14.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton14.Text = "toolStripButton14";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 494);
            this.Controls.Add(this.editTS);
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
            this.editTS.ResumeLayout(false);
            this.editTS.PerformLayout();
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
        private System.Windows.Forms.ToolStrip editTS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripButton toolStripButton14;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem 开始编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存编辑内容ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 分割ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 构造点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平行复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 合并ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缓冲ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 联合ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 裁剪ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 捕捉ToolStripMenuItem;

        private System.Windows.Forms.ToolStripButton openMapDoc;

        private System.Windows.Forms.ToolStripMenuItem 点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 面ToolStripMenuItem;

    }
}

