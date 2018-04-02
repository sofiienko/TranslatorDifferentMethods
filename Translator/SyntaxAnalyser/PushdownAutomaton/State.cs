using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.StackedAutomatic
{

    struct Pair
    {
        public int key;
        public State value;
        public Pair(int key, State value)
        {
            this.key = key;
            this.value = value;
        }
    }

    //class State
    //{
    //    public int Number { get; private set; }
    //    StackedAutomatic stackedAutomatic;
    //    Dictionary<int, State> map;

    //    public Action Equal { get; set; }
    //    public Action NotEqual { get; set; }

    //    public State(StackedAutomatic stackedAutomatic,int number,Action equal,Action notEqual)
    //    {
    //        this.stackedAutomatic = stackedAutomatic;
    //        this.Number = number;
    //        this.Equal = equal;
    //        this.NotEqual = notEqual;
    //    }

    //    public void AddReference(params Pair[] dictionary)
    //    {
    //        foreach (var item in dictionary)
    //            this.map.Add(item.key, item.value);
    //    }


    //    public void Execute()
    //    {

    //    }
    //}
}
