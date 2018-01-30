using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.LexicalAnalyser;

namespace Translator.LexicalAnalyser.FiniteStateMachine
{
    class FiniteMachine:ILexicalAnalyser
    {
        public List<Lexem> LexemList { get; private set; }
        public List<Idnt> IdentifierList { get; private set; }
        public List<Const> ConstantList { get; private set; }

        public List<string> SourceCode { get; set; }

        private List<State> tos;//tabel of state

        public FiniteMachine(List<string> sourceCode)
        {
            this.SourceCode = sourceCode;
            InitialTableOfSate();
        }

        public bool Analize()
        {

            LexemList = new List<Lexem>();
            IdentifierList = new List<Idnt>();
            ConstantList = new List<Const>();

            try
            {
                int row = 0;
                foreach (var line in SourceCode)
                {
                    row++;
                    List<LexicalUnit> tempList = new List<LexicalUnit>();
                    for (int i = 0; i <= line.Length - 1;)
                        // tos[0].Execute(ref row, line, null,ref i);
                        State.Execute(tos[0], ref row, line, null, ref i);
                    LexemList.Add(new Lexem(row, "¶", 3));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return true;
        }

        private void InitialTableOfSate()
        {
            tos = new List<State>();
            tos.Add(new State(1, ReservedLexem));
            tos.Add(new State(2, ReservedOrIdentifier));
            tos.Add(new State(3, Constant));

            tos.Add(new State(4, Constant));
            tos.Add(new State(5, MinusOrLexem));
            tos.Add(new State(6, null /*ReservedLexem*/));

            tos.Add(new State(7, ReservedLexem));
            tos.Add(new State(8, ReservedLexem));
            tos.Add(new State(9, ReservedLexem));

            //1
            tos[0].AddReference(
                new Pair(Symbol.letter, tos[2 - 1]),
                new Pair(Symbol.digit, tos[3 - 1]),
                new Pair(Symbol.minus, tos[5 - 1]),
                new Pair(Symbol.point, tos[6 - 1]),
                new Pair(Symbol.equals, tos[7 - 1]),
                new Pair(Symbol.not, tos[8 - 1]),
                new Pair(Symbol.moreLass, tos[9 - 1]),
                new Pair(Symbol.separator, null)
                );
            //2
            tos[1].AddReference(
                new Pair(Symbol.digit, tos[2 - 1]),
                new Pair(Symbol.letter, tos[2 - 1])
                );
            //3
            tos[2].AddReference(
                new Pair(Symbol.digit, tos[3 - 1]),
                new Pair(Symbol.point, tos[4 - 1])
                );
            //4
            tos[3].AddReference(
                new Pair(Symbol.digit, tos[4 - 1])
                );
            //5
            tos[4].AddReference(
                new Pair(Symbol.digit, tos[3 - 1]),
                new Pair(Symbol.point, tos[6 - 1])
                );
            //6
            tos[5].AddReference(
                new Pair(Symbol.digit, tos[4 - 1])
                );
            //7
            tos[6].AddReference(
                new Pair(Symbol.equals, null)
                );
            //8
            tos[7].AddReference(
                 new Pair(Symbol.equals, null)
                );
            //9
            tos[8].AddReference(
                new Pair(Symbol.equals, null)
                );

        }


        Lexem ReservedLexem(ref int row, string substring)
        {

            if (substring == Environment.NewLine)
            {
                LexemList.Add(new Lexem(row, "¶", 3));
                row++;
            }
            else
            {
                int code = Check.IsReservedLexem(substring);
                if (code != -1) LexemList.Add(new Lexem(row, substring, code));
                else throw new Exception($"Oops, I can`t understand constant '{substring}' in {row} row");
            }

            return null;
        }

        Lexem ReservedOrIdentifier(ref int row, string substring)
        {
            if (Check.IsReservedLexem(substring) != -1) return ReservedLexem(ref row, substring);
            else return Identifier(ref row, substring);
        }

        int AddConstant(float constant, string type)
        {
            Const tempConst = ConstantList.Find(x => x._Const == constant);

            if (tempConst == null)
            {
                ConstantList.Add(new Const(constant, ConstantList.Count, type));
                return ConstantList.Count - 1;
            }
            else return tempConst.Index;
        }

        Lexem Constant(ref int row, string substring)
        {
            float tempFloat;
            if (float.TryParse(substring.Replace('.', ','), out tempFloat))
            {
                int tempInt;
                int index;
                if (int.TryParse(substring, out tempInt))
                    index = AddConstant(tempInt, "int");
                else index = AddConstant(tempFloat, "float");

                LexemList.Add(new Lexem(row, substring, 38, indexConst: index));
            }
            else throw new Exception($"Oops, I can`t understand constant {substring} in {row} row");
            return null;
        }

        Lexem Identifier(ref int row, string substring)
        {
            Idnt tempIdnt = IdentifierList.Find(x => x.Name == substring);
            if (tempIdnt != null) { LexemList.Add(new Lexem(row, tempIdnt.Name, 34, indexIdnt: tempIdnt.Index)); return null; }
            else
            {
                if (LexemList.Count == 0) throw new Exception("Identifier can`t be first");

                Lexem temp = LexemList.Last<Lexem>();
                if (temp.Code == 30) IdentifierList.Add(new Idnt(substring, IdentifierList.Count, "int"));
                else if (temp.Code == 31) IdentifierList.Add(new Idnt(substring, IdentifierList.Count, "float"));
                else if (temp.Code == 0) IdentifierList.Add(new Idnt(substring, IdentifierList.Count, "program"));
                else throw new Exception($"Opps,I see a problem.Maybe you forgot about type 'int', 'float'or 'program' type in row {row}");
            }
            LexemList.Add(new Lexem(row, substring, 34, indexIdnt: IdentifierList.Count - 1));
            return null;
        }

        Lexem MinusOrLexem(ref int row, string substring)
        {
            // Lexem last = LexemList.Last<Lexem>();
            // if (last.Code == 34 || last.Code == 26 || last.Code == 38)
            //  {
            LexemList.Add(new Lexem(row, "-", 22));
            substring = substring.Remove(0, 1);
            //  }
            //     else throw new Exception();
            //else    Constant(ref row, substring);

            return null;
        }

    }
}
