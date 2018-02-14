using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    class LexemB:ISymbol
    {
        public string Substring { get; set; }

        public override string ToString()
        {
            return Substring;
        }

        public LexemB(string substring)
        {
            this.Substring = substring;
        }

        public static ISymbol[] LexemBConstructor(params string[] lexemsText)
        {
            ISymbol[] mass = new ISymbol[lexemsText.Length];

            for (int i = 0; i < lexemsText.Length; i++)
                mass[i] = new LexemB(lexemsText[i]);

            return mass;
        }
    }
}
