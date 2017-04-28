using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Text;
using MetroFramework.Forms;
using MetroFramework;

namespace Steganography
{
    public partial class Steganography : MetroForm
    {
        private Bitmap b = null;
        private string messageExt = string.Empty;

        public Steganography()
        {
            InitializeComponent();
        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            Stopwatch aes = new Stopwatch();
            aes.Start();

            b = (Bitmap)imagePictureBox.Image;

            string text = dataTextBox.Text;

            if (text.Equals(""))
            {
                MetroMessageBox.Show(this, "The text you want to hide can't be empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (encryptCheckBox.Checked)
            {
                if (encrypTypeDES.Checked)
                {
                    if (passwordTextBox.Text.Length !=8)
                    {
                        MetroMessageBox.Show(this, "Please enter a password of 8 characters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     
                        return;
                    }
                    else
                    {
                        byte[] bytes = ASCIIEncoding.ASCII.GetBytes(passwordTextBox.Text);
                        text = DES.Encrypt(text, bytes);
                    }
                }
                else if(encrypTypeAES.Checked)
                {
                    if (passwordTextBox.Text.Length <8)
                    {
                        MetroMessageBox.Show(this, "Please enter a password of minimum 8 characters", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {

                        text = Crypto.AES_Enc(text, passwordTextBox.Text);
                    }
                }
                
            }

            dataTextBox.Text = text;
            b = SteganographyHelper.msgHiding(text, b);

            aes.Stop();
            MetroMessageBox.Show(this, "Your text was hidden in the image successfully!", "Succcess", MessageBoxButtons.OK, MessageBoxIcon.Information);
          
            aes_time.Text = aes.Elapsed.ToString();

        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            b = (Bitmap)imagePictureBox.Image;

            string messageExt = SteganographyHelper.msgRetrieve(b);

            if (encryptCheckBox.Checked)
            {
                if(encrypTypeDES.Checked)
                {
                    try
                    {
                        byte[] bytes = ASCIIEncoding.ASCII.GetBytes(passwordTextBox.Text);
                        messageExt = DES.Decrypt(messageExt, bytes);
                    }
                    catch
                    {
                        MetroMessageBox.Show(this, "Wrong password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                }
                else if(encrypTypeAES.Checked)
                {
                    try
                    {

                        messageExt = Crypto.AES_Dec(messageExt, passwordTextBox.Text);
                    }
                    catch
                    {
                        MetroMessageBox.Show(this, "Wrong password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                
            }

            dataTextBox.Text = messageExt;
        }

        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files (*.jpeg; *.png; *.b)|*.jpg; *.png; *.b";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imagePictureBox.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Png Image|*.png|Bitmap Image|*.b";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                switch (sfd.FilterIndex)
                {
                    case 0:
                        {
                            b.Save(sfd.FileName, ImageFormat.Png);
                        }break;
                    case 1:
                        {
                            b.Save(sfd.FileName, ImageFormat.Bmp);
                        } break;
                }
            }
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, dataTextBox.Text);
            }
        }

        private void textToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dataTextBox.Text = File.ReadAllText(ofd.FileName);
            }
        }

        private void encryptCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(encryptCheckBox.Checked)
            {
                encrypTypeAES.Enabled = true;
                encrypTypeDES.Enabled = true;
            }
            else
            {
                encrypTypeAES.Enabled = false;
                encrypTypeDES.Enabled = false;
            }
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void imagePictureBox_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aes_time_TextChanged(object sender, EventArgs e)
        {

        }

        private void Steganography_Load(object sender, EventArgs e)
        {
            dataTextBox.Focus();
        }

        private void Steganography_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}