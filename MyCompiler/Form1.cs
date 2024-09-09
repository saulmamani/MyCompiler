using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCompiler
{
    public partial class Form1 : Form
    {
        string _currentFileName = string.Empty;
        private Lexico _lexico = new Lexico();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sourceTextBox.Language = FastColoredTextBoxNS.Language.CSharp;

        }

        private void btnLexico_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _lexico.Analizar(sourceTextBox.Text);

            if (_lexico.Errors.Count > 0)
            {
                MessageBox.Show("Se encontraron errores léxicos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            listErrors.DataSource = _lexico.Errors;
            SetMessage("Análisis léxico finalizado");
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(sourceTextBox.Text))
            {
                DialogResult dialogResult = MessageBox.Show("Seguro que quiere crear un nuevo documento?", "My Compiler", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.OK)
                {
                    NewDocument();
                }
            }
            else
            {
                NewDocument();
            }
        }

        private void NewDocument()
        {
            sourceTextBox.Clear();
            _currentFileName = string.Empty;

            SetMessage("Nuevo documento creado");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFileName))
            {

                DialogResult dialogResult = saveFileDialog1.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog1.FileName, sourceTextBox.Text);
                    SetMessage("Documento guardado");
                }
            }
            else
            {
                System.IO.File.WriteAllText(_currentFileName, sourceTextBox.Text);
                SetMessage("Documento guardado");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(sourceTextBox.Text))
            {
                OpenDocument();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Seguro que quiere abrir un nuevo documento?", "My Compiler", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.OK)
                {
                    OpenDocument();
                }
            }
        }

        private void OpenDocument()
        {
            DialogResult dialogResult = openFileDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _currentFileName = openFileDialog1.FileName;
                sourceTextBox.Text = System.IO.File.ReadAllText(_currentFileName);

                SetMessage("Documento abierto");
            }
        }

        private void SetMessage(string message)
        {
            toolStripStatusMessage.Text = message;
        }
    }
}
