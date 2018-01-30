using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.LexicalAnalyser
{
    class LexicalUnit
    {
        public string Substring { get; set; }
        public int Row { get; set; }

        public LexicalUnit(string substring, int row)
        {
            this.Substring = substring;
            this.Row = row;

        }
    }
}
