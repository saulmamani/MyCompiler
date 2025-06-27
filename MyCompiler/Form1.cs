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
        List<Simbolo> _tablaSimbolos = new List<Simbolo>();
        private Lexico _lexico = new Lexico();
        private Sintactico _sintactico = new Sintactico();

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
            _tablaSimbolos.Clear();
            _tablaSimbolos = _lexico.Analizar(sourceTextBox.Text);
            dataGridView1.DataSource = _tablaSimbolos;

            if (_lexico.Errors.Count > 0)
            {
                MessageBox.Show("Se encontraron errores léxicos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _tablaSimbolos.Clear();
                listErrors.DataSource = _lexico.Errors;
            }
            else
            {
                listErrors.DataSource = null;
                SetMessage("Análisis léxico finalizado sin errores");
            }

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

        private void btnSintactico_Click(object sender, EventArgs e)
        {
            if (_tablaSimbolos.Count <= 0)
                MessageBox.Show("Ejecute el Analizador Lexico Primero", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                bool resultado = _sintactico.Analizar(_tablaSimbolos);
                if (!resultado)
                {
                    MessageBox.Show("Se encontraron errores sintácticos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listErrors.DataSource = _sintactico.Errors;
                }
                else
                {
                    listErrors.DataSource = null;
                    SetMessage("Análisis sintáctico finalizado sin errores");
                }
            }
        }
    }
}
