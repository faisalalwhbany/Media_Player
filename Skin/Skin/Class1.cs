
/*faisal alwhbany*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Skin
{

   class Api
    {
        public const int WM_ERASEBKGND = 0x14;
    }



    public class skine
    {

        #region variable

        
        private Icon ico;
        private bool sizeT = false;
        private bool sizeT_L = false;
        //private bool isActive = false;
        private bool sizeB = false;
        private bool sizeR = false;
        private bool sizeL = false;
        private bool sizeL_B = false;
        private bool sizeT_R = false;
        private bool sizeR_B = false;
        private bool Move = false;
        private bool Maxim = false;
        private bool controlBox = true;
        private bool maxmieze = false;
        //private bool Holde = false;
        private string text = "";


        private bool isClosing = false;
        private bool isMinimizing = false;
        //private Graphics g;
        public static bool isActive = true;

        private int Border_Size = 3;
        private int Panel_Move_Size = 29;
        private Color m_TextColor = Color.Black;
        private  Color Border_Top_light = Color.FromArgb(255, 0, 245, 255);
        private  Color Border_Top_Dark = Color.FromArgb(255, 51, 102, 204);
        private  Color Border_Bottom_light = Color.FromArgb(255, 0, 245, 255);
        private  Color Border_Bottom_Dark = Color.FromArgb(255, 51, 102, 204);
        private  Color Border_LR_light = Color.FromArgb(255, 0, 245, 255);
        private  Color Border_LR_Dark = Color.FromArgb(255, 51, 102, 204);
        private  Color Panel_Move_light = Color.FromArgb(255, 0, 245, 255);
        private  Color Panel_Move_Dark = Color.FromArgb(255, 51, 102, 204);
        private Font m_Font = new Font("Vani", 9f, System.Drawing.FontStyle.Bold);
        private Font mm_font = new Font("Arial", 8f, FontStyle.Bold);
        
        Point isPoint = new Point();
        //Point isPoint2 = new Point();
        private Rectangle top;
        private Rectangle bottom;
        private Rectangle left;
        private Rectangle rigt;
        private Rectangle Left_Top;
        private Rectangle Left_B;
        private Rectangle Top_R;
        private Rectangle Right_B;
        private Rectangle Panel_move;
        private Rectangle rectMax;
        private Rectangle rectClose;
        private Rectangle rectMin;
        private Rectangle iconRec;
        private Rectangle TitRec;
        private  TextFormatFlags textFlags;

        private Bitmap bmp_Close = new Bitmap(Resou.Close);
        private Bitmap bmp_CloseOver = new Bitmap(Resou.CloseHot);
        private Bitmap bmp_CloseDown = new Bitmap(Resou.ClosePressed);

        private Bitmap bmp_Min = new Bitmap(Resou.Minimize);
        private Bitmap bmp_MinOver = new Bitmap(Resou.MinimizeHot);
        private Bitmap bmp_MinDown = new Bitmap(Resou.MinimizePressed);


        private Bitmap bmp_Max = new Bitmap(Resou.Maximize);
        private Bitmap bmp_MaxOver = new Bitmap(Resou.MaximizeHot);
        private Bitmap bmp_MaxDown = new Bitmap(Resou.maxclick);

        private Bitmap bmp_Max2 = new Bitmap(Resou.Maximize2);
        private Bitmap bmp_MaxOver2 = new Bitmap(Resou.MaxmizeHot2);
        private Bitmap bmp_MaxDown2 = new Bitmap(Resou.maximizePressed2);


        public  Form m_Parent;

        
        
        #endregion variable

        #region effect
        private void effectOpec()
        {
            double i = 1;
            while (i > 0)
            {
                m_Parent.Opacity = i;
                m_Parent.Refresh();
                i-=0.08;
            }
        }
        private void effectDrop()
        {
             int i = 1;
                
                while (i < 80)
                {
                    m_Parent.Top = m_Parent.Top - i;
                   
                    m_Parent.Refresh();
                    i += 9;
                   
                
                }
        }

        #endregion

        #region constructor
        public skine()
        {


        }


        public skine(Form myForm)
        {
            m_Parent = myForm;

           // MeD = new DBGraphics();
            textFlags = new TextFormatFlags();
            textFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            myForm.Paint += OnPaint;
            myForm.Resize += OnResize;
            myForm.MouseDown += OnMouseDown;
            myForm.MouseUp += OnMouseUp;
            myForm.MouseMove += OnMouseMove;
            myForm.MouseDoubleClick += OnMouseDoubbleClick;
            m_Parent.Padding = new System.Windows.Forms.Padding(Border_Size, (Panel_Move_Size + Border_Size),
                Border_Size, Border_Size);
            m_Parent.Invalidate();
           // myForm. VisibleChanged += OnVisibleChanged;
            m_Parent.FormBorderStyle = FormBorderStyle.None;

            WindowListener wndlisten = new WindowListener();
            wndlisten.AssignHandle(this.Handle);
            m_Parent.Invalidate();

        }
        #endregion //constructor

        #region Property




        protected IntPtr Handle
        {
            get { return m_Parent.Handle; }
        }

        public Color TextColor
        {
            get { return m_TextColor; }

            set
            {
                if ((value != (m_TextColor)))
                {

                    m_TextColor = value;
                }
                m_Parent.Invalidate();
            }

        }

        public bool _controlbox
        {
            get { return controlBox; }
            set { controlBox = value;
            m_Parent.Invalidate();
            }
            
        }

        public int Border_Size2
        {
            get { return Border_Size; }

            set
            {
                if ((value != (Border_Size)))
                {

                    Border_Size = value;
                }
                m_Parent.Invalidate();
            }

        }


        public int Panel_Move_Size2
        {
            get { return Panel_Move_Size; }

            set
            {
                if ((value != (Panel_Move_Size)))
                {

                    Panel_Move_Size = value;
                }
                m_Parent.Invalidate();
            }

        }


        public Color Border_Top_light2
        {
            get { return Border_Top_light; }

            set
            {
                if ((value != (Border_Top_light)))
                {

                    Border_Top_light = value;
                }
                m_Parent.Invalidate();
            }

        }

        public Color Border_Top_Dark2
        {
            get { return Border_Top_Dark; }

            set
            {
                if ((value != (Border_Top_Dark)))
                {

                    Border_Top_Dark = value;
                }
                m_Parent.Invalidate();
            }

        }

        public Color Border_Bottom_light2
        {
            get { return Border_Bottom_light; }

            set
            {
                if ((value != (Border_Bottom_light)))
                {

                    Border_Bottom_light = value;
                }
                m_Parent.Invalidate();
            }

        }

        public Color Border_Bottom_Dark2
        {
            get { return Border_Bottom_Dark; }

            set
            {
                if ((value != (Border_Bottom_Dark)))
                {

                    Border_Bottom_Dark = value;
                }
                m_Parent.Invalidate();
            }

        }

        public Color Border_LR_light2
        {
            get { return Border_LR_light; }

            set
            {
                if ((value != (Border_LR_light)))
                {

                    Border_LR_light = value;
                }
                m_Parent.Invalidate();
            }

        }

        public Color Border_LR_Dark2
        {
            get { return Border_LR_Dark; }

            set
            {
                if ((value != (Border_LR_Dark)))
                {

                    Border_LR_Dark = value;
                }
                m_Parent.Invalidate();
            }

        }

        public Color Panel_Move_light2
        {
            get { return Panel_Move_light; }

            set
            {
                if ((value != (Panel_Move_light)))
                {

                    Panel_Move_light = value;
                }
                m_Parent.Invalidate();
            }

        }


        public Color Panel_Move_Dark2
        {
            get { return Panel_Move_Dark; }

            set
            {
                if ((value != (Panel_Move_Dark)))
                {

                    Panel_Move_Dark = value;
                }
                m_Parent.Invalidate();
            }

        }


        protected Rectangle ClientRectangle
        {
            get { return m_Parent.ClientRectangle; }
        }

        protected Form Parent
        {
            get { return m_Parent; }
        }


        protected string Text
        {
            get { return m_Parent.Text; }
        }
        public string FileName
        {
            get { return text; }
            set { text = value; }
        }

        #endregion Property




        #region CaptionButton Method


        private void DrawMaxButtonNormal()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_Max, rectMax);
                    g.Dispose();
                }
            }

        }

        private void DrawMaxButtonOver()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_MaxOver, rectMax);
                    g.Dispose();
                }
            }

        }

        private void DrawMaxButtonDown()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_MaxDown, rectMax);
                    g.Dispose();
                }
            }

        }

        private void DrawMaxButtonNormal2()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_Max2, rectMax);
                    g.Dispose();
                }
            }

        }

        private void DrawMaxButtonOver2()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_MaxOver2, rectMax);
                    g.Dispose();
                }
            }

        }

        private void DrawMaxButtonDown2()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_MaxDown2, rectMax);
                    g.Dispose();
                }
            }

        }


        private void DrawCloseButtonNormal()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_Close, rectClose);
                    g.Dispose();
                }
            }

        }

        private void DrawCloseButtonOver()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_CloseOver, rectClose);
                    g.Dispose();
                }
            }

        }

        private void DrawCloseButtonDown()
        {
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_CloseDown, rectClose);
                    g.Dispose();
                }
            }

        }

        private void DrawMinButtonNormal()
        {

            // Draw the image to the form
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_Min, rectMin);
                    g.Dispose();
                }
            }


        }

        private void DrawMinButtonOver()
        {

            // Draw the image to the form
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_MinOver, rectMin);
                    g.Dispose();
                }
            }


        }

        private void DrawMinButtonDown()
        {

            // Draw the image to the form
            if (controlBox)
            {
                using (Graphics g = Graphics.FromHwnd(m_Parent.Handle))
                {
                    g.DrawImage(bmp_MinDown, rectMin);
                    g.Dispose();
                }
            }


        }

        #endregion // CaptionButton Method

        


        #region  events




        protected virtual void OnPaint(object sender, PaintEventArgs e)
        {

            if (m_Parent.WindowState != FormWindowState.Minimized)
            {

                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
                top = new Rectangle(Border_Size, 0, m_Parent.ClientRectangle.Width - (Border_Size * 2),
                    Border_Size);

                rigt = new Rectangle(m_Parent.ClientRectangle.Width - Border_Size, Border_Size, Border_Size,
                    m_Parent.ClientRectangle.Height - (Border_Size * 2));
                bottom = new Rectangle(Border_Size,
                    m_Parent.ClientRectangle.Height - Border_Size, m_Parent.ClientRectangle.Width - (Border_Size * 2),
                    Border_Size);
                Left_Top = new Rectangle(0, 0, Border_Size, Border_Size);
                Left_B = new Rectangle(0, m_Parent.ClientRectangle.Height - Border_Size,
                    Border_Size, Border_Size);
                Panel_move = new Rectangle(Border_Size, Border_Size,
                    m_Parent.ClientRectangle.Width - (Border_Size * 2), Panel_Move_Size);
                if (controlBox)
                {
                    DrawCloseButtonNormal();
                    DrawMaxButtonNormal();
                    DrawMinButtonNormal();
                }






                Top_R = new Rectangle(m_Parent.ClientRectangle.Width - Border_Size, 0,
                    Border_Size, Border_Size);

                Right_B = new Rectangle(m_Parent.ClientRectangle.Width - Border_Size,
                    m_Parent.ClientRectangle.Height - Border_Size, Border_Size, Border_Size);

                left = new Rectangle(0, Border_Size, Border_Size, 
                    m_Parent.ClientRectangle.Height - (Border_Size * 2));
                LinearGradientBrush lgb_top = new LinearGradientBrush(top, this.Border_Top_light,
                    this.Border_Top_Dark, 90, true);
                LinearGradientBrush lgb_LR = new LinearGradientBrush(left, this.Border_LR_Dark,
                    this.Border_LR_light, 180, true);
                LinearGradientBrush lgb_Bottom = new LinearGradientBrush(bottom, this.Border_Bottom_light,
                    this.Border_Bottom_Dark, 90, true);
                LinearGradientBrush lgb_PanelMov = new LinearGradientBrush(Panel_move, this.Panel_Move_Dark,
                    this.Panel_Move_light, 90, true);
                lgb_PanelMov.SetBlendTriangularShape(0.5f);
                lgb_PanelMov.SetSigmaBellShape(0.5f);

                SizeF m_size = e.Graphics.MeasureString(this.Text, m_Font);
                SizeF mm_size  = e.Graphics.MeasureString(text, mm_font);

                ico = new Icon(m_Parent.Icon, 16, 16);
               
                e.Graphics.FillRectangle(lgb_LR, left);
                e.Graphics.FillRectangle(lgb_LR, rigt);
                e.Graphics.FillRectangle(lgb_LR, Left_Top);
                e.Graphics.FillRectangle(lgb_Bottom, Left_B);
                e.Graphics.FillRectangle(lgb_LR, Top_R);
                e.Graphics.FillRectangle(lgb_Bottom, Right_B);
                e.Graphics.FillRectangle(lgb_PanelMov, Panel_move);
                e.Graphics.FillRectangle(lgb_Bottom, bottom);
                e.Graphics.FillRectangle(lgb_top, top);
                rectClose = new Rectangle(m_Parent.ClientRectangle.Right - (bmp_Close.Width + 6),
                    (Panel_Move_Size + Border_Size - bmp_Close.Height) / 2, bmp_Close.Width, bmp_Close.Height);
                rectMax = new Rectangle(m_Parent.ClientRectangle.Right - (bmp_Close.Width + 8 + bmp_Max.Width),
                    ((Panel_Move_Size + Border_Size) - bmp_Max.Height) / 2, bmp_Max.Width, bmp_Max.Height);
                rectMin = new Rectangle(m_Parent.ClientRectangle.Right - (bmp_Close.Width + 10 + bmp_Min.Width * 2),
                    ((Panel_Move_Size + Border_Size) - bmp_Min.Height) / 2, bmp_Min.Width, bmp_Min.Height);
                iconRec = new Rectangle(Panel_move.X + 2, (Panel_Move_Size - ico.Height + 4) / 2,
                    ico.Width, ico.Height);
                TitRec = new Rectangle(Panel_move.X + (iconRec.Width + 5 + (int)m_size.Width),
                    Panel_move.Y, Panel_move.Size.Width - (rectMin.Width*14), Panel_move.Height);
                //e.Graphics.FillRectangle(Brushes.Bisque, TitRec);
                TextRenderer.DrawText(e.Graphics, this.Text, m_Font, 
                    new Rectangle(Panel_move.X + ico.Width ,Panel_move.Y,
                    (int)m_size.Width,(int)m_size.Height), m_TextColor,TextFormatFlags.VerticalCenter);
                TextRenderer.DrawText(e.Graphics, text, mm_font,TitRec, Color.White, Color.Transparent, textFlags);
                e.Graphics.DrawIconUnstretched(ico, iconRec);
               
                
                



            }

        }

        protected virtual void OnMouseDoubbleClick(object sender, MouseEventArgs e)
        {
            if (Panel_move.Contains(e.Location))
            {
                if (!maxmieze)
                {
                    m_Parent.WindowState = FormWindowState.Maximized;
                    maxmieze = true;
                }
                else
                {
                    m_Parent.WindowState = FormWindowState.Normal;
                    maxmieze = false;
                }
            }


        }
        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (rigt.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeWE;
            }
            else if (left.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeWE;
            }
            else if (bottom.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeNS;
            }
            else if (top.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeNS;
            }
            else if (Left_Top.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeNWSE;
            }
            else if (Left_B.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeNESW;
            }
            else if (Top_R.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeNESW;
            }
            else if (Right_B.Contains(e.Location))
            {
                m_Parent.Cursor = Cursors.SizeNWSE;
            }
            else
            {
                m_Parent.Cursor = Cursors.Default;
                //m_Parent.Refresh();
            } 
            

            if ((MouseButtons.Left & e.Button) == MouseButtons.Left)
            {


                if (sizeT)
                {

                    m_Parent.Cursor = Cursors.SizeNS;
                    //int x = e.X - isPoint.X;
                    int y = e.Y - isPoint.Y;
                    // this.Location = new Point(x, y);
                   
                    m_Parent.Height = m_Parent.Height - y;
                       m_Parent.Top = m_Parent.Top + y;
                    
                    // this.Width = this.Width + x;

                }
                else if (rectClose.Contains(e.Location))
                {
                    DrawCloseButtonOver();
                }

                else if (rectMin.Contains(e.Location))
                {
                    DrawMinButtonOver();
                }
                /*else if (rectMax.Contains(e.Location))
                {
                    if (Maxim == false)
                    {
                        DrawMaxButtonNormal();
                    }
                    else
                    {
                        DrawMaxButtonNormal2();
                    }
                }

                else if (!rectMax.Contains(e.Location))
                {
                    if (Maxim == false)
                    {
                        DrawMaxButtonNormal();
                    }
                    else
                    {
                        DrawMaxButtonNormal2();
                    }
                }*/

                else if (!rectClose.Contains(e.Location) & isClosing)
                {
                    isClosing = false;
                    DrawCloseButtonNormal();
                   // m_Parent.Refresh();
                }

                else if (!rectMin.Contains(e.Location) & isMinimizing)
                {
                    isMinimizing = false;
                    DrawMinButtonNormal();
                   // m_Parent.Refresh();
                }
                else if (sizeB)
                {
                    m_Parent.Cursor = Cursors.SizeNS;
                    int y = e.Y - isPoint.Y;
                    m_Parent.Height = e.Location.Y;
                }
                else if (sizeR)
                {
                    m_Parent.Cursor = Cursors.SizeWE;
                    m_Parent.Width = e.Location.X;
                    

                }
                else if (sizeL)
                {
                    m_Parent.Cursor = Cursors.SizeWE;
                    
                    int x = e.Location.X - isPoint.X;
                    
                        m_Parent.Width = m_Parent.Width - x;
                        m_Parent.Left = m_Parent.Left + x;
                    
                }

                else if (sizeT_L)
                {
                    m_Parent.Cursor = Cursors.SizeNWSE;
                    int x = e.Location.X - isPoint.X;
                    int y = e.Y - isPoint.Y;
                    m_Parent.Width = m_Parent.Width - x;
                    m_Parent.Left = m_Parent.Left + x;
                    m_Parent.Height = m_Parent.Height - y;
                    m_Parent.Top = m_Parent.Top + y;
                }
                else if (sizeL_B)
                {
                    m_Parent.Cursor = Cursors.SizeNESW;
                    int x = e.X - isPoint.X;
                    int y = e.Y - isPoint.Y;
                    m_Parent.Width = m_Parent.Width - x;
                    m_Parent.Left = m_Parent.Left + x;
                 
                     m_Parent.Height = e.Location.Y;
                    // this.Top= this.Top + y;
                }
                else if (sizeT_R)
                {
                    m_Parent.Cursor = Cursors.SizeNESW;
                    int x = e.X - isPoint.X;
                    int y = e.Y - isPoint.Y;
                    m_Parent.Height = m_Parent.Height - y;
                    m_Parent.Top = m_Parent.Top + y;
                   m_Parent.Width = e.Location.X;
                    // this.Top= this.Top + y;
                }
                else if (sizeR_B)
                {
                    m_Parent.Cursor = Cursors.SizeNWSE;
                    //int x = e.X - isPoint.X;
                    // int y = e.Y - isPoint.Y;
                    m_Parent.Width = e.Location.X;
                    //this.Left = this.Left + x;
                   m_Parent.Height = e.Location.Y;
                    // this.Top= this.Top + y;
                }
                else if (Move)
                {
                    int x = m_Parent.Location.X + (e.X - isPoint.X);
                    int y = m_Parent.Location.Y + (e.Y - isPoint.Y);

                     m_Parent.Location = new Point(x, y);
                }
                

            }
            else
            {

                if (rectMax.Contains(e.Location))
                {
                    if (Maxim == false)
                    {
                        DrawMaxButtonOver();
                       
                       // m_Parent.Refresh();
                    }
                    else
                    {
                        DrawMaxButtonOver2();
                       // m_Parent.Refresh();
                    }
                }

                else
                {
                    if (Maxim == false)
                    {
                        DrawMaxButtonNormal();
                       // m_Parent.Refresh();
                    }
                    else
                    {
                        DrawMaxButtonNormal2();
                        //m_Parent.Refresh();
                    }
                }

                if (rectClose.Contains(e.Location))
                {
                    DrawCloseButtonOver();
                }

                else
                {
                    DrawCloseButtonNormal();
                }

                if (rectMin.Contains(e.Location))
                {
                    DrawMinButtonOver();
                }
                else
                {
                    DrawMinButtonNormal();
                }
            }
        }


        protected virtual void OnMouseUp(object sender, MouseEventArgs e)
        {

            if (sizeT)
            {
                sizeT = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (sizeB)
            {
                sizeB = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (sizeR)
            {
                sizeR = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (sizeL)
            {
                sizeL = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (sizeT_L)
            {
                sizeT_L = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (sizeL_B)
            {
                sizeL_B = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (sizeR_B)
            {
                sizeR_B = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (sizeT_R)
            {
                sizeT_R = false;
                m_Parent.Cursor = Cursors.Default;
            }
            else if (Move)
            {
                Move = false;
                
            }
            else if (rectClose.Contains(e.Location) & isClosing)
            {
               
                //effectOpec();
                effectDrop();
                m_Parent.Close();
            }
            else if (rectMax.Contains(e.Location))
            {
                if (Maxim == false)
                {
                    m_Parent.WindowState = FormWindowState.Maximized;
                    Maxim = true;
                }
                else
                {
                    m_Parent.WindowState = FormWindowState.Normal;
                    m_Parent.Invalidate();
                    Maxim = false;
                }
               
            }

            

            else if (rectMin.Contains(e.Location) & isMinimizing)
            {


                
                m_Parent.WindowState = FormWindowState.Minimized;
            }

        }


        protected virtual void OnResize(object sender, EventArgs e)
        {
            
            
            m_Parent.Invalidate();
            m_Parent.Refresh();
           



        }


        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {

            if ((MouseButtons.Left & e.Button) == MouseButtons.Left)
            {
                 if (rectClose.Contains(e.Location))
                {
                    DrawCloseButtonDown();
                    isClosing = true;
                }

                else if (rectMin.Contains(e.Location))
                {
                    DrawMinButtonDown();
                    isMinimizing = true;
                }
                 else if (rectMax. Contains(e.Location))
                 {
                     if (Maxim == false)
                     {
                         DrawMaxButtonDown();
                     }
                     else
                     {
                         DrawMaxButtonDown2();
                     }
                 }
                else if (top.Contains(e.Location))
                {

                    sizeT = true;
                    //isPoint.X = e.X;
                    isPoint.Y = e.Y;
                }
                else if (bottom.Contains(e.Location))
                {
                    sizeB = true;
                    isPoint.Y = e.Y;
                }
                else if (rigt.Contains(e.Location))
                {
                    sizeR = true;
                    isPoint.X = e.X;
                }
                else if (left.Contains(e.Location))
                {
                    sizeL = true;
                    isPoint.X =  e.X;
                }
                else if (Left_Top.Contains(e.Location))
                {

                    sizeT_L = true;
                    isPoint.X = e.X;
                    isPoint.Y = e.Y;
                }
                else if (Left_B.Contains(e.Location))
                {

                    sizeL_B = true;
                    isPoint.X = e.X;
                    isPoint.Y = e.Y;
                }

                else if (Top_R.Contains(e.Location))
                {

                    sizeT_R = true;
                    isPoint.X = e.X;
                    isPoint.Y = e.Y;
                }
                else if (Right_B.Contains(e.Location))
                {

                    sizeR_B = true;
                    isPoint.X = e.X;
                    isPoint.Y = e.Y;
                }
                else if (Panel_move.Contains(e.Location))
                {

                    Move = true;
                    isPoint.X = e.X;
                    isPoint.Y = e.Y;
                }
               
            }
        }

       
        #endregion events





    }


    


    #region Classes

    internal class WindowListener : NativeWindow
    {

        private const int WM_ACTIVATEAPP = 0x1c;
        public WindowListener()
        {
        }

        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            
            switch ((m.Msg))
            {
               /* case Api.WM_ERASEBKGND:
                    Bitmap bmp;
                    Rectangle srceRect;
                    Rectangle destRect;
                    
                    // Create a memory bitmap to use as double buffer
                    Bitmap offScreenBmp;
                    offScreenBmp = new Bitmap(skine.m_Parent.Width,skine.m_Parent.Height);
                    Graphics g = Graphics.FromImage(offScreenBmp);

                    if (skine.m_Parent.BackgroundImage != null)
                    {
                        bmp = new Bitmap(skine.m_Parent.BackgroundImage);
                        srceRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                        destRect = new Rectangle(0, 0, skine.m_Parent.Width + 100, skine.m_Parent.Height);
                       // destRect = new Rectangle(0, 0, skine.m_Parent.Width, skine.m_Parent.Height);
                        g.DrawImage(bmp, destRect, srceRect, GraphicsUnit.Pixel);
                        bmp.Dispose();
                    }
                    else
                    {
                        SolidBrush myBrush = new SolidBrush(skine.m_Parent.BackColor);
                        g.FillRectangle(myBrush, 0, 0, skine.m_Parent.Width-6, skine.m_Parent.Height-32);
                        myBrush.Dispose();


                        


                    }
                    g = Graphics.FromHdc(m.WParam);
                    g.DrawImage(offScreenBmp, 0, 0);
                    g.Dispose();
                    offScreenBmp.Dispose();

                    break;*/
               
                case WM_ACTIVATEAPP:
                    
                   //skine.m_Parent.Invalidate();
                    break;
            }
            base.WndProc(ref m);
        }
    }

    #endregion // Classes

}
