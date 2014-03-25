namespace CellAO_Launcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.bx_IPAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UseEncryption = new System.Windows.Forms.CheckBox();
            this.bx_AOExe = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.bx_converted = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.bx_Port = new System.Windows.Forms.TextBox();
            this.cbx_DebugMode = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(337, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Launch";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server IP Address:";
            // 
            // bx_IPAddress
            // 
            this.bx_IPAddress.Location = new System.Drawing.Point(27, 90);
            this.bx_IPAddress.Name = "bx_IPAddress";
            this.bx_IPAddress.Size = new System.Drawing.Size(157, 20);
            this.bx_IPAddress.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe Print", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(385, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(341, 65);
            this.label2.TabIndex = 3;
            this.label2.Text = "CellAO Launcher";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "AO Executable:";
            // 
            // UseEncryption
            // 
            this.UseEncryption.AutoSize = true;
            this.UseEncryption.Location = new System.Drawing.Point(487, 90);
            this.UseEncryption.Name = "UseEncryption";
            this.UseEncryption.Size = new System.Drawing.Size(144, 17);
            this.UseEncryption.TabIndex = 5;
            this.UseEncryption.Text = "Use Encrypted Launcher";
            this.UseEncryption.UseVisualStyleBackColor = true;
            // 
            // bx_AOExe
            // 
            this.bx_AOExe.Location = new System.Drawing.Point(27, 133);
            this.bx_AOExe.Name = "bx_AOExe";
            this.bx_AOExe.ReadOnly = true;
            this.bx_AOExe.Size = new System.Drawing.Size(854, 20);
            this.bx_AOExe.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(887, 130);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Find AO exe";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(551, 179);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Quit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(763, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "IPAddress Converted:";
            this.label4.Visible = false;
            // 
            // bx_converted
            // 
            this.bx_converted.Location = new System.Drawing.Point(766, 53);
            this.bx_converted.Name = "bx_converted";
            this.bx_converted.Size = new System.Drawing.Size(160, 20);
            this.bx_converted.TabIndex = 10;
            this.bx_converted.Visible = false;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(779, 79);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(129, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Convert IP - Test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(226, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Server Port:";
            // 
            // bx_Port
            // 
            this.bx_Port.Location = new System.Drawing.Point(229, 90);
            this.bx_Port.Name = "bx_Port";
            this.bx_Port.Size = new System.Drawing.Size(100, 20);
            this.bx_Port.TabIndex = 13;
            // 
            // cbx_DebugMode
            // 
            this.cbx_DebugMode.AutoSize = true;
            this.cbx_DebugMode.Location = new System.Drawing.Point(809, 179);
            this.cbx_DebugMode.Name = "cbx_DebugMode";
            this.cbx_DebugMode.Size = new System.Drawing.Size(88, 17);
            this.cbx_DebugMode.TabIndex = 14;
            this.cbx_DebugMode.Text = "Debug Mode";
            this.cbx_DebugMode.UseVisualStyleBackColor = true;
            this.cbx_DebugMode.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(962, 202);
            this.ControlBox = false;
            this.Controls.Add(this.cbx_DebugMode);
            this.Controls.Add(this.bx_Port);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.bx_converted);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bx_AOExe);
            this.Controls.Add(this.UseEncryption);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bx_IPAddress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox bx_IPAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox UseEncryption;
        private System.Windows.Forms.TextBox bx_AOExe;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox bx_converted;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox bx_Port;
        private System.Windows.Forms.CheckBox cbx_DebugMode;
    }
}

