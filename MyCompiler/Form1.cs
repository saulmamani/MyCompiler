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
        }
    }
}
