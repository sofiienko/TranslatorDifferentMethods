using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    public class Const
    {
        public  static List<Const> AllConstFromCode { get; set; }

        public float _Const { get; private set; }
        public int Index { get; private set; }
        public string Type { get; private set; }

        public Const(float _const, int index, string type)
        {
            this._Const = _const;
            this.Index = index;
            this.Type = type;
        }
    }
}
