using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.LexicalAnalyser.FiniteStateMachine
{
    delegate Lexem Action(ref int row, string buffer = null);
    delegate int CheckChar(char c);

    struct Pair
    {
        public Symbol key;
        public State value;
        public Pair(Symbol key, State value)
        {
            this.key = key;
            this.value = value;
        }
    }

    class State
    {
        public int Number { get; private set; }
        Dictionary<Symbol, State> map;
        Action equal;


        public State(int number, Action equal, params Pair[] dictionary)
        {
            map = new Dictionary<Symbol, State>();
            this.Number = number;
            this.equal = equal;


            foreach (var item in dictionary)
                this.map.Add(item.key, item.value);
        }


        public State(int number, Action equal)
        {
            map = new Dictionary<Symbol, State>();
            this.Number = number;
            this.equal = equal;
        }

        public void AddReference(params Pair[] dictionary)
        {
            foreach (var item in dictionary)
                this.map.Add(item.key, item.value);
        }

        public static void Execute(State st, ref int row, string line, string buffer, ref int pointer)
        {
            State current = st;
            State next;

            while (pointer < line.Length)
            {
                char c = line[pointer];

                while (current.Number == 1 && line[pointer] == ' ' && pointer < line.Length)
                    pointer++;

                Symbol s = Check.SymbolType(line[pointer]);

                //myLittleCrutch
                if (current.Number == 5)
                {
                    Symbol p = Check.SymbolType(line[pointer - 2]);
                    char c1 = line[pointer - 2];
                    if (p == Symbol.digit || line[pointer - 2] == ')' || p == Symbol.letter)
                    { current.equal(ref row, "-"); return; }
                }


                if (current.map.TryGetValue(s, out next))
                {
                    buffer += line[pointer];


                    if (next == null) { pointer++; break; }
                    pointer++;
                    current = next;
                    continue;
                }
                else if (current.equal != null)
                {
                    if (buffer == null) { buffer += line[pointer]; pointer++; }
                    current.equal(ref row, buffer);
                    // pointer++;
                    return;
                }
                else
                {
                    if (buffer == null) buffer += line[pointer];
                    // pointer++;
                    return;
                }

            }
            current.equal?.Invoke(ref row, buffer);
        }

    }
}
