using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    /// <summary>
    /// Reverse Polish Notation
    /// ПОЛІЗ:Польський Інвертний Запис
    /// </summary>
    class RPN
    {
        static Dictionary<ISymbol[], string> RPNdictionary { get; set; } = new Dictionary<ISymbol[], string>();

        //workaround for indetifiсation id and constant
        static string cnst = ")C(";
        static string idn = ")I(";
        static string end = ")E(";


        static RPN()
        {
            RPNdictionary.Add(LexemB.LexemBConstructor("id"), idn);
            RPNdictionary.Add(LexemB.LexemBConstructor("const"), cnst);
            RPNdictionary.Add(LexemB.LexemBConstructor("<term>", "*", "<multiple>"), "*");
            RPNdictionary.Add(LexemB.LexemBConstructor("<term>", "/", "<multiple>"), "/");
            RPNdictionary.Add(LexemB.LexemBConstructor("<expression>", "+", "<term1>"), "+");
            RPNdictionary.Add(LexemB.LexemBConstructor("<expression>", "-", "<term1>"), "-");
            RPNdictionary.Add(LexemB.LexemBConstructor("<expression>"), end);
        }

        public  List<string> Current { get; private set; } = new List<string>();
        static public List<string[]> AllRPN { get; private set; } = new List<string[]>();

        public void Add(ISymbol[] lexemList)
        {
            try
            {
               string temp = RPNdictionary[lexemList];
                if (temp == "idn")
                {
                    
                }

            }
            catch (KeyNotFoundException)
            {
                return;
            }
            
        }




        
    }
}
