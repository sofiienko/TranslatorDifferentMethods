using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.StackedAutomatic
{
    interface IMapValue
    {
    }

    class MapValue:IMapValue
    {
        public State NextState { get; private set; }
        public State InStack { get; private set; }

        public MapValue(IState nextState, IState inStack)
        {
            this.NextState = (State)nextState;
            this.InStack = (State)inStack;
        }

        public MapValue(IState nextState)
        {
            this.NextState = InStack;
        }
    }

    class State : IState,IMapValue
    {
        public int Number { get; set; }
        public Dictionary<int, IMapValue> Map { get; set; }
        //  string transferingMarker;

        Action equal;
        Action notEqual;

        bool isEnd = false;
        
        public static List<Translator.Lexem> Lexemlist { get; set; }
        public static int counterLexem;
        public static Stack<State> Stack { get; set; }
        public static List<Snap> SnapList { get; set; }

        public State(int number, Action notEqual, Action equal, State inStack = null, bool isEnd = false)
        {
            counterLexem = -1;
            this.Map = new Dictionary<int, IMapValue>();
            this.Number = number;
            this.notEqual = notEqual;
            this.equal = equal;

            this.isEnd = isEnd;

        }

        //public State(int number, Action notEqual, Action equal, int code, IState state, IState inStack = null, bool isEnd = true)
        //{
        //    this.map = new Dictionary<int, IMapValue>();
        //    this.number = number;
        //    this.notEqual = notEqual;
        //    this.equal = equal;
        //    this.inStack = (State)inStack;
        //    this.isEnd = isEnd;

        //    AddReference(code, (State)state);
        //}


        //public void AddReference(int code, IState state, IState inStack)
        //{
        //    map.Add(code, (State)state);
        //}

        public void AddReference(int code, IMapValue state)
        {
            Map.Add(code, state);
        }

        public void Execute()
        {
            if (Lexemlist == null) throw new Exception("Oops, lexem list are empty:(");
            Translator.Lexem temp = Lexemlist[++counterLexem];
            SnapList.Add(new Snap(temp.Substring, Number, Stack.ToList()));
            Console.WriteLine("#"+Number);
            try
            {
               // State nextState = map[temp.Code];


                IMapValue nextIMap = Map[temp.Code];

                State nextState;
                if(nextIMap is  MapValue)
                {
                    MapValue mapValue = nextIMap as MapValue;
                    nextState = mapValue.NextState;
                    Stack.Push(mapValue.InStack);
                }
                else
                {
                    nextState = nextIMap as State;
                }

                if (nextState != null) nextState.Execute();
                else if(Map.Keys.Contains(temp.Code))
                {
                    //counterLexem--;
                    if (equal != null) equal.Invoke();
                    else throw new Exception("the value for this key does not exist");
                }
                else throw new Exception("Error");
            }
            catch (KeyNotFoundException)
            {
                counterLexem--;
                if (notEqual != null)
                    notEqual.Invoke();
                else throw new Exception("notEqual is empty");
            }

        }
    }
}
