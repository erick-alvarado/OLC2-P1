using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OLC2__Proyecto_1.Symbol_
{
    class Optimization
    {
        private String type, rule, code_deleted, code_add;
        private int row;

        public Optimization(string type, string rule, string code_deleted, string code_add, int row)
        {
            this.type = type;
            this.rule = rule;
            this.code_deleted = code_deleted;
            this.code_add = code_add;
            this.row = row;
        }
    }
}
