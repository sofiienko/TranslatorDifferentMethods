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
        UniversalTableWindwos widowsDijkstrasAlgorithmSnap = new UniversalTableWindwos();
        UniversalTableWindwos widowslabelTable = new UniversalTableWindwos();
        List<DijkstrasAlgorithmSnap> snapList = new List<DijkstrasAlgorithmSnap>();

        OperatorRepository operatorRepo = new OperatorRepository();
        LabelControler labelControler = new LabelControler();

        List<IRPNElement> inputListLexems;

        int currentPosInListLexems = 0;

        Stack<IOperator> stack = new Stack<IOperator>();
        List<IRPNElement> outputList = new List<IRPNElement>();

        Dictionary<Operator, WorkWithStack> OperatorDictionary = new Dictionary<Operator, WorkWithStack>();


        /// <summary>
        /// Initialization OperatorDictionary
        /// </summary>
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
            OperatorDictionary.Add(operatorRepo["="], WorkWithStackDefault);

            OperatorDictionary.Add(operatorRepo[")"], ClosingBracket);
            OperatorDictionary.Add(operatorRepo["]"], ClosingBracket);

            //OperatorDictionary.Add(operatorRepo["do"], DoNothing);
            OperatorDictionary.Add(operatorRepo["while"], OperatorWhile);
            OperatorDictionary.Add(operatorRepo["enddo"], OperatorEndDo);

            OperatorDictionary.Add(operatorRepo["if"], OperatorIf);
            OperatorDictionary.Add(operatorRepo["then"], OperatorThen);
            OperatorDictionary.Add(operatorRepo["fi"], OperatorFi);


            OperatorDictionary.Add(operatorRepo["¶"], WorkWithStackDefaultWithoutInsert);

        }

        public void BuildRPN(List<Model.Lexem> lexemList)
        {

            this.inputListLexems = lexemList.Cast<IRPNElement>().ToList();
            for (int i=0;i<inputListLexems.Count;i++) 
            {

                var temp = inputListLexems[i];
                if (inputListLexems[i] is Constant || inputListLexems[i] is Link) outputList.Add(inputListLexems[i]);
                else
                {
                    var _operator = operatorRepo[(inputListLexems[i] as Model.Lexem).Substring];
                    if (_operator == null) continue;

                    OperatorDictionary[_operator](_operator);
                }
                snapList.Add(new DijkstrasAlgorithmSnap(inputListLexems.Skip(i+1).ToList(), stack, outputList));
            }

            widowsDijkstrasAlgorithmSnap.Table = snapList;
            widowsDijkstrasAlgorithmSnap.Show();

            widowslabelTable.Table = labelControler.ArrayLabels;
            widowslabelTable.Show();

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

        /// <summary>
        /// WorkWithStackDefaultWithoutInserting in outputList
        /// </summary>
        /// <param name="_operator"></param>
        private void WorkWithStackDefaultWithoutInsert(Operator _operator)
        {
            if (stack.Count > 0)
            {
                if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                {
                    outputList.Add((IRPNElement)stack.Pop());
                }
            }
          
        }

        /// <summary>
        /// ) or ]
        /// </summary>
        /// <param name="_operator"></param>
        private void ClosingBracket(Operator _operator)
        {
            //todo: I am not sure  here:(
            //if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
            //    outputList.Add((IRPNElement)stack.Pop());

            while(stack.Peek()!= operatorRepo["("] && stack.Peek() != operatorRepo["["])
                outputList.Add((IRPNElement)stack.Pop());
            stack.Pop();
        }

        private void DoNothing(Operator _operator) { }

        private void OperatorIf(Operator _operator)
        {
            var label = labelControler.NewLabelLink();
            outputList.Add(label);
            outputList.Add(new CTbM()); // conditional transition by mistake

            stack.Push(new OperatorComponent(operatorRepo["if"], label));
        }
        private void OperatorThen(Operator _operator)
        {


        }
        private void OperatorFi(Operator _operator)
        {
            while (!(stack.Peek() is OperatorComponent))
                outputList.Add((IRPNElement)stack.Pop());

            outputList.Add(((Label)((stack.Pop() as OperatorComponent)
                .Components.LastOrDefault()))
                .SetPostion(outputList.Count));
               
        }


        private int labelCounter = 0;
        private void OperatorWhile(Operator _operator)
        {
            var label1 = labelControler.NewLabel(outputList.Count);
            outputList.Add(label1);

            outputList.Add(new CTbM()); // conditional transition by mistake

            var label2 = labelControler.NewLabelLink();
            outputList.Add(label2);


            stack.Push(new OperatorComponent(operatorRepo["while"],label1,label2));

        }
        private void OperatorEndDo(Operator _operator)
        {
            while (!(stack.Peek() is OperatorComponent))
                outputList.Add((IRPNElement)stack.Pop());


            OperatorComponent fromStack = stack.Pop() as OperatorComponent;

            if(fromStack==null)
            {
                throw new Exception("oops, unexpected problem");
            }

            outputList.Add(((Label)fromStack[1]).SetPostion(outputList.Count+1));
            outputList.Add(new UT());
            outputList.Add(((Label)fromStack[2]).SetPostion(outputList.Count + 1));
        }
    }

    public class DijkstrasAlgorithmSnap
    {
        public string Input { get; set; }
        public string Stack { get; set; }
        public string Output { get; set; }


       public DijkstrasAlgorithmSnap(List<IRPNElement> input, Stack<IOperator>  stack,List<IRPNElement> output)
        {
            StringBuilder s = new StringBuilder();
            foreach (var item in input)
                s.Append(item);
            this.Input = s.ToString();

            s = new StringBuilder();
            foreach (var item in stack)
                s.Append(item);
            this.Stack = s.ToString();


            s = new StringBuilder();
            foreach (var item in output)
                s.Append(item);
            this.Output = s.ToString();
        }

    }

    public static class ExtenisonForList
    {
        public static void Push<T>(this List<T> list, T element)
        {
            list.Insert(0, element);
        }

        public static T Pop<T>(this List<T> list)
        {
            T element = list[0];
            list.RemoveAt(0);
            return element;
        }
        public static T Peek<T>(this List<T> list)
        {
            T element = list[0];
            return element;
        }
    }

}
