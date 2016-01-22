using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

using Win32;
namespace SmartFplayer
{

   

    public partial class VISTA_Track : UserControl
    {
        public enum toolTipVewo:byte
        {
            none,
            timespine,
            average
           
        }
        public enum TrickType:byte
        {
            Round,
            Rect

        }
    
        private toolTipVewo typeveowtool = toolTipVewo.none;
       // private string tooltiptext = "";
        private const bool Vert = false;
        private const bool Horz = true;
       // private bool showTextToolTip=false;
        private Size sizTrick ;//= new Size(10, 10);
        private double value = 50;
        private double minimum = 0;
        private double maximum = 100;
        private double loopstart1 = 0;
        private double loopend1 = 0;
        private int rangee = 0;
        private double inc = 0;
        private double dra = 0;
        private double dra2 = 0;
        private TrickType tricktype = TrickType.Round;
        private bool ThumbMoving = false;
        private static int WasValue = 0;

        private Color color_LEFTorTOPblue = Color.FromArgb(95, 140, 180); //(95, 140, 180)
        private Color color_LEFTorTOPdark = Color.FromArgb(55, 60, 74); //(55, 60, 74)
        private Color color_MIDDLEblue = Color.FromArgb(21, 56, 152); //(21, 56, 152)

        private Color color_MIDDLEdark = Color.FromArgb(0, 0, 0); //(0, 0, 0)
        private Color color_RIGHTorBOTTOMblue = Color.FromArgb(99, 130, 208); //(99, 130, 208)
        private Color color_RIGHTorBOTTOMdark = Color.FromArgb(87, 94, 110); //(87, 94, 110)

        private Color color_Trick1 = Color.FromArgb(255, Color.Red);
        private Color color_Trick2 = Color.FromArgb(200, Color.Cyan);

        public VISTA_Track()
        {
            InitializeComponent();
            sizTrick = BLUE_Thumb.Size;
            
        }

        private void TRACK_Load(object sender, EventArgs e)
        {
            if (minimum == 0 && maximum == 0)
            {
                // Set default value
                value = 50;
                if (Orientation() == Horz)
                { minimum = 0; maximum = 100; }
                else
                { minimum = 100; maximum = 0; }
            }
            // FORM_Tooltip colors
            //toolTip.BackColor = SK.TooltipBackColor;
            //toolTip.ForeColor = SK.TooltipForeColor;
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                
                rangee = ClientRectangle.Width;
                inc = (maximum - minimum) / rangee;
                dra = (loopend1) / inc;
                dra2 = (loopstart1) / inc;
                //float fx = BLUE_Thumb.Width * .50f;
               // float fy = BLUE_Thumb.Height * .40f;
                //Bitmap offScreenBmp;
                //offScreenBmp = new Bitmap(this.Width, this.Height);
                //Graphics g = Graphics.FromImage(offScreenBmp);
                Rectangle destRect1 = new Rectangle(BLUE_Thumb.Left, BLUE_Thumb.Top, BLUE_Thumb.Width, BLUE_Thumb.Height);

                 GraphicsPath path = new GraphicsPath();
                

                    if (tricktype == TrickType.Round)
                    {
                        path.AddEllipse(destRect1);
                    }
                    else
                    {
                        path.AddRectangle(destRect1);
                    }
                    Region reg = new Region(path);


                  LinearGradientBrush lgb = new LinearGradientBrush(destRect1,
                       color_Trick1, color_Trick2, 90, true);
                  lgb.SetBlendTriangularShape(0.5f);

                        //Graphics g = BLUE_Thumb.CreateGraphics())
                        //g.FillRegion(lgb, reg);

                        BLUE_Thumb.Region = reg;
                    
                
                
                switch (m.Msg)
                {
                    case Api.WM_ERASEBKGND:
                        Bitmap bmp;
                        Rectangle srceRect;
                        Rectangle destRect;


                    
                        // Create a memory bitmap to use as double buffer
                        Bitmap offScreenBmp;
                        offScreenBmp = new Bitmap(this.Width, this.Height);
                       Graphics g = Graphics.FromImage(offScreenBmp);
                       //g.SmoothingMode = SmoothingMode.AntiAlias;
                        if (this.BackgroundImage != null)
                        {
                            bmp = new Bitmap(this.BackgroundImage);
                            srceRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                            destRect = new Rectangle(0, 0, this.Width + 100, this.Height);
                            g.DrawImage(bmp, destRect, srceRect, GraphicsUnit.Pixel);
                            bmp.Dispose();
                        }
                        else
                        {
                            SolidBrush myBrush = new SolidBrush(this.BackColor);
                            g.FillRectangle(myBrush, 0, 0, this.Width, this.Height);
                            myBrush.Dispose();

                            
                           
                        }

                        Pen LEFTorTOPblue = new Pen(color_LEFTorTOPblue);
                        Pen LEFTorTOPdark = new Pen(color_LEFTorTOPdark);
                        Pen MIDDLEblue = new Pen(color_MIDDLEblue);

                        Pen MIDDLEdark = new Pen(color_MIDDLEdark);
                        Pen RIGHTorBOTTOMblue = new Pen(color_RIGHTorBOTTOMblue);
                        Pen RIGHTorBOTTOMdark = new Pen(color_RIGHTorBOTTOMdark);
                        Pen loopred=new Pen(Color.Red);

                        if (Orientation() == Horz)
                        {
                            int y = ClientRectangle.Height / 2;
                            if (y * 2 < ClientRectangle.Height) y -= 1;

                            if (Minimum > Maximum)
                            {
                                g.DrawLine(LEFTorTOPdark, new Point(0, y - 1), new Point(BLUE_Thumb.Left + BLUE_Thumb.Width / 2, y - 1));
                                g.DrawLine(LEFTorTOPblue, new Point(BLUE_Thumb.Left + 1 + BLUE_Thumb.Width / 2, y - 1), new Point(ClientRectangle.Width, y - 1));
                                g.DrawLine(MIDDLEdark, new Point(0, y), new Point(BLUE_Thumb.Left + BLUE_Thumb.Width / 2, y));
                                g.DrawLine(MIDDLEblue, new Point(BLUE_Thumb.Left + 1 + BLUE_Thumb.Width / 2, y), new Point(ClientRectangle.Width, y));
                                g.DrawLine(RIGHTorBOTTOMdark, new Point(0, y + 1), new Point(BLUE_Thumb.Left + BLUE_Thumb.Width / 2, y + 1));
                                g.DrawLine(RIGHTorBOTTOMblue, new Point(BLUE_Thumb.Left + 1 + BLUE_Thumb.Width / 2, y + 1), new Point(ClientRectangle.Width, y + 1));
                            }
                            else
                            {
                                g.DrawLine(LEFTorTOPblue, new Point(0, y - 1), new Point(BLUE_Thumb.Left + BLUE_Thumb.Width / 2, y - 1));
                                g.DrawLine(LEFTorTOPdark, new Point(BLUE_Thumb.Left + 1 + BLUE_Thumb.Width / 2, y - 1), new Point(ClientRectangle.Width, y - 1));
                                g.DrawLine(MIDDLEblue, new Point(0, y), new Point(BLUE_Thumb.Left + BLUE_Thumb.Width / 2, y));
                                g.DrawLine(MIDDLEdark, new Point(BLUE_Thumb.Left + 1 + BLUE_Thumb.Width / 2, y), new Point(ClientRectangle.Width, y));
                                g.DrawLine(RIGHTorBOTTOMblue, new Point(0, y + 1), new Point(BLUE_Thumb.Left + BLUE_Thumb.Width / 2, y + 1));
                                g.DrawLine(RIGHTorBOTTOMdark, new Point(BLUE_Thumb.Left + 1 + BLUE_Thumb.Width / 2, y + 1), new Point(ClientRectangle.Width, y + 1));
                                g.DrawLine(loopred, (int)(dra), y,(int)dra2, y);
                            }
                        }
                        else
                        {
                            int x = ClientRectangle.Width / 2;

                            if (Minimum > Maximum)
                            {
                                g.DrawLine(LEFTorTOPdark, new Point(x - 1, 0), new Point(x - 1, BLUE_Thumb.Top + BLUE_Thumb.Width / 2));
                                g.DrawLine(LEFTorTOPblue, new Point(x - 1, BLUE_Thumb.Top + 1 + BLUE_Thumb.Width / 2), new Point(x - 1, ClientRectangle.Height));
                                g.DrawLine(MIDDLEdark, new Point(x, 0), new Point(x, BLUE_Thumb.Top + BLUE_Thumb.Width / 2));
                                g.DrawLine(MIDDLEblue, new Point(x, BLUE_Thumb.Top + 1 + BLUE_Thumb.Width / 2), new Point(x, ClientRectangle.Height));
                                g.DrawLine(RIGHTorBOTTOMdark, new Point(x + 1, 0), new Point(x + 1, BLUE_Thumb.Top + BLUE_Thumb.Width / 2));
                                g.DrawLine(RIGHTorBOTTOMblue, new Point(x + 1, BLUE_Thumb.Top + 1 + BLUE_Thumb.Width / 2), new Point(x + 1, ClientRectangle.Height));
                            }
                            else
                            {
                                g.DrawLine(LEFTorTOPblue, new Point(x - 1, 0), new Point(x - 1, BLUE_Thumb.Top + BLUE_Thumb.Width / 2));
                                g.DrawLine(LEFTorTOPdark, new Point(x - 1, BLUE_Thumb.Top + 1 + BLUE_Thumb.Width / 2), new Point(x - 1, ClientRectangle.Height));
                                g.DrawLine(MIDDLEblue, new Point(x, 0), new Point(x, BLUE_Thumb.Top + BLUE_Thumb.Width / 2));
                                g.DrawLine(MIDDLEdark, new Point(x, BLUE_Thumb.Top + 1 + BLUE_Thumb.Width / 2), new Point(x, ClientRectangle.Height));
                                g.DrawLine(RIGHTorBOTTOMblue, new Point(x + 1, 0), new Point(x + 1, BLUE_Thumb.Top + BLUE_Thumb.Width / 2));
                                g.DrawLine(RIGHTorBOTTOMdark, new Point(x + 1, BLUE_Thumb.Top + 1 + BLUE_Thumb.Width / 2), new Point(x + 1, ClientRectangle.Height));
                            }
                        }

                        // Draw thumb tracker
                      //bmp = new Bitmap(BLUE_Thumb.BackgroundImage);
                       bmp = new Bitmap(BLUE_Thumb.Width,BLUE_Thumb.Height);
                        bmp.MakeTransparent(Color.FromArgb(255, 0, 255));
                        srceRect = new Rectangle(0, 0, BLUE_Thumb.Width, BLUE_Thumb.Height);
                        destRect = new Rectangle(BLUE_Thumb.Left, BLUE_Thumb.Top, BLUE_Thumb.Width, BLUE_Thumb.Height);
                         path = new GraphicsPath();
                        if (tricktype == TrickType.Round)
                        {
                            path.AddEllipse(destRect);
                        }
                        else
                        {
                            path.AddRectangle(destRect);
                        }
                        reg = new Region(path);
                        lgb = new LinearGradientBrush(destRect,
                              color_Trick1, color_Trick2, 90,true);
                        //lgb.SetBlendTriangularShape(0.5f);
                       
                                            g.FillRegion(lgb, reg);
                        BLUE_Thumb.Region = reg;
                       g.DrawImage(bmp, destRect, srceRect, GraphicsUnit.Pixel);
                        path.Dispose();
                        reg.Dispose();
                        //borderBrush.Dispose();
                        
                        lgb.Dispose();
                        bmp.Dispose();

                        
                            
                        

                        // Release pen resources
                        LEFTorTOPblue.Dispose();
                        LEFTorTOPdark.Dispose();
                        MIDDLEblue.Dispose();
                        MIDDLEdark.Dispose();
                        RIGHTorBOTTOMblue.Dispose();
                        RIGHTorBOTTOMdark.Dispose();

                        // Release graphics
                        g.Dispose();

                        // Swap memory bitmap (End double buffer)
                        g = Graphics.FromHdc(m.WParam);
                        g.DrawImage(offScreenBmp, 0, 0);
                        g.Dispose();
                        offScreenBmp.Dispose();

                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            catch { };
        }

//----------------------------------------
        [Category("Action"), Description("Occurs when the slider is moved")]
        public event EventHandler ValueChanged;
        [Category("Action"), Description("Occurs when the mouseUp")]
        public event EventHandler MouseUped;
        private void OnMouseup()
        {
            if (MouseUped != null)
            {
                MouseUped(this, new EventArgs());
            }
            else
            {
                Api.SendMessage(Api.GetForegroundWindow(), Api.WM_COMMAND, (uint)this.Handle, (int)this.Handle);
            }
        }
        private void SendNotification()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, new EventArgs());
            }
            else
            {
                Api.SendMessage(Api.GetForegroundWindow(), Api.WM_COMMAND, (uint)this.Handle, (int)this.Handle);
            }
        }
//----------------------------------------

        private void SetThumbLocation()
        {
            Point pos = PointToClient(Cursor.Position);

            if (Orientation() == Horz)
            {
                BLUE_Thumb.Left = Math.Min(Math.Max(pos.X - BLUE_Thumb.Width / 2, 0), BLUE_Thumb.Parent.Width - BLUE_Thumb.Width);
                BLUE_Thumb.Top = ((ClientRectangle.Height - BLUE_Thumb.Width ) / 2);
                int range = ClientRectangle.Width - BLUE_Thumb.Width;
                double increment = (maximum - minimum) / range;
                value = (increment * BLUE_Thumb.Left) + minimum;
                
                

            }
            else
            {
                BLUE_Thumb.Left = (ClientRectangle.Width - BLUE_Thumb.Width) / 2 + 1;
                BLUE_Thumb.Top = Math.Min(Math.Max(pos.Y - BLUE_Thumb.Height / 2, 0), BLUE_Thumb.Parent.Height - BLUE_Thumb.Height);

                int range = ClientRectangle.Height - BLUE_Thumb.Height;
                double increment = (maximum - minimum) / range;
                value = (increment * BLUE_Thumb.Top) + minimum;
            }
            Value = value;

            if (WasValue != (int)value)
            {
                this.Invalidate();
                SendNotification();
                WasValue = (int)value;

                if (CheckOverThumb(pos.X, pos.Y))
                {
                    switch(typeveowtool)
                    { case toolTipVewo.none:
                        
                    toolTip.SetToolTip(BLUE_Thumb, ((int)value).ToString());
                    toolTip.SetToolTip(this, ((int)value).ToString());
                    break;
                        case toolTipVewo.average:
                    int evrg =(int)(Math.Ceiling((value / maximum) * 100));

                            toolTip.SetToolTip(BLUE_Thumb, evrg.ToString() + "%");
                            toolTip.SetToolTip(this, evrg.ToString() + "%");
                    break;
                        case toolTipVewo.timespine:
                            int m = 0;
                           int s = 0;
                           int h = 0;

                           h = Convert.ToInt32(Math.Floor(value / 3600000));
                           m = Convert.ToInt32(Math.Floor((value - (h * 3600000)) / 60000));
                           s = Convert.ToInt32((value - ((h * 3600000) + (60000 * m))) / 1000);

                    TimeSpan sp = new TimeSpan(h,m,s);
                            toolTip.SetToolTip(BLUE_Thumb, sp.ToString());
                            toolTip.SetToolTip(this, sp.ToString());
                    break;

                }
                }
            }
        }

        private void THUMB_MouseDown(object sender, MouseEventArgs e)
        {
            ThumbMoving = true;
        }
        
        private bool CheckOverThumb(int x, int y)
        {
            Api.RECT r = new Api.RECT();
            r.left = BLUE_Thumb.Left;
            r.top = BLUE_Thumb.Top;
            r.right = r.left + BLUE_Thumb.Width;
            r.bottom = r.top + BLUE_Thumb.Height;
            Api.POINT p = new Api.POINT();
            p.x = x; p.y = y;
            return (Api.PtInRect(ref r, p));
        }

        private void THUMB_MouseMove(object sender, MouseEventArgs e)
        {
            
           if (ThumbMoving) SetThumbLocation();

            //BUG Vista, the animation stops while the tooltip is being shown!
            //----------------------------------------------------------------
            if (CheckOverThumb(e.X, e.Y) == false)
            {
                
                
            }

        }

        private void TRACK_MouseDown(object sender, MouseEventArgs e)
        {
            ThumbMoving = true;
            SetThumbLocation();
        }

        private void THUMB_MouseUp(object sender, MouseEventArgs e)
        {
            OnMouseup();
            ThumbMoving = false;
        }

        // Retrieve the control orientation
        private bool Orientation()
        {
            bool orientation = Vert;
            if (this.Width > this.Height) orientation = Horz;
            return orientation;
        }

        #region property

        /*
        public bool ShowTextToolTip
        {
            get { return showTextToolTip; }
            set
            {
                showTextToolTip = value;
                this.Invalidate();
            }
        }
        public string ToolTipText
        {
            get {return tooltiptext;}
            set
            {
                tooltiptext = value;
                this.Invalidate();
            }

        }*/

        public Color Color_Trick1
        {
            get { return color_Trick1; }
            set
            {
                color_Trick1 = value;
                this.Invalidate();
            }
        }

        public Color Color_Trick2
        {
            get { return color_Trick2; }
            set
            {
                color_Trick2 = value;
                this.Invalidate();
            }
        }

        public toolTipVewo ToolTipVewo
        {
            get { return typeveowtool; }
            set
            {
                typeveowtool = value;
                this.Invalidate();

            }
        }

        public Size SizeTrick
        {

            get { return sizTrick; }
            set
            {
                sizTrick = value;
                BLUE_Thumb.Size = sizTrick;
                this.Invalidate();
            }
        }

        public TrickType TrickTyp
        {
            get { return tricktype; }
            set
            {
                tricktype = value;
                this.Invalidate();
            }
        }


        public double LoopStart1
        {
            get { return loopstart1;
            
            }
            set { loopstart1 = value;
                this.Refresh(); }
        }
        public double LoopEnd1
        {
            get { return loopend1; }
            set { loopend1 = value;
                this.Refresh(); }
            
        }


        public Color color_LEFTorTOPblue2
        {
            get
            {
                return color_LEFTorTOPblue;
            }
            set
            {
                color_LEFTorTOPblue = value;
                this.Invalidate();
            }
           
        }

        public Color color_LEFTorTOPdark2
        {
            get
            {
                return color_LEFTorTOPdark;
            }
            set
            {
                color_LEFTorTOPdark = value;
                this.Invalidate();
            }
        }



        public Color color_MIDDLEblue2
        {
            get
            {
                return color_MIDDLEblue;
            }
            set
            {
                color_MIDDLEblue = value;
                this.Invalidate();
            }
        }


        public Color color_MIDDLEdark2
        {
            get
            {
                return color_MIDDLEdark;
            }
            set
            {
                color_MIDDLEdark = value;
                this.Invalidate();
            }
        }

        public Color color_RIGHTorBOTTOMblue2
        {
            get
            {
                return color_RIGHTorBOTTOMblue;
            }
            set
            {
                color_RIGHTorBOTTOMblue = value;
                this.Invalidate();
            }
        }


        public Color color_RIGHTorBOTTOMdark2
        {
            get
            {
                return color_RIGHTorBOTTOMdark;
            }
            set
            {
                color_RIGHTorBOTTOMdark = value;
                this.Invalidate();
            }
        }

        public double Minimum
        {
            get
            {
                return (minimum);
            }
            set
            {
                double minimumBackup = minimum;
                minimum = value;
                ShowThumbPos();
            }
        }

        public double Maximum
        {
            get
            {
                return (maximum);
            }
            set
            {
                double maximumBackup = maximum;
                maximum = value;
                ShowThumbPos();
            }
        }

        public double Value
        {
            get
            {
                return (value);
            }
            set
            {
                double valueBackup = this.value;
                if (minimum > maximum)
                {
                    this.value = Math.Max(Math.Min(value, minimum), maximum);
                }
                else
                {
                    this.value = Math.Max(Math.Min(value, maximum), minimum);
                }
                ShowThumbPos();
            }
        }

        #endregion //property

        private void ShowThumbPos()
        {
            if (Orientation() == Horz)
            {
                BLUE_Thumb.Top = (ClientRectangle.Height - BLUE_Thumb.Width) / 2;
                int range = ClientRectangle.Width - BLUE_Thumb.Width;
                double increment = (maximum - minimum) / range;
                if (increment == 0) {
                    BLUE_Thumb.Left = 0; }
                else {
                BLUE_Thumb.Left = (int)((value - minimum) / increment);
                }
            }
            else
            {
                BLUE_Thumb.Left = (ClientRectangle.Width - BLUE_Thumb.Width) / 2 + 1;
                int range = ClientRectangle.Height - BLUE_Thumb.Height;
                double increment = (maximum - minimum) / range;
                if (increment == 0) { 
                    BLUE_Thumb.Top = 0; }
                else {
                BLUE_Thumb.Top = (int)((value - minimum) / increment);
                }
            }
            this.Invalidate();
        }

        #region paint
        private void BLUE_Thumb_Paint(object sender, PaintEventArgs e)
        {
           try
            {
                
                GraphicsPath path = new GraphicsPath();
                Rectangle rec = new Rectangle(BLUE_Thumb.Left, BLUE_Thumb.Top, BLUE_Thumb.Width, BLUE_Thumb.Height);
                if (tricktype == TrickType.Round)
                {
                    path.AddEllipse(rec);
                }
                else
                {
                    path.AddRectangle(rec);
                }
                Region reg = new Region(path);
                float fx = BLUE_Thumb.Width * .50f;
                float fy = BLUE_Thumb.Height * .40f;
               
                using (LinearGradientBrush lgb = new LinearGradientBrush(rec,
                             color_Trick1, color_Trick2, 90, true))
                {
                    //lgb.SetBlendTriangularShape(0.5f);
                    //Blend blend = new Blend();

                    //blend.Factors = new float[] { .2f, .3f, .9f, .9f, .3f, .2f };

                    //blend.Positions = new float[] { 0.0f, .2f, .4f, .6f, .8f, 1.0f };
                    //lgb.Blend = blend;
                    //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillRegion(lgb, reg);
                }

                BLUE_Thumb.Region = reg;
            }
            catch { }
        }

        #endregion //paint

        


        //private void toolTip_Popup(object sender, PopupEventArgs e)
        //{

        //}

    }
}
