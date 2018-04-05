using Translator.Model;

namespace Translator.Processing
{

    public delegate void WorkWithStack(Operator key = null);
    

    /// <summary>
    /// conditional transition by mistake
    /// </summary>
    public class CTbM: IRPNElement
    {
        public override string ToString()
        {
            return " CTbM ";
        }
    }


    /// <summary>
    /// unconditional transition
    /// </summary>
    public class UT : IRPNElement
    {
        public override string ToString()
        {
            return "UT";
        }
        
    }

    /// <summary>
    /// write on screen
    /// </summary>
    public class WT : IRPNElement
    {
        public override string ToString()
        {
            return "WT";
        }

    }


    /// <summary>
    /// Read from console
    /// </summary>
    public class RD : IRPNElement
    {
        public override string ToString()
        {
            return "RD";
        }

    }

}
