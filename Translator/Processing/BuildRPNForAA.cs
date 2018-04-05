using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;
using Translator.SyntaxAnalyser.AscendingAnalysis;

namespace Translator.Processing
{
    /// <summary>
    /// Build RPN for Acsending Analys
    /// </summary>
    class BuildRPNForAA:RPN
    {
        /// <summary>
        ///  Here should write rules for RPN
        /// </summary>
        static Dictionary<LFET[], Operator> RPNdictionary { get; set; } = new Dictionary<LFET[], Operator>();
        static BuildRPNForAA()
        {
            RPNdictionary.Add(LFET.LexemBConstructor("id"), new Operator("idn"));
            RPNdictionary.Add(LFET.LexemBConstructor("const"), new Operator("const"));
            RPNdictionary.Add(LFET.LexemBConstructor("<term>", "*", "<multiple>"), new Operator("*"));
            RPNdictionary.Add(LFET.LexemBConstructor("<term>", "/", "<multiple>"), new Operator("/"));
            RPNdictionary.Add(LFET.LexemBConstructor("<expression>", "+", "<term1>"), new Operator("+"));
            RPNdictionary.Add(LFET.LexemBConstructor("<expression>", "-", "<term1>"), new Operator("-"));
            RPNdictionary.Add(LFET.LexemBConstructor("id", "=", "<expression1>"), new Operator(/*"end"*/"assign"));
            RPNdictionary.Add(LFET.LexemBConstructor("<type>", "id", "=", "<expression1>"), new Operator(/*"end"*/"declare"));
        }

        public void AddLexemsToCurrentRPN(Model.ISymbol[] inputLexemList)
        {
            Operator typeAction = GetOperatorByKey(inputLexemList);
            if (typeAction == null) return;


            else if ((typeAction.Sign == "assign") && (inputLexemList[0] is Model.Link idnt))
            {
                double calcultaionResult = Calculation(Current);
                idnt.Value = calcultaionResult;
                AllRPN.Add(new RPNSnap(CurrentRPNtoString(), calcultaionResult));
                Current = new List<IRPNElement>();
            }

            else if ((typeAction.Sign == "declare") && (inputLexemList[1] is Model.Link idn))
            {
                double calcultaionResult = Calculation(Current);
                idn.Value = calcultaionResult;
                AllRPN.Add(new RPNSnap(CurrentRPNtoString(), Calculation(Current)));
                Current = new List<IRPNElement>();
            }
            else if (typeAction.Sign == "end")
            {
                //AllRPN.Add(CurrentRPNtoString(), calcultaionResult);
                Current = new List<IRPNElement>();
            }
            else if (typeAction.Sign == "const" || typeAction.Sign == "idn") Current.Add((IRPNElement)inputLexemList[0]);
            else Current.Add(typeAction);
        }

        Operator GetOperatorByKey(Model.ISymbol[] mass)
        {
            foreach (var item in RPNdictionary)
            {
                if (mass.Length == item.Key.Length)
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

    }
}
