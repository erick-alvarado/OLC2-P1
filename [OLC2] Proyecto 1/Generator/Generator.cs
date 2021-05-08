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
            this.addNativePrint();
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
            this.temporal = this.label = SP = HP = 0;
            this.code = new LinkedList<String>();
            this.temps = new LinkedList<String>();
            this.addNativePrint();
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
                    if (i < this.temps.Count-1)
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
            String ret = this.getHeader();
            String main_code = "";

            bool change = false;
            
            ret += "/********Natives********/\r\n";
            for(int i = 0; i < this.code.Count; i++)
            {
                String line = this.code.ElementAt(i);
                if (line == "void")
                {
                    ret += "void " + this.code.ElementAt(i + 1) + "(){\r\n";
                    change = true;
                    i++;
                    continue;
                }
                else if (line == "}")
                {
                    change = false;
                    ret += "\r\n\treturn;\r\n}\r\n";
                    continue;
                }

                if (change)
                {
                    ret += "\t" + line + "\r\n";
                }
                else
                {
                    main_code += "\t" + line + "\r\n";
                }
            }

            ret +="\r\nvoid main(){\r\n\tSP=0; \r\n\t HP=0;\r\n\r\n";
            ret += main_code;
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
            this.code.AddLast("stack[(int)SP] = " +value.ToString() + ";");
            addSP();
        }
        public void AddHeap(object value)
        {
            this.code.AddLast("heap[(int)HP] = " + value.ToString() + ";");
            addHP();
        }

        public void SetStack(String temp_pos,object value)
        {
            this.code.AddLast("stack[(int)"+temp_pos+"] = " + value.ToString() + ";");
        }
        public void SetHeap(String temp_pos, object value)
        {
            this.code.AddLast("heap[(int)" + temp_pos + "] = " + value.ToString() + ";");
        }
        public void AddCom(String com)
        {
            this.code.AddLast("/*********" + com + "*********/");
        }

        public void addGoto(String label)
        {
            this.code.AddLast("goto " + label + ";");
        }

        public void addIf(String left, String op, String right, String label)
        {
            this.code.AddLast("if (" + left + op + right + ") goto " + label + ";");
        }


        public void addPrint(String format, String value)
        {
            String type="";
            if (format == "c")
            {
                type = "char";
            }else if(format == "d")
            {
                type = "int";
            }
            else
            {
                type = "float";
            }
            this.code.AddLast("printf(\"%" + format + "\", ("+type+")" + value + ");");
        }

        public void printTrue()
        {
            this.addPrint("c", "116");
            this.addPrint("c", "114");
            this.addPrint("c", "117");
            this.addPrint("c", "101");
        }

        public void printFalse()
        {
            this.addPrint("c", "102");
            this.addPrint("c", "97");
            this.addPrint("c", "108");
            this.addPrint("c", "115");
            this.addPrint("c", "101");
        }
        public void printSpace()
        {
            this.addPrint("c", "10");
        }
        public void addCall(String name)
        {   
            this.code.AddLast(name + "();") ;
        }
        public void addNativePrint()
        {
            String lbl1 = this.newLabel();
            String lbl0 = this.newLabel();

            String temp2 = this.newTemp();
            String temp3 = this.newTemp();


            this.code.AddLast("void");
            this.code.AddLast("printString");
            this.code.AddLast(temp2+"= SP;");
            this.code.AddLast(lbl1+":");
            this.code.AddLast(temp3+"= heap[(int)"+temp2+"];");
            this.code.AddLast("if ("+temp3+" == -1) goto "+lbl0 + ";");
            this.code.AddLast("printf("+"\"%c\""+", (char)"+temp3+");");
            this.code.AddLast(temp2+"= "+temp2+" + 1;");
            this.code.AddLast("goto "+lbl1+";");
            this.code.AddLast(lbl0+":");
            this.code.AddLast("}");

        }
    }
}
