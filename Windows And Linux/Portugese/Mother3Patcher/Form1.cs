using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LibUPS;
using System.IO;

namespace Mother3Patcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Gameboy Advance ROM (*.gba)|*.gba";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtRomPath.Text = ofd.FileName;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRomPath.Text))
            {
                MessageBox.Show("No ROM selecionado!", "Erros", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string inputFile = Path.GetFullPath(txtRomPath.Text);
            string outputFile = inputFile + ".tmp";
            string patchFile = Path.GetFullPath("mother3.ups");
            try
            {
                LibUPS.LibUPS.ApplyUPS(inputFile, outputFile, patchFile);
                if (chkMakeBackup.Checked)
                {
                    File.Move(inputFile, inputFile + ".bak");
                }
                else
                {
                    File.Delete(inputFile);
                }
                File.Move(outputFile, inputFile);
                MessageBox.Show("Patch completo!", "Patch completo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Corrigindo falhou!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
