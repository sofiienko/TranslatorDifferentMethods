using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;
using Translator.SyntaxAnalyser;

namespace Translator.Processing.DijkstrasAlgorithmFolder
{
    public delegate void WorkWithStack(Operator key = null);
    public class DijkstrasAlgorithm //: RPN
    {
        UniversalTableWindwos widowsDijkstrasAlgorithmSnap = new UniversalTableWindwos();
        UniversalTableWindwos widowslabelTable = new UniversalTableWindwos();
        List<DijkstrasAlgorithmSnap> snapList = new List<DijkstrasAlgorithmSnap>();

        OperatorRepository operatorRepo = new OperatorRepository();
        LabelControler labelControler = new LabelControler();

        //   List<IRPNElement> inputListLexems;
        List<Model.Lexem> inputListLexems;

        int cursor = 0;

        Stack<IOperator> stack = new Stack<IOperator>();
        public List<IRPNElement> OutputList { get; private set; } = new List<IRPNElement>();

        Dictionary<Operator, WorkWithStack> OperatorDictionary = new Dictionary<Operator, WorkWithStack>();


        /// <summary>
        /// Initialization OperatorDictionary
        /// </summary>
        public DijkstrasAlgorithm()
        {
            OperatorDictionary.Add(operatorRepo["("], OpenBracket);
            OperatorDictionary.Add(operatorRepo["["], OpenBracket);

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

            OperatorDictionary.Add(operatorRepo[")"], ClosingBracketSpec);
            OperatorDictionary.Add(operatorRepo["]"], ClosingBracket);

            //OperatorDictionary.Add(operatorRepo["do"], DoNothing);
            OperatorDictionary.Add(operatorRepo["while"], OperatorWhileNew);
            OperatorDictionary.Add(operatorRepo["enddo"], OperatorEndDoNew);

            OperatorDictionary.Add(operatorRepo["if"], OperatorIfNew);
            OperatorDictionary.Add(operatorRepo["then"], OperatorThenNew);
            OperatorDictionary.Add(operatorRepo["fi"], OperatorFi);

            OperatorDictionary.Add(operatorRepo["¶"], EndOfLine);

            OperatorDictionary.Add(operatorRepo["?"], TernarOperatorStart);
            OperatorDictionary.Add(operatorRepo[":"], TernarOperatorSecondPart);

            OperatorDictionary.Add(operatorRepo["read"], OperatorRead);
            OperatorDictionary.Add(operatorRepo["write"], OperatorWrite);
        }

        public void BuildRPN(List<Model.Lexem> lexemList)
        {

            this.inputListLexems = lexemList;//.Cast<IRPNElement>().ToList();

            for (; cursor < inputListLexems.Count; cursor++)
            {

                var temp = inputListLexems[cursor];// this row only for debug

                if (inputListLexems[cursor] is Constant || inputListLexems[cursor] is Link)
                {
                    OutputList.Add(inputListLexems[cursor]);
                }
                else
                {
                    //var _operator = operatorRepo[(inputListLexems[i] as Model.Lexem).Substring];
                    var _operator = operatorRepo[inputListLexems[cursor].Substring];

                    if (_operator == null) continue;

                    OperatorDictionary[_operator](_operator);
                }
                snapList.Add(new DijkstrasAlgorithmSnap(
                    inputListLexems.Skip(cursor + 1).Cast<IRPNElement>().ToList(),
                    stack,
                    OutputList));
            }

            widowsDijkstrasAlgorithmSnap.Table = snapList;
            widowsDijkstrasAlgorithmSnap.Show();

            widowslabelTable.Table = labelControler.ArrayLabels;
            widowslabelTable.Show();

        }



        public DijkstrasAlgorithm(List<Model.Lexem> lexemList)
        {
            this.inputListLexems = lexemList;//.Cast<IRPNElement>().ToList();
        }

        public void OpenBracket(Operator _operator)
        {
            stack.Push(_operator);
        }
        private void WorkWithStackDefault(Operator _operator)
        {
            if (stack.Count > 0)
                if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                    OutputList.Add((IRPNElement)stack.Pop());
            stack.Push(_operator);

        }

        /// <summary>
        /// WorkWithStackDefaultWithoutInserting in outputList
        /// </summary>
        /// <param name="_operator"></param>
        private void EndOfLine(Operator _operator)
        {
            if (stack.Count > 0)
            {
                while (stack.Count != 0 && stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                {
                    if (stack.Peek() is OperatorComponent component)
                    {
                        OutputList.Add(((Label)component[1]).SetPostion(OutputList.Count/* + 1*/));
                        stack.Pop();
                    }
                    else
                    {
                        OutputList.Add((IRPNElement)stack.Pop());
                    }
                }
            }

        }

        private void WorkWithStackDefaultWithoutInsert(Operator _operator)
        {
            if (stack.Count == 0) return;

            if (stack.Peek().СomparativePriority >= _operator.СomparativePriority)
                OutputList.Add((IRPNElement)stack.Pop());
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

            Operator closeBracket = _operator.Sign == ")" ? operatorRepo["("] : operatorRepo["["];
            while (stack.Count > 0 && stack.Peek() != closeBracket)
                OutputList.Add((IRPNElement)stack.Pop());
            stack.Pop();
        }


        private void ClosingBracketSpec(Operator _operator)
        {
            ClosingBracket(_operator);

            if (stack.Count > 0 && stack.Peek() is OperatorComponent op)
            {
                if (((Operator)op[0]) != operatorRepo["while"]) return;
                else if (op[1] != null && op[2] == null)
                {
                    op = (OperatorComponent)stack.Pop();
                    var label = labelControler.NewLabelLink();
                    OutputList.Add(label);
                    OutputList.Add(operatorRepo["CTbM"]);
                    stack.Push(new OperatorComponent(op[0], op[1], label));
                }
            }
        }

        private void DoNothing(Operator _operator) { }

        private void OperatorIf(Operator _operator)
        {
            var label = labelControler.NewLabelLink();
            OutputList.Add(label);
            OutputList.Add(operatorRepo["CTbM"]); // conditional transition by mistake

            stack.Push(new OperatorComponent(operatorRepo["if"], label));
        }


        private void OperatorThen(Operator _operator)
        {
        }


        private void OperatorFi(Operator _operator)
        {
            while (!(stack.Peek() is OperatorComponent))
                OutputList.Add((IRPNElement)stack.Pop());

            OutputList.Add(((Label)((stack.Pop() as OperatorComponent)
                .Components.LastOrDefault()))
                .SetPostion(OutputList.Count));
        }


        //  private int labelCounter = 0;
        private void OperatorWhile(Operator _operator)
        {
            var label1 = labelControler.NewLabel(OutputList.Count);
            OutputList.Add(label1);

            OutputList.Add(operatorRepo["CTbM"]); // conditional transition by mistake

            var label2 = labelControler.NewLabelLink();
            OutputList.Add(label2);


            stack.Push(new OperatorComponent(operatorRepo["while"], label1, label2));

        }
        private void OperatorEndDoNew(Operator _operator)
        {
            while (!(stack.Peek() is OperatorComponent))
                OutputList.Add((IRPNElement)stack.Pop());


            OperatorComponent fromStack = stack.Pop() as OperatorComponent;

            if (fromStack == null)
            {
                throw new Exception("oops, unexpected problem");
            }

            OutputList.Add(((Label)fromStack[1])/*.SetPostion(outputList.Count+1)*/);
            OutputList.Add(operatorRepo["UT"]);
            OutputList.Add(((Label)fromStack[2]).SetPostion(OutputList.Count/* + 1*/));
        }

        private void OperatorThenNew(Operator _operator)
        {
            var label = labelControler.NewLabelLink();
            OutputList.Add(label);
            OutputList.Add(operatorRepo["CTbM"]); // conditional transition by mistake

            //while (stack.Peek() != operatorRepo["if"])
            //{
            //    outputList.Add((IRPNElement)stack.Pop());
            //}

            while (!(stack.Peek() is OperatorComponent))
            {
                OutputList.Add((IRPNElement)stack.Pop());
            }

            stack.Pop();
            stack.Push(new OperatorComponent(operatorRepo["if"], label));

        }

        private void OperatorWhileNew(Operator _operator)
        {
            var label1 = labelControler.NewLabel(OutputList.Count);
            OutputList.Add(label1);

            stack.Push(new OperatorComponent(operatorRepo["while"], label1));

        }

        private void OperatorIfNew(Operator _operator)
        {
            //WorkWithStackDefaultWithoutInsert(operatorRepo["if"]);
            stack.Push(new OperatorComponent(operatorRepo["if"]));
        }

        private void OperatorRead(Operator _operator)
        {
            if (inputListLexems[++cursor].Substring != "(") throw new Exception($"After Read should be '(' lexem, row {inputListLexems[cursor - 1].Row}");

            while (inputListLexems[++cursor].Substring != ")")
            {
                if (inputListLexems[cursor] is Constant || inputListLexems[cursor] is Link)
                {
                    OutputList.Add(inputListLexems[cursor]);
                    OutputList.Add(operatorRepo["RD"]);

                    if (inputListLexems[cursor+1].Substring == ",") continue;
                }
            }
        }
        private void OperatorWrite(Operator _operator)
        {
            if (inputListLexems[++cursor].Substring != "(") throw new Exception($"After Read should be '(' lexem, row {inputListLexems[cursor - 1].Row}");

            while (inputListLexems[++cursor].Substring != ")")
            {
                if (inputListLexems[cursor] is Constant || inputListLexems[cursor] is Link)
                {
                    OutputList.Add(inputListLexems[cursor]);
                    OutputList.Add(operatorRepo["WT"]);

                    if (inputListLexems[cursor + 1].Substring == ",") continue;
                }
            }
        }

        private void TernarOperatorStart(Operator _operator)
        {
            var label = labelControler.NewLabel(OutputList.Count);

            OutputList.Add(label);
            OutputList.Add(operatorRepo["CTbM"]);

            stack.Push(new OperatorComponent(operatorRepo[":"], label));
        }


        private void TernarOperatorSecondPart(Operator _operator)
        {


            while (!(stack.Peek() is OperatorComponent))
            {
                OutputList.Add((IRPNElement)stack.Pop());
            }

            var label = labelControler.NewLabelLink();
            OutputList.Add(label);
            OutputList.Add(operatorRepo["UT"]);


            OperatorComponent oldComponent = (OperatorComponent)stack.Pop();

            ((Label)oldComponent[1]).SetPostion(OutputList.Count/* + 1*/);
            OutputList.Add(oldComponent[1]);

            stack.Push(new OperatorComponent(oldComponent[0], label, oldComponent[1]));
        }

        private void TernarOperatorEnd(Operator _operator)
        {
            WorkWithStackDefault(_operator);

            if (stack.Pop() is OperatorComponent component)
            {
                OutputList.Add(((Label)component[1]).SetPostion(OutputList.Count /*+ 1*/));
            }
            else throw new Exception("problem");
        }
    }

    public class DijkstrasAlgorithmSnap
    {
        public string Input { get; set; }
        public string Stack { get; set; }
        public string Output { get; set; }


        public DijkstrasAlgorithmSnap(List<IRPNElement> input, Stack<IOperator> stack, List<IRPNElement> output)
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

}
