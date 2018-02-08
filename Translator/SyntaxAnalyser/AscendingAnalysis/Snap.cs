using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
  public  class Snap
    {
        static private int counter = 0;
        public int Number { get; private set; }

        public string Stack { get; private set; }

        public string Base { get; private set; }

        public string Relation { get; private set; }
        public string Input { get; private set; }


        public Snap(Stack<ISymbol> stack,string relation, List<ISymbol> inputList)
        {
            Number = counter++;

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var element in stack.Reverse())
            {
                stringBuilder.Append(element);
                stringBuilder.Append(" ");
            }
            Stack = stringBuilder.ToString();

            Relation = relation;

            stringBuilder = new StringBuilder();
            foreach (var element in inputList/*.Take(10)*/)
            {
                stringBuilder.Append(element);
                stringBuilder.Append(" ");
            }
            Input = stringBuilder.ToString();
        }    

        public void SetBase(List<ISymbol> baseList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var element in baseList)
            {
                stringBuilder.Append(element);
                stringBuilder.Append(" ");
            }

            Base = stringBuilder.ToString();
        }

        public void SetRelation(string relation)
        {
            Relation = relation;
        }

    }
}
