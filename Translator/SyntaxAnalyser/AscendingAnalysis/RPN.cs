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

        public List<LexemB> Current { get; private set; } = new List<LexemB>();
        static public Dictionary<LexemB[],double> AllRPN { get; private set; } = new Dictionary<LexemB[],double>();

        public void AddLexemToCurrentRPN(LexemB[] lexemList)
        {
            string typeAction = GetValueByKey(lexemList);
            if (typeAction == null) return;
            else if (typeAction == end)
            {
               // AllRPN.Add(Current.ToArray());
                Current = new List<LexemB>();
            }
            else if (typeAction == cnst || typeAction == idn) Current.Add(lexemList[0]);
            else Current.Add(new LexemB { Substring = typeAction });
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



        private double Calculation(List<LexemB> list)
        {
            
            double result=0;
            Stack<LexemB> stack = new Stack<LexemB>();
            
            for(int i=0;i<list.Count; i++)
            {
                
            }
            


            return result;
        }



         public string CurrentRPNtoString()
         {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Current)
            {
                if (item is Lexem lexem)
                {
                    if (lexem.IndexConst != null) sb.Append(Const.AllConstFromCode.Find(c => c.Index == lexem.IndexConst)._Const.ToString());
                    if (lexem.IndexIdnt!=null) sb.Append(Idnt.AllIdnFromCode.Find(c => c.Index == lexem.IndexIdnt).Name.ToString());
                }                                    
                else sb.Append(item);

                sb.Append(" ");
            }
            return sb.ToString();
         }

        
    }
}
