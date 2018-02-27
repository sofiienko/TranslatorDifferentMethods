using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    /// <summary>
    /// Lexem For Expression template
    /// </summary>
    /// 
    class RPNSnap
    {
     public   string RPN { get; private set; }
     public   double Result { get; private set; }

     public RPNSnap(string rpn ,double result)
        {
            this.RPN = rpn;
            this.Result = result;
        }

        public override string ToString()
        {
            return RPN + " = " + Result;
        }
    }


    class LFET
    {
        public string Substring { get; set; }

        public override string ToString()
        {
            return Substring;
        }

        public LFET(string substring)
        {
            this.Substring = substring;
        }

        public static LFET[] LexemBConstructor(params string[] lexemsText)
        {
            LFET[] mass = new LFET[lexemsText.Length];

            for (int i = 0; i < lexemsText.Length; i++)
                mass[i] = new LFET(lexemsText[i]);

            return mass;
        }
    }

    class Operator:IOperator
    {
        // uint Priority;
        //uint AmountOperands
       public string Sign { get; private set; }
       public Operator(string sign)
        {
            this.Sign = sign;
        }

        //public static bool operator ==(Operator o1,Operator o2)
        //{
        //    if (o1.Sign == o2.Sign) return true;
        //    else return false;
        //}
        public override bool Equals(object obj)
        {
           if(obj is Operator temp)return this.Sign == temp.Sign;
            return false;
        }

        public override string ToString()
        {
            return Sign;
        }

        //public static bool operator !=(Operator o1, Operator o2)
        //{
        //    if (o1.Sign == o2.Sign) return false;
        //    else return true;
        //}
    }


    /// <summary>
    /// Reverse Polish Notation
    /// ПОЛІЗ:Польський Інвертний Запис
    /// </summary>
    class RPN
    {
        static Dictionary<LFET[], Operator> RPNdictionary { get; set; } = new Dictionary<LFET[], Operator>();
        static public List<RPNSnap> AllRPN{ get; private set; } = new List<RPNSnap>();

        /// <summary>
        ///  Here should write rules for RPN
        /// </summary>
        static RPN()
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

        public List<IOperator> Current { get; private set; } = new List<IOperator>();
        

        public void AddLexemsToCurrentRPN(Model.ISymbol[] inputLexemList)
        {
            Operator typeAction = GetActionValueByKey(inputLexemList);
            if (typeAction == null) return; 


            else if ((typeAction.Sign == "assign")&&(inputLexemList[0] is Model.Identifier idnt))
            {
                double calcultaionResult = Calculation(Current);
                idnt.Value = calcultaionResult;
                AllRPN.Add(new  RPNSnap( CurrentRPNtoString(),calcultaionResult));
                Current = new List<IOperator>();
            }

            else if ((typeAction.Sign == "declare")&&(inputLexemList[1] is Model.Identifier idn))
            {
                double calcultaionResult= Calculation(Current);
                idn.Value = calcultaionResult;
                AllRPN.Add(new RPNSnap( CurrentRPNtoString(), Calculation(Current)));
                Current = new List<IOperator>();
            }
            else if (typeAction.Sign == "end")
            {
                //AllRPN.Add(CurrentRPNtoString(), calcultaionResult);
                Current = new List<IOperator>();
            }
            else if (typeAction.Sign=="const" || typeAction.Sign =="idn") Current.Add((IOperator)inputLexemList[0]);
            else Current.Add(typeAction);
        }

         Operator GetActionValueByKey(Model.ISymbol[] mass)
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

        private double Calculation(List<IOperator> list)
        {
            
            Stack<double> stack = new Stack<double>();
            
            for(int i=0;i<list.Count; i++)
            {
                if (list[i] is Model.Identifier id) stack.Push(id.Value.Value);
                if (list[i] is Model.Constant c) stack.Push(c.Value);
                if (list[i] is Operator oprator)
                {
                    double operant2 = stack.Pop();
                    double operant1 = stack.Pop();
                    double resultOperation=0;

                    if (oprator.Sign == "*") resultOperation = Multiple(operant1, operant2);
                    else if (oprator.Sign == "/") resultOperation = Devide(operant1, operant2);
                    else if (oprator.Sign == "+") resultOperation = Plus(operant1, operant2);
                    else if (oprator.Sign == "-") resultOperation = Minus(operant1, operant2);

                    stack.Push(resultOperation);
                }
            }
            if (stack.Count != 1) throw new Exception("Oops, problem with calcultation");
            else return stack.Pop();
        }

         public string CurrentRPNtoString()
         {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Current)
            {
                if (item is Operator _operator) sb.Append(_operator.Sign);
                else if (item is Model.Identifier idn) sb.Append(idn.Name);
                else if (item is Model.Constant cnst) sb.Append(cnst.Value.ToString());

                else throw new Exception("Problem with converting IOperator to string");
                    //if (lexem.IndexConst != null) sb.Append(Const.AllConstFromCode.Find(c => c.Index == lexem.IndexConst)._Const.ToString());
                    //if (lexem.IndexIdnt!=null) sb.Append(Idnt.AllIdnFromCode.Find(c => c.Index == lexem.IndexIdnt).Name.ToString());

                sb.Append(" ");
            }
            return sb.ToString();
         }


        public double Multiple(double operant1,double operant2)
        {
            return operant1 * operant2;
        }

        public double Devide(double operant1, double operant2)
        {
            try
            {
                return operant1 / operant2;
            }
            catch(DivideByZeroException)
            {
                Console.WriteLine("Dividing by zero");
                throw new Exception("Dividing by zero");
            }
        }

        public double Plus(double operant1, double operant2)
        {
            return operant1 + operant2;
        }

        public double Minus(double operant1, double operant2)
        {
            return operant1 - operant2;
        }
    }
}
