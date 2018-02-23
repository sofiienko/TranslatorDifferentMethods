using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
    class Identifier:Lexem
    {

        public uint NumberInIdentifierList { get; private set; }

        //todo:change terminal code to real code
        public new const TerminalCode TerminalCode = 0;
        public string Name { get; private set; }

        //todo:change terminal code to real type
        public TerminalCode Type { get; private set; }

        public double? Value { get; set; }
    }
}
