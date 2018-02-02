using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    class AscendingAnalys:ISyntaxAnalyser
    {
        List<string> ListLexem { get; set; }

        RelationTable relationTableWindows;
        RealtionMatrix relationMatrix;
        private bool PrepareRelationMatrix()
        {
             relationTableWindows = new RelationTable();
             relationMatrix = new RealtionMatrix();

            relationMatrix.InitializeGeammar();
            relationMatrix.BuildMatrix(relationTableWindows.GetDataGridView);
            relationTableWindows.Show();

            return true;
        }

        public bool CheckSyntax(List<Lexem> listLexem)
        {

            ListLexem = (from lexem in listLexem
                         select
                         (lexem.Code == 38) ? "const" :  ///????
                         (lexem.Code == 34) ? "id" :
                          lexem.Substring ).ToList<string>();

            PrepareRelationMatrix();
            Parse();
            return true;
        }

        bool Parse()
        {
            
            var matrix = relationMatrix.Matrix;
            ListLexem.Add("#");
            Stack<string> stack = new Stack<string>();
            stack.Push("#");

            string relation;
            var enumerator = ListLexem.GetEnumerator();
            
            foreach(var lexem in ListLexem)
            {
                Console.WriteLine("relation: "+ stack.Peek()+" " + lexem);
                relation = GetRelation(stack.Peek(), lexem);
                if (relation == "=" || relation == "<")
                {
                    stack.Push(lexem);
                }
                else if (relation == ">") stack = ReplaceBase(stack);
                else Console.WriteLine("oops,problem");
            }

            return true;
        }

        string GetRelation(string firstLexem,string secondLexem)
        {
            int length = relationMatrix.Matrix.GetLength(0);
            int i = 0;
            for (; i < length; i++)
                if (relationMatrix.Matrix[i, 0] == firstLexem) break;

            int j = 0;
            for (; j < length; j++)
            {
                //Console.WriteLine(relationMatrix.Matrix[0, j]??"null");
                if (relationMatrix.Matrix[0, j] == secondLexem) break;// return relationMatrix.Matrix[0, j];
            }


            Console.WriteLine("-->"+relationMatrix.Matrix[i, j]);
             return (relationMatrix.Matrix[i, j]!="")? relationMatrix.Matrix[i, j]: throw new Exception("relation doesn`t exist between " + firstLexem + " and " + secondLexem);            
        }

        Stack<string> ReplaceBase(Stack<string> stack)
        {
            Stack<string> newStack = new Stack<string>();


            string relation;
            //var arrayFS = stack.Reverse().ToList();//array from stack
            var arrayFS = stack.ToList();//array from stack
            //for (int i=arrayFS.Count-1;i>=1;i--)
            for (int i =1 ; i <= arrayFS.Count - 1; i++)
            {

                //Console.WriteLine("r-->"+arrayFS[i-1]+" "+ arrayFS[i]);
                Console.WriteLine("r-->" + arrayFS[i] + " " + arrayFS[i-1]);

                //relation = GetRelation(arrayFS[i-1], arrayFS[i]);
                relation = GetRelation(arrayFS[i], arrayFS[i-1]);
                if (relation == "<")
                {
                    //for (int j = i - 1; j >= 0; j--)
                    for (int j = arrayFS.Count - 1; j >=i ; j--)
                        newStack.Push(arrayFS[j]);
                    
                    newStack.Push(GetNotTerminal(arrayFS, i));
                    return newStack;
                    //var part = stack.Reverse().ToList().Except(arrayFS).ToList();
                    //newStack.Push(GetNotTerminal(part));
                }
            }
            return null;// newStack;
        }

        string GetNotTerminal(List<string> list,int end)
        {
            List<string> part = new List<string>();
            // for (int i = list.Count-1; i >= start; i--)
            for (int i = 0; i <= end - 1; i++)
                part.Add(list[i]);


            foreach (var item in relationMatrix.Grammar)
                if (item.Value.ContainsSequence(part)) return item.Key;

            return null;
        }
        string GetNotTerminal(List<string> list)
        {
            foreach (var item in relationMatrix.Grammar)
                if (item.Value.ContainsSequence(list)) return item.Key;

            return null;
        }

    }
}
