using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Skin;
namespace SmartFplayer
{
    
    
    public partial class Form2 : Form
    {
        public List<string> albume = new List<string>();
        public XmlDocument doc = new XmlDocument();
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(
                    @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                    @"\F_Player\albumes\");
        public Form2()
        {
            InitializeComponent();

            this.MinimumSize = new Size(this.Width, this.Height);
        }

        void FillViwe()
        {
            lv.Items.Clear();
            for (int i = 0; i < albume.Count; i++)
            {
                string FileName = System.IO.Path.GetFileName(albume[i]);
                ListViewItem file = new ListViewItem(FileName);
                file.SubItems.Add(albume[i]);
                lv.Items.Add(file);
            }
        }
        void FillCombo()
        {
            try
            {
                //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(
                   // @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                   // @"\F_Player\albumes\");
                doc.Load(@dir+@"albumes.xml");
                XmlNode alnd = doc["albumes"].FirstChild;
                
                while (alnd != null)
                {
                    alc.Items.Add(alnd.Name);
                    alnd = alnd.NextSibling;
                }
            }
            catch
            {
                MessageBox.Show("2حدث خطأ عند استرجاع الألبومات");
            }
        }
        void ReadAlbume(string albumeName)
        {
            try
            {
                //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(
                   // @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                   // @"\F_Player\albumes\");
                doc.Load(@dir + @"albumes.xml");
                XmlNode Filend = doc["albumes"][albumeName].FirstChild;
                albume.Clear();
                while (Filend != null)
                {
                    albume.Add(Filend.InnerText);
                    Filend = Filend.NextSibling;
                }
                FillViwe();
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثنأ أسترجاع بيانات الألبومات");
            }
        }
        void DeletAlbume(string albumeName)
        {
            try
            {
                //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(
                   // @Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                   // @"\F_Player\albumes\");
                if (MessageBox.Show("هل أنت متأكد أنك تريد الحذف", "تعديل", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2)==DialogResult.Yes)
                {
                    doc.Load(@dir+@"albumes.xml");
                        doc["albumes"].RemoveChild(doc["albumes"][albumeName]);
                    doc.Save(@dir+@"albumes.xml");
                    alc.Items.RemoveAt(alc.SelectedIndex);
                   lv.Items.Clear();
                }
            }
            catch
            {
            }
        }
        
        void WriteAlbume(string albumeName)
        {
            try
            {

                doc.Load(@dir+ @"albumes.xml");
                if (doc["albumes"][albumeName] != null)
                {
                    doc["albumes"].RemoveChild(doc["albumes"][albumeName]);
                }
                else
                {
                    alc.Items.Add(alc.Text);
                }
                XmlNode albumend = doc.CreateElement(albumeName);
                for (int i = 0; i < albume.Count; i++)
                {
                    XmlNode filend = doc.CreateElement("file");
                    XmlNode path = doc.CreateTextNode(albume[i]);
                    filend.AppendChild(path);
                    albumend.AppendChild(filend);
                }
                doc["albumes"].AppendChild(albumend);
                doc.Save(@dir + @"albumes.xml");
            }
            catch
            {
                MessageBox.Show("حدث خطأ أثنا حفظ بيانات الألبومات");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (OF.ShowDialog() == DialogResult.OK)
            {
                albume.AddRange(OF.FileNames);
                FillViwe();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lv.SelectedIndices.Count > 0)
            {
                albume.RemoveAt(lv.SelectedIndices[0]);
                FillViwe();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lv.SelectedIndices.Count > 0)
            {
                int sel = lv.SelectedIndices[0];
                if (sel > 0)
                {
                    string tmp = albume[sel];
                    albume[sel] = albume[sel - 1];
                    albume[sel - 1] = tmp;
                    FillViwe();
                    lv.Items[sel - 1].Selected = true;
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lv.SelectedIndices.Count > 0)
            {
                int sel = lv.SelectedIndices[0];
                if (sel < lv.Items.Count-1)
                {
                    string tmp = albume[sel];
                    albume[sel] = albume[sel + 1];
                    albume[sel + 1] = tmp;
                    FillViwe();
                    lv.Items[sel + 1].Selected = true;
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            skine sk1 = new skine(this);
            sk1._controlbox = false;
            sk1.Panel_Move_Dark2 = Properties.Settings.Default.panelMoveColorDark;
            sk1.Panel_Move_light2 = Properties.Settings.Default.panelMoveColorLight;
            sk1.Border_LR_Dark2 = sk1.Panel_Move_Dark2;
            sk1.Border_LR_light2 = sk1.Panel_Move_Dark2;
            sk1.Border_Top_Dark2 = sk1.Panel_Move_Dark2;
            sk1.Border_Top_light2 = sk1.Panel_Move_Dark2;
            sk1.Border_Bottom_Dark2 = sk1.Panel_Move_Dark2;
            sk1.Border_Bottom_light2 = sk1.Panel_Move_Dark2;
            
            sk1.TextColor = Color.Yellow;
            
            FillViwe();
            FillCombo();
           
        }

        private void alc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadAlbume(alc.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DeletAlbume(alc.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            WriteAlbume(alc.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lv.Items.Clear();
            albume.Clear();
            alc.Text = "";
        }

        
    }
}
