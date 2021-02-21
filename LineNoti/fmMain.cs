using FormSerialisation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineNoti
{
    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();

            llToLineNotiToken.Text = "Click here to get your line notify token...";
            llToLineNotiToken.Links.Add(0, 10, "https://notify-bot.line.me");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lNote = new LineNotify(txtToken.Text);
            using (new WaitCursor())
            {
                //txtResult.Text = lNote.NotifyAsync(txtMessage.Text, txtImagePath.Text, 2, 30);
                //txtResult.Text = lNote.NotifyAsync(txtMessage.Text, txtImagePath.Text, 1, 8, true);
                txtResult.Text = lNote.NotifyAsync(txtMessage.Text, txtImagePath.Text);
            }
        }

        private void llToLineNotiToken_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink(e.Link.LinkData.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked: "+ ex.ToString());
            }
        }

        private void VisitLink(string link)
        {
            //Call the Process.Start method to open the default browser
            //with a URL:
            System.Diagnostics.Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
            //System.Diagnostics.Process.Start(link);
        }

        private void fmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormSerialisor.Serialise(this, Application.StartupPath + @"\serialise.xml");
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            FormSerialisor.Deserialise(this, Application.StartupPath + @"\serialise.xml");
            txtResult.Text = "";
        }

        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtImagePath.Text = openFileDialog1.FileName;
            }
        }
    }
}
