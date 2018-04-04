using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
    public interface IRPNElement
    {
    }


    public class Lexem : Terminal, IRPNElement
    {
        public uint Number { get; set; }
        public uint Row { get; set; }

        public Lexem(uint number, uint row, string substring, TerminalCode code)
            : base(substring, code)
        {
            this.Number = number;
            this.Row = row;
        }

        public override string ToString()
        {
            return this.Substring+" ";
        }

    }
}
