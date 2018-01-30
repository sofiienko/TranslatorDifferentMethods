using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.RecursiveDescentParser
{
    class AtLeastOne : IExecute
    {
        Unit must;
        Сonsecutive[] notNecessarily;

        public AtLeastOne(Unit must, params Сonsecutive[] notNecessarily)
        {
            this.must = must;
            this.notNecessarily = notNecessarily;
        }

        public bool Execute()
        {
            Console.WriteLine("Execute AtLeast");
            if (!must.Execute()) return false;
            if (notNecessarily != null)
                for (int i = 0; i < notNecessarily.Length; i++)
                    if (NotNecessarilyCheck(i)) return true;

            return true;
        }

        private bool NotNecessarilyCheck(int i)
        {
            bool flag = false;
            while (notNecessarily[i].Execute())
            {
                flag = true;
            }
            return flag;
        }
    }
}
