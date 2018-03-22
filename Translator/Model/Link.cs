using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
  public  class Link:Lexem
    {
        private Identifier idntObject;

        public uint NumberInIdentifierList { get; private set; }

        public string Name
        {
            get
            {
                return idntObject.Name;
            }
        }

        public TerminalCode Type
        {
            get { return idntObject.Type; }
        }

        public double? Value
        {
            get
            {
                return idntObject.Value;
            }
            set
            {
                idntObject.Value = value;
            }
        } 

        public Link(Identifier identifierObject,uint number, uint row ):
            base(number, row, "idn", identifierObject.Type)
        {
            this.idntObject = identifierObject;
        }

    }
}
