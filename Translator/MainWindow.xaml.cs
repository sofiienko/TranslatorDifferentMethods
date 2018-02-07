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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Translator.LexicalAnalyser;
using Translator.LexicalAnalyser.DiagramOfState;
using Translator.LexicalAnalyser.FiniteStateMachine;
using Translator.SyntaxAnalyser;
using Translator.SyntaxAnalyser.AscendingAnalysis;
using Translator.SyntaxAnalyser.RecursiveDescentParser;

namespace Translator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            Paragraph paragraph = this.codeTextBox.Document.Blocks.FirstBlock as Paragraph;
            paragraph.LineHeight = 8;


            Console.SetOut(new MultiTextWriter(new ControlWriter(consoleBox), Console.Out));
            //Console.OutputEncoding = Encoding.UTF8;
        }


        List<string> GetListStringFromRichTextBox(RichTextBox rtb)
        {
            var document = rtb.Document;
            List<string> listString = new List<string>();

            foreach (var item in document.Blocks)
            {
                listString.Add(new TextRange(item.ContentStart, item.ContentEnd).Text);
            }

            return listString;
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            List<string> sourceCode =GetListStringFromRichTextBox(this.codeTextBox);

            ILexicalAnalyser analyser =new FiniteMachine(sourceCode);
            analyser.Analize();

            if(showLexemTable.IsChecked) new LexemTable(analyser.LexemList, analyser.IdentifierList, analyser.ConstantList).Show();


            ISyntaxAnalyser syntaxAnalyser = new AscendingAnalys();

            try
            {
                syntaxAnalyser.CheckSyntax(analyser.LexemList);
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+" "+ex.Source);
            }
}

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {

        }





    }
}
