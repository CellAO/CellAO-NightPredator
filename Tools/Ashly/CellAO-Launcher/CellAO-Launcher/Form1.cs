#region License

// Copyright (c) 2005-2014, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

#region Usings ...

using _config = CellAO_Launcher.Config.ConfigReadWrite;

#endregion

namespace CellAO_Launcher
{
    #region Usings ...

    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;
        }

        // These are the keys that will be sent to CellAO.
        /// <summary>
        /// </summary>
        private const string FUNCOM_KEY_PUBLIC = "9c32cc23d559ca90fc31be72df817d0e124769e809f936bc14360ff4bed758f260a0d596584eacbbc2b88bdd410416163e11dbf62173393fbc0c6fefb2d855f1a03dec8e9f105bbad91b3437d8eb73fe2f44159597aa4053cf788d2f9d7012fb8d7c4ce3876f7d6cd5d0c31754f4cd96166708641958de54a6def5657b9f2e92";

        /// <summary>
        /// </summary>
        private string FUNCOM_KEY_PRIME =
            "eca2e8c85d863dcdc26a429a71a9815ad052f6139669dd659f98ae159d313d13c6bf2838e10a69b6478b64a24bd054ba8248e8fa778703b418408249440b2c1edd28853e240d8a7e49540b76d120d3b1ad2878b1b99490eb4a2a5e84caa8a91cecbdb1aa7c816e8be343246f80c637abc653b893fd91686cf8d32d6cfe5f2a6f";

        /// <summary>
        /// </summary>
        private int FUNCOM_KEY_GENERATOR = 5;

        // Referance Only
        /// <summary>
        /// </summary>
        private string CELLAO_KEY_PRIVATE = "7ad852c6494f664e8df21446285ecd6f400cf20e1d872ee96136d7744887424b";

        /// <summary>
        /// </summary>
        private string CELLAO_KEY_PUBLIC =
            "26b5a3b4ac1177f24a2d9de44bafef477ff23ef1cb5f646919b1be26516053030b65d5afb60cef6f49de539958ba0b7922a099319b8016a8673cb27a696ae4b60fdece25ddcdad42e7f0056b87fc35687fe033b242e17e960d79806fd46c4a79cbc64f558660a50cabc1c242dace70de6af452e3433f97e30e202567f187de70";

        /// <summary>
        /// </summary>
        private string CELLAO_KEY_PRIME =
            "eca2e8c85d863dcdc26a429a71a9815ad052f6139669dd659f98ae159d313d13c6bf2838e10a69b6478b64a24bd054ba8248e8fa778703b418408249440b2c1edd28853e240d8a7e49540b76d120d3b1ad2878b1b99490eb4a2a5e84caa8a91cecbdb1aa7c816e8be343246f80c637abc653b893fd91686cf8d32d6cfe5f2a6f";

        /// <summary>
        /// </summary>
        private int CELLAO_KEY_GENERATOR = 5;

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Button2Click(object sender, EventArgs e)
        {
            OpenFileDialog browseFile = new OpenFileDialog();
            browseFile.Filter = "Exe Files (*.exe)|*.exe";
            browseFile.Title = "Browse EXE files";
            browseFile.FileName = "AnarchyOnline.exe";

            if (browseFile.ShowDialog() == DialogResult.OK)
            {
                this.bx_AOExe.Text = browseFile.FileName;
            }
        }

        /// <summary>
        /// </summary>
        private uint ipConverted;

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.bx_AOExe.Text) && !string.IsNullOrEmpty(this.bx_IPAddress.Text)
                && !string.IsNullOrEmpty(this.bx_Port.Text))
            {
                this.ipConverted = this.ConvertHostToIp(this.bx_IPAddress.Text);
                if (this.ipConverted == 0)
                {
                    MessageBox.Show("Please enter a valid IP Address or valid domain name.");
                }
                else
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();

                    startInfo.FileName = this.bx_AOExe.Text;
                    startInfo.Arguments = " IA " + this.ipConverted + " IP " + this.bx_Port.Text + " UI";
                    startInfo.WorkingDirectory = Path.GetDirectoryName(this.bx_AOExe.Text);
                    Process AO = Process.Start(startInfo);

                    if (this.UseEncryption.Checked)
                    {
                        // Wait for Process to get into main loop
                        AO.WaitForInputIdle();

                        // Suspend client
                        Injector.SuspendProcess(AO.Id);

                        // Search and replace keys
                        Injector.SearchAndReplace(AO.Id, AO.Handle, FUNCOM_KEY_PUBLIC, this.CELLAO_KEY_PUBLIC);

                        // Uncomment this when we have a own Prime - Algorithman
                        // Injector.SearchAndReplace(AO.Id, AO.Handle, FUNCOM_KEY_PRIME, CELLAO_KEY_PRIME);

                        // Resume client
                        Injector.ResumeProcess(AO.Id);
                    }

                    // Save configuration data
                    _config.Instance.CurrentConfig.UseEncryption = this.UseEncryption.Checked;
#if DEBUG
                    this.cbx_DebugMode.Checked = true;
                    _config.Instance.CurrentConfig.AOExecutable = this.bx_AOExe.Text;
                    _config.Instance.CurrentConfig.ServerIP = this.bx_IPAddress.Text;
                    _config.Instance.CurrentConfig.ServerPort = Convert.ToInt32(this.bx_Port.Text);
                    _config.Instance.SaveConfig();
                    Application.Exit();
#else
                    this.cbx_DebugMode.Checked = false;
                    _config.Instance.CurrentConfig.AOExecutable = this.bx_AOExe.Text;
                    _config.Instance.CurrentConfig.ServerIP = this.bx_IPAddress.Text;
                    _config.Instance.CurrentConfig.ServerPort = Convert.ToInt32(this.bx_Port.Text);
                    _config.Instance.SaveConfig();
                    Application.Exit();
#endif
                }
            }
            else
            {
                MessageBox.Show("Please Enter the proper information.");
                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Form1Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(_config.Instance.CurrentConfig.Url);
            this.bx_IPAddress.Text = _config.Instance.CurrentConfig.ServerIP;
            this.bx_AOExe.Text = _config.Instance.CurrentConfig.AOExecutable;
            this.bx_Port.Text = Convert.ToString(_config.Instance.CurrentConfig.ServerPort);
#if DEBUG
            this.cbx_DebugMode.Checked = true; ;
            this.UseEncryption.Checked = false;
#else 
            this.cbx_DebugMode.Checked = false;
            this.UseEncryption.Checked = _config.Instance.CurrentConfig.UseEncryption;

                this.label4.Visible = true;
                this.bx_converted.Visible = true;
                this.bx_converted.ReadOnly = true;
                this.button4.Visible = true;
#endif

        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Button3Click(object sender, EventArgs e)
        {
            _config.Instance.CurrentConfig.UseEncryption = this.UseEncryption.Checked;
#if DEBUG
            this.cbx_DebugMode.Checked = true;
#endif 
            _config.Instance.CurrentConfig.AOExecutable = this.bx_AOExe.Text;
            _config.Instance.CurrentConfig.ServerIP = this.bx_IPAddress.Text;
            _config.Instance.CurrentConfig.ServerPort = Convert.ToInt32(this.bx_Port.Text);
            _config.Instance.SaveConfig();

            Application.Exit();
        }

        /// <summary>
        /// </summary>
        /// <param name="hostname">
        /// </param>
        /// <returns>
        /// </returns>
        private uint ConvertHostToIp(string hostname)
        {
            IPAddress tempIpAddress;
            if (IPAddress.TryParse(hostname, out tempIpAddress))
            {
                this.ipConverted = BitConverter.ToUInt32(IPAddress.Parse(hostname).GetAddressBytes(), 0);
            }
            else
            {
                IPHostEntry host = Dns.GetHostEntry(this.bx_IPAddress.Text);
                if (host.AddressList.Length > 0)
                {
                    this.ipConverted = BitConverter.ToUInt32(host.AddressList[0].GetAddressBytes(), 0);
                }
                else
                {
                    return 0;
                }
            }

            return this.ipConverted;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void Button4Click(object sender, EventArgs e)
        {
            uint converted = this.ConvertHostToIp(this.bx_IPAddress.Text);
            if (converted == 0)
            {
                MessageBox.Show("Please enter a valid IP Address or valid domain name.");
            }
            else
            {
                this.bx_converted.Text = converted.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbx_DebugMode.Checked == true)
            {
                this.bx_converted.Visible = true;
                this.label4.Visible = true;
                this.button4.Visible = true;
            }
            else
            {
                this.bx_converted.Visible = false;
                this.label4.Visible = false;
                this.button4.Visible = false;
            }
        }
    }
}