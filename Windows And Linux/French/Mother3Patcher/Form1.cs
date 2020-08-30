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
                MessageBox.Show("Aucune ROM Sélectionné!", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string inputFile = Path.GetFullPath(txtRomPath.Text);
            string outputFile = inputFile + ".tmp";
            string patchFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),"mother3.ups");
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
                MessageBox.Show("Installation terminée!", "Installation terminée", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Échec de l'installation!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
