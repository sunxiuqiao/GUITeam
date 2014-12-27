// *********************************
// Message from Original Author:
//
// 2008 Jose Menendez Poo
// Please give me credit if you use this code. It's all I ask.
// Contact me for more info: menendezpoo@gmail.com
// *********************************
//
// Original project from http://ribbon.codeplex.com/
// Continue to support and maintain by http://officeribbon.codeplex.com/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.RibbonHelpers;

namespace System.Windows.Forms
{
    public class RibbonForm
        : Form, IRibbonForm
    {
        private Ribbon ribbon1;
        private RibbonOrbMenuItem ribbonOrbMenuItem1;
        private RibbonButton ribbonButton1;
        private RibbonTab ribbonTab1;
        private RibbonTab ribbonTab2;

        #region Fields

        private RibbonFormHelper _helper;

        #endregion

        #region Ctor

        public RibbonForm()
        {
           if (WinApi.IsWindows && !WinApi.IsGlassEnabled)
           {
              FormBorderStyle = FormBorderStyle.None;
              SetStyle(ControlStyles.ResizeRedraw, true);
              SetStyle(ControlStyles.Opaque, WinApi.IsGlassEnabled);
              SetStyle(ControlStyles.AllPaintingInWmPaint, true);
              DoubleBuffered = true;
           }
           _helper = new RibbonFormHelper(this);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Just for debugging messages
        /// </summary>
        /// <param name="m"></param>
        protected override void OnNotifyMessage(Message m)
        {
            base.OnNotifyMessage(m);
        }

        /// <summary>
        /// Overrides the WndProc funciton
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
           if (!Helper.WndProc(ref m))
            {
                base.WndProc(ref m);
            }
        }

        protected override CreateParams CreateParams
        {
           get
           {
              CreateParams cp = base.CreateParams;
              if (WinApi.IsWindows && !WinApi.IsGlassEnabled)
              {
                 cp.Style |= 0x20000 | 0x80000 | 0x40000; //WS_MINIMIZEBOX | WS_SYSMENU | WS_SIZEBOX;
                 //cp.ClassStyle |= 0x8 | 0x20000; //CS_DBLCLKS | CS_DROPSHADOW;
              }
              return cp;
           }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
           // override OnPaint and do NOT call base, otherwise problems as MDI parent occur
           _helper.Form_Paint(this, e);
        }

        #endregion

        #region IRibbonForm Members

        /// <summary>
        /// Gets the helper for making the form a ribbon form
        /// </summary>
        public RibbonFormHelper Helper
        {
            get { return _helper; }
        }

        #endregion

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RibbonForm));
            this.ribbon1 = new System.Windows.Forms.Ribbon();
            this.ribbonButton1 = new System.Windows.Forms.RibbonButton();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonTab2 = new System.Windows.Forms.RibbonTab();
            this.ribbonOrbMenuItem1 = new System.Windows.Forms.RibbonOrbMenuItem();
            this.SuspendLayout();
            // 
            // ribbon1
            // 
            this.ribbon1.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuItem1);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 116);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.OrbImage = null;
            this.ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2013;
            this.ribbon1.OrbText = "hehe";
            // 
            // 
            // 
            this.ribbon1.QuickAcessToolbar.Items.Add(this.ribbonButton1);
            this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbon1.Size = new System.Drawing.Size(845, 118);
            this.ribbon1.TabIndex = 0;
            this.ribbon1.Tabs.Add(this.ribbonTab1);
            this.ribbon1.Tabs.Add(this.ribbonTab2);
            this.ribbon1.TabsMargin = new System.Windows.Forms.Padding(12, 26, 20, 0);
            this.ribbon1.Text = "File";
            this.ribbon1.ThemeColor = System.Windows.Forms.RibbonTheme.Blue;
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.Image")));
            this.ribbonButton1.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Compact;
            this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
            this.ribbonButton1.Text = "ribbonButton1";
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Text = "Start";
            // 
            // ribbonTab2
            // 
            this.ribbonTab2.Text = "Insert";
            // 
            // ribbonOrbMenuItem1
            // 
            this.ribbonOrbMenuItem1.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonOrbMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuItem1.Image")));
            this.ribbonOrbMenuItem1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuItem1.SmallImage")));
            this.ribbonOrbMenuItem1.Text = "ribbonOrbMenuItem1";
            // 
            // RibbonForm
            // 
            this.ClientSize = new System.Drawing.Size(845, 476);
            this.Controls.Add(this.ribbon1);
            this.Name = "RibbonForm";
            this.ResumeLayout(false);

        }
    }
}
