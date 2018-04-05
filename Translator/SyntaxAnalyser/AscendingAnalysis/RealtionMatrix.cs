using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    class RealtionMatrix
    {       
        public Dictionary<string, RightPart> Grammar = new Dictionary<string, RightPart>();
        public Dictionary<string, List<string>> equals = new Dictionary<string, List<string>>();

        public Dictionary<string, List<string>> lastPlus = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> firstPlus = new Dictionary<string, List<string>>();

        List<string> allLexem = new List<string>();
        public string[,] Matrix { get; private set; }
        public List<string> terminals = new List<string>();


        void FindingEqluals()
        {
            string previousLexem = null;
            foreach (var paralel in Grammar.Values)
                foreach (string[] consistently in paralel.Paralel)
                {
                    previousLexem = null;
                    foreach (string currentLexem in consistently)
                        if (previousLexem == null) previousLexem = currentLexem;
                        else
                        {
                            if (!equals.Keys.Contains(previousLexem))
                                equals.Add(previousLexem, new List<string>());
                            equals[previousLexem].Add(currentLexem);

                            previousLexem = currentLexem;
                        }
                }
        }
        void FindingFirstPlus()
        {
            foreach (var left in Grammar.Keys)
            {
                var tempList = FirstPlus(left);
                if (tempList.Any())
                    firstPlus.Add(left, tempList);

            }
        }
        void FindingLastPlus()
        {
            foreach (var left in Grammar.Keys)
            {
                var tempList = LastPlus(left);
                if (tempList.Any())
                    lastPlus.Add(left, tempList);

            }
        }

        string DetermineReleation(string first,string second)
        {
            if (first == null || first == "") throw new Exception("First parametr doesn`t dermine");
            if (second == null || second == "") throw new Exception("Second parametr doesn`t dermine");

            string result="";

            if (first == "#") result =  "<";
            if (second == "#") result= ">";

            if (equals.Keys.Contains(first)&&equals[first].Contains(second))
                result+= "=";

            if (Less(first, second))
                result += "<";

            if (More(first, second))
                result += ">";

            if (result.Length > 1)
                Console.WriteLine("error " + result+"relation in [" + first + " " + second + "]");

            return result;
        }
       
        public void BuildMatrix(DataGridView table)
        {
            allLexem.Add(Grammar.First().Key);
            foreach (RightPart lp in Grammar.Values)
                foreach (string[] lexems in lp.Paralel)
                    foreach (string lexem in lexems)
                    {
                        if (!allLexem.Contains(lexem)) allLexem.Add(lexem);
                    }

            foreach (string lexem in allLexem)
                if (!(lexem.Contains('<') && lexem.Contains('>')))
                    terminals.Add(lexem);


            Console.WriteLine("text parsered");

            allLexem.Add("#");
            int size = allLexem.Count;
            Matrix = new string[size + 1, size + 1];

            for (int i = 1; i < size+1; i++)
            {
                Matrix[0, i] = allLexem[i-1];
                Matrix[i, 0] = allLexem[i-1];

            }


            FindingEqluals();
            FindingFirstPlus();
            FindingLastPlus();

            for (int i = 1; i < size + 1; i++)
                for (int j = 1; j < size + 1; j++)
                    Matrix[i, j] = DetermineReleation(Matrix[i, 0], Matrix[0, j]);

            table.ColumnCount = size+2;
            for (int i = 0; i < size; i++)
            {
                table.Rows.Add();
                for (int j = 0; j < size; j++)
                {
                    table.Rows[i].Cells[j].Value = Matrix[i+1, j+1];
                }

            }
            
            for(int i = 0; i < size; i++)
            {
                table.Rows[i].HeaderCell.Value = Matrix[0, i+1];
                table.Columns[i].HeaderCell.Value = Matrix[0, i+1];
            }

        }
        public List<string> First(string u)
        {
            List<string> result = new List<string>();

            if (Grammar.Keys.Contains(u))
                foreach (var i in Grammar[u].Paralel)
                    if (!result.Contains(i[0]))
                        result.Add(i[0]);

            return result;
        }

        public List<string> FirstPlus(string u)
        {
            List<string> result = new List<string>();
            //List<string> first = new List<string>();
            // List<string> result = new List<string>();

            result.AddRange(First(u));
            if (!result.Any()) return result;

            for (int i = 0; i < result.Count; i++)
                foreach (string e in First(result[i]))
                    if (!result.Contains(e))
                        result.Add(e);


            return result;
        }

        public List<string> Last(string u)
        {

            List<string> result = new List<string>();

            if (Grammar.Keys.Contains(u))
                foreach (var i in Grammar[u].Paralel)
                    if (!result.Contains(i[i.Length - 1]))
                        result.Add(i[i.Length - 1]);

            return result;
        }

        public List<string> LastPlus(string u)
        {
            List<string> result = new List<string>();

            result.AddRange(Last(u));
            if (!result.Any()) return result;

            for (int i = 0; i < result.Count; i++)
                foreach (string e in Last(result[i]))
                    if (!result.Contains(e))
                        result.Add(e);

            return result;
        }

        public bool Less(string R, string S)
        {
            if (R == null) return false;
            if (!equals.Keys.Contains(R)) return false;

            List<string> vList;
            vList = equals[R];

            var vListExceptTerminals = vList.Except(terminals);


            foreach (string v in vListExceptTerminals)
                if (firstPlus.Keys.Contains(v))
                    if (firstPlus[v].Contains(S)) return true;

            return false;
        }
        public bool More(string R, string S)
        {
            if (R == null || S == null) return false;
            //if (!terminals.Contains(S)) return false;

            var vList = from v in @equals
                        where v.Value.Contains(S) && !terminals.Contains(v.Key)
                        select v.Key;


            foreach (string v in vList)
                if (LastPlus(v).Contains(R)) return true;

            //or///
            var containsR = from v in @equals
                            where LastPlus(v.Key).Contains(R)
                            select v;

            foreach (var i in containsR)
                foreach (string w in i.Value)
                    if (!terminals.Contains(w))
                        if (FirstPlus(w).Contains(S)) return true;

            return false;
        }

        public void InitializeGeammar()
        {
            string input =
                "<program># program <NL> begin <NL> <OL> end <NL>\n" +
                "<NL># <NewLine>\n" +
                "<NewLine># ¶ | <NewLine> ¶\n" +
                "<OL># <OperatorList>\n" +
                 "<OperatorList># <operator> ¶ | <OperatorList> <operator> ¶\n" +
                                                                                                                                                                         
                "<operator># id = <expression1> |" +
                "read ( <il1> ) |" +
                "write ( <il1> ) |" +
                "do while [ <logicalExpression1> ] <NL> <OL>  enddo |" +
                "if [ <logicalExpression1> ] then <NL> <OL> fi |" +//
                "id = [ <logicalExpression1> ] ? <expression1> : <expression1> |" +
                "<type> id = <expression1>\n" +

                "<il1>#<idList>\n" +
                "<idList># , id | <idList>\n" +

                "<logicalExpression1># <logicalExpression>\n" +
                "<logicalExpression># <logicalTerm1> | <logicalExpression> or <logicalTerm1> \n" +

                "<logicalTerm1># <logicalTerm>\n" +
                "<logicalTerm># <logicalMultiple1> | <logicalTerm> and <logicalMultiple1>\n" +

                "<logicalMultiple1># <logicalMultiple> \n" +
                "<logicalMultiple># <relation> | not <logicalMultiple> | [ <logicalExpression1> ] \n" +//changed

                "<relation># <expression> <relationSign> <expression1>\n" +
                "<relationSign># != | <= | >= | < | > | == \n" +

                "<expression1># <expression>\n" +
                "<expression># <term1> | <expression> + <term1>| <expression> - <term1>\n" +

                "<term1># <term> \n" +
                "<term># <multiple> | <term> * <multiple> |<term> / <multiple>\n" +
                "<multiple># ( <expression1> ) | id | const\n" +

                "<type># int | float | unsigned int | unsigned float";

            //string input = "<logicalExpression1># <logicalExpression>\n" +
            //    "<logicalExpression># <logicalTerm1> | <logicalExpression> or <logicalTerm1>\n" +

            //    "<logicalTerm1># <logicalTerm>\n" +
            //    "<logicalTerm># <logicalMultiple1> | <logicalTerm> and <logicalMultiple1>\n" +

            //    "<logicalMultiple1># <logicalMultiple>\n" +
            //    "<logicalMultiple># <relation> | not <logicalMultiple> | ( <logicalExpression1> )\n" +//changed

            //    "<relation># <expression> <relationSign> <expression1>\n" +
            //    "<relationSign># < | > | = \n" +

            //    "<expression1># <expression>\n" +
            //    "<expression># <term1> | <expression> + <term1>| <expression> - <term1>\n" +

            //    "<term1># <term>\n" +
            //    "<term># <multiple> | <term> * <assign1> |<term> / <assign1>\n" +
            //    "<assign1># <assign> \n"+
            //    "<assign># <idn> := <term1>\n" +
            //    "<multiple># ( <assign1> ) | id | const";


            string[] rows = input.Split('\n');

            foreach (string row in rows)
            {
                string[] splitRow = row.Split('#');
                Grammar.Add(splitRow[0], new RightPart(splitRow[1]));
            }
        }
    }
}
