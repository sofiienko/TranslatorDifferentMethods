using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.StackedAutomatic
{
    class Snap
    {
        static int amount;
        public int Number { get; private set; }
        public string InputLexem { get; private set; }
        public int NumberState { get; private set; }
        public string StackString { get; private set; }

        State[] stack;

        static Snap()
        {
            amount = 0;
        }

        public Snap(string inputLexem, int numberState, List<State> statesFromStack)
        {
            this.Number = ++amount;
            this.InputLexem = inputLexem;
            this.NumberState = numberState;

            StringBuilder tempString = new StringBuilder();
            
            if (statesFromStack.Count==0) return;
            tempString.Append(statesFromStack[0].Number);

            for (int i = 1; i < statesFromStack.Count; i++)
                tempString.Append(" " + statesFromStack[i].Number);

            if(tempString!=null)this.StackString = tempString.ToString();


        }
    }
}
