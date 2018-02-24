using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.LexicalAnalyser.DiagramOfState
{
    class DiagramOfStateAnalyser:ILexicalAnalyser
    {
        public List<Lexem> LexemList { get; private set; }
        public List<Idnt> IdentifierList { get; private set; }
        public List<Const> ConstantList { get; private set; }

        public List<string> SourceCode { get; set; }

        public DiagramOfStateAnalyser(List<string> sourceCode)
        {
            this.SourceCode = sourceCode;
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
                    for (int i = 0; i < line.Length; i++)
                    {
                        tempList.Add(State.State1(line, ref i, row, ""));
                    }

                    ParserLine(tempList);
                    LexemList.Add(new Lexem(row, "¶", 3));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return true;
        }

        private void ParserLine(List<LexicalUnit> lexUnitInRowList)
        {
            int tempInt;
            float tempFloat;
            foreach (var unit in lexUnitInRowList)
            {
                //if we have this lexem in reserved lexem
                tempInt = Check.IsReservedLexem(unit.Substring);
                if (tempInt >= 0)
                {
                    LexemList.Add(new Lexem(unit.Row, unit.Substring, tempInt));
                    continue;
                }

                //if this lexem is a constant
                if (float.TryParse(unit.Substring.Replace('.', ','), out tempFloat))
                {
                    if (int.TryParse(unit.Substring, out tempInt))
                    {
                        var const_ = ConstantList.Find(x => x._Const == tempInt);

                        if (const_ != null)
                            LexemList.Add(new Lexem(unit.Row, unit.Substring, 38, indexConst: const_.Index));
                        else
                        {
                            ConstantList.Add(new Const(tempInt, ConstantList.Count, "int"));
                            LexemList.Add(new Lexem(unit.Row, unit.Substring, 38, indexConst: ConstantList.Count - 1));
                        }
                    }
                    else
                    {
                        var const_ = ConstantList.Find(x => x._Const == tempInt);

                        if (const_ != null)
                            LexemList.Add(new Lexem(unit.Row, unit.Substring, 38, indexConst: const_.Index));
                        else
                        {
                            ConstantList.Add(new Const(tempInt, ConstantList.Count, "float"));
                            LexemList.Add(new Lexem(unit.Row, unit.Substring, 38, indexConst: ConstantList.Count - 1));
                        }
                    }
                    continue;
                }

                //if this lexem is a idnt;
                Idnt tempIdnt = IdentifierList.Find(x => x.Name == unit.Substring);
                if (tempIdnt != null) LexemList.Add(new Lexem(unit.Row, tempIdnt.Name, 35, indexIdnt: tempIdnt.Index));
                else
                {
                    if (lexUnitInRowList.Exists(x => (x.Substring.Contains("float"))) &&
                                                    (lexUnitInRowList.Exists(x => x.Substring.Contains("="))))
                    {
                        IdentifierList.Add(new Idnt(unit.Substring, IdentifierList.Count, "float"));
                        LexemList.Add(new Lexem(unit.Row, unit.Substring, 35, indexIdnt: IdentifierList.Count - 1));
                    }


                    else if (lexUnitInRowList.Exists(x => (x.Substring.Contains("int"))) &&
                                  (lexUnitInRowList.Exists(x => x.Substring.Contains("="))))
                    {
                        IdentifierList.Add(new Idnt(unit.Substring, IdentifierList.Count, "int"));
                        LexemList.Add(new Lexem(unit.Row, unit.Substring, 35, indexIdnt: IdentifierList.Count - 1));
                    }


                    else if ((lexUnitInRowList.Exists(x => x.Substring.Contains("program"))))
                    {
                        IdentifierList.Add(new Idnt(unit.Substring, IdentifierList.Count, "program"));
                        LexemList.Add(new Lexem(unit.Row, unit.Substring, 35, indexIdnt: IdentifierList.Count - 1));
                    }

                    else throw new Exception($"not defined variable {unit.Substring} ");
                }


            }
        }

    }
}
