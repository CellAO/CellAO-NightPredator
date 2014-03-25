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

namespace CellAO_Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFile = new OpenFileDialog();
            browseFile.Filter = "Exe Files (*.exe)|*.exe";
            browseFile.Title = "Browse EXE files";

            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                bx_AOExe.Text = browseFile.FileName;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex);
            }
        }

        private void bx_AOExe_TextChanged(object sender, EventArgs e)
        {
        
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] temp = bx_IPAddress.Text.Split('.');
            int ipConverted = int.Parse(temp[3]) + int.Parse(temp[2]) * 256 + int.Parse(temp[1]) * 256 * 256 + int.Parse(temp[0]) * 256 * 256 * 256;
            ProcessStartInfo startInfo = new ProcessStartInfo();

            if (UseEncryption.Checked == true)
            {
                MessageBox.Show("Not Implamented yet.");
            }
            startInfo.FileName = bx_AOExe.Text;
            startInfo.Arguments = Convert.ToString(ipConverted);
            Process.Start(startInfo);
        }
    }
}
