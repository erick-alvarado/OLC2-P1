using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
namespace _OLC2__Proyecto_1.Gramm
{
    class Gramm_: Grammar
    {
        public Gramm_() : base(caseSensitive: false)
        {
            #region RE
            StringLiteral STR = new StringLiteral("STR", "\"");
            var INTEGER = new NumberLiteral("INTEGER");
            IdentifierTerminal ID = new IdentifierTerminal("ID");

            CommentTerminal lineComment = new CommentTerminal("lineComment", "//", "\n", "\r\n");
            CommentTerminal parComment = new CommentTerminal("parComment", "/*", "*/");
            #endregion

            #region Terminals
            var RTYPE = ToTerm("type");
            var RMAIN = ToTerm("main");
            var RINCLUDE = ToTerm("include");
            var RSTUDIO = ToTerm("stdio");
            var RH = ToTerm("h");

            var HASH = ToTerm("#");

            var TIMES = ToTerm("*");
            var DIVISION = ToTerm("/");
            var MODULE = ToTerm("%");
            var PLUS = ToTerm("+");
            var MINUS = ToTerm("-");

            var GREAT_EQ = ToTerm(">=");
            var LESS_EQ = ToTerm("<=");
            var DISTINCT = ToTerm("!=");
            var GREAT = ToTerm(">");
            var LESS = ToTerm("<");

            var OR = ToTerm("||");
            var AND = ToTerm("&&");
            var NOT = ToTerm("!");

            var EQUAL = ToTerm("=");
            var EQ_EQ = ToTerm("==");
            var SEMICOLON = ToTerm(";");
            var LEFTKEY = ToTerm("{");
            var RIGHTKEY = ToTerm("}");
            var LEFTPAR = ToTerm("(");
            var RIGHTPAR = ToTerm(")");
            var LEFTCOR = ToTerm("[");
            var RIGHTCOR = ToTerm("]");
            var COMMA = ToTerm(",");
            var POINT = ToTerm(".");
            var POINTS = ToTerm(":");

            var RSTRING = ToTerm("string");
            var RINT = ToTerm("int");
            var RFLOAT = ToTerm("float");
            var RVOID = ToTerm("void");
            var RGOTO = ToTerm("goto");

            var RIF = ToTerm("if");


           
            var RPRINTF = ToTerm("printf");
            var RRETURN = ToTerm("return");

            RegisterOperators(1, Associativity.Left, OR);
            RegisterOperators(2, Associativity.Left, AND);
            RegisterOperators(3, Associativity.Right, NOT);
            RegisterOperators(4, Associativity.Left, EQUAL, DISTINCT, LESS_EQ, GREAT_EQ, LESS, GREAT);
            RegisterOperators(5, Associativity.Left, PLUS, MINUS);
            RegisterOperators(6, Associativity.Left, TIMES, DIVISION, MODULE);

            NonGrammarTerminals.Add(lineComment);
            NonGrammarTerminals.Add(parComment);
            #endregion

            #region NonTerminals
            NonTerminal start = new NonTerminal("start");
            NonTerminal ret = new NonTerminal("ret");

            NonTerminal main = new NonTerminal("main");
            NonTerminal import = new NonTerminal("import");

            NonTerminal instructionList = new NonTerminal("instructionList");
            NonTerminal instruction = new NonTerminal("instruction");

            NonTerminal statements = new NonTerminal("statements");
            NonTerminal argument = new NonTerminal("argument");
            NonTerminal argumentList = new NonTerminal("argumentList");
            NonTerminal parameterList = new NonTerminal("parameterList");



            // TYPES
            NonTerminal type = new NonTerminal("type");
            NonTerminal label = new NonTerminal("label");



            // FUNCTIONS
            NonTerminal functionST = new NonTerminal("functionST");
            NonTerminal callFuncST = new NonTerminal("callFuncST");


            // DECLARATION
            NonTerminal declara = new NonTerminal("declara");
            NonTerminal declaration = new NonTerminal("declaration");

            NonTerminal declarationList = new NonTerminal("declarationList");



            // ASSIGNMENT
            NonTerminal assignmentST = new NonTerminal("assignmentST");

            // PRINT
            NonTerminal printST = new NonTerminal("printST");

            /* CONDITIONS */
            // IF
            NonTerminal ifST = new NonTerminal("ifST");

           
            /* EXPRESION */
            NonTerminal expression = new NonTerminal("expression");
            NonTerminal expressionList = new NonTerminal("expressionList");
            NonTerminal finalExp = new NonTerminal("finalExp");

            /* GENERAL */
            NonTerminal access = new NonTerminal("access");
            //Extra
            NonTerminal idList = new NonTerminal("idList");
            NonTerminal expListArray = new NonTerminal("expListArray");


            #endregion

            #region Grammar
            start.Rule = import + instructionList +  main
                | import + instructionList + main + instructionList
                | import + main + instructionList
                ;
            import.Rule = HASH + RINCLUDE + LESS + RSTUDIO + POINT + RH + GREAT + SEMICOLON
                | HASH + RINCLUDE + LESS + RSTUDIO + POINT + RH + GREAT
                ;
            //DECLARATION

            declaration.Rule = type + ID + EQUAL + expression + SEMICOLON
                | type + idList + LEFTCOR + expression + RIGHTCOR + SEMICOLON
                | type + idList + SEMICOLON
                ;
           

            idList.Rule = idList + COMMA + ID
                | ID
                ;


            assignmentST.Rule = ID+ expListArray+ EQUAL + expression + SEMICOLON
                ;

;
            expListArray.Rule = LEFTCOR + expression + RIGHTCOR
                | Empty
                ;
            statements.Rule = LEFTKEY + instructionList + RIGHTKEY
                ;

            //MAIN
            main.Rule = RVOID + RMAIN + LEFTPAR+RIGHTPAR+ statements
                ;


            instructionList.Rule = instructionList + instruction 
                | instruction 
                ;

            instruction.Rule = assignmentST
                | RGOTO + ID + SEMICOLON
                | label
                | declaration
                | ifST
                | printST
                | callFuncST
                | RRETURN + LEFTPAR + expression + RIGHTPAR + SEMICOLON
                | RRETURN + LEFTPAR + RIGHTPAR + SEMICOLON
                | ret
                | Empty
                ;
            ret.Rule = RRETURN + SEMICOLON
                ;
            label.Rule = ID + POINTS + instructionList
                ;
            //CONDITIONS
            ifST.Rule = RIF + expression + RGOTO + ID + SEMICOLON + RGOTO + ID + SEMICOLON + ID + POINTS
                | RIF + expression + RGOTO + ID + SEMICOLON 
                ;


            //EXPRESSIONS
            expressionList.Rule= expressionList + COMMA + expression  
                | expression
                ;

            expression.Rule = MINUS + expression
                | NOT + expression
                | expression + PLUS + expression
                | expression + MINUS + expression
                | expression + TIMES + expression
                | expression + DIVISION + expression
                | expression + MODULE + expression
                | expression + LESS + expression
                | expression + GREAT + expression
                | expression + LESS_EQ + expression
                | expression + GREAT_EQ + expression
                | expression + EQUAL + expression
                | expression + EQ_EQ + expression
                | expression + DISTINCT + expression
                | expression + OR + expression
                | expression + AND + expression
                | finalExp
                ;

            finalExp.Rule = LEFTPAR + expression + RIGHTPAR
                | callFuncST
                | access
                | INTEGER
                ;
            type.Rule = RINT | RVOID | RFLOAT
                ;

            access.Rule = ID + expListArray
                ;
            // Functions
            functionST.Rule = type + ID + LEFTPAR + argumentList + RIGHTPAR+statements
                ;
            
            callFuncST.Rule = ID + LEFTPAR + parameterList + RIGHTPAR + SEMICOLON
                | ID + LEFTPAR + parameterList + RIGHTPAR
                ;

            parameterList.Rule = parameterList + COMMA + expression
                | expression
                | Empty
                ;
            argumentList.Rule = argumentList + SEMICOLON + argument
               | argument
               | Empty
               ;
            argument.Rule = type + idList 
                ;
           
            //LOOPS

            printST.Rule = RPRINTF + LEFTPAR +STR+COMMA+  expression + RIGHTPAR + SEMICOLON
                | RPRINTF + SEMICOLON
                ;

            //ErrorRule
            main.ErrorRule = SyntaxError + RIGHTKEY;
            instruction.ErrorRule = SyntaxError + SEMICOLON
                | SyntaxError + RIGHTKEY
                ;
            #endregion

            #region Preferences
            this.Root = start;
            #endregion
        }
    }
}
