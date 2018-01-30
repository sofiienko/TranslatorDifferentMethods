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
        public string[,] matrix;
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

        public void BuildMatrix(DataGridView table)
        {
            foreach (RightPart lp in Grammar.Values)
                foreach (string[] lexems in lp.Paralel)
                    foreach (string lexem in lexems)
                    {
                        //lexem.Trim(' ');
                        if (!allLexem.Contains(lexem)) allLexem.Add(lexem);
                    }

            foreach (string lexem in allLexem)
                if (!(lexem.Contains('<') && lexem.Contains('>')))
                    terminals.Add(lexem);

            MessageBox.Show("text parsered");
            int size = allLexem.Count;
            matrix = new string[size + 1, size + 1];

            for (int i = 0; i < size; i++)
            {
                matrix[0, i] = allLexem[i];
                matrix[i, 0] = allLexem[i];

            }


            FindingEqluals();
            FindingFirstPlus();
            FindingLastPlus();


            for (int i = 0; i < size; i++)
                if (equals.Keys.Contains(matrix[i, 0]))
                    for (int j = 0; j < size; j++)
                        if (equals[matrix[i, 0]].Contains(matrix[0, j]))
                            matrix[i + 1, j + 1] = "=";

            for (int i = 0; i <= size; i++)
                for (int j = 0; j <= size; j++)
                    if (Less(matrix[i, 0], matrix[0, j]))
                        matrix[i + 1, j + 1] += "<";

            for (int i = 0; i <= size; i++)
                for (int j = 0; j <= size; j++)
                    if (More(matrix[i, 0], matrix[0, j]))
                        matrix[i + 1, j + 1] += ">";

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (matrix[i + 1, j + 1] != null)
                        if (matrix[i + 1, j + 1].Length > 1) MessageBox.Show("error " + matrix[i + 1, j + 1] + " in [" + matrix[i, 0] + " " + matrix[0, j] + "]");

            MessageBox.Show("equils relation was build ");


            table.ColumnCount = size;
            for (int i = 1; i <= size; i++)
            {
                table.Rows.Add();
                for (int j = 1; j <= size; j++)
                {
                    table.Rows[i - 1].Cells[j - 1].Value = matrix[i, j];

                }

            }
            int counter = 0;
            foreach (DataGridViewRow row in table.Rows)
                row.HeaderCell.Value = matrix[0, counter++];
            counter = 0;
            foreach (DataGridViewColumn column in table.Columns)
                column.HeaderText = matrix[0, counter++];

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

            //   result.AddRange(First(result[i]));
            //foreach (var i in result)
            //    first.AddRange(First(i));

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
                "<програма># program id <ПнНР> begin <ПнНР> <СО> end\n" +//changed
                "<ПнНР># <ПерехідНаНовуСтроку>\n" +
                //   "<НР># <ПнНР>\n" +
                "<ПерехідНаНовуСтроку># ¶ | <ПерехідНаНовуСтроку> ¶\n" +
                "<СО># <CписокОператорів>\n" +
                 "<CписокОператорів># <опертор> ¶|<CписокОператорів> <опертор> ¶\n" +//changed
                                                                                     //"<CписокОператорів># <опертор> <ПнНР>|<CписокОператорів> <опертор> <ПнНР>\n" +//changed
                                                                                     //"<CписокОператорів># <опертор> | <CписокОператорів> <ПнНР> <опертор>\n" +
                "<опертор># id = <вираз1> |" +
                "read ( <сп1> )|" +
                "write ( <сп1> )|" +
                "do while [ <логічийВираз1> ] <СО>  enddo |" +
                "if [ <логічнийВираз1> ] then <ПнНР> <СО> fi |" +//
                "іd = [ <логічнийВираз1> ] ? <вираз1> : <вираз1> |" +
                "<тип> id = <вираз1>\n" +

                "<сп1>#<списокІдентифікаторів>\n" +
                "<списокІдентифікаторів># id | <списокІдентифікаторів>\n" +

                "<логічнийВираз1># <логічнийВираз>\n" +
                "<логічнийВираз># <логічнийТерм1> | <логічнийВираз> or <логічниТерм1> \n" +

                "<логічнийТерм1># <логічнийТерм>\n" +
                "<логічнийТерм># <логічнийМножник1> | <логічнийТерм> and <логічнаМножник1>\n" +

                "<логічнийМножник1># <логічнийМножник> \n" +
                "<логічнийМножник># <відношення> | not <логічнийМножник> | [ <логічнийВираз1> ] \n" +//changed

                "<відношення># <вираз> <знакВідношення> <вираз1>\n" +
                "<знакВідношення># !=| <= | >= | < | > |== \n" +

                "<вираз1># <вираз>\n" +
                "<вираз># <терм1> | <вираз> + <терм1>| <вираз> - <терм1>\n" +

                "<терм1># <терм> \n" +
                "<терм># <множина> | <терм> * <множина> |<терм> / <множина>\n" +
                "<вираз2># <вираз>\n" +
                "<множина># ( <вираз1> ) | idn | const\n" +

                "<тип># int | float | unsigned int | unsigned float";

            //string input =
            //    "Z#b M b \n" +
            //    "M# ( L | a\n" +
            //    "L# M a )";

            string input3 =
                "<огоголошення>#<сп.ід.> <тип>\n" +
                "<сп.ід.>#id | <сп.ід.> , id\n" +
                "<тип># real|integer";

            string input4 = "<E1># <E>\n" +
                "<E># <E> + <T1> | <T1>\n" +
                "<T1># <T>\n" +
                "<T># <T> * <F> | <F>\n" +
                "<F># ( <E1> ) | i";

            string input2 =
                "<E># <E> + <T> | <T>\n" +
                "<T># <T> * <F> | <F>\n" +
                "<F># ( <E> ) | i";

            string input99 = "<program># program id <declaration_list1> begin <operator_list1> end.\n" +
             "<declaration_list># <declaration> ¶|<declaration_list> <declaration> ¶\n" +
             "<declaration># double <id_list1>|int <id_list1>\n" +
             "<id_list># , id|<id_list> , id\n" +
             "<operator_list># <operator> ¶|<operator_list> <operator> ¶\n" +
             "<operator># <pr>|<write>|<read>|<loop>|<jump>|<tern>\n" +
             "<tern># id = <logE1> ? <E1> : <E1>\n" +
             "<pr># id = <E1>\n" +
             "<write># write ( <id_list1> )\n" +
             "<read># read ( <id_list1> )\n" +
             "<loop># while <logE1> do <operator_list1>\n" +
             "<jump># if <logE1> then <operator_list1> else <operator_list1> endif\n" +
             "<z># <|>|<=|>=|==|!=\n" +
             "<E># <T1>|<E> + <T1>|<E> - <T1>\n" +
             "<T># <M>|<T> * <M>|<T> / <M>\n" +
             "<M># id|const|( <E1> )\n" +
             "<declaration_list1># <declaration_list>\n" +
             "<id_list1># <id_list>\n" +
             "<operator_list1># <operator_list>\n" +
             "<logE># <lt1>|<logE> // <lt1>\n" +
             "<lt># <lm>|<lt> && <lm>\n" +
             "<lm># <relation>|! <lm>|[ <logE1> ]\n" +
             "<relation># <E> <z> <E1>\n" +
             "<logE1># <logE>\n" +
             "<E1># <E>\n" +
             "<lt1># <lt>\n" +
             "<T1># <T>";


            string[] rows = input.Split('\n');

            foreach (string row in rows)
            {
                string[] splitRow = row.Split('#');
                Grammar.Add(splitRow[0], new RightPart(splitRow[1]));
            }



        }
    }
}
