using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator.SyntaxAnalyser.AscendingAnalysis
{
    public partial class RelationTable : Form
    {
        public RelationTable()
        {
            InitializeComponent();
        }
        public DataGridView GetDataGridView
        {
            get
            {
                return dataGridView1;
            }
        }
        private void RelationTable_Load(object sender, EventArgs e)
        {

        }
    }
}
