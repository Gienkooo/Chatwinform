
namespace Chat_winform
{
    partial class Popup
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.usernameInputBox = new System.Windows.Forms.TextBox();
            this.ipInputBox = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username: ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP: ";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // usernameInputBox
            // 
            this.usernameInputBox.Location = new System.Drawing.Point(84, 6);
            this.usernameInputBox.MaxLength = 20;
            this.usernameInputBox.Name = "usernameInputBox";
            this.usernameInputBox.Size = new System.Drawing.Size(111, 23);
            this.usernameInputBox.TabIndex = 2;
            this.usernameInputBox.Text = "TESTuser";
            // 
            // ipInputBox
            // 
            this.ipInputBox.Location = new System.Drawing.Point(84, 35);
            this.ipInputBox.MaxLength = 21;
            this.ipInputBox.Name = "ipInputBox";
            this.ipInputBox.Size = new System.Drawing.Size(110, 23);
            this.ipInputBox.TabIndex = 3;
            this.ipInputBox.Text = "127.0.0.1:2137";
            this.ipInputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.submitButton_KeyPress);
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(12, 65);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(182, 23);
            this.submitButton.TabIndex = 4;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.button1_Click);
            this.submitButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.submitButton_KeyPress);
            // 
            // Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 100);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.ipInputBox);
            this.Controls.Add(this.usernameInputBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Popup";
            this.Text = "Popup";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.submitButton_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button submitButton;
        public System.Windows.Forms.TextBox usernameInputBox;
        public System.Windows.Forms.TextBox ipInputBox;
        private Form1 mainForm;

    }
}