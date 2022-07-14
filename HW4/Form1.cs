using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;

namespace HW4
{
    public partial class Form1 : Form
    {
        private Graphics m_graphic;
        private Timer m_timer1;
        private Timer m_timer2;
        private float m_width;
        private float m_height;
        private Color m_bgcolor;     //背景
        private Color m_backcolor;   //內圈
        private Color m_scalecolor;  //刻度
        private Color m_seccolor;    //秒針
        private Color m_mincolor;    //分針
        private Color m_hourcolor;   //時針
        private float m_radius;      //半徑
        private Color m_mseccolor;    //豪秒針

        int secend;
        int minute;
        int hour;

        int cmsecend = 0;
        int csecend = 0;
        int cminute = 0;
        int chour = 0;

        System.DateTime TimeNow = new DateTime();
        TimeSpan TimeCount = new TimeSpan();

        public Form1()
        {
            InitializeComponent();
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                button1.Visible = false;
                button2.Visible = false;
                label1.Visible = true;
                label2.Visible = false;
                label1.Text = string.Format("{0:00}:{1:00}:{2:00}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
            else if(radioButton2.Checked == true)
            {
                button1.Visible = true;
                button2.Visible = true;
                label1.Visible = false;
                label2.Visible = true;
                label2.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", TimeCount.Hours, TimeCount.Minutes, TimeCount.Seconds, TimeCount.Milliseconds);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer,true);

            m_timer1 = new Timer();
            m_timer1.Interval = 1000;
            m_timer1.Enabled = true;
            m_timer1.Tick += new EventHandler(timer1_Tick);

            m_timer2 = new Timer();
            m_timer2.Interval = 1;
            m_timer2.Tick += new EventHandler(timer2_Tick);

            m_width = this.ClientSize.Width;
            m_height = this.ClientSize.Height;
            m_bgcolor = Color.AliceBlue;
            m_backcolor = Color.Gray;
            m_scalecolor = Color.Gray;
            m_seccolor = Color.Red;
            m_mincolor = Color.Blue;
            m_hourcolor = Color.Green;
            m_mseccolor = Color.Black;

            if (m_width > m_height)
            {
                m_radius = (float)(m_height - 8) / 2;
            }
            else
            {
                m_radius = (float)(m_width - 8) / 2;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            m_graphic = e.Graphics;
            //座標原點
            m_graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            m_graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //外邊框
            m_graphic.TranslateTransform((float)(m_width / 2), (float)(m_height / 2));
            m_graphic.FillEllipse(new SolidBrush(m_bgcolor), -m_radius, -m_radius, m_radius * 2, m_radius * 2);
            //內邊框
            Pen pen = new Pen(m_bgcolor, 2);
            m_graphic.DrawEllipse(pen, m_radius * (-1), m_radius * (-1), m_radius * 2, m_radius * 2);
            //小刻度
            for (int i = 0; i < 60; i++)
            {
                m_graphic.FillRectangle(new SolidBrush(m_scalecolor), -3, 2 - m_radius, 6, 18);
                m_graphic.RotateTransform(6);
            }
            //大刻度
            for (int i = 0; i < 12; i++)
            {
                m_graphic.FillRectangle(new SolidBrush(m_scalecolor), -2, 2 - m_radius, 4, 10);
                m_graphic.RotateTransform(30);
            }

            if(radioButton1.Checked == true)
            {
                //獲取時間
                int msecend = DateTime.Now.Millisecond;
                secend = DateTime.Now.Second;
                minute = DateTime.Now.Minute;
                hour = DateTime.Now.Hour;

                //秒針
                pen.Color = m_seccolor;
                m_graphic.RotateTransform(6 * secend);
                m_graphic.DrawLine(pen, 0, 0, 0, (-1) * (float)(m_radius / 1.5));
                //分針
                pen.Color = m_mincolor;
                m_graphic.RotateTransform(6 * secend * (-1));
                m_graphic.RotateTransform((float)(6 / 60 * secend + 6 * minute));
                m_graphic.DrawLine(pen, 0, 0, 0, (-1) * (float)(m_radius / 2));
                //時針
                pen.Color = m_hourcolor;
                m_graphic.RotateTransform((float)(6 / 60 * secend + 6 * minute) * (-1));
                m_graphic.RotateTransform((float)(30 / 3600 * secend + 30 / 60 * minute + hour * 30));
                m_graphic.DrawLine(pen, 0, 0, 0, (-1) * (float)(m_radius / 2.5));
            }
            else if(radioButton2.Checked == true)
            {
                pen.Color = m_mseccolor;

                if(button1.Text == "開始")
                {
                    cmsecend = 0;
                    csecend = 0;
                    cminute = 0;
                    chour = 0;
                }
                else
                {
                    cmsecend = TimeCount.Milliseconds;
                    csecend = TimeCount.Seconds;
                    cminute = TimeCount.Minutes;
                    chour = TimeCount.Hours;
                }
                
                m_graphic.RotateTransform((float)(0.36 * cmsecend));
                m_graphic.DrawLine(pen, 0, 0, 0, (-1) * (float)(m_radius / 1));

                pen.Color = m_seccolor;
                m_graphic.RotateTransform((float)(0.36 * cmsecend) * (-1));
                m_graphic.RotateTransform((float)(0.36 / 60 * cmsecend + 6 * csecend));
                m_graphic.DrawLine(pen, 0, 0, 0, (-1) * (float)(m_radius / 1.5));

                pen.Color = m_mincolor;
                m_graphic.RotateTransform((float)(0.36 / 60 * cmsecend + 6 * csecend) * (-1));
                m_graphic.RotateTransform((float)(0.36 / 3600 * cmsecend + 6/60 * csecend + 6 * cminute));
                m_graphic.DrawLine(pen, 0, 0, 0, (-1) * (float)(m_radius / 2));

                pen.Color = m_hourcolor;
                m_graphic.RotateTransform((float)(0.36 / 3600 * cmsecend + 6 / 60 * csecend + 6 * cminute) * (-1));
                m_graphic.RotateTransform((float)(30 / 36000000 * cmsecend + 30 / 3600 * csecend + 30 / 60 * cminute + chour * 30));
                m_graphic.DrawLine(pen, 0, 0, 0, (-1) * (float)(m_radius / 2.5));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "開始")
            {
                button1.Text = "暫停";
                timer2.Start();
                TimeNow = DateTime.Now;
            }
            else if (button1.Text == "暫停")
            {
                button1.Text = "繼續";
                timer2.Stop();
            }
            else if(button1.Text == "繼續")
            {
                button1.Text = "暫停";
                timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
            TimeCount = DateTime.Now - TimeNow;
            label2.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", TimeCount.Hours, TimeCount.Minutes, TimeCount.Seconds, TimeCount.Milliseconds);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            label2.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:000}", 0, 0, 0, 0);
            cmsecend = 0;
            csecend = 0;
            cminute = 0;
            chour = 0;
            timer2.Enabled = false;
            button1.Text = "開始";
        }
    }
}
