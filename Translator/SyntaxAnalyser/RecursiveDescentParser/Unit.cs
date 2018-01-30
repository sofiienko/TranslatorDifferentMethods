using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.RecursiveDescentParser
{
    class Unit : IExecute
    {
        List<Lexem> lexemList;
        int startPosition;
        public RecursiveDescent syntaxisAnalizer;
        bool oneMoreTime = false;

        object objct;
        public SyntaxException except;


        public Unit(RecursiveDescent recursiveDescent, object objct, SyntaxException except, bool oneMoreTime = false)
        {
            this.syntaxisAnalizer = recursiveDescent;
            this.lexemList = recursiveDescent.LexemList;

            this.except = except;

            if (objct is Parallel || objct is int) this.objct = objct;
            else throw new Exception("Wrong argumen object must be int or Execute");

        }


        public bool Execute()
        {
            this.startPosition = RecursiveDescent.currentLexem;

            if (objct is int)
            {
                int code = (int)objct;
                // count =CheckLexem(code);
                if (startPosition < syntaxisAnalizer.LexemList.Count && code == lexemList[startPosition].Code)
                {
                    Console.WriteLine("++ " + lexemList[startPosition].Substring);
                    RecursiveDescent.currentLexem++;
                    return true;
                }
                else if (except != null)
                {
                    except.Source = "row " + lexemList[RecursiveDescent.currentLexem - 1].Row.ToString()
                        + " lexem #" + RecursiveDescent.currentLexem.ToString();
                    throw except;
                }

            }
            else
            {
                Parallel row = (Parallel)objct;
                //  count = ExecuteRow(row);
                if (row.Execute()) return true;
                else if (except != null)
                {
                    except.Source = "row " + lexemList[RecursiveDescent.currentLexem - 1].Row.ToString()
                        + " lexem #" + RecursiveDescent.currentLexem.ToString();
                    //   + " now" + lexemList[syntaxisAnalizer.currentLexem].Code;
                    throw except;
                }
            }
            //syntaxisAnalizer.currentLexem += count;
            //return (count == 0) ? false : true;
            return false;

        }

        private int ExecuteRow(Parallel row)
        {
            int count = 0;
            bool end = false;

            while ((count + startPosition < lexemList.Count) &&
                  ((oneMoreTime == false && count < 1) ||
                  (oneMoreTime == true && end == false)))
            {
                Console.WriteLine("Execute" + row);
                if (row.Execute()) return 0;// count++;
                else break;
            }

            if (count == 0 && except != null)
            {
                except.Source = "row " + lexemList[RecursiveDescent.currentLexem].Row.ToString()
                    + " lexem #" + RecursiveDescent.currentLexem.ToString()
                    + " now" + lexemList[RecursiveDescent.currentLexem].Code;
                throw except;
            }
            else return count;

        }


        private int CheckLexem(int dlgt)
        {
            int count = 0;
            bool end = false;

            while ((count + startPosition < lexemList.Count) &&
                  ((oneMoreTime == false && count < 1) ||
                  (oneMoreTime == true && end == false)))
            {
                if (dlgt == lexemList[startPosition + count].Code) count++;
                else break;
            }

            if (count == 0 && except != null)
            {
                except.Source = "row " + lexemList[RecursiveDescent.currentLexem].Row.ToString()
                 + " lexem #" + RecursiveDescent.currentLexem.ToString()
                 + " now" + lexemList[RecursiveDescent.currentLexem].Code;
                throw except;
            }
            else return count;

        }
    }
}
