using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.LexicalAnalyser;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    /// <summary>
    /// Reverse Polish Notation
    /// ПОЛІЗ:Польський Інвертний Запис
    /// </summary>
    class RPN
    {
        static Dictionary<LexemB[], string> RPNdictionary { get; set; } = new Dictionary<LexemB[], string>();

        //workaround for indetifiсation id and constant
        static string cnst = ")C(";
        static string idn = ")I(";
        static string end = ")E(";



        /// <summary>
        ///  Here should write rules for RPN
        /// </summary>
        static RPN()
        {
            RPNdictionary.Add(LexemB.LexemBConstructor("id"), idn);
            RPNdictionary.Add(LexemB.LexemBConstructor("const"), cnst);
            RPNdictionary.Add(LexemB.LexemBConstructor("<term>", "*", "<multiple>"), "*");
            RPNdictionary.Add(LexemB.LexemBConstructor("<term>", "/", "<multiple>"), "/");
            RPNdictionary.Add(LexemB.LexemBConstructor("<expression>", "+", "<term1>"), "+");
            RPNdictionary.Add(LexemB.LexemBConstructor("<expression>", "-", "<term1>"), "-");
            RPNdictionary.Add(LexemB.LexemBConstructor("id", "=", "<expression1>"), end);
            RPNdictionary.Add(LexemB.LexemBConstructor("<type>", "id", "=", "<expression1>"), end);
        }

        public List<string> Current { get; private set; } = new List<string>();
        static public List<string[]> AllRPN { get; private set; } = new List<string[]>();

        public void AddLexemToCurrentRPN(LexemB[] lexemList)
        {
            string typeAction = GetValueByKey(lexemList);
            if (typeAction == null) return;


            if(typeAction ==cnst &&  lexemList[0] is Lexem constant)
                Current.Add(Const.AllConstFromCode.Find(c => c.Index == constant.IndexConst)._Const.ToString());

            else  if (typeAction == idn && lexemList[0] is Lexem identifier)
                Current.Add(Idnt.AllIdnFromCode.Find(c => c.Index == identifier.IndexIdnt).Name.ToString());

            else if (typeAction == end)
            {
                AllRPN.Add(Current.ToArray());
                Current = new List<string>();
            }
            else Current.Add(typeAction);
        }

         string GetValueByKey(LexemB[] mass)
        {
            foreach(var item in RPNdictionary)
            {
                if(mass.Length==item.Key.Length)
                {
                    bool flag = true;
                    for (int i = 0; i < mass.Length; i++)
                        if (mass[i].Substring != item.Key[i].Substring)
                        {
                            flag = false;
                            break;
                        }
                    if (flag) return item.Value;
                }
            }
            return null;
        }



         public string CurrentRPNtoString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Current)
            {
                sb.Append(item);
                sb.Append(" ");
            }
            return sb.ToString();
        }

        
    }
}
