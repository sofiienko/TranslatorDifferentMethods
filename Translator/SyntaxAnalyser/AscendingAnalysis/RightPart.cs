using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    class RightPart
    {
        public List<string[]> Paralel = new List<string[]>();

        public RightPart(string leftPart)
        {
            char[] splitter = { '|' };
            char[] splitter2 = { ' ' };
            string[] paralel = leftPart.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string p in paralel)
                Paralel.Add(p.Split(splitter2, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
