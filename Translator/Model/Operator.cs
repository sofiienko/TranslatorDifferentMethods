using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
    public class Operator : IRPNElement
    {
        public uint? СomparativePriority { get; private set; }
        public uint? StackPriority { get; private set; }

        public string Sign { get; private set; }
        public Operator(string sign)
        {
            this.Sign = sign;
        }
        public Operator(string sign, uint priority)
        {
            this.Sign = sign;
            this.СomparativePriority = priority;
        }

        public Operator(string sign, uint comparativePriority, uint stackPriority)
        {
            this.Sign = sign;
            this.СomparativePriority = comparativePriority;
            this.StackPriority = stackPriority;
        }

        public override bool Equals(object obj)
        {
            if (obj is Operator temp) return this.Sign == temp.Sign;
            return false;
        }
        public override int GetHashCode()
        {
            return Sign.GetHashCode();
        }

        public override string ToString()
        {
            return Sign;
        }




        //public static bool operator !=(Operator o1, Operator o2)
        //{
        //    if (o1.Sign == o2.Sign) return false;
        //    else return true;
        //}

        //public static bool operator ==(Operator o1,Operator o2)
        //{
        //    if (o1.Sign == o2.Sign) return true;
        //    else return false;
        //}
    }


    public class OperatorRepository
    {
        private List<Operator> items;

        public OperatorRepository()
        {
            items = new List<Operator>
            {
                new Operator("(",0),
                new Operator("[",0),
                new Operator("if",0),
                new Operator("do",0),


               
                new Operator(")",1),
                new Operator("]",1),
                new Operator("or",1),
                new Operator("then",1),
                 new Operator("while",1),

                new Operator("and",2),

                new Operator("not",3),

                new Operator("<",4),
                new Operator(">",4),
                new Operator("<=",4),
                new Operator(">=",4),
                new Operator("==",4),
                new Operator("!=",4),

                new Operator("+",5),
                new Operator("-",5),

                new Operator("*",6),
                new Operator("/",6),


                new Operator("fi"),
                new Operator("enddo")

            };
        }

        public Operator this[string sign]=>items.FirstOrDefault(i=>i.Sign==sign);

    }
}
