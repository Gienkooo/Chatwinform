using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chat_winform
{
    public partial class Popup : Form
    {
        public Popup(Form1 form1)
        {
            InitializeComponent();
            mainForm = form1;
            /*
            this.BringToFront();
            this.Activate();
            this.Focus();
            */
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.submitData(this.usernameInputBox.Text, this.ipInputBox.Text);
            this.Hide();
        }

        private void submitButton_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) && ModifierKeys != Keys.Alt)
            {
                e.Handled = true;
                button1_Click(sender, e);
            }
        }
    }
}
