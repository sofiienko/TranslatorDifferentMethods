using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Translator.LexicalAnalyser;
using Translator.LexicalAnalyser.FiniteStateMachine;
using Translator.Processing.DijkstrasAlgorithmFolder;
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


        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            if (codeTextBox.Document == null)
                return;

            TextRange documentRange = new TextRange(codeTextBox.Document.ContentStart, codeTextBox.Document.ContentEnd);
            documentRange.ClearAllProperties();



            TextPointer navigator = codeTextBox.Document.ContentStart;

            while (navigator.CompareTo(codeTextBox.Document.ContentEnd) < 0)
            {

                TextPointerContext context = navigator.GetPointerContext(LogicalDirection.Backward);

                if (context == TextPointerContext.ElementStart && navigator.Parent is Run)
                {
                    //CheckWordsInRun((Run)navigator.Parent);
                }
                navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);

            }

        }



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

            Const.AllConstFromCode = analyser.ConstantList;
            Idnt.AllIdnFromCode = analyser.IdentifierList;
            if(showLexemTable.IsChecked) new LexemTable(analyser.LexemList, analyser.IdentifierList, analyser.ConstantList).Show();




            ISyntaxAnalyser syntaxAnalyser = new AscendingAnalys();
            ISyntaxAnalyser syntaxAnalyser2 = new RecursiveDescent();
            AdapterFromOldToNewModel adapter = new AdapterFromOldToNewModel(analyser.LexemList,analyser.IdentifierList, analyser.ConstantList );


            //try
            //{

            syntaxAnalyser2.CheckSyntax(adapter.ModelLexemList);

            syntaxAnalyser.CheckSyntax(adapter.ModelLexemList);

            DijkstrasAlgorithm dijkstra = new DijkstrasAlgorithm();
            dijkstra.BuildRPN(adapter.ModelLexemList);

            //}
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message+" "+ex.Source);
            //    }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Open_Click(object sender, RoutedEventArgs e)
        {

            TextRange range;
            range = new TextRange(codeTextBox.Document.ContentStart, codeTextBox.Document.ContentEnd);

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
               range.Text = File.ReadAllText(openFileDialog.FileName);
        }
    }
}
