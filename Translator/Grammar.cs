using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.SyntaxAnalyser.AscendingAnalysis;

namespace Translator
{
 public class Grammar
    {
        string FilePath = "/Grammar/grammar.gm";
        public Dictionary<string, RightPart> Rules { get; private set; }
      //  public string[] GrammarText { get; private set; }

        string[]  ReadFromFile(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath);
            }
            catch(FileNotFoundException ex)
            {
                throw new Exception("file not found. InnerException: "+ex.Message);
            }
            
        }

        void Parse(string[] rows)
        {

            foreach (string row in rows)
            {
                string[] splitRow = row.Split('#');
                Rules.Add(splitRow[0], new RightPart(splitRow[1]));
            }
        }

        public void InitializeGrammar()
        {
            Parse(ReadFromFile(FilePath));
        }

        public void InitializeGrammar(string grammarRow)
        {

            Parse(ReadFromFile(FilePath));
        }


    }
}
