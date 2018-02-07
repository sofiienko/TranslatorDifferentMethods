using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.LexicalAnalyser
{
    enum Symbol { letter, digit, separator, moreLass, point, minus, equals, not }

    static class Check
    {
        private static string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static string digits = "1234567890";
        private static string separoters = "+*/?(){}[]:, ";//+Environment.NewLine;

        static string[] reservedLexem = new string[]
        {
            "program",//0++++
            "begin",
            "end",
            "/n/r",
            "read",//4
            "write",
            "do",//6
            "while",
            "if",
            "then",
            "fi",//10
            "or",
            "and",//12
            "not",
            "!=",
            "<=",//15
            ">=",
            "<",//17
            ">",
            "==",//19
            "?",
            "+",//21
            "-",
            "*",
            "/",
            "(",//25
            ")",
            "[",
            "]",
            ":",
            "int",//30
            "float",
            "unsigned",
            "enddo",
            "idn",//34
            ",",
            "=",
            "const",
            //38
          
        };

        static public int IsReservedLexem(string substring)
        {
            int count = 0;
            foreach (string lexem in reservedLexem)
            {
                if (lexem == substring) return count;
                //if (lexem.Contains(substring)) return count;
                else count++;
            }

            return ((count > 0) && (count != 38)) ? count : -1;
        }



        static public Symbol SymbolType(char c)
        {
            if (IsLetter(c)) return Symbol.letter;
            else if (IsDigit(c)) return Symbol.digit;
            else if (separoters.Contains(c)) return Symbol.separator;
            else if (c == '>' || c == '<') return Symbol.moreLass;
            else if (c == '.') return Symbol.point;
            else if (c == '-') return Symbol.minus;
            else if (c == '=') return Symbol.equals;
            else if (c == '!') return Symbol.not;
            else throw new Exception("I can`t undestand character");
        }

        static public bool IsLetter(char c)
        {
            return (letters.Contains(c)) ? true : false;
        }

        static public bool IsDigit(char c)
        {
            return (digits.Contains(c)) ? true : false;
        }

        static public bool IsPoint(char c)
        {
            return (c == '.') ? true : false; ;
        }

        static public bool IsSeparator(char c)
        {
            return (separoters.Contains(c)) ? true : false;
        }

        static public bool IsComperative(char c)
        {
            return (c == '<' || c == '>') ? true : false;
        }

    }
}
