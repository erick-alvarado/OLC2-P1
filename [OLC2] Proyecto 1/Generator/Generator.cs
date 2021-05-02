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
        private LinkedList<String> tempStorage;
        public String isFunc;

        private int temporal;
        private int label;


        public Generator()
        {
            this.temporal = this.label = 0;
            this.temps = new LinkedList<String>();
            this.code = new LinkedList<String>();
            this.tempStorage = new LinkedList<String>();
        }

        public static Generator getInstance()
        {
            if(generator == null)
            {
                generator = new Generator();
            }
            return generator;
        }

        public LinkedList<String> getTempStorage()
        {
            return this.tempStorage;
        }

        public void clearTempStorage()
        {
            this.tempStorage = new LinkedList<String>();
        }

        public void setTempStorage(LinkedList<String> temp)
        {
            this.tempStorage = temp;
        }

        public void clearCode()
        {
            this.temporal = this.label = 0;
            this.code = new LinkedList<String>();
            this.temps = new LinkedList<String>();
            this.tempStorage = new LinkedList<String>();
        }

        public void addCode(String code)
        {
            this.code.AddLast(this.isFunc + code);
        }

        public String getHeader()
        {
            return "#include <stdio.h>\nfloat heap[100000];\nfloat stack[100000];\nfloat P;\nfloat H;\n" + this.getTempsString() + "\n";
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
            String ret = this.getHeader() + "\nvoid main(){\n\tP=0; \n\t H=0;\n\n";

            foreach (String line in this.code)
            {
                ret += "\t" + line + "\n";
            }
            ret += "\n\treturn;\n}";
            return ret;
        }

        public void addSpace()
        {
            this.code.AddLast("\n");
        }

        public String newTemp()
        {
            String temp = "t" + this.temporal++;
            this.tempStorage.AddLast(temp);
            this.temps.AddLast(temp);
            return temp;
        }

        public String newLabel()
        {
            return "L" + this.label++;
        }

        public void addLabel(String label)
        {
            this.code.AddLast(this.isFunc + label + ":");
        }

        public void AddExp(String target, String left, String right = "", String op = "")
        {
            this.code.AddLast(this.isFunc + target + " = " + left + op + right + ";");
        }

        public void addGoto(String label)
        {
            this.code.AddLast(this.isFunc + "goto " + label + ";");
        }

        public void addIf(String left, String right, String op, String label)
        {
            this.code.AddLast(this.isFunc + "if " + left + op + right + " goto " + label + ";");
        }

        // TODO:
        public void addPrint(String format, String value)
        {
            this.code.AddLast(this.isFunc + "print(\"%" + format + "\", " + value + ");");
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
