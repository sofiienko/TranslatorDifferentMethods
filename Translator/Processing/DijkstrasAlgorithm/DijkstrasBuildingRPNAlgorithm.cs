using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;

namespace Translator.Processing
{
    class DijkstrasAlgorithm //: RPN
    {
        List<Operator> OperatorPriorityList = new List<Operator>
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

        Stack<IRPNElement> stack = new Stack<IRPNElement>();
        List<IRPNElement> outputList = new List<IRPNElement>();

        public DijkstrasAlgorithm(List<Model.Lexem> lexemList)
        {
            foreach(var item in lexemList)
            {
                if (item is Constant || item is Link) outputList.Add(item);
                //else 
            }
        }


    }
}
