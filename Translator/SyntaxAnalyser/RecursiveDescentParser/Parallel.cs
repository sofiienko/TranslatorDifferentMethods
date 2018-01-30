using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.RecursiveDescentParser
{ 
    class Parallel
    {
        List<IExecute> massElement = new List<IExecute>();

        public Parallel(params IExecute[] mass)
        {
            this.massElement.AddRange(mass);
        }
        public void AddElement(params IExecute[] mass)
        {
            this.massElement.AddRange(mass);
        }
        public bool Execute1()
        {

            Console.WriteLine("Execute Row");
            int cur = RecursiveDescent.currentLexem;
            foreach (var item in massElement)
                if (item.Execute())
                { Console.WriteLine("row true"); return true; }
                else
                {
                    RecursiveDescent.currentLexem = cur;
                    Console.WriteLine("row false");
                }
            //    flag = flag & item.Execute();
            // return flag;
            Console.WriteLine("row all false");
            return false;
        }


        public bool Execute()
        {

            Console.WriteLine("Execute Row");
            int cur = RecursiveDescent.currentLexem;
            for (int i = 0; i < massElement.Count; i++)
                if (massElement[i].Execute())
                { Console.WriteLine("row true"); return true; }
                else
                {
                    //                    RecursiveDescent.currentLexem = cur;
                    Console.WriteLine("row false");
                }
            //    flag = flag & item.Execute();
            // return flag;
            Console.WriteLine("row all false");
            return false;
        }
    }
}
