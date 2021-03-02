using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class DeclarationType : Instruction
    {
        private Type_ type;
        private Type_ type2;
        private LinkedList<Expression> idList = new LinkedList<Expression>();
        private LinkedList<Instruction> declarationList = new LinkedList<Instruction>();

        //Declaration Type 
        public DeclarationType(LinkedList<Expression> idList, Type_ type)
        {
            this.idList = idList;
            this.type = type;
        }

        
        //Declaration Object
        public DeclarationType(LinkedList<Expression> idList, Type_ type, Type_ type2)
        {
            this.idList = idList;
            this.type = type;
            this.type2 = type2;
        }
        //Declaration array
        public DeclarationType(LinkedList<Expression> idList, LinkedList<Instruction> declarationList)
        {
            this.idList = idList;
            this.declarationList = declarationList;
        }
        public override object execute(Environment_ environment)
        {
            //TODO ESTA SHIT HAY QUE CAMBIARLA
            Return val = this.value != null ? this.value.execute(environment) : new Return(null, type);
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
