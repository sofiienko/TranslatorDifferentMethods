using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
  public  class NonTerminal:ISymbol
    {
        public string Substring { get; set; }
        public override string ToString()
        {
            return this.Substring;
        }
    }
}
