using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Translator.Model
{
    enum TerminalCode
    {
        Pogram,
        Begin,
        End,
        NewLine,
        Read,
        Write,
        Do,
        While,
        If,
        Then,
        FI,
        Or,
        And,
        Not,
        NotEqual,
        MoreEqual,
        LessEqual,
        More,
        Less,
        Equal,
        QuestionSign,
        Plus,
        Minus,
        Multiple,
        Devide,
        OpenBracket,
        CloseBracket,
        OpenSquareBracket,
        CloseSquareBracket,
        DoublePoint,
        Int,
        Float,
        Unsigned,
        Enddo,
        Identifier,
        Comma,
        Assign,
        Constant

    }
    enum CharacterType
    {
          Letter,
          Digit,
          Separator,
          MoreLass,
          Point,
          Minus,
          Equals,
          Not
    }

    class Alphabet
    {
        public static string[] ReservedLexem { get; } = new string[]
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
        public static string Letters { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static string Separoters { get; } = "+*/?(){}[]:, ";


        static public int IsReservedLexem(string substring)
        {
            int count = 0;
            foreach (string lexem in ReservedLexem)
            {
                if (lexem == substring) return count;
                else count++;
            }
            return -1;
            //return ((count > 0) && (count != ReservedLexem.Length)) ? count : -1;
        }

        static public CharacterType SymbolType(char c)
        {
            if (IsLetter(c)) return CharacterType.Letter;
            else if (IsDigit(c)) return CharacterType.Digit;
            else if (Separoters.Contains(c)) return CharacterType.Separator;
            else if (c == '>' || c == '<') return CharacterType.MoreLass;
            else if (c == '.') return CharacterType.Point;
            else if (c == '-') return CharacterType.Minus;
            else if (c == '=') return CharacterType.Equals;
            else if (c == '!') return CharacterType.Not;

            else throw new Exception($"Unexpected character '{c}'");
        }

        static public bool IsLetter(char c)
        {
            return (Letters.Contains(c)) ? true : false;
        }

        static public bool IsDigit(char c)
        {
            return char.IsDigit(c);
        }

        static public bool IsPoint(char c)
        {
            return (c == '.') ? true : false; ;
        }

        static public bool IsSeparator(char c)
        {
            return (Separoters.Contains(c)) ? true : false;
        }

        static public bool IsComperative(char c)
        {
            return (c == '<' || c == '>') ? true : false;
        }
    }
}
