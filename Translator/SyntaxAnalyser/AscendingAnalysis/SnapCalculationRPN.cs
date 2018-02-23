using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    class SnapCalculationRPN
    {
        static private int counter = 0;

        public int Number { get; private set; }
        public string Stack { get;private set; }

        public string RPN { get; private set; }

        public SnapCalculationRPN(Stack<LexemB> stack, List<LexemB> rpn)
        {
            this.Number = ++counter;


            StringBuilder stackString = new StringBuilder();
            foreach (var item in stack)
                stackString.Append(item.Substring);

            this.Stack = stackString.ToString();

            StringBuilder rpnString = new StringBuilder();
            foreach (var item in rpn)
                rpnString.Append(item.Substring);

            this.RPN = rpn.ToString();
        }
    }
}
