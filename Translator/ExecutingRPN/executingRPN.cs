using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;
using Translator.Processing;
using Translator.Model;
namespace Translator.ExecutingRPN
{

    public class BoolType:IRPNElement
    {
        public bool Value { get; set; }
        public BoolType(bool result)
        {
            this.Value = result;
        }
    }
    public class DigitType : IRPNElement
    {
        public double Value { get; set; }
        public DigitType(double result)
        {
            this.Value = result;
        }
    }
    public class ExecutingRPN
    {
        Stack<IRPNElement> stack = new Stack<IRPNElement>();
        List<IRPNElement> inputList;
        private int i;
        public  ExecutingRPN(List<IRPNElement> inputList )
        {
            this.inputList = inputList;
            this.i = 0;
        }

        public void Execute()
        {
            
            for (; i < inputList.Count; i++)
            {
                if (inputList[i] is Model.Link) stack.Push(inputList[i]);
                else if (inputList[i] is Model.Constant) stack.Push(inputList[i]);
                else if (inputList[i] is Model.Label label)
                {
                    if(label.Position.Value!=i)stack.Push(inputList[i]);
                }
                else if (inputList[i] is Operator oprator)
                {

                    IRPNElement resultOperation = null;

                    if (oprator.Sign == "*") resultOperation = Multiple(GetFromStackDigitValue(), GetFromStackDigitValue());
                    else if (oprator.Sign == "/") resultOperation = Devide(GetFromStackDigitValue(), GetFromStackDigitValue());
                    else if (oprator.Sign == "+") resultOperation = Plus(GetFromStackDigitValue(), GetFromStackDigitValue());
                    else if (oprator.Sign == "-") resultOperation = Minus(GetFromStackDigitValue(), GetFromStackDigitValue());

                    else if (oprator.Sign == "and") resultOperation = And(GetFromStackBoolValue(), GetFromStackBoolValue());
                    else if (oprator.Sign == "or") resultOperation = Or(GetFromStackBoolValue(), GetFromStackBoolValue());
                    else if (oprator.Sign == "not") resultOperation = Not(GetFromStackBoolValue());

                    else if (oprator.Sign == ">") resultOperation = More(GetFromStackDigitValue(), GetFromStackDigitValue());
                    else if (oprator.Sign == "<") resultOperation = Less(GetFromStackDigitValue(), GetFromStackDigitValue());
                    else if (oprator.Sign == ">=") resultOperation = MoreEqual(GetFromStackDigitValue(), GetFromStackDigitValue());
                    else if (oprator.Sign == "<=") resultOperation = LessEqual(GetFromStackDigitValue(), GetFromStackDigitValue());
                    else if (oprator.Sign == "==") resultOperation = Equal(GetFromStackDigitValue(), GetFromStackDigitValue());

                    else if (oprator.Sign == "RD")
                    {
                        Link link = stack.Pop() as Link;
                        if (link == null) throw new Exception($"parameter in Read must be Link type ");

                        try
                        {
                            link.idntObject.Value = Double.Parse(Console.ReadLine());
                            stack.Push(link);
                        }
                        catch (FormatException)
                        {
                            throw new Exception("Inputed value isn`t digit");
                        }

                    }
                    else if (oprator.Sign == "WT")
                    {
                        var value = stack.Pop();
                        if (value is Link link)
                        {
                            Console.WriteLine(link.Value == null ? "null" : link.Value.Value.ToString());
                        }
                        else if (value is Constant cons)
                        {
                            Console.WriteLine(cons.Value);
                        }
                    }

                    else if (oprator.Sign == "UT")
                    {
                        var value = stack.Pop();
                        if (value is Label l)
                        {
                            if (i != l.Position.Value)
                            {
                                i = l.Position.Value;
                            }
                        }
                        else throw new Exception("Problem with uncoditional  transmit");
                    }
                    else if (oprator.Sign == "CTbM")
                    {
                        var value1 = stack.Pop();
                        var value2 = stack.Pop();

                        //todo:here maybe problem
                        if (value1 is Label l && value2 is BoolType b)
                        {
                            if (i != l.Position.Value && b.Value == false)
                            {
                                i = l.Position.Value;
                            }
                        }
                        else throw new Exception("Problem with coditional  transmit by mistake");
                    }

                    else if (oprator.Sign == "=")
                    {
                        var value1 = stack.Pop();
                        var value2 = stack.Pop();
                        if (value2 is Link link)
                        {
                            if (value1 is Constant cons) link.idntObject.Value = cons.Value;
                            else if (value1 is DigitType d) link.idntObject.Value = d.Value;
                        }
                        else throw new Exception("Seems, we have a problem:  first operand isn`t link");
                    }
                    else continue;

                    if (resultOperation != null)
                    {
                        stack.Push(resultOperation);
                    }

                }
            }
            //if (stack.Count != 1) throw new Exception("Oops, problem with calcultation");
            //else return stack.Pop();
        }


       double GetFromStackDigitValue()
        {
            var value = stack.Pop();

            if (value is Constant c) return c.Value;
            else if (value is Link l) return l.Value.Value;
            else if (value is DigitType d) return d.Value;
            else throw new Exception("Can`t convert into constant ot Link value from stack");
        }


        bool GetFromStackBoolValue()
        {
            var value = stack.Pop();
            if (value is BoolType b) return b.Value;
            else throw new Exception("Can`t convert into Bool value from stack");
        }

        private DigitType Multiple(double operant2, double operant1)
        {
            return new DigitType(operant1 * operant2);
        }
        private DigitType Devide(double operant2, double operant1)
        {
            try
            {
                return new DigitType(operant1 / operant2);
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Dividing by zero");
                throw new Exception("Dividing by zero");
            }
        }
        private DigitType Plus(double operant2, double operant1)
        {
            return new DigitType(operant1 + operant2);
        }
        private DigitType Minus(double operant2, double operant1)
        { 
            return  new DigitType(operant1 - operant2);
        }

        private BoolType And(bool operand2, bool operand1)
        {
            return new BoolType(operand1 && operand2);
        }

        private BoolType Or(bool operand2, bool operand1)
        {
            return new BoolType(operand1 || operand2);
        }

        private BoolType Not(bool operand)
        {
            return new BoolType(!operand);
        }

        private BoolType More(double operant2, double operant1)
        {
            return new BoolType (operant1 > operant2);
        }
        private BoolType Less(double operant2, double operant1)
        {
            return new BoolType(operant1 < operant2);
        }

        private BoolType MoreEqual(double operant2, double operant1)
        {
            return new BoolType(operant1 >= operant2);
        }
        private BoolType LessEqual(double operant2, double operant1)
        {
            return new BoolType(operant1 <= operant2);
        }

        private BoolType Equal(double operant2, double operant1)
        {
            return new BoolType(operant1 == operant2);
        }

    }
}
