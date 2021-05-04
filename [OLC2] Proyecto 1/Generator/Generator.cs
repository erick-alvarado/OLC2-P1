using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compilador.Generator
{
    class Generator
    {
        private static Generator generator;
        private LinkedList<String> code;
        private LinkedList<String> temps;

        private int temporal=0, label = 0, SP = 0, HP = 0;



        public Generator()
        {
            this.temporal = this.label = 0;
            this.temps = new LinkedList<String>();
            this.code = new LinkedList<String>();
        }

        public static Generator getInstance()
        {
            if(generator == null)
            {
                generator = new Generator();
            }
            return generator;
        }

       
        public void clearCode()
        {
            this.temporal = this.label = 0;
            this.code = new LinkedList<String>();
            this.temps = new LinkedList<String>();
        }

        public void addCode(String code)
        {
            this.code.AddLast(code);
        }
        public int getTempNumber()
        {
            return this.temporal++;
        }
        public String getHeader()
        {
            return "#include <stdio.h>\r\nfloat heap[100000];\r\nfloat stack[100000];\r\nfloat SP;\r\nfloat HP;\r\n" + this.getTempsString() + "\r\n";
        }

        public String getTempsString()
        {
            String ret = "";

            if (this.temps.Count > 0)
            {
                ret += "float ";
                for (int i = 0; i < this.temps.Count; i++)
                {
                    String temp = this.temps.ElementAt(i);
                    ret += temp;
                    if (i < this.temps.Count)
                    {
                        ret += ",";
                    }
                }
                ret += ";";
                return ret;
            }
            else
            {
                return "";
            }
            
        }

        public String getCode()
        {
            String ret = this.getHeader() + "\r\nvoid main(){\r\n\tSP=0; \r\n\t HP=0;\r\n\r\n";

            foreach (String line in this.code)
            {
                ret += "\t" + line + "\r\n";
            }
            ret += "\r\n\treturn;\r\n}";
            return ret;
        }

        public void addSpace()
        {
            this.code.AddLast("\r\n");
        }

        public String newTemp()
        {
            String temp = "T" + this.temporal++;
            this.temps.AddLast(temp);
            return temp;
        }

        public String newLabel()
        {
            return "L" + this.label++;
        }

        public void addLabel(String label)
        {
            this.code.AddLast(label + ":");
        }
        public int addSP()
        {
            this.SP++;
            this.code.AddLast("SP = SP + 1;");
            return this.SP;
        }
        public int addHP()
        {
            this.HP++;
            this.code.AddLast("HP = HP + 1;");
            return this.HP;
        }
        public int getSP()
        {
            return this.SP-1;
        }
        public int getHP()
        {
            return this.HP-1;
        }
        public void AddExp(String target, String left, String right = "", String op = "")
        {
            this.code.AddLast(target + " = " + left + op + right + ";");
            
        }
        public void AddStack(object value)
        {
            this.code.AddLast("stack[SP] = " +value.ToString() + ";");
            
            addSP();
            

        }
        public void AddHeap(object value)
        {
            this.code.AddLast("heap[HP] = " + value.ToString() + ";");
            
            addHP();
            
        }

        public void addGoto(String label)
        {
            this.code.AddLast("goto " + label + ";");
        }

        public void addIf(String left, String right, String op, String label)
        {
            this.code.AddLast("if " + left + op + right + " goto " + label + ";");
        }

        // TODO:
        public void addPrint(String format, String value)
        {
            this.code.AddLast("print(\"%" + format + "\", " + value + ");");
        }

        public void printTrue()
        {
            this.addPrint("c", "t");
            this.addPrint("c", "r");
            this.addPrint("c", "u");
            this.addPrint("c", "e");
        }

        public void printFalse()
        {
            this.addPrint("c", "f");
            this.addPrint("c", "a");
            this.addPrint("c", "l");
            this.addPrint("c", "s");
            this.addPrint("s", "e");
        }
    }
}
