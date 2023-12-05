
namespace Chat_winform
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.ExecuteOnMainThreadWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // inputBox
            // 
            this.inputBox.Location = new System.Drawing.Point(12, 263);
            this.inputBox.MaxLength = 1020;
            this.inputBox.Multiline = true;
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(413, 57);
            this.inputBox.TabIndex = 1;
            this.inputBox.TextChanged += new System.EventHandler(this.inputBox_TextChanged);
            this.inputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputBox_KeyPress);
            // 
            // sendButton
            // 
            this.sendButton.Location = new System.Drawing.Point(431, 263);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 57);
            this.sendButton.TabIndex = 2;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(12, 12);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(494, 245);
            this.logTextBox.TabIndex = 3;
            this.logTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // ExecuteOnMainThreadWorker
            // 
            this.ExecuteOnMainThreadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ExecuteOnMainThreadWorker_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 336);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.inputBox);
            this.MaximumSize = new System.Drawing.Size(534, 375);
            this.MinimumSize = new System.Drawing.Size(534, 375);
            this.Name = "Form1";
            this.Text = "Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Form popup;
        private System.Windows.Forms.TextBox inputBox;
        private System.Windows.Forms.Button sendButton;
        private System.String _username;
        public System.String username
        {
            get => _username;
            set
            {
                if (value.Length > 20)
                {
                    throw new System.Exception("Username is too long");
                }
                for (int i = 0; i < value.Length; ++i)
                {
                    if (!System.Char.IsLetterOrDigit(value[i])) throw new System.Exception("Invalid username");
                }
                _username = value;
            }
        }
        private System.String IP;
        public Connection connection;
        public System.Windows.Forms.TextBox logTextBox;
        private System.ComponentModel.BackgroundWorker ExecuteOnMainThreadWorker;
    }
}

