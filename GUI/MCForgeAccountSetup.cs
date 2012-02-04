using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MCForge.Gui
{
    public partial class MCForgeAccountSetup : Form
    {
        public MCForgeAccountSetup()
        {
            InitializeComponent();
        }

        private void MCForgeAccountSetup_Load(object sender, EventArgs e)
        {
            textBox1.Text = Server.mcforgeUser;
            textBox2.Text = Server.mcforgePass;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server.mcforgeUser = textBox1.Text.Trim();
            Server.mcforgePass = textBox2.Text.Trim();
            new Thread(new ThreadStart(delegate {
                this.Invoke(new MethodInvoker(delegate { this.Enabled = false; }));
                if (MCForgeAccount.Login())
                {
                    MessageBox.Show("You are now logged into MCForge.net!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Invoke(new MethodInvoker(delegate { this.Dispose(); }));
                }
                else
                {
                    MessageBox.Show("Login failed, check your login info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Invoke(new MethodInvoker(delegate { this.Enabled = true; this.Focus(); }));
                }
            })).Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.mcforge.net/forums/member.php?action=register");
            }
            catch
            {
                MessageBox.Show(this, "Failed to open link!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
