using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Translator
{
    public class ControlWriter : TextWriter
    {
        private TextBox textbox;
        public ControlWriter(TextBox textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            textbox.Text += value;
        }

        public override void Write(string value)
        {
            textbox.Text += value;
        }
        public new   void WriteLine(string value)
        {
            textbox.Text += value+"\n";
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }

    public class ControlReader : TextReader
    {
        private string oldText = "";
        private TextBox textbox;

        private bool pressedEnter = false;
        public ControlReader(TextBox textbox)
        {
            this.textbox = textbox;
            textbox.PreviewKeyDown += Textbox_PreviewKeyDown;
        }

        public override string ReadLine()
        {
            //WaitBeforePressEnter();
            string temp = textbox.Text;
            temp = temp.Skip(oldText.Length).ToString();
            oldText = textbox.Text;
            return temp ;
        }
        private bool WaitBeforePressEnter()
        {
            string text = textbox.Text;

            while (pressedEnter == false)
            {
                textbox.Text += ":";
                
                Thread.Sleep(500);
                textbox.Text = text;
            }
            pressedEnter = false;

            return true;
        }
        private void Textbox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               
            }
        }
    }




    public class MultiTextWriter : TextWriter
    {
        private IEnumerable<TextWriter> writers;
        public MultiTextWriter(IEnumerable<TextWriter> writers)
        {
            this.writers = writers.ToList();
        }
        public MultiTextWriter(params TextWriter[] writers)
        {
            this.writers = writers;
        }

        public override void Write(char value)
        {
            foreach (var writer in writers)
                writer.Write(value);
        }

        public override void Write(string value)
        {
            foreach (var writer in writers)
                writer.Write(value);
        }

        public override void Flush()
        {
            foreach (var writer in writers)
                writer.Flush();
        }

        public override void Close()
        {
            foreach (var writer in writers)
                writer.Close();
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }

    //public class MultiTextReader : TextReader
    //{
    //    private IEnumerable<TextReader> readers;
    //    public MultiTextReader(IEnumerable<TextReader> readers)
    //    {
    //        this.readers = readers.ToList();
    //    }
    //    public MultiTextReader(params TextReader[] readers)
    //    {
    //        this.readers = readers;
    //    }

    //    public override void Read(char value)
    //    {
    //        foreach (var writer in readers)
    //            writer.Write(value);
    //    }

    //    public override void Write(string value)
    //    {
    //        foreach (var writer in readers)
    //            writer.Write(value);
    //    }
    //    public override string ReadLine()
    //    {
    //        foreach (var writer in readers)
    //            writer.Write(value);
    //    }

    //}
}
