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
        static List<Operator> OperatorPriorityList = new List<Operator>
         {
             new Operator("(",0),new Operator("[",0),
             new Operator(")",1),new Operator("]",1),
             new Operator("=",2,10),
             new Operator("or",3),
             new Operator("and",4),
             new Operator("not",5),
             //new Operator("<?>.....",6),
             new Operator("+",7),new Operator("-",7),
             new Operator("*",7),new Operator("/",7),
         };
        static Dictionary<Operator, WorkWithStack> OperatorDictionary = new Dictionary<Operator, WorkWithStack>();

        static DijkstrasAlgorithm()
        {
            OperatorDictionary.Add(new Operator("(", 0),WorkWithStack()
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


        private void WorkWithStackDefault(Operator _operator)
        {
            stack.Push(_operator);


            if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                outputList.Add(stack.Pop());
            stack.Push(_operator);
        }

    }
}
