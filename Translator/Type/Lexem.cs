using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
   public class Lexem:LexemB,IEqualityComparer<LexemB>
    {
        public int Row { get; }
        //public string Substring { get; }
        public int Code { get; }

        /// <summary>
        /// Index in indentifiуr table
        /// </summary>
        public int? IndexIdnt { get; }

        /// <summary>
        /// Index in constant table
        /// </summary>
        public int? IndexConst { get; }

        public Lexem(int row, string substring, int code)
        {
            this.Row = row;
            this.Substring = substring;
            this.Code = code;
        }

        public Lexem(int row, string substring, int code, int? indexIdnt = null, int? indexConst = null)
        {
            this.Row = row;
            this.Substring = substring;
            this.Code = code;
            if (indexIdnt != null && indexConst == null) this.IndexIdnt = indexIdnt;
            else if (indexIdnt == null && indexConst != null) this.IndexConst = indexConst;
            else throw new Exception("You can`t add parametr indexIdnt and indexConst in sametime");
        }

        public override string ToString()
        {
            return Substring;
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
    }
}
