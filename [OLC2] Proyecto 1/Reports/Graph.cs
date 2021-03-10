using _OLC2__Proyecto_1.Symbol_;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Reports
{
    public class Graph
    {
        public static void generateAst(ParseTreeNode root)
        {
            String graphDot = Dot.getDot(root);
            var filename = "tree.txt";
            SaveToFile(graphDot, filename);
            System.Diagnostics.Process.Start(filename);
            string path = Directory.GetCurrentDirectory();
            GenerateGraph(filename, path);
            System.Diagnostics.Process.Start("tree.svg");

        }
        public static void generateVar(Environment_ e)
        {
            String graphDot = Dot.getDotVar(e);
            var filename = "Var.txt";
            SaveToFile(graphDot, filename);
            System.Diagnostics.Process.Start(filename);
            string path = Directory.GetCurrentDirectory();
            GenerateGraph(filename, path);
            System.Diagnostics.Process.Start("Var.svg");

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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private static void SaveToFile(String text, string filename)
        {
            TextWriter tw = new StreamWriter(filename);
            tw.WriteLine(text);
            tw.Close();
        }
    }
}
