using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Translator.LexicalAnalyser;

namespace Translator
{
   public class AdapterFromOldToNewModel
    {
        public List<Model.Lexem> ModelLexemList { get; set; } = new List<Model.Lexem>();

        public  AdapterFromOldToNewModel(List<Lexem> lexemList, List<Idnt> idntList, List<Const> constList)
        {
            int i = 0;
            foreach (var lexem in lexemList)
            {
                if (lexem.Code == (int)Model.TerminalCode.Constant|| lexem.Code==38)
                {
                    double Value = constList[lexem.IndexConst.Value]._Const;
                    Model.TerminalCode type = ConvertFromStrinToTerminalCode(constList[lexem.IndexConst.Value].Type);
                    uint number = (uint)i++;
                    uint numberInConstList = (uint)constList[lexem.IndexConst.Value].Index;
                    ModelLexemList.Add(new Model.Constant(Value, type, number, numberInConstList, (uint)lexem.Row));
                }
                else if (lexem.Code == (int)Model.TerminalCode.Identifier)
                {
                    string name = idntList[lexem.IndexIdnt.Value].Name;
                    double? value =null;


                    Model.TerminalCode type = ConvertFromStrinToTerminalCode(constList[lexem.IndexIdnt.Value].Type);
                    uint number = (uint)i++;
                    uint numberInIdntList = (uint)idntList[lexem.IndexIdnt.Value].Index;
                    ModelLexemList.Add(new Model.Identifier(name,value,type,number,numberInIdntList, (uint)lexem.Row));
                }
                else  ModelLexemList.Add(new Model.Lexem((uint)i++, (uint)lexem.Row, lexem.Substring, (Model.TerminalCode)lexem.Code));

            }
        }

            private Model.TerminalCode ConvertFromStrinToTerminalCode(string typeString)
            {
                Model.TerminalCode typeTerminalCode;
                switch (typeString)
                {
                    case "int":
                        typeTerminalCode = Model.TerminalCode.Int;
                        break;
                    case "float":
                        typeTerminalCode = Model.TerminalCode.Float;
                        break;
                    case "unsigned int":
                        typeTerminalCode = Model.TerminalCode.Unsigned & Model.TerminalCode.Int;
                        break;
                    case "unsigned float":
                        typeTerminalCode = Model.TerminalCode.Unsigned & Model.TerminalCode.Float;
                        break;
                    default: throw new Exception($"Can`t covert type {typeString}  from string to TerminalCOde ");
                }
                return typeTerminalCode;
            }
        }
    }

