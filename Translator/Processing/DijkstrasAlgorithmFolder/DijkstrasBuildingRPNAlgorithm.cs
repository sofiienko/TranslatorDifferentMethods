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
            return "CTM";
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

}
