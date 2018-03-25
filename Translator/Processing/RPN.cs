using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;

namespace Translator.Processing
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

    /// <summary>
    /// Element for BuildRPNForAscending Analys
    /// </summary>
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




    /// <summary>
    /// Reverse Polish Notation
    /// ПОЛІЗ:Польський Інвертний Запис
    /// </summary>
    class RPN
    {
       
        static public List<RPNSnap> AllRPN{ get; protected set; } = new List<RPNSnap>();



        public List<IRPNElement> Current { get; protected set; } = new List<IRPNElement>();

        protected double Calculation(List<IRPNElement> list)
        {
            
            Stack<double> stack = new Stack<double>();
            
            for(int i=0;i<list.Count; i++)
            {
                if (list[i] is Model.Link id) stack.Push(id.Value.Value);
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
                else if (item is Model.Link idn) sb.Append(idn.Name);
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
