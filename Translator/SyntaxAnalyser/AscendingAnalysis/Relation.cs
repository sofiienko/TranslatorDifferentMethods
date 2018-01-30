using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    class Relation
    {
        public string S { get; set; }
        public string R { get; set; }
        public Relation(string S, string R)
        {
            this.S = S;
            this.R = R;
        }
    }
}
