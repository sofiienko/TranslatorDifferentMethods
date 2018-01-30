using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.LexicalAnalyser
{
    interface ILexicalAnalyser
    {
        List<Lexem> LexemList { get; }
        List<Const> ConstantList { get; }
        List<Idnt> IdentifierList { get; }
        List<string> SourceCode { get; set; }

        bool Analize();

    }
}
