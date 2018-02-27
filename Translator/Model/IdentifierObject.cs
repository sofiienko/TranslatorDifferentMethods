using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
 public   class IdentifierObject
    {
        public TerminalCode Type { get; private set; }
        public double? Value { get; set; }
        public string Name { get; private set; }

        public uint Number { get; private set; }
        public IdentifierObject(string name, double? Value, TerminalCode type, uint number)
        {

            if (!(type == TerminalCode.Float ||
                  type == TerminalCode.Int ||
                  type == (TerminalCode.Unsigned & TerminalCode.Float) ||
                  type == (TerminalCode.Unsigned & TerminalCode.Int)
                  )) throw new Exception("Invalid type. Type must be TerminalCode.Int, TerminalCode.Float,TerminalCode.Unsigned& TerminalCode.Int,TerminalCode.Unsigned & TerminalCode.Float");

            this.Name = name;
            this.Type = type;
            this.Value = Value;
            this.Number = number;
        }

    }
}
