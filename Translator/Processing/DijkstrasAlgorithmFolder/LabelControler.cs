using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.Model;

namespace Translator.Processing.DijkstrasAlgorithm
{
    public class LabelControler
    {

        private int cursor = -1;
        List<ILabel> arrayLabels = new List<ILabel>();


        public LabelControler()
        {
            arrayLabels = new List<ILabel>();
        }

        public Label GetNewLabel(int position)
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
