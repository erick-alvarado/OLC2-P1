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
using System.IO;
using System.Text;

namespace _OLC2__Proyecto_1
{
    public partial class Form1 : Form
    {

        private Dot graphviz;
        private Analyzer n;
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
            n = new Analyzer();
          
            textBox2.Text= n.analyze(textBox1.Text);

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private static void generateAst(ParseTreeNode root)
        {
            String graphDot = Dot.getDot(root);
            var filename = "tree.txt";
            SaveToFile(graphDot, filename);
            System.Diagnostics.Process.Start(filename);
            string path = Directory.GetCurrentDirectory();
            GenerateGraph(filename, path);
            System.Diagnostics.Process.Start("tree.svg");

        }
        private static void GenerateGraph(string filename, string path)
        {
            try
            {
                var command = string.Format("dot -Tsvg \"{0}\" -o \"{1}\"", Path.Combine(path, filename), Path.Combine(path, filename.Replace(".txt", ".svg")));
                var procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);
                var proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                proc.WaitForExit();
            } 
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private void tablaDeSimbolosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            generateAst(n.root);
        }

        private static void SaveToFile(String text, string filename)
        {
            TextWriter tw = new StreamWriter(filename);
            tw.WriteLine(text);
            tw.Close();
        }
    }
}
