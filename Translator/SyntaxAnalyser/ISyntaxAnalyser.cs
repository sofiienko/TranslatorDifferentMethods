using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser
{
    interface ISyntaxAnalyser
    {
       // bool CheckSyntax(List<Lexem> lexemList);
        bool CheckSyntax(List<Model.Lexem> lexemList);
    }
}
