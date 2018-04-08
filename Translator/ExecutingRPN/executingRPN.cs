using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;
using Translator.Processing;

namespace Translator.ExecutingRPN
{
    interface IResult { }

    public class BoolResult:IResult
    {
        public bool Result { get; set; }

        public BoolResult(bool result)
        {
            this.Result = result;
        }
    }

    public class IntResult : IResult
    {
        public int Result { get; set; }

        public IntResult(int result)
        {
            this.Result = result;
        }
    }

    public class FloatResult : IResult
    {
        public double Result { get; set; }

        public FloatResult(double result)
        {
            this.Result = result;
        }
    }



    public class ExecutingRPN
    {

        List<IRPNElement> inputList;
        private int cursor;
        public void ExecuteRPN(List<IRPNElement> inputList )
        {
            this.inputList = inputList;
            this.cursor = 0;
        }


        public void Execute()
        {

        }


    }
}
