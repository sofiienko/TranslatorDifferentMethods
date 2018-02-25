using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Translator.LexicalAnalyser;
using Translator.LexicalAnalyser.FiniteStateMachine;
using Translator.SyntaxAnalyser;
using Translator.SyntaxAnalyser.AscendingAnalysis;

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

            Const.AllConstFromCode = analyser.ConstantList;
            Idnt.AllIdnFromCode = analyser.IdentifierList;
            if(showLexemTable.IsChecked) new LexemTable(analyser.LexemList, analyser.IdentifierList, analyser.ConstantList).Show();




            ISyntaxAnalyser syntaxAnalyser = new AscendingAnalys();
            AdapterFromOldToNewModel adapter = new AdapterFromOldToNewModel(analyser.LexemList,analyser.IdentifierList, analyser.ConstantList );


            //try
            //{
                syntaxAnalyser.CheckSyntax(adapter.ModelLexemList);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message+" "+ex.Source);
            //}
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
