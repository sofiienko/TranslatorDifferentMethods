//#define Release

using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    class AscendingAnalys : ISyntaxAnalyser
    {
        bool showRelationTable = true;
        public bool ShowRealtionTable
        {
            get { return showRelationTable; }
            set
            {
                showRelationTable = value;
                if (showRelationTable == true) PrepareRelationMatrix();
            }
        }
        private bool showParsingTable = true;
        public bool ShowParsingTable
        {
            get { return showParsingTable; }
            set
            {
                showParsingTable = value;
                if (value == true || snapList != null) new ParsingTableWindows(snapList).Show();
            }
        }
        List<Model.ISymbol> ListLexem { get; set; } = new List<Model.ISymbol>();

        public List<Snap> snapList;
        private Snap currentSnap;
        public RPN rpn = new RPN();

        RelationTable relationTableWindows;
        RealtionMatrix relationMatrix;

        //ParsingTableWindows parsingTable;

        private bool PrepareRelationMatrix()
        {
            relationTableWindows = new RelationTable();
            relationMatrix = new RealtionMatrix();

            relationMatrix.InitializeGeammar();
            relationMatrix.BuildMatrix(relationTableWindows.GetDataGridView);

            if (showRelationTable) relationTableWindows.Show();

            return true;
        }

        public bool CheckSyntax(List<Model.Lexem> listLexem)
        {
            //ListLexem = listLexem.Cast<ISymbol>().ToList() ;
            snapList = new List<Snap>();

            //ListLexem = (from lexem in listLexem
            //             select
            //             (lexem.Code == 38) ? new LexemB { Substring = "const" } :  ///????
            //             (lexem.Code == 34) ? new LexemB { Substring = "id" } :
            //              new LexemB { Substring = lexem.Substring }).ToList<LexemB>();


            foreach (var item in listLexem)
            {
                if (item is Model.Constant ) item.Substring = "const";
                if (item is Model.Identifier) item.Substring = "id";
                ListLexem.Add(item);
            }


            PrepareRelationMatrix();
            Parse();

            if (showParsingTable) new ParsingTableWindows(snapList).Show();


            return true;
        }
        public bool CheckSyntax(List<Model.ISymbol> listLexem)
        {
            //ListLexem = listLexem.Cast<ISymbol>().ToList() ;
            snapList = new List<Snap>();

            //ListLexem = (from lexem in listLexem
            //             select
            //             (lexem.Code == 38) ? new LexemB { Substring = "const" } :  ///????
            //             (lexem.Code == 34) ? new LexemB { Substring = "id" } :
            //              new LexemB { Substring = lexem.Substring }).ToList<LexemB>();


            foreach (var item in listLexem)
            {
                if (item is Model.Constant constant) constant.Substring = "const";
                if (item is Model.Identifier idn) idn.Substring = "id";
                ListLexem.Add(item);
            }


            PrepareRelationMatrix();
            Parse();

            if (showParsingTable) new ParsingTableWindows(snapList).Show();


            return true;
        }
        bool Parse()
        {
            var matrix = relationMatrix.Matrix;

            ListLexem.Add(new Model.NonTerminal { Substring = "#" });

            Stack<Model.ISymbol> stack = new Stack<Model.ISymbol>();
            stack.Push(new Model.NonTerminal { Substring = "#" });

            string relation;
            var enumerator = ListLexem.GetEnumerator();
            bool isNext = enumerator.MoveNext();

            int i = 0;
            while (!(stack.Peek().Substring == "<program>"))
            {
                //#if DEBUG
                //                Console.WriteLine("relation: "+ stack.Peek()+" " + enumerator.Current);
                //#endif

                relation = GetRelation(stack.Peek(), enumerator.Current);

                currentSnap = new Snap(stack, relation, RemainderInputLexem(i));

                if (relation == "=" || relation == "<")
                {
                    stack.Push(enumerator.Current);
                    isNext = enumerator.MoveNext();
                    i++;
                }
                else if (relation == ">") stack = ReplaceBase(stack);
                else Console.WriteLine("oops,problem. Wrong relation between " + stack.Peek() + " and " + enumerator.Current + " ( " + ((Lexem)enumerator.Current).Row + "row");

                snapList.Add(currentSnap);

            }
            Console.WriteLine("Verification completed succsesfully");

            return false;
        }

        List<Model.ISymbol> RemainderInputLexem(int i)
        {

            List<Model.ISymbol> newList = new List<Model.ISymbol>();
            for (; i < ListLexem.Count; i++)
                newList.Add(ListLexem[i]);

            return newList;
        }

        string GetRelation(Model.ISymbol firstLexem,Model.ISymbol secondLexem)
        {
            int length = relationMatrix.Matrix.GetLength(0);
            int i = 0;

            for (; i < length; i++)
                if (relationMatrix.Matrix[i, 0] == firstLexem.Substring) break;
            if (i >= length) throw new Exception("first lexem wasn`t found in relation matrix");


            int j = 0;
            for (; j < length; j++)
                if (relationMatrix.Matrix[0, j] == secondLexem.Substring) break;
            if (j >= length) throw new Exception("second lexem " + secondLexem + " wasn't found");


            Lexem s_Lexem = secondLexem as Lexem;
            string row = (s_Lexem != null) ? s_Lexem.Row.ToString() : null;
            return (relationMatrix.Matrix[i, j] != "") ? relationMatrix.Matrix[i, j]
                : throw new Exception("relation doesn`t exist between " + firstLexem + " and " + secondLexem + " (" + row + " row )");
        }

        Stack<Model.ISymbol> ReplaceBase(Stack<Model.ISymbol> stack)
        {
            Stack<Model.ISymbol> newStack = new Stack<Model.ISymbol>();

            string relation;

            var arrayFS = stack.ToList();//array from stack

            for (int i = 1; i <= arrayFS.Count - 1; i++)
            {
                //#if DEBUG
                //                Console.WriteLine("r-->" + arrayFS[i] + " " + arrayFS[i-1]);
                //#endif
                relation = GetRelation(arrayFS[i], arrayFS[i - 1]);
                if (relation == "<")
                {
                    for (int j = arrayFS.Count - 1; j >= i; j--)
                        newStack.Push(arrayFS[j]);

                    newStack.Push(new Model.NonTerminal { Substring = GetNotTerminal(arrayFS, i) });
                    return newStack;
                }
            }

            throw new Exception("Could not find base");
        }

        string GetNotTerminal(List<Model.ISymbol> list, int end)
        {
            List<Model.ISymbol> part = new List<Model.ISymbol>();
            // for (int i = list.Count-1; i >= start; i--)
            for (int i = 0; i <= end - 1; i++)
                part.Add(list[i]);

            part.Reverse();
            currentSnap.SetBase(part);

            foreach (var item in relationMatrix.Grammar)
                if (item.Value.ContainsSequence(part))
                {

                    rpn.AddLexemsToCurrentRPN(part.ToArray());
                    currentSnap.SetRPN(rpn.CurrentRPNtoString());
                    return item.Key;
                }

            throw new Exception("Сould not find noterminal for base: " + string.Join(", ", part));
        }

    }
}
