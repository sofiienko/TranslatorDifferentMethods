using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    class AscendingAnalys:ISyntaxAnalyser
    {

        private bool PrepareRelationMatrix()
        {
            RelationTable relationTableWindows = new RelationTable();
            RealtionMatrix reltionMatrix = new RealtionMatrix();

            reltionMatrix.InitializeGeammar();
            reltionMatrix.BuildMatrix(relationTableWindows.GetDataGridView);
            relationTableWindows.Show();

            return true;
        }

        public bool CheckSyntax(List<Lexem> listLexem)
        {

            PrepareRelationMatrix();

            return true;
        }
    }
}
