using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
   public class LexemB: IEqualityComparer<LexemB>
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
        public LexemB()
        { }
        public static LexemB[] LexemBConstructor(params string[] lexemsText)
        {
            LexemB[] mass = new LexemB[lexemsText.Length];

            for (int i = 0; i < lexemsText.Length; i++)
                mass[i] = new LexemB(lexemsText[i]);

            return mass;
        }

        public bool Equals(LexemB obj1, LexemB obj2)
        {
            if (obj1.Substring == obj2.Substring) return true;
            else return false;
        }

        public int GetHashCode(LexemB lexemB)
        {
            return lexemB.Substring.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (this.Substring == ((LexemB)obj).Substring) return true;
            else return false;
        }

        public override int GetHashCode()
        {
            return this.Substring.GetHashCode();
        }
    }
}
