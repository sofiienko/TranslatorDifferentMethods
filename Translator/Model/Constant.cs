using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
 public   class Constant : Lexem
    {

        public uint NumberInConstantList { get; private set; }

        public Constant(double Value, TerminalCode type, uint number, uint numberInConstList, uint row) :
            base(number, row, Value.ToString(), TerminalCode.Constant)
        {

            if (!(type == TerminalCode.Float ||
                  type == TerminalCode.Int ||
                  type == (TerminalCode.Unsigned & TerminalCode.Float) ||
                  type == (TerminalCode.Unsigned & TerminalCode.Int)
                  )) throw new Exception("Invalid type. Type must be TerminalCode.Int, TerminalCode.Float,TerminalCode.Unsigned& TerminalCode.Int,TerminalCode.Unsigned & TerminalCode.Float");

            this.NumberInConstantList = NumberInConstantList;
            this.Type = type;
            this.Value = Value;
        }
        public TerminalCode Type { get; private set; }

        public double  Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
