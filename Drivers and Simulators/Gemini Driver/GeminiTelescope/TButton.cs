using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ASCOM.GeminiTelescope
{

    public class TButton : System.Windows.Forms.Button
    {
        public TButton()
            : base()
        {

            Point[] pts = { new Point(0, 31), new Point(31, 0), new Point(63, 31), new Point(31, 63) };
            GraphicsPath p = new GraphicsPath();
            p.AddPolygon(pts);
            p.CloseFigure();
            p.FillMode = FillMode.Alternate;
            this.Region = new Region(p);

            ImageList = new System.Windows.Forms.ImageList();
            ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            ImageList.ImageSize = new System.Drawing.Size(64, 64);


            ImageList.Images.Add(((System.Drawing.Image)(Resource1.ResourceManager.GetObject("diamond1"))));
            ImageList.Images.Add(((System.Drawing.Image)(Resource1.ResourceManager.GetObject("diamond1_down"))));
            ImageIndex = 0;

        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
        {
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs mevent)
        {
            this.ImageIndex = 1;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs mevent)
        {
            this.ImageIndex = 0;
            base.OnMouseUp(mevent);
        }
    }
}
