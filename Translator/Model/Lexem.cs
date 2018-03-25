using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
   public  interface IRPNElement { }


     public   class Lexem:Terminal,IRPNElement
        {
           public  uint Number { get; set; }
           public uint Row { get; set; } 

            public Lexem(uint number, uint row,string substring, TerminalCode code)
                : base(substring, code)
            {
                this.Number = number;
                this.Row = row;
            }
        }



    public class OperatorComponent : IRPNElement
    {
      public  IEnumerable<IRPNElement> Components { get; private set; }
    }

    /// <summary>
    /// mark  currnet position
    /// </summary>
    /// 
 //   public interface ILable { }
    public class Label : IRPNElement//,ILable
    {
        public int Position { get; private set; }
        public int NumberLabel { get; private set; }
        public Label(int position, int numberLabel)
        {
            Position = position;
            NumberLabel = numberLabel;
        }
    }

    /// <summary>
    /// link on place in code where should move
    /// </summary>
    public class LabelLink : IRPNElement//,ILable
    {
        public int LinkToPosition { get; private set; }
        public int NumberLabel { get; private set; }
        public LabelLink(int position, int numberLabel)
        {
            LinkToPosition = position;
            NumberLabel = numberLabel;
        }
        public LabelLink(Label label)
        {
            LinkToPosition = label.Position;
            NumberLabel = label.NumberLabel;
        }
    }

    public class LabelControler
    {
        List<Queue<Label>> arrayQueue = new List<Queue<Label>>();
        public LabelControler()
        {
            arrayQueue[0] = new Queue<Label>();
        }
       
        public Label GenerateNawLabel(int position)
        {
            var label = new Label(position, arrayQueue.Count);
            arrayQueue[0].Enqueue(label);
            return label;
        }

        public LabelLink GetLabelFormQueue()
        {
            return new LabelLink(arrayQueue[0].Dequeue());
        }

    }

    
}
