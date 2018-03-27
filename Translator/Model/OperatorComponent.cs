using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
    public class OperatorComponent : IRPNElement
    {
        public IEnumerable<IRPNElement> Components { get; private set; }
    }
}
