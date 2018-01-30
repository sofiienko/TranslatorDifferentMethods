using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.RecursiveDescentParser
{
    class Сonsecutive : IExecute
    {
        string name;
        IExecute[] massUnit;

        public Сonsecutive(params IExecute[] massUnit)
        {
            this.massUnit = massUnit;
        }
        public Сonsecutive(string name, params IExecute[] massUnit)
        {
            this.name = name;
            this.massUnit = massUnit;
        }
        public bool Execute1()
        {
            Console.WriteLine("<<<<<<<<<");
            Console.WriteLine("Execute Parallel" + name);
            foreach (Unit item in massUnit)
            {


                if (!item.Execute())
                {
                    Console.WriteLine("Paralel fasle " + name);
                    Console.WriteLine(">>>>>>>>>");
                    return false;
                }
                else Console.WriteLine("paralel true");
            }
            // flag = flag | item.Execute();
            // return flag;
            Console.WriteLine("all PAralel true " + name);
            Console.WriteLine(">>>>>>>>");
            return true;

        }


        public bool Execute()
        {
            Console.WriteLine("<<<<<<<<<");
            Console.WriteLine("Execute Parallel" + name);
            for (int i = 0; i < massUnit.Length; i++)
            {


                if (!massUnit[i].Execute())
                {
                    Console.WriteLine("Paralel fasle " + name);
                    Console.WriteLine(">>>>>>>>>");
                    return false;
                }
                else Console.WriteLine("paralel true");
            }
            // flag = flag | item.Execute();
            // return flag;
            Console.WriteLine("all PAralel true " + name);
            Console.WriteLine(">>>>>>>>");
            return true;

        }
    }
}
