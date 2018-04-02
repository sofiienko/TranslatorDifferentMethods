using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;
using Translator.SyntaxAnalyser;

namespace Translator.Processing.DijkstrasAlgorithmFolder
{
    public class DijkstrasAlgorithm //: RPN
    {
        UniversalTableWindwos widows = new UniversalTableWindwos();
        List<DijkstrasAlgorithmSnap> snapList = new List<DijkstrasAlgorithmSnap>();



        OperatorRepository operatorRepo = new OperatorRepository();
        LabelControler labelControler = new LabelControler();

        List<IRPNElement> inputListLexems;

        int currentPosInListLexems = 0;

        Stack<IOperator> stack = new Stack<IOperator>();
        List<IRPNElement> outputList = new List<IRPNElement>();

        Dictionary<Operator, WorkWithStack> OperatorDictionary = new Dictionary<Operator, WorkWithStack>();

        public DijkstrasAlgorithm()
        {
            OperatorDictionary.Add(operatorRepo["("], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["["], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo["+"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["-"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["*"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["/"], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo["or"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["and"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["not"], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo["<"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo[">"], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["<="], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo[">="], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["=="], WorkWithStackDefault);
            OperatorDictionary.Add(operatorRepo["!="], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo[")"], ClosingBracket);
            OperatorDictionary.Add(operatorRepo["]"], ClosingBracket);

            OperatorDictionary.Add(operatorRepo["do"], DoNothing);
            OperatorDictionary.Add(operatorRepo["while"], OperatorWhile);
            OperatorDictionary.Add(operatorRepo["enddo"], OperatorEndDo);

            OperatorDictionary.Add(operatorRepo["if"], OperatorIf);
            OperatorDictionary.Add(operatorRepo["then"], OperatorThen);
            OperatorDictionary.Add(operatorRepo["fi"], OperatorFi);

        }

        public void BuildRPN(List<Model.Lexem> lexemList)
        {

            this.inputListLexems = lexemList.Cast<IRPNElement>().ToList();
            foreach (var item in inputListLexems)
            {
                if (item is Constant || item is Link) outputList.Add(item);
                else
                {
                    var _operator = operatorRepo[(item as Model.Lexem).Substring];
                    if (_operator == null) continue;

                    OperatorDictionary[_operator](_operator);

                    snapList.Add(new DijkstrasAlgorithmSnap(item, stack, outputList.LastOrDefault()));

                }

                widows.Table = snapList;
                widows.Show();
            }


        }



        public DijkstrasAlgorithm(List<Model.Lexem> lexemList)
        {
            this.inputListLexems = lexemList.Cast<IRPNElement>().ToList();
        }


        private void WorkWithStackDefault(Operator _operator)
        {
            try
            {
                if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                    outputList.Add((IRPNElement)stack.Pop());
                stack.Push(_operator);
            }
            catch (InvalidOperationException)
            {
                stack.Push(_operator);
            }
        }
        private void ClosingBracket(Operator _operator)
        {
            //todo: I am not sure  here:(
            if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                outputList.Add((IRPNElement)stack.Pop());
        }

        private void DoNothing(Operator _operator) { }

        private void OperatorIf(Operator _operator)
        {
            var label = labelControler.NewLabelLink();
            outputList.Add(label);
            outputList.Add(new CTM()); // conditional transition by mistake

            stack.Push(new OperatorComponent(operatorRepo["if"], label));
        }
        private void OperatorThen(Operator _operator)
        {


        }
        private void OperatorFi(Operator _operator)
        {
            while (stack.Peek() is OperatorComponent)
                outputList.Add((IRPNElement)stack.Pop());

            outputList.Add(((Label)((stack.Pop() as OperatorComponent).Components.LastOrDefault())).SetPostion(outputList.Count));
               
        }


        private int labelCounter = 0;
        private void OperatorWhile(Operator _operator)
        {
            outputList.Add(labelControler.NewLabel(outputList.Count));

            outputList.Add(new CTM()); // conditional transition by mistake

            outputList.Add(labelControler.NewLabelLink());


            stack.Push(operatorRepo["while"]);

        }
        private void OperatorEndDo(Operator _operator)
        {


        }
    }

    public class DijkstrasAlgorithmSnap
    {
        public string Input { get; set; }
        public string Stack { get; set; }
        public string Output { get; set; }


       public DijkstrasAlgorithmSnap(IRPNElement input, Stack<IOperator>  stack,IRPNElement output)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in stack)
                s.Append(item);

            this.Stack = s.ToString();
            this.Input = input.ToString();
            this.Output = output.ToString();

        }

    }

}
