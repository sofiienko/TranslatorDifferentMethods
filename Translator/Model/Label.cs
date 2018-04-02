using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{ 
    /// 
    public interface ILabel { }

    /// <summary>
    /// link on place in code where should move
    /// </summary>

    public class LabelLink : IRPNElement,ILabel
    {
        public virtual int? Position
        {
            get
            {
                return null;
            }
        }
        public int NumberLabel { get; private set; }
        public LabelLink(int numberLabel)
        {
            NumberLabel = numberLabel;
        }
    }

    
    /// <summary>
    /// mark   position
    /// </summary>
    public class Label : LabelLink
    {
        private int? position;
        public  override int? Position
        {
            get
            {
                return position;
            }
        }
        public Label(int? position,int numberLabel):base(numberLabel)
        {
            this.position = position;
        }

        public Label SetPostion(int position)
        {
            this.position = position;
            return this;
        }
    }



    public class LabelControler
    {

        private int cursor = -1;

        List<ILabel> arrayLabels = new List<ILabel>();


        public LabelControler()
        {
            arrayLabels = new List<ILabel>();
        }

        public Label NewLabel(int position)
        {
            var label = new Label(position, arrayLabels.Count);

            arrayLabels.Add(label);

            cursor++;

            return label;
        }

        public LabelLink NewLabelLink()
        {
            var label = new Label(null, arrayLabels.Count);
            arrayLabels.Add(label);

            cursor++;

            return label;
        }

        public Label GetLabel()
        {
            return arrayLabels[cursor--] as Label;
        }

        public LabelLink GetLabelLink()
        {
            return arrayLabels[cursor--] as LabelLink;
        }

    }
}
