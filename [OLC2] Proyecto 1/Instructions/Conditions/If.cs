using System;
using System.Collections.Generic;
using System.Text;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;

namespace _OLC2__Proyecto_1.Instructions.Conditions
{
    class If : Instruction
    {
        private Expression condition;
        private Instruction code, elseST;
        private If elseIfST;
        
        public If(Expression condition, Instruction code, Instruction elseST = null, If elseIfST=null)
        {
            this.condition = condition;
            this.code = code;
            this.elseST = elseST;
            this.elseIfST = elseIfST;
            setLineColumn(line, column);
        }

        public override object compile(Environment_ environment)
        {
            Generator gen = Generator.getInstance();
            if (gen.getEndLbl() == "")
            {
                gen.setEndLbl(gen.newLabel());
            }

            gen.AddCom("If");

            String lbl_else = gen.newLabel();
            Return condition = this.condition.compile(environment);
            gen.addGoto(lbl_else);


            gen.addLabel(condition.value.ToString());
            this.code.compile(environment);
            gen.addGoto(gen.getEndLbl());
            
            gen.addLabel(lbl_else);

            if (elseIfST != null)
            {
                this.elseIfST.compile(environment);
            }
            if (this.elseST != null)
            {
                this.elseST.compile(environment);
            }
            if (gen.getEndLbl() != "")
            {
                gen.addLabel(gen.getEndLbl());
                gen.setEndLbl("");
            }
            return null;

        }
        public override object execute(Environment_ environment)
        {
            Return condition = this.condition.execute(environment);
            if(condition.type != Type_.BOOLEAN)
            {
                throw new Error_(this.line, this.column, "Semantico", "La condición no es booleana");
            }

            if (Boolean.Parse(condition.value.ToString()))
            {
                return this.code.execute(environment);
            }
            else
            {
                if (elseIfST != null)
                {
                    return this.elseIfST.execute(environment);
                }
                else
                {
                    return this.elseST != null ? this.elseST.execute(environment) : null;
                }
            }
        }

        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}
