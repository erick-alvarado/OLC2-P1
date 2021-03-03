using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Symbol_;
namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class DeclarationType : Instruction
    {
        private Type_ type= Type_.DEFAULT;
        private Type_ type2= Type_.DEFAULT;
        private LinkedList<Expression> expression;
        private LinkedList<Expression> expression2;
        private LinkedList<Access> idList = new LinkedList<Access>();
        private LinkedList<Instruction> declarationList = new LinkedList<Instruction>();

        //Declaration Type 
        public DeclarationType(LinkedList<Access> idList, Type_ type, int line, int column)
        {
            this.idList = idList;
            this.type = type;
            setLineColumn(line, column);
        }


        //Declaration ARRAY
        public DeclarationType(LinkedList<Access> idList, Type_ type, Type_ type2, LinkedList<Expression> expression, LinkedList<Expression> expression2, int line, int column)
        {
            this.type = type;
            this.type2 = type2;
            this.idList = idList;
            this.expression = expression;
            this.expression2 = expression2;
            setLineColumn(line, column);
        }
        //Declaration OBJECT
        public DeclarationType(LinkedList<Access> idList, LinkedList<Instruction> declarationList, int line, int column)
        {
            this.idList = idList;
            this.declarationList = declarationList;
            setLineColumn(line, column);
        }
        public override object execute(Environment_ environment)
        {
            //TODO ESTA SHIT HAY QUE CAMBIARLA
            //Return val = this.value != null ? this.value.execute(environment) : new Return(null, type);
            //environment.saveVar(id, val.value, type);
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
