﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Model
{
    class Lexem:Terminal
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
}
