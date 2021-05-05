using _OLC2__Proyecto_1.Gramm;
using _OLC2__Proyecto_1.Reports;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _OLC2__Proyecto_1.Symbol_;
using _OLC2__Proyecto_1.Instructions.Functions;

namespace _OLC2__Proyecto_1
{
    public partial class Form1 : Form
    {
        private Analyzer n= new Analyzer();
        private Analyzer_ n2= new Analyzer_();
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "Pascal files (.txt) |*.txt";
                open.RestoreDirectory = true;

                if (open.ShowDialog() == DialogResult.OK)
                {
                    String dir = open.FileName;
                    String content;
                    var fileStream = open.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        content = reader.ReadToEnd();
                    }
                    textBox1.Text = content;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            n2 = new Analyzer_();
            textBox2.Text= n2.analyze(textBox1.Text);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            n = new Analyzer();
            textBox2.Text = n.analyze(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
       
        
        private void tablaDeSimbolosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        
        private void setVariables(Environment_ environment)
        {
            foreach(Symbol b in environment.variables)
            {
                data1.Rows.Add(b.id, b.type, b.type_name, environment.name, b.value,b.position,b.aux_value);
                if (b.type_name == "function")
                {
                    Function temp = (Function)b.value;
                    setVariables(temp.environmentAux);
                }
                if (b.type_name == "object")
                {
                    Environment_ temp = (Environment_)b.value;
                    setVariables(temp);
                }

            }
        }
        public void setErrors(List<Error_> list)
        {
            foreach(Error_ e in list)
            {
                data2.Rows.Add(e.type, e.msg, e.scope, e.line, e.column);
            }
        }

        public void setOptimizations(List<Optimization> list)
        {
            foreach (Optimization e in list)
            {
                data3.Rows.Add(e.type, e.rule, e.code_deleted, e.code_add, e.row);
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            data1.Rows.Clear();
            data1.Refresh();
            setVariables(n.environment);
            data1.Sort(this.data1.Columns["Environment"], ListSortDirection.Descending);

            data2.Rows.Clear();
            data2.Refresh();
            setErrors(Analyzer.errors);

            data3.Rows.Clear();
            data3.Refresh();
            setOptimizations(Analyzer_.opList);

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void aSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Graph.generateAst(this.n.root);
            }
            catch (Exception)
            {

            }
        }

        private void vARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Graph.generateVar(this.n.environment);
            }
            catch (Exception)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            n = new Analyzer();
            textBox2.Text = n.analyze(textBox1.Text);
        }
    }
}
