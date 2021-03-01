using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
namespace _OLC2__Proyecto_1.Instructions.Variables
{
    class Declaration : Instruction
    {
        private List<String> id= new List<String>();
        private Type_ type;
        private Expression value;

        public Declaration(List<String> id, Type_ type, int line, int column)
        {
            this.id = id;
            this.type = type;

            setLineColumn(line, column);
        }
        public Declaration(String id, Type_ type, Expression value, int line, int column)
        {
            this.id.Add(id);
            this.type = type;
            this.value = value;

            setLineColumn(line, column);
        }
        public override object execute(Environment_ environment)
        {
            //TODO ESTA SHIT HAY QUE CAMBIARLA
            Return val = this.value != null ? this.value.execute(environment) : new Return(null, type);
            environment.saveVar(id, val.value, type);
            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
