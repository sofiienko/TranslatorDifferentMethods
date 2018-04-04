using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
    public class OperatorComponent : IRPNElement, IOperator
    {
        public List<IRPNElement> Components { get; private set; }

        public OperatorComponent(params IRPNElement[] mass)
        {
            Components = mass.ToList();
        }

        public IRPNElement this[int number]
        {
            get
            {
                return Components[number]??null;
            }
        }

        public int? СomparativePriority
        {
            get
            {
                try
                {
                    return (Components[0] as Operator).СomparativePriority;
                }
                catch (NullReferenceException)
                {
                    throw new Exception("Bad order argumnet function.  First element should be Operator type");

                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in Components)
                sb.Append(item);

            return sb.ToString();
        }
    }
}
