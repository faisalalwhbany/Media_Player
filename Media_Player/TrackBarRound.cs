using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SmartFplayer
{
    public enum HuePickerType
    {
        Value,
        Region
    }

    /// <summary>
    /// Summary description for HuePicker.
    /// </summary>
    public class TrackBareRound : System.Windows.Forms.Control
    {
        private HuePickerType type = HuePickerType.Value;

        private Pen blackPen;
        private Brush blackBrush;
        private Pen whitePen;
        private Brush whiteBrush;
        private Point ptCenter = new Point(0, 0);
        private Point ptMin = new Point(0, 0);
        private Point ptMax = new Point(0, 0);
        private int trackMode = 0;

        private int min = 0;
        private int max = 359;
        private int valueStart = 0;
        private int maximum = 359;

        // values changed event
        public event EventHandler ValuesChanged;

        private void SetValue()
        {
            int range = 359;
            double increment = (maximum) / range;
            if (type == HuePickerType.Value)
            {
                min = (int)(Math.Max(0, Math.Min(359, valueStart / increment)));
            }
            else
            {


                max = (int)(Math.Max(0, Math.Min(359, 359 - valueStart / increment)));
            }



        }

        [DefaultValue(0)]
        public int Value
        {
            get
            {
                int range = 359;
                double increment = (maximum) / range;
                if (type == HuePickerType.Value)
                {
                    return valueStart = (int)((min) * increment);
                }
                else
                {

                    return valueStart = (int)((359 - max) * increment);
                }
            }
            set
            {
                valueStart = Math.Max(Math.Min(value, maximum), 0);

                SetValue();
                Invalidate();


            }


        }


        public int Maxum
        {
            get { return maximum; }
            set
            {
                maximum = value;
                //SetValue();
                Invalidate();
            }

        }

        // Min property
        /* [DefaultValue(0)]
         public int Min
         {
             get { return min; }
             set
             {
                 int range = 359;
                 int increment = (Maxum-max) / range;
                 min = Math.Max(0, Math.Min(359, value));
                 Invalidate();
             }
         }
         // Max property
         [DefaultValue(359)]
         public int Max
         {
             get {

                 int range = 359;
                 int increment = (Maxum) / range;
                
                 return max; }
             set
             {

                 int range = 359;
                 int increment = (Maxum) / range;
                 max = Math.Max(0, Math.Min(359,359- value/increment));
                
                 Invalidate();
             }
         }*/
        // Type property
        [DefaultValue(HuePickerType.Value)]
        public HuePickerType Type
        {
            get { return type; }
            set
            {
                type = value;
                Invalidate();
            }
        }

        // Constructor
        public TrackBareRound()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor, true);

            blackPen = new Pen(Color.Black, 1);
            blackBrush = new SolidBrush(Color.Black);
            whitePen = new Pen(Color.White, 1);
            whiteBrush = new SolidBrush(Color.White);
            //this.BackColor = Color.Transparent;
        }

        // Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                blackPen.Dispose();
                blackBrush.Dispose();
                whitePen.Dispose();
                whiteBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        // Init component
        private void InitializeComponent()
        {
            // 
            // HSLPicker
            // 
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HSLPicker_MouseUp);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HSLPicker_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HSLPicker_MouseDown);

        }

        // Paint control
        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
            Rectangle rc = this.ClientRectangle;
            Rectangle rcPie;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //Brush brush;
            LinearGradientBrush lgb;
            rcPie = new Rectangle(4, 4, Math.Min(rc.Right, rc.Bottom) - 8, Math.Min(rc.Right, rc.Bottom) - 8);



            if (type == HuePickerType.Value)
            {
                // draw HSL pie
                for (int i = 0; i < 360; i++)
                {
                    lgb = new LinearGradientBrush(rcPie, Color.Red, Color.DarkRed, 90, true);
                    //brush = new SolidBrush(Color.Red);
                    // draw one hue value
                    g.FillPie(lgb, rcPie, i, -1);

                    //brush.Dispose();
                    lgb.Dispose();
                }
            }
            else
            {
                // draw HSL pie
                for (int i = 0; i < 360; i++)
                {
                    if (((min < max) && (i >= min) && (i <= max)) ||
                        ((min > max) && ((i >= min) || (i <= max))))
                    {
                        lgb = new LinearGradientBrush(rcPie, Color.Gray, Color.DarkGray, 360, true);
                        //lgb.SetSigmaBellShape(0.5f);
                        //brush = new SolidBrush(Color.Gray);
                    }
                    else
                    {
                        lgb = new LinearGradientBrush(rcPie, Color.DarkBlue, Color.Cyan, 360, true);
                        //lgb.SetBlendTriangularShape(0.5F);
                        //lgb.SetSigmaBellShape(0.5f);
                        //brush = new SolidBrush(Color.DarkBlue);
                    }


                    g.FillPie(lgb, rcPie, -i, -1);
                    lgb.Dispose();
                    //brush.Dispose();
                }
            }

            //
            double halfWidth = (double)rcPie.Width / 2;
            double angleRad = -min * Math.PI / 180;
            double angleCos = Math.Cos(angleRad);
            double angleSin = Math.Sin(angleRad);

            double x = halfWidth * angleCos;
            double y = halfWidth * angleSin;

            ptCenter.X = rcPie.Left + (int)(halfWidth);
            ptCenter.Y = rcPie.Top + (int)(halfWidth);
            ptMin.X = rcPie.Left + (int)(halfWidth + x);
            ptMin.Y = rcPie.Top + (int)(halfWidth + y);

            // draw MIN pointer
            g.FillEllipse(blackBrush,
                rcPie.Left + (int)(halfWidth + x) - 4,
                rcPie.Top + (int)(halfWidth + y) - 4,
                8, 8);
            g.DrawLine(blackPen, ptCenter, ptMin);

            // check picker type
            if (type == HuePickerType.Region)
            {
                angleRad = -max * Math.PI / 180;
                angleCos = Math.Cos(angleRad);
                angleSin = Math.Sin(angleRad);

                x = halfWidth * angleCos;
                y = halfWidth * angleSin;

                ptMax.X = rcPie.Left + (int)(halfWidth + x);
                ptMax.Y = rcPie.Top + (int)(halfWidth + y);

                // draw MAX pointer
                g.FillEllipse(whiteBrush,
                    rcPie.Left + (int)(halfWidth + x) - 4,
                    rcPie.Top + (int)(halfWidth + y) - 4,
                    8, 8);
                g.DrawLine(whitePen, ptCenter, ptMax);
            }

            base.OnPaint(pe);
        }

        // On mouse down
        private void HSLPicker_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // check coordinates of MIN pointer
            if ((e.X >= ptMin.X - 4) && (e.Y >= ptMin.Y - 4) &&
                (e.X < ptMin.X + 4) && (e.Y < ptMin.Y + 4))
            {
                trackMode = 1;
            }
            if (type == HuePickerType.Region)
            {
                // check coordinates of MAX pointer
                if ((e.X >= ptMax.X - 4) && (e.Y >= ptMax.Y - 4) &&
                    (e.X < ptMax.X + 4) && (e.Y < ptMax.Y + 4))
                {
                    trackMode = 2;
                }
            }

            if (trackMode != 0)
                this.Capture = true;
        }

        // On mouse up
        private void HSLPicker_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (trackMode != 0)
            {
                // release capture
                this.Capture = false;
                trackMode = 0;

                // notify client
                if (ValuesChanged != null)
                    ValuesChanged(this, new EventArgs());
            }
        }



        // On mouse move
        private void HSLPicker_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (trackMode != 0)
            {
                int dy = e.Y - ptCenter.Y;
                int dx = e.X - ptCenter.X;

                if (trackMode == 1)
                {
                    // MIN pointer tracking
                    if (dx == 0)
                    {
                        min = (dy < 0) ? 90 : 270;
                    }
                    else
                    {
                        min = ((int)(-Math.Atan((double)dy / dx) * 180 / Math.PI));

                        if (dx < 0)
                            min = 180 + min;
                        if (min < 0)
                            min = 360 + min;
                    }
                }
                else
                {
                    // MAX pointer tracking
                    if (dx == 0)
                    {
                        max = (dy < 0) ? 90 : 270;
                    }
                    else
                    {
                        max = ((int)(-Math.Atan((double)dy / dx) * 180 / Math.PI));

                        if (dx < 0)
                            max = 180 + max;
                        if (max < 0)
                            max = 360 + max;
                    }
                }

                // repaint control
                Invalidate();
            }
        }
    }
}
