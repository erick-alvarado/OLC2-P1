﻿using _OLC2__Proyecto_1.Abstract;
using _OLC2__Proyecto_1.Expressions;
using _OLC2__Proyecto_1.Symbol_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Instructions.Functions
{
    class Function : Instruction
    {
        private String id;
        public LinkedList<Expression> parameterList = new LinkedList<Expression>();//Lo que trae de la llamada
        public LinkedList<Argument> argumentList = new LinkedList<Argument>();//Lo que tiene originalmente
        private LinkedList<Instruction> declarationList = new LinkedList<Instruction>();
        private Statement statements;
        private Type_ return_;
        private bool exec = true;
        public Environment_ environmentAux;
        public Function(int line, int column,string id, LinkedList<Argument> argumentList, LinkedList<Instruction> declarationList, Statement statements, Type_ return_)
        {
            this.id = id;
            this.argumentList = argumentList;
            this.declarationList = declarationList;
            this.statements = statements;
            this.return_ = return_;
            setLineColumn(line, column);
        }
        public override object execute(Environment_ environment)
        {
            if (this.exec)
            {
                this.exec = false;
                this.environmentAux = new Environment_(environment, this.id);
            }
            else
            {
                
                foreach (Instruction i in this.declarationList)
                {
                    i.execute(this.environmentAux);
                }
                

            }

            return null;
        }
        public override void setLineColumn(int line, int column)
        {
            this.line = line; this.column = column;
        }
    }
}