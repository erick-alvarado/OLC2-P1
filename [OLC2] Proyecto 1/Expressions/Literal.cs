using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Text;

namespace _OLC2__Proyecto_1.Expressions
{
    public class Literal : Expression
    {
        private Type_ type;
        private object value;

        public Literal(object value, Type_ type, int line, int column)
        {
            this.value = value;
            this.type = type;
            this.setLineColumn(line, column);
        }
        public override Return compile(Environment_ environment)
        {
            switch (this.type)
            {
                case Type_.STRING:
                    return new Return(this.value.ToString(), this.type);
                case Type_.INTEGER:
                    return new Return(int.Parse(this.value.ToString()), this.type);
                case Type_.REAL:
                    return new Return(Double.Parse(this.value.ToString()), this.type);
                case Type_.BOOLEAN:
                    return new Return(Boolean.Parse(this.value.ToString()), this.type);
                default:
                    return new Return(value, this.type);
            }
        }
        public override Return execute(Environment_ environment)
        {
            switch (this.type)
            {
                case Type_.STRING:
                    return new Return(this.value.ToString(), this.type);
                case Type_.INTEGER:
                    return new Return(int.Parse(this.value.ToString()), this.type);
                case Type_.REAL:
                    return new Return(Double.Parse(this.value.ToString()), this.type);
                case Type_.BOOLEAN:
                    return new Return(Boolean.Parse(this.value.ToString()), this.type);
                default:
                    return new Return(value,this.type);
            }
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }
}
