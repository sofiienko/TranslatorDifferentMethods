using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;

namespace Translator.Processing
{

    public delegate void WorkWithStack(Operator key = null);
    public class DijkstrasAlgorithm //: RPN
    {
       OperatorRepository operatorRepo = new OperatorRepository();


        //static List<Operator> OperatorPriorityList = new List<Operator>
        // {
        //     new Operator("(",0),new Operator("[",0),
        //     new Operator(")",1),new Operator("]",1),
        //     new Operator("=",2,10),
        //     new Operator("or",3),
        //     new Operator("and",4),
        //     new Operator("not",5),
        //     //new Operator("<?>.....",6),
        //     new Operator("+",7),new Operator("-",7),
        //     new Operator("*",7),new Operator("/",7),
        // };
        Dictionary<Operator, WorkWithStack> OperatorDictionary = new Dictionary<Operator, WorkWithStack>();

        public DijkstrasAlgorithm()
        {
            OperatorDictionary.Add(operatorRepo["("], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["["], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo["+"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["-"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["*"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["/"], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo["or"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["and"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["not"], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo["<"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo[">"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["<="], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo[">="], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["=="], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["!="], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo[")"], ClosingBracket);
            OperatorDictionary.Add(operatorRepo["]"], ClosingBracket);

            OperatorDictionary.Add(operatorRepo["do"], DoNothing);
            OperatorDictionary.Add(operatorRepo["while"], OperatorWhile);
            OperatorDictionary.Add(operatorRepo["enddo"], OperatorEndDo);

            OperatorDictionary.Add(operatorRepo["if"], OperatorIf);
            OperatorDictionary.Add(operatorRepo["then"], OperatorThen);
            OperatorDictionary.Add(operatorRepo["fi"], OperatorFi);

        }



        Stack<Operator> stack = new Stack<Operator>();
        List<IRPNElement> outputList = new List<IRPNElement>();

        public DijkstrasAlgorithm(List<Model.Lexem> lexemList)
        {
            foreach(var item in lexemList)
            {
                if (item is Constant || item is Link) outputList.Add(item);
                //else 
            }
        }


        private  void WorkWithStackDefault(Operator _operator)
        {
            if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                outputList.Add(stack.Pop());
            stack.Push(_operator);
        }
        private void ClosingBracket(Operator _operator)
        {
            //todo: I am not sure  here:(
            if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                outputList.Add(stack.Pop());
        }

        private void DoNothing(Operator _operator) { }

        private void OperatorIf(Operator _operator) { }
        private void OperatorThen(Operator _operator) { }
        private void OperatorFi(Operator _operator) { }


        private void OperatorWhile(Operator _operator)
        {



        }
        private void OperatorEndDo(Operator _operator) { }
    }

    /// <summary>
    /// conditional transition by mistake
    /// </summary>
    public class CTM: IRPNElement
    {
        public override string ToString()
        {
            return "CTM";
        }
    }
    /// <summary>
    /// unconditional transition
    /// </summary>
    public class UT : IRPNElement
    {
        public override string ToString()
        {
            return "UT";
        }
        
    }

}
