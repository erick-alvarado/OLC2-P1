using _OLC2__Proyecto_1.Symbol_;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace _OLC2__Proyecto_1.Gramm
{
    class Analyzer_
    {
        public static String output;
        public static List<Optimization> opList = new List<Optimization>();
        public ParseTreeNode root;
        private bool jump = false;
        private String label = "";
        private String code;

        public Analyzer_()
        {
            output = "";
            code = "";
        }
        public String analyze(String input)
        {
            opList.Clear();
            code = "";
            Gramm_ grammar = new Gramm_();
            LanguageData language = new LanguageData(grammar);
            Parser parser = new Parser(language);
            ParseTree tree = parser.Parse(input);

            root = tree.Root;
            ErrorHandler errorHandler = new ErrorHandler(tree, root);

            if (!errorHandler.hasErrors())
            {
                Analyzer.output = AST("", root).ToLower();

            }
            return Analyzer.output;
        }
        private String AST(String optimization, ParseTreeNode childs)
        {
            for(int i = 0; i< childs.ChildNodes.Count; i++)
            {
                ParseTreeNode child = childs.ChildNodes[i];
                if (child.ChildNodes.Count == 0 && child.Token != null)
                {
                    if (jump)
                    {
                        code += child.Token.Text;
                    }
                    else
                    {
                        switch (child.Token.Text)
                        {
                            case "goto":
                                //Rule 1
                                jump = true;
                                label = childs.ChildNodes[i + 1].Token.Text;

                                optimization += child.Token.Text;
                                optimization += " ";
                                optimization += childs.ChildNodes[i + 1].Token.Text;
                                optimization += childs.ChildNodes[i + 2].Token.Text;
                                optimization += "\r\n";
                                i += 2;
                                break;
                            case "if":
                                if (childs.ChildNodes.Count == 10)
                                {
                                    ParseTreeNode exp = childs.ChildNodes[i + 1].ChildNodes[0];//exp -> final exp
                                    exp = exp.ChildNodes[1];//expression final
                                    String operation = exp.ChildNodes[1].Token.Text;
                                    try
                                    {
                                        if (exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token == null || exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token == null)
                                        {
                                            throw new Exception("xd");
                                        }
                                        int val1 = Int32.Parse(exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text);
                                        int val2 = Int32.Parse(exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token.Text);
                                        bool result = false;
                                        switch (operation)
                                        {
                                            case ">":
                                                if (val1 > val2)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                                break;
                                            case "<":
                                                if (val1 < val2)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                                break;
                                            case ">=":
                                                if (val1 >= val2)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                                break;
                                            case "<=":
                                                if (val1 <= val2)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                                break;
                                            case "==":
                                                if (val1 == val2)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                                break;
                                            case "!=":
                                                if (val1 != val2)
                                                {
                                                    result = true;
                                                }
                                                else
                                                {
                                                    result = false;
                                                }
                                                break;
                                        }
                                        if (result)
                                        {
                                            //Rule 3
                                            opList.Add(new Optimization("Bloque", "Rule 3", "if(" + val1 + operation + val2 + ") goto " + childs.ChildNodes[i + 3].Token.Text + "; goto " + childs.ChildNodes[i + 6].Token.Text + ";", "goto " + childs.ChildNodes[i + 3].Token.Text + ";", child.Token.Location.Line));
                                            optimization += childs.ChildNodes[i + 2].Token.Text + " ";
                                            optimization += childs.ChildNodes[i + 3].Token.Text;
                                            optimization += childs.ChildNodes[i + 4].Token.Text;
                                            optimization += "\r\n";
                                        }
                                        else
                                        {
                                            //Rule 4
                                            opList.Add(new Optimization("Bloque", "Rule 4", "if(" + val1 + operation + val2 + ") goto " + childs.ChildNodes[i + 3].Token.Text + "; goto " + childs.ChildNodes[i + 6].Token.Text + ";", "goto " + childs.ChildNodes[i + 6].Token.Text + ";", child.Token.Location.Line));
                                            optimization += childs.ChildNodes[i + 5].Token.Text + " ";
                                            optimization += childs.ChildNodes[i + 6].Token.Text;
                                            optimization += childs.ChildNodes[i + 7].Token.Text;
                                            optimization += "\r\n";
                                        }
                                        i += 7;
                                    }
                                    catch (Exception)
                                    {
                                        //Rule 2
                                        String val1, val2;
                                        if (exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token == null)
                                        {
                                            val1 = exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                        }
                                        else
                                        {
                                            val1 = exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                        }
                                        if (exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token == null)
                                        {
                                            val2 = exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                        }
                                        else
                                        {
                                            val2 = exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token.Text;
                                        }

                                        optimization += child.Token.Text;
                                        optimization += "(";
                                        String neg = "";
                                        switch (operation)
                                        {
                                            case ">":
                                                neg = val1 + "<" + val2;
                                                break;
                                            case "<":
                                                neg = val1 + ">" + val2;
                                                break;
                                            case ">=":
                                                neg = val1 + "<" + val2;
                                                break;
                                            case "<=":
                                                neg = val1 + ">" + val2;
                                                break;
                                            case "==":
                                                neg = val1 + "!=" + val2;
                                                break;
                                            case "!=":
                                                neg = val1 + "==" + val2;
                                                break;
                                        }
                                        opList.Add(new Optimization("Bloque", "Rule 2", "(" + val1 + operation + val2 + ")" + childs.ChildNodes[i + 2].Token.Text + " " + childs.ChildNodes[i + 3].Token.Text, "(" + neg + ")" + childs.ChildNodes[i + 5].Token.Text + " " + childs.ChildNodes[i + 6].Token.Text, child.Token.Location.Line));
                                        optimization += neg;
                                        optimization += ") ";
                                        optimization += childs.ChildNodes[i + 5].Token.Text + " ";
                                        optimization += childs.ChildNodes[i + 6].Token.Text;
                                        optimization += childs.ChildNodes[i + 7].Token.Text;
                                        optimization += "\r\n";
                                        i += 9;
                                    }

                                }
                                else
                                {
                                    optimization += child.Token.Text;
                                    optimization += AST("", childs.ChildNodes[i + 1]);
                                    optimization += childs.ChildNodes[i + 2].Token.Text + " ";
                                    optimization += childs.ChildNodes[i + 3].Token.Text;
                                    optimization += childs.ChildNodes[i + 4].Token.Text;
                                    optimization += "\r\n";
                                    i += 4;
                                }
                                break;
                            default:
                                if (child.Term.ToString() == "ID")
                                {
                                    try
                                    {
                                        String id = child.Token.Text;
                                        String val1, val2;
                                        ParseTreeNode exp = childs.ChildNodes[3];
                                        String operation = exp.ChildNodes[1].Token.Text;
                                        if (exp.ChildNodes.Count == 3)
                                        {
                                            if (exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token == null)
                                            {
                                                val1 = exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                            }
                                            else
                                            {
                                                val1 = exp.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                            }
                                            if (exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token == null)
                                            {
                                                val2 = exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Text;
                                            }
                                            else
                                            {
                                                val2 = exp.ChildNodes[2].ChildNodes[0].ChildNodes[0].Token.Text;
                                            }


                                            switch (operation)
                                            {
                                                case "+":
                                                    //regla 6
                                                    if (id == val1)
                                                    {
                                                        if (val2 == "0")
                                                        {
                                                            opList.Add(new Optimization("Mirilla", "Rule 6", id + "=" + id + "+0;", "", child.Token.Location.Line));
                                                            return optimization;
                                                        }
                                                        else
                                                        {
                                                            optimization += child.Token.Text;
                                                        }
                                                    }
                                                    else if (id == val2)
                                                    {
                                                        if (val1 == "0")
                                                        {
                                                            opList.Add(new Optimization("Mirilla", "Rule 6", id + "=" + id + "+0;", "", child.Token.Location.Line));
                                                            return optimization;
                                                        }
                                                        else
                                                        {
                                                            optimization += child.Token.Text;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        optimization += child.Token.Text;
                                                    }
                                                    break;
                                                case "-":
                                                    //regla 7
                                                    if (id == val1)
                                                    {
                                                        if (val2 == "0")
                                                        {
                                                            opList.Add(new Optimization("Mirilla", "Rule 7", id + "=" + id + "-0;", "", child.Token.Location.Line));
                                                            return optimization;
                                                        }
                                                        else
                                                        {
                                                            optimization += child.Token.Text;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        optimization += child.Token.Text;
                                                    }
                                                    break;
                                                case "*":
                                                    if (val2 == "1")
                                                    {
                                                        if (id == val1)
                                                        {
                                                            //regla 8
                                                            opList.Add(new Optimization("Mirilla", "Rule 8", id + "=" + id + "*1;", "", child.Token.Location.Line));
                                                            return optimization;
                                                        }
                                                        else
                                                        {
                                                            //regla 12
                                                            opList.Add(new Optimization("Mirilla", "Rule 12", id + "=" + val1 + "*1;", id + "=" + val1 + ";", child.Token.Location.Line));
                                                            optimization += child.Token.Text + "=" + val1;
                                                            i += 3;
                                                        }
                                                    }
                                                    else if (val1 == "1")
                                                    {
                                                        if (id == val2)
                                                        {
                                                            //regla 8
                                                            opList.Add(new Optimization("Mirilla", "Rule 8", id + "=" + "1*" + id + ";", "", child.Token.Location.Line));
                                                            return optimization;
                                                        }
                                                        else
                                                        {
                                                            //regla 12
                                                            opList.Add(new Optimization("Mirilla", "Rule 12", id + "= 1*" + val2 + ";", id + "=" + val2 + ";", child.Token.Location.Line));
                                                            optimization += child.Token.Text + "=" + val2;
                                                            i += 3;
                                                        }
                                                    }
                                                    else if (val2 == "0")
                                                    {
                                                        //regla 15
                                                        opList.Add(new Optimization("Mirilla", "Rule 15", id + "=" + val1 + "*0;", id + "=0;", child.Token.Location.Line));
                                                        optimization += child.Token.Text + "= 0";
                                                        i += 3;
                                                    }
                                                    else if (val1 == "0")
                                                    {
                                                        //regla 15
                                                        opList.Add(new Optimization("Mirilla", "Rule 15", id + "=0*" + val2 + ";", id + "=0;", child.Token.Location.Line));
                                                        optimization += child.Token.Text + "= 0";
                                                        i += 3;
                                                    }
                                                    else if (val2 == "2")
                                                    {
                                                        //Rule 14
                                                        opList.Add(new Optimization("Mirilla", "Rule 14", id + "=" + val1 + "*2;", id + "=" + val1 + "+" + val1 + ";", child.Token.Location.Line));
                                                        optimization += child.Token.Text + "=" + val1 + "+" + val1;
                                                        i += 3;
                                                    }
                                                    else if (val1 == "2")
                                                    {
                                                        //Rule 14
                                                        opList.Add(new Optimization("Mirilla", "Rule 14", id + "=2*" + val2 + ";", id + "=" + val2 + "+" + val2 + ";", child.Token.Location.Line));
                                                        optimization += child.Token.Text + "=" + val2 + "+" + val2;
                                                        i += 3;
                                                    }
                                                    else
                                                    {
                                                        optimization += child.Token.Text;
                                                    }
                                                    break;
                                                case "/":
                                                    if (val2 == "1")
                                                    {
                                                        if (id == val1)
                                                        {
                                                            //regla 9
                                                            opList.Add(new Optimization("Mirilla", "Rule 9", id + "=" + id + "/1;", "", child.Token.Location.Line));
                                                            return optimization;
                                                        }
                                                        else
                                                        {
                                                            //regla 13
                                                            opList.Add(new Optimization("Mirilla", "Rule 13", id + "=" + val1 + "/1;", id + "=" + val1 + ";", child.Token.Location.Line));
                                                            optimization += child.Token.Text + "=" + val1;
                                                            i += 3;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (val1 == "0")
                                                        {
                                                            //regla 16
                                                            opList.Add(new Optimization("Mirilla", "Rule 16", id + "=" + "0/" + val2 + ";", id + "=" + "0;", child.Token.Location.Line));
                                                            optimization += child.Token.Text + "= 0";
                                                            i += 3;
                                                        }
                                                        else
                                                        {
                                                            optimization += child.Token.Text;
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            optimization += child.Token.Text;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        optimization += child.Token.Text;
                                    }
                                }
                                else
                                {
                                    optimization += child.Token.Text;
                                }
                                break;
                        }
                        if (child.Token.Text == ":" || child.Token.Text == "{" || child.Token.Text == ";" || child.Token.Text == "}")
                        {
                            optimization += "\r\n";
                        }
                        optimization += " ";
                    }
                }

                else if (child.Term.Name == "label" || child.Term.Name == "ret")
                {
                    if (child.Term.Name == "label" )
                    {
                        opList.Add(new Optimization("Bloque", "Rule 1", code, "", child.ChildNodes[0].Token.Location.Line - 1));
                    }
                    jump = false;
                    code = "";
                }

                optimization = AST(optimization,child);
            }
            return optimization;
        }
    }
}
