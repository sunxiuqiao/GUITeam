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
        private RibbonTab ribbonTab1;
        private RibbonPanel ribbonPanel1;
        private RibbonPanel ribbonPanel2;
        private RibbonWrappedDropDown ribbonWrappedDropDown1;

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
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.ribbonWrappedDropDown1 = new System.Windows.Forms.RibbonWrappedDropDown();
            this.SuspendLayout();
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Panels.Add(this.ribbonPanel1);
            this.ribbonTab1.Panels.Add(this.ribbonPanel2);
            this.ribbonTab1.Text = "ribbonTab1";
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.Text = "ribbonPanel1";
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.Text = "ribbonPanel2";
            // 
            // ribbonWrappedDropDown1
            // 
            this.ribbonWrappedDropDown1.AutoSize = false;
            this.ribbonWrappedDropDown1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.ribbonWrappedDropDown1.Name = "ribbonWrappedDropDown1";
            // 
            // RibbonForm
            // 
            this.ClientSize = new System.Drawing.Size(741, 280);
            this.Name = "RibbonForm";
            this.Load += new System.EventHandler(this.RibbonForm_Load);
            this.ResumeLayout(false);

        }

        private void RibbonForm_Load(object sender, EventArgs e)
        {

        }
    }
}
