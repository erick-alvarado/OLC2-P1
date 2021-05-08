using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Symbol_;
using Compilador.Generator;
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
            Generator gen = Generator.getInstance();
            String temp = "";
            switch (this.type)
            {
                case Type_.STRING:
                    temp = gen.newTemp();
                    gen.AddExp(temp,"HP");

                    foreach (byte b in System.Text.Encoding.UTF8.GetBytes(this.value.ToString().ToCharArray()))
                    {
                        gen.AddHeap(b);
                    }
                    gen.AddHeap(-1);
                    return new Return(temp, Type_.HEAP,Type_.STRING);
                case Type_.INTEGER:
                    temp = gen.newTemp();
                    gen.AddExp(temp,this.value.ToString());
                    return new Return(temp, Type_.STACK,Type_.INTEGER);
                case Type_.REAL:
                    temp = gen.newTemp();
                    gen.AddExp(temp, this.value.ToString());
                    return new Return(temp, Type_.STACK,Type_.REAL);
                case Type_.BOOLEAN:
                    temp = gen.newTemp();

                    if ((Boolean) this.value)
                    {
                        gen.AddExp(temp, "1");
                    }
                    else
                    {
                        gen.AddExp(temp, "0");
                    }

                    return new Return(temp, Type_.STACK,Type_.BOOLEAN);
                default:
                    return new Return(value, Type_.HEAP);
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
