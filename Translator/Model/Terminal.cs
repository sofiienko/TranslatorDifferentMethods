using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{

    
    class Terminal : ISymbol
    {
        public string Substring { get; set; }
        public TerminalCode TerminalCode { get; set; }
    }
}
