using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.RecursiveDescentParser
{
  public  class SyntaxException : Exception
    {
        string text;
        public SyntaxException(string text) : base(text)
        { }

        public void TextAdd(string text)
        {
            this.text = text;
        }
    }
}
