using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.StackedAutomatic
{
    interface IState
    {
        void AddReference(int code, IMapValue state);
      //  void AddReference(int code, IState state, IState inStack);
    }
}
