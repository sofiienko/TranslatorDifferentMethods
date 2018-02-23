using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
    class Constant : Lexem
    {
        public uint NumberInConstantList { get; private set; }

        //todo:change terminal code to real code
        public new const TerminalCode TerminalCode = 0;

        //todo:change terminal code to real type
        public TerminalCode Type { get; private set; }

        public double  Value { get; set; }
    }
}
