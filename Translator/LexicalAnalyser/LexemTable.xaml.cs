using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Translator;

namespace Translator.LexicalAnalyser
{
    /// <summary>
    /// Логика взаимодействия для LexemTable.xaml
    /// </summary>
    public partial class LexemTable : Window
    {
        List<Lexem> LexemList { set; get; }
        List<Idnt> IdentifierList { get; set; }
        List<Const> ConstantList { get; set; }

        public LexemTable(List<Lexem> lexemList, List<Idnt> identifierList, List<Const> constantList)
        {
            InitializeComponent();

            this.LexemList = lexemList;
            this.IdentifierList = identifierList;
            this.ConstantList = constantList;


            this.lexemGrid.ItemsSource = LexemList;
            this.identifierGrid.ItemsSource = IdentifierList;
            this.constantGrid.ItemsSource = ConstantList;

            

           
        }
    }
}
