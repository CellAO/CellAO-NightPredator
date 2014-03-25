using System;
using System.Diagnostics;
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

        //These are the keys that will be sent to CellAO.
        private string FUNCOM_KEY_PUBLIC = "9c32cc23d559ca90fc31be72df817d0e124769e809f936bc14360ff4bed758f260a0d596584eacbbc2b88bdd410416163e11dbf62173393fbc0c6fefb2d855f1a03dec8e9f105bbad91b3437d8eb73fe2f44159597aa4053cf788d2f9d7012fb8d7c4ce3876f7d6cd5d0c31754f4cd96166708641958de54a6def5657b9f2e92";
        private string FUNCOM_KEY_PRIME =  "eca2e8c85d863dcdc26a429a71a9815ad052f6139669dd659f98ae159d313d13c6bf2838e10a69b6478b64a24bd054ba8248e8fa778703b418408249440b2c1edd28853e240d8a7e49540b76d120d3b1ad2878b1b99490eb4a2a5e84caa8a91cecbdb1aa7c816e8be343246f80c637abc653b893fd91686cf8d32d6cfe5f2a6f";
        private int FUNCOM_KEY_GENERATOR = 5;

        //Referance Only
        private string CELLAO_KEY_PRIVATE = "7ad852c6494f664e8df21446285ecd6f400cf20e1d872ee96136d7744887424b";
        private string CELLAO_KEY_PUBLIC  = "26b5a3b4ac1177f24a2d9de44bafef477ff23ef1cb5f646919b1be26516053030b65d5afb60cef6f49de539958ba0b7922a099319b8016a8673cb27a696ae4b60fdece25ddcdad42e7f0056b87fc35687fe033b242e17e960d79806fd46c4a79cbc64f558660a50cabc1c242dace70de6af452e3433f97e30e202567f187de70";
        private int CELLAO_KEY_GENERATOR = 5;

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
            if (bx_AOExe.Text != null || bx_IPAddress.Text != null || bx_Port.Text != null)
            {
                if (cbx_DNSType.Text != null)
                {
                    if (cbx_DNSType.Text == "IPAddress")
                    {
                        string[] temp = bx_IPAddress.Text.Split('.');
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
                    startInfo.Arguments = " IA " + ipConverted + " IP " + bx_Port.Text + " UI";
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
            else { MessageBox.Show("Please Enter the proper information."); return; }
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
