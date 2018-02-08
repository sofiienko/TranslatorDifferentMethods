using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
 public   class RightPart
    {
        public List<string[]> Paralel = new List<string[]>();

        public RightPart(string rightPart)
        {
            char[] splitter = { '|' };
            char[] splitter2 = { ' ' };
            string[] paralel = rightPart.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string p in paralel)
                Paralel.Add(p.Split(splitter2, StringSplitOptions.RemoveEmptyEntries));
        }

        public bool ContainsSequence(List<ISymbol> inputString)
        {

            foreach (string[] sequence in Paralel)
            {
                if (sequence.Length == inputString.Count)
                {
                    bool flag = true;
                    for (int i = 0; i < inputString.Count; i++)
                        if (inputString[i].Substring != sequence[i])
                        {
                            flag = false;
                            break;
                        }
                    if (flag) return true;

                }
            }
            return false;

        }

        public bool ContainsSequence(List<string> inputString)
        {

            foreach(string[] sequence in Paralel)
            {
                if (sequence.Length == inputString.Count)
                {
                    bool flag = true;
                    for (int i = 0; i < inputString.Count; i++)
                        if (inputString[i] != sequence[i])
                        {
                            flag = false;
                            break;
                        }
                    if (flag) return true;
                        
                }
            }
            return false;

        }
    }
}
