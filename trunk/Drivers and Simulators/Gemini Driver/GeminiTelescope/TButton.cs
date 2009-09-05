using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ASCOM.GeminiTelescope
{

    public class TButton : System.Windows.Forms.Button
    {
        const int _buttonsize = 64;

        public TButton()
            : base()
        {

            Point[] pts = {   new Point(0, _buttonsize / 2 - 1), 
                              new Point(_buttonsize / 2 - 1, 0), 
                              new Point(_buttonsize - 1, _buttonsize / 2 - 1), 
                              new Point(_buttonsize / 2 - 1, _buttonsize - 1) };

            GraphicsPath p = new GraphicsPath();
            p.AddPolygon(pts);
            p.CloseFigure();
            p.FillMode = FillMode.Alternate;
            this.Region = new Region(p);

            ImageList = new System.Windows.Forms.ImageList();
            ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            ImageList.ImageSize = new System.Drawing.Size(_buttonsize, _buttonsize);


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
