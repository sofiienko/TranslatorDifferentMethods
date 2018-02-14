using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.LexicalAnalyser
{
   public class Idnt
    {
        public static List<Const> AllIdnFromCode { get; set; }

        public string Name { get; private set; }
        public int Index { get; private set; }
        public string Type { get; private set; }

        public Idnt(string name, int index, string type)
        {
            this.Name = name;
            this.Index = index;
            this.Type = type;

        }
    }
}
