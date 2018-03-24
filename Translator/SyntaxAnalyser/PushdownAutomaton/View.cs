using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.StackedAutomatic
{
    class View
    {
        private static int counter;

        public int Number
        {
            private set;
            get;
        }

        public string LexemString
        {
            get;set;
        }

        public int State
        {
            get;set;
        }

        public string Stack
        {
            get;set;
        }


        static View()
        {
            counter = 0;
        }

        public View()
        {
            Number = counter;
        }
        public View(string lexemString,int state,int[] stack)
        {
            Number = counter;
            LexemString = lexemString;
            State = state;

            StringBuilder stackToString  =new StringBuilder();
            for (int i = 0; i < stack.Length; i++)
                stackToString.Append(" "+ stack[i].ToString());

            Stack = stackToString.ToString();
        }

    }
}
