using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.LexicalAnalyser.DiagramOfState
{
    static class State
    {
        static public LexicalUnit State1(string line, ref int pointer, int row, string buffer = "")
        {

            if (line == "") throw new Exception("line is empty");
            while (line[pointer] == ' ') pointer++;
            if (line.Length <= pointer) throw new Exception("some problem");

            if (Check.IsLetter(line[pointer])) return State2(line, ref pointer, row, buffer += line[pointer]);
            else if (Check.IsDigit(line[pointer])) return State3(line, ref pointer, row, buffer += line[pointer]);
            else if (line[pointer] == '-') return State5(line, ref pointer, row, buffer += line[pointer]);
            else if (line[pointer] == '.') return State6(line, ref pointer, row, buffer += line[pointer]);
            else if (line[pointer] == '=') return State7(line, ref pointer, row, buffer += line[pointer]);
            else if (line[pointer] == '!') return State8(line, ref pointer, row, buffer += line[pointer]);
            else if (Check.IsComperative(line[pointer])) return State9(line, ref pointer, row, buffer += line[pointer]);

            else if (Check.IsSeparator(line[pointer])) return new LexicalUnit(buffer += line[pointer], row);
            else throw new Exception($"Unclear lexem in {row} row -  '{buffer}'");
        }

        public static LexicalUnit State2(string line, ref int pointer, int row, string buffer = null)
        {
            pointer += 1;

            if (pointer >= line.Length) return new LexicalUnit(buffer, row);


            if (Check.IsDigit(line[pointer])) return State2(line, ref pointer, row, buffer += line[pointer]);
            if (Check.IsLetter(line[pointer])) return State2(line, ref pointer, row, buffer += line[pointer]);

            pointer -= 1;
            return new LexicalUnit(buffer/* += line[pointer]*/, row);
        }


        public static LexicalUnit State3(string line, ref int pointer, int row, string buffer = null)
        {
            pointer += 1;
            if (pointer >= line.Length) return new LexicalUnit(buffer, row);


            if (Check.IsDigit(line[pointer])) return State3(line, ref pointer, row, buffer += line[pointer]);
            if (line[pointer] == '.') return State4(line, ref pointer, row, buffer += line[pointer]);

            pointer -= 1;
            return new LexicalUnit(buffer, row);

        }


        public static LexicalUnit State4(string line, ref int pointer, int row, string buffer = null)
        {
            pointer += 1;

            if (pointer >= line.Length) return new LexicalUnit(buffer, row);

            if (Check.IsDigit(line[pointer])) return State4(line, ref pointer, row, buffer += line[pointer]);


            // pointer -= 1;
            return new LexicalUnit(buffer/* += line[pointer]*/, row);
        }

        public static LexicalUnit State5(string line, ref int pointer, int row, string buffer = null)
        {
            pointer++;

            if (pointer >= line.Length) return new LexicalUnit(buffer, row);

            if (line[pointer] == '.') return State6(line, ref pointer, row, buffer += line[pointer]);
            if (Check.IsDigit(line[pointer])) return State3(line, ref pointer, row, buffer += line[pointer]);

            throw new Exception($"I can`t undertadn lexem in {row} rows {buffer}");
        }

        public static LexicalUnit State6(string line, ref int pointer, int row, string buffer = null)
        {
            pointer += 1;
            if (pointer >= line.Length) return new LexicalUnit(buffer, row);

            if (Check.IsDigit(line[pointer])) return State4(line, ref pointer, row, buffer += line[pointer]);

            throw new Exception($"I can`t undertadn lexem in {row} rows {buffer}");
        }

        public static LexicalUnit State7(string line, ref int pointer, int row, string buffer = null)
        {

            if (line[pointer + 1] == '=') return new LexicalUnit(buffer += line[pointer + 1], row);
            return new LexicalUnit(buffer, row);
        }

        public static LexicalUnit State8(string line, ref int pointer, int row, string buffer = null)
        {
            if (line[pointer + 1] == '=') return new LexicalUnit(buffer += line[pointer + 1], row);
            throw new Exception($"I can`t undertadn lexem in {row} rows {buffer}");
        }

        public static LexicalUnit State9(string line, ref int pointer, int row, string buffer = null)
        {
            if (line[pointer + 1] == '=') return new LexicalUnit(buffer += line[pointer + 1], row);
            else return new LexicalUnit(buffer, row);
        }
    }
}
