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
using System.Net;
using _config = CellAO_Launcher.Config.ConfigReadWrite;
using System.IO;

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


        private uint ipConverted;
        private void button1_Click(object sender, EventArgs e)
        {
            //TODO: Add a check if the IP box is empty as well as the Exe Box.
            if (cbx_DNSType.Text != null)
            {
                if (cbx_DNSType.Text == "IPAddress")
                {
                    string[] temp = bx_IPAddress.Text.Split('.');
                   // ipConverted = uint.Parse(temp[0]) + uint.Parse(temp[1]) * 256 + uint.Parse(temp[2]) * 256 * 256 + uint.Parse(temp[3]) * 256 * 256 * 256;
                    ipConverted = uint.Parse(temp[0]) | (uint.Parse(temp[1]) << 8) | (uint.Parse(temp[2]) << 16) | (uint.Parse(temp[3]) << 24);
                }
                else
                {
                    IPHostEntry host = Dns.GetHostEntry(bx_IPAddress.Text);
                    string[] temp = Convert.ToString(host.AddressList[0]).Split('.');
                    ipConverted = uint.Parse(temp[0]) | (uint.Parse(temp[1]) << 8) | (uint.Parse(temp[2]) << 16) | (uint.Parse(temp[3]) << 24);
                }
            }
            else { MessageBox.Show("Please choose a DNS Type."); return; }

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
                startInfo.Arguments =  " IA "+ ipConverted + " IP " + bx_Port.Text + " UI";
                //startInfo.WorkingDirectory = Path.GetDirectory(bx_AOExe.Text);
                startInfo.WorkingDirectory = Path.GetDirectoryName(bx_AOExe.Text);
                Process.Start(startInfo);

                if (UseEncryption.Checked == true) { _config.Instance.CurrentConfig.UseEncryption = true; }
                else { _config.Instance.CurrentConfig.UseEncryption = false; }

                if (cbx_DebugMode.Checked == true) { _config.Instance.CurrentConfig.Debug = true; }
                else { _config.Instance.CurrentConfig.Debug = false; }

                _config.Instance.CurrentConfig.HostType = cbx_DNSType.Text;
                _config.Instance.CurrentConfig.AOExecutable = bx_AOExe.Text;
                _config.Instance.CurrentConfig.ServerIP = bx_IPAddress.Text;
                _config.Instance.CurrentConfig.ServerPort = Convert.ToInt32(bx_Port.Text);
                _config.Instance.SaveConfig();

                Process _proc = Process.GetCurrentProcess();
                _proc.Kill();
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bx_IPAddress.Text = _config.Instance.CurrentConfig.ServerIP;
            bx_AOExe.Text = _config.Instance.CurrentConfig.AOExecutable;
            bx_Port.Text = Convert.ToString(_config.Instance.CurrentConfig.ServerPort);
            cbx_DNSType.SelectedItem = _config.Instance.CurrentConfig.HostType;
            if (_config.Instance.CurrentConfig.Debug == true) { cbx_DebugMode.Checked = true; }
            else { cbx_DebugMode.Checked = false; }
            if (_config.Instance.CurrentConfig.UseEncryption == true) { UseEncryption.Checked = true; }
            else { UseEncryption.Checked = false; }
            //For Debug mode.
            if (_config.Instance.CurrentConfig.Debug == true) { label4.Visible = true; bx_converted.Visible = true; bx_converted.ReadOnly = true; button4.Visible = true; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (UseEncryption.Checked == true) { _config.Instance.CurrentConfig.UseEncryption = true; }
            else { _config.Instance.CurrentConfig.UseEncryption = false; }

            if (cbx_DebugMode.Checked == true) { _config.Instance.CurrentConfig.Debug = true; }
            else { _config.Instance.CurrentConfig.Debug = false; }

            _config.Instance.CurrentConfig.HostType = cbx_DNSType.Text;
            _config.Instance.CurrentConfig.AOExecutable = bx_AOExe.Text;
            _config.Instance.CurrentConfig.ServerIP = bx_IPAddress.Text;
            _config.Instance.CurrentConfig.ServerPort = Convert.ToInt32(bx_Port.Text);
            _config.Instance.SaveConfig();

            Application.Exit();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cbx_DNSType.Text != null)
            {
                if (cbx_DNSType.Text == "IPAddress")
                {
                    string[] temp = bx_IPAddress.Text.Split('.');
                    bx_converted.Text = Convert.ToString( uint.Parse(temp[0]) | (uint.Parse(temp[1]) <<8) | (uint.Parse(temp[2]) <<16) | (uint.Parse(temp[3])<<24));
                }
                else
                {
                    IPHostEntry host = Dns.GetHostEntry(bx_IPAddress.Text);
                    string[] temp = Convert.ToString(host.AddressList[0]).Split('.');
                    bx_converted.Text = Convert.ToString( uint.Parse(temp[0]) | (uint.Parse(temp[1]) <<8) | (uint.Parse(temp[2]) <<16) | (uint.Parse(temp[3])<<24));
                }
            }
            else { MessageBox.Show("Please choose a DNS Type."); return; }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_DebugMode.Checked == true)
            {
                bx_converted.Visible = true;
                label4.Visible = true;
                button4.Visible = true;
            }
            else
            {
                bx_converted.Visible = false;
                label4.Visible = false;
                button4.Visible = false;
            }
        }

    }
}
