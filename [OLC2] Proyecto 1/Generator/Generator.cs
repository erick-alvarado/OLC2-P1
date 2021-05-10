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
        public LinkedList<String> tempsAux;//Temps no utilizados

        private int temporal=0, label = 0, SP = 0, HP = 0;


        public Generator()
        {
            this.temporal = this.label = 0;
            this.temps = new LinkedList<String>();
            this.code = new LinkedList<String>();
            this.tempsAux = new LinkedList<String>();
            this.addNativePrint();
            this.addNativeCompareStrings();
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
            this.tempsAux = new LinkedList<String>();
            this.addNativePrint();
            this.addNativeCompareStrings();

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
            this.tempsAux.AddLast(temp);
            return temp;
        }
        public String newTemp2()
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
            return this.SP;
        }
        public int addSP(int value)
        {
            this.SP +=value;
            return this.SP;
        }
        public int addHP()
        {
            this.HP++;
            return this.HP;
        }
        public int getSP()
        {
            return this.SP;
        }
        public void reduceSP()
        {
            this.SP--;
        }
        public int getHP()
        {
            return this.HP;
        }
        public void AddExp(String target, String left, String right = "", String op = "")
        {
            try
            {
                this.tempsAux.Remove(left);
                this.tempsAux.Remove(right);
                this.tempsAux.Remove(op);

            }
            catch (Exception)
            {

            }
            this.code.AddLast(target + " = " + left + op + right + ";");
            
        }

        public void SetStack(String temp_pos,object value)
        {
            this.code.AddLast("stack[(int)"+temp_pos+"] = " + value.ToString() + ";");
            this.addSP();
            try
            {
                this.tempsAux.Remove(temp_pos.ToString());
                this.tempsAux.Remove(value.ToString());
            }
            catch (Exception)
            {

            }
        }

        public void SetHeap(String temp_pos, object value)
        {
            this.code.AddLast("heap[(int)" + temp_pos + "] = " + value.ToString() + ";");
            this.addHP();

            try
            {
                this.tempsAux.Remove(temp_pos.ToString());
                this.tempsAux.Remove(value.ToString());
            }
            catch (Exception)
            {

            }
        }
        public void AddStack(object value)
        {
            String temp = this.newTemp();
            this.AddExp(temp, this.SP.ToString());
            this.code.AddLast("stack[(int)" + temp + "] = " + value.ToString() + ";");
            this.addSP();

            try
            {
                this.tempsAux.Remove(temp);
                this.tempsAux.Remove(value.ToString());
            }
            catch (Exception)
            {

            }
        }
        public void AddHeap(object value)
        {
            String temp = this.newTemp();
            this.AddExp(temp, this.HP.ToString());
            this.code.AddLast("heap[(int)" + temp + "] = " + value.ToString() + ";");
            this.addHP();

            try
            {
                this.tempsAux.Remove(temp);
                this.tempsAux.Remove(value.ToString());
            }
            catch (Exception)
            {

            }
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
            try
            {
                this.tempsAux.Remove(left);
                this.tempsAux.Remove(right);
            }
            catch (Exception)
            {

            }
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
            try
            {
                this.tempsAux.Remove(value);
            }
            catch (Exception)
            {

            }
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

            String temp1 = this.newTemp();
            String temp2 = this.newTemp();
            String temp3 = this.newTemp();
            try
            {
                this.tempsAux.Remove(temp1);
                this.tempsAux.Remove(temp2);
                this.tempsAux.Remove(temp3);
            }
            catch (Exception)
            {

            }

            this.code.AddLast("void");
            this.code.AddLast("printString");
            this.code.AddLast(temp1 + "= SP+1;");
            this.code.AddLast(temp2+"= stack[(int)"+temp1+"];");
            this.code.AddLast(lbl1+":");
            this.code.AddLast(temp3+"= heap[(int)"+temp2+"];");
            this.code.AddLast("if ("+temp3+" == -1) goto "+lbl0 + ";");
            this.code.AddLast("printf("+"\"%c\""+", (char)"+temp3+");");
            this.code.AddLast(temp2+"= "+temp2+" + 1;");
            this.code.AddLast("goto "+lbl1+";");
            this.code.AddLast(lbl0+":");
            this.code.AddLast("}");

        }
        public void addNativeCompareStrings()
        {
            String temp3 = this.newTemp();
            String temp4 = this.newTemp();
            String temp5 = this.newTemp();
            String temp6 = this.newTemp();
            String temp7 = this.newTemp();
            try
            {
                this.tempsAux.Remove(temp3);
                this.tempsAux.Remove(temp4);
                this.tempsAux.Remove(temp5);
                this.tempsAux.Remove(temp6);
                this.tempsAux.Remove(temp7);
            }
            catch (Exception)
            {

            }

            String lbl0 = this.newLabel();
            String lbl1 = this.newLabel();
            String lbl2 = this.newLabel();
            String lbl3 = this.newLabel();

            this.code.AddLast("void");
            this.code.AddLast("compareString");

            this.code.AddLast(temp3 + " = SP + 1;");
            this.code.AddLast(temp4 + " = stack[(int)"+temp3+"];");
            this.code.AddLast(temp3 + " = " + temp3 + " + 1;");
            this.code.AddLast(temp5 +"= stack[(int)"+temp3+"];");
            this.code.AddLast(lbl1+":");
            this.code.AddLast(temp6 +"= heap[(int)"+temp4+"];");
            this.code.AddLast(temp7 +"= heap[(int)"+temp5+"];");
            this.code.AddLast("if ("+temp6+" != "+temp7+") goto "+lbl3+";");
            this.code.AddLast("if ("+temp6+" == -1) goto "+lbl2+";");
            this.code.AddLast(temp4+" = "+temp4+" + 1;");
            this.code.AddLast(temp5+" = "+temp5+" + 1;");
            this.code.AddLast("goto "+lbl1+";");
            this.code.AddLast(lbl2+":");
            this.code.AddLast("stack[(int)SP] = 1;");
            this.code.AddLast("goto "+lbl0+";");
            this.code.AddLast(lbl3+":");
            this.code.AddLast("stack[(int)SP] = 0;");
            this.code.AddLast(lbl0+":");
            this.code.AddLast("}");

        }

    }
}
