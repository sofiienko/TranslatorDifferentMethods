using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
  public  class Identifier:Lexem
    {

        public uint NumberInIdentifierList { get; private set; }

        public string Name { get; private set; }

        public TerminalCode Type { get; private set; }

        public double? Value { get; set; }

        public Identifier(string name,double? Value, TerminalCode type, uint number, uint numberInIdentifierList, uint row) :
            base(number, row, Value.ToString(), TerminalCode.Identifier)
        {

            if (!(type == TerminalCode.Float ||
                  type == TerminalCode.Int ||
                  type == (TerminalCode.Unsigned & TerminalCode.Float) ||
                  type == (TerminalCode.Unsigned & TerminalCode.Int)
                  )) throw new Exception("Invalid type. Type must be TerminalCode.Int, TerminalCode.Float,TerminalCode.Unsigned& TerminalCode.Int,TerminalCode.Unsigned & TerminalCode.Float");

            this.NumberInIdentifierList = numberInIdentifierList;
            this.Name = name;
            this.Type = type;
            this.Value = Value;
        }
    }
}
