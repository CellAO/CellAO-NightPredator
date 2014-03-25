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
using _config = CellAO_Launcher.Config.ConfigReadWrite;

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
            browseFile.FileName = "AnarchyOnline.exe";

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


        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: Add a check if the IP box is empty as well as the Exe Box.
            //if (bx_AOExe.Text == null || bx_IPAddress.Text == null)
            //    MessageBox.Show("Please fill out the information and try again."); return;
          
            string[] temp = bx_IPAddress.Text.Split('.');
            int ipConverted = int.Parse(temp[3]) + int.Parse(temp[2]) * 256 + int.Parse(temp[1]) * 256 * 256 + int.Parse(temp[0]) * 256 * 256 * 256;
            ProcessStartInfo startInfo = new ProcessStartInfo();

            if (UseEncryption.Checked == true)
            {
                //TODO: Add the ability for us to enject our own SecretKey here to make AO think we are using real encryption.
                MessageBox.Show("Feature is not implimented yet.");
                return;
            }
            else 
            {
                startInfo.FileName = bx_AOExe.Text;
                startInfo.Arguments =  "IA "+ ipConverted + " IP " + Convert.ToInt32(bx_Port.Text) + " UI";
                Process.Start(startInfo);
                Application.Exit();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bx_IPAddress.Text = _config.Instance.CurrentConfig.ServerIP;
            bx_AOExe.Text = _config.Instance.CurrentConfig.AOExecutable;
            bx_Port.Text = Convert.ToString(_config.Instance.CurrentConfig.ServerPort);
            if (_config.Instance.CurrentConfig.UseEncryption == true) { UseEncryption.Checked = true; }
            else { UseEncryption.Checked = false; }
            //For Debug mode.
            if (_config.Instance.CurrentConfig.Debug == true) { label4.Visible = true; bx_converted.Visible = true; bx_converted.ReadOnly = true; button4.Visible = true; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (UseEncryption.Checked == true) { _config.Instance.CurrentConfig.UseEncryption = true; }
            else { _config.Instance.CurrentConfig.UseEncryption = false; }

            _config.Instance.CurrentConfig.AOExecutable = bx_AOExe.Text;
            _config.Instance.CurrentConfig.ServerIP = bx_IPAddress.Text;
            _config.Instance.CurrentConfig.ServerPort = Convert.ToInt32(bx_Port.Text);
            _config.Instance.SaveConfig();

            Application.Exit();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] temp = bx_IPAddress.Text.Split('.');
            bx_converted.Text = Convert.ToString(int.Parse(temp[3]) + int.Parse(temp[2]) * 256 + int.Parse(temp[1]) * 256 * 256 + int.Parse(temp[0]) * 256 * 256 * 256);
        }

    }
}
