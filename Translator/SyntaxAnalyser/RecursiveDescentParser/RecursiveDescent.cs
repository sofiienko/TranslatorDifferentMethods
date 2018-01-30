using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Translator.SyntaxAnalyser.RecursiveDescentParser
{

    class RecursiveDescent:ISyntaxAnalyser
    {
    public    List<Lexem> LexemList { get; set; }

        public static int currentLexem;

        public static SyntaxException[] massExeption;


        public bool CheckSyntax(List<Lexem> lexemList)
        {

            LexemList = lexemList;
            InitalizeException();
            InitalizeSyntaxis();

            return true;
        }

        private void InitalizeException()
        {
            massExeption = new SyntaxException[]
            {
                new SyntaxException("Error 1. You miss 'progrma' "),
                new SyntaxException("Error 2. You miss name of program' "),
                new SyntaxException("Error 3. You miss enter  "),
                new SyntaxException("Error 4. You miss begin' "),
                new SyntaxException("Error 5. program must constist  at least one oparator   "),
                new SyntaxException("Error 6. You miss type "),
                new SyntaxException("Error 7. You miss 'end' "),
                new SyntaxException("Error 8. You miss 'identifier' "),
                new SyntaxException("Error 9. You miss '=' "),
                new SyntaxException("Error 10. You miss 'identifier'"),
                new SyntaxException("Error 11 You miss '(' "),
                new SyntaxException("Error 12. You miss 'listID'"),
                new SyntaxException("Error 13 You miss ')' "),
                new SyntaxException("Error 14 You miss 'while' "),
                new SyntaxException("Error 15 You miss logical expression "),
                new SyntaxException("Error 16 You miss 'then' "),
                new SyntaxException("Error 17 You miss ',' "),
                new SyntaxException("Error 18 You miss term "),
                new SyntaxException("Error 19 You miss lexem after + or - "),
                new SyntaxException("Error 20 You miss multiple "),
                new SyntaxException("Error 21 You wrong expression "),
                new SyntaxException("Error 22 You miss lexem after * or / "),
                new SyntaxException("Error 23 You miss 'enddo' "),
                new SyntaxException("Error 24 You miss 'fi' "),
                new SyntaxException("Error 25 You miss  logic term "),
                new SyntaxException("Error 26 You miss ']' "),
                new SyntaxException("Error 27 Invalid declaration of ternary operator "),
                new SyntaxException("Error 28 You forget about right part expression "),
                new SyntaxException("Error 29 You miss logical term "),
                new SyntaxException("Error 30 You missclogical multiplier ')' "),
            };
        }

        private void InitalizeSyntaxis()
        {
            Parallel progrma = new Parallel();
            Parallel enter = new Parallel();
            Parallel operatrList = new Parallel();
            Parallel operatr = new Parallel();

            Parallel listId = new Parallel();
            Parallel logicalExpression = new Parallel();
            Parallel logicalTerm = new Parallel();
            Parallel logicalMul = new Parallel();

            //new
            Parallel logicalbracket = new Parallel();

            Parallel ratio = new Parallel();
            Parallel signRatio = new Parallel();
            Parallel expression = new Parallel();
            Parallel term = new Parallel();
            Parallel mul = new Parallel();
            Parallel type = new Parallel();


            progrma.AddElement(new Сonsecutive("main progrm",
                new Unit(this, 0, massExeption[0]),//program
                new Unit(this, 34, massExeption[1]),//name program
                new Unit(this, enter, massExeption[2]),//enter
                new Unit(this, 1, massExeption[3]),//begin
                new Unit(this, enter, massExeption[2]),//enter
                new Unit(this, operatrList, massExeption[4]),//operator list
                                                             //              new Unit(this, enter, massExeption[2]),//enter
                new Unit(this, 2, massExeption[6])//end
            ));

            enter.AddElement(new AtLeastOne(new Unit(this, 3, null/*massExeption[2]*/), new Сonsecutive(new Unit(this, 3, null))));

            operatrList.AddElement(new AtLeastOne(
                new Unit(this, operatr, massExeption[4]),
                new Сonsecutive(new Unit(this, enter, massExeption[2]), new Unit(this, operatr, null))
                ));


            operatr.AddElement(new Сonsecutive("type  id = expression",
                                new Unit(this, type, null),
                                new Unit(this, 34, massExeption[7]),//id
                                new Unit(this, 36, massExeption[8]),//=
                                new Unit(this, expression, massExeption[9])//expression
                               ),
                               new Сonsecutive("id=expression",
                                new Unit(this, 34, null),//id
                                new Unit(this, 36, massExeption[8]),//=
                                new Unit(this, expression, massExeption[9])//expression
                                ),
                               new Сonsecutive("read(idntlis)",
                                   new Unit(this, 4, null),//read
                                   new Unit(this, 25, massExeption[10]),//(
                                   new Unit(this, listId, massExeption[11]),
                                   new Unit(this, 26, massExeption[12])
                               ),
                               new Сonsecutive("write(idntlis)",
                                   new Unit(this, 5, null),//write
                                   new Unit(this, 25, massExeption[10]),//(
                                   new Unit(this, listId, massExeption[11]),
                                   new Unit(this, 26, massExeption[12])//)
                               ),
                               new Сonsecutive(//do while
                                   new Unit(this, 6, null),//do
                                   new Unit(this, 7, massExeption[13]),//while
                                   new Unit(this, 25, massExeption[10]),//(
                                   new Unit(this, logicalExpression, massExeption[14]),//logical expression
                                   new Unit(this, 26, massExeption[12]),//)
                                   new Unit(this, enter, null),//enter
                                   new Unit(this, operatrList, massExeption[4]),//operator list
                                                                                // new Unit(this, enter, null),//enter
                                   new Unit(this, 33, massExeption[22])//enddo
                                   ),
                               new Сonsecutive(//if
                                   new Unit(this, 8, null),//if
                                   new Unit(this, 25, massExeption[10]),//(
                                   new Unit(this, logicalExpression, massExeption[14]),//logical expression
                                   new Unit(this, 26, massExeption[12]),//)
                                   new Unit(this, 9, massExeption[15]),//then
                                   new Unit(this, enter, null),
                                   new Unit(this, operatrList, massExeption[4]),//operator list
                                                                                // //  //new Unit(this, enter, null),  
                                   new Unit(this, 10, massExeption[23])//fi
                                   )//,
                               //new Сonsecutive( //ternar operator
                               //    new Unit(this, 28, massExeption[26]),//?
                               //    new Unit(this, 34, massExeption[26]),//idn
                               //    new Unit(this, 36, massExeption[26]),//=
                               //    new Unit(this, 25, massExeption[10]),//(
                               //    new Unit(this, logicalExpression, massExeption[14]),//logical expression
                               //    new Unit(this, 26, massExeption[12]),//)
                               //    new Unit(this, 20, massExeption[26]),//?
                               //    new Unit(this, expression, massExeption[26]),//expresion
                               //    new Unit(this, 29, massExeption[26]),//:
                               //    new Unit(this, expression, massExeption[26])//expresion
                               //    )
                                   );

            listId.AddElement(new AtLeastOne(//id ,id
                new Unit(this, 34, massExeption[7]), new Сonsecutive(new Unit(this, 35, null/*massExeption[16]*/), new Unit(this, 34, massExeption[7]))
                ));

            expression.AddElement(// term or expr+ter or expr-term
                                new AtLeastOne(new Unit(this, term, null),
                                new Сonsecutive(new Unit(this, 21, null), new Unit(this, expression, massExeption[18])),
                                new Сonsecutive(new Unit(this, 22, null), new Unit(this, expression, massExeption[18]))
                ));


            term.AddElement(// term or mul*ter or mul/term
                    new AtLeastOne(new Unit(this, mul, null),
                    new Сonsecutive(new Unit(this, 23, null), new Unit(this, term, massExeption[21])),
                    new Сonsecutive(new Unit(this, 24, null), new Unit(this, term, massExeption[21]))
                ));


            //multiple
            mul.AddElement(new Unit(this, 34, null), new Unit(this, 38, null),//idn const
                           new Сonsecutive(new Unit(this, 25, null), new Unit(this, expression, massExeption[14]), new Unit(this, 26, null /*massExeption[12]*/))
                    );


            //type
            type.AddElement(new Unit(this, 30, null), new Unit(this, 31, null),
                            new Сonsecutive(new Unit(this, 32, null), new Unit(this, 30, null)),//unsigned int
                            new Сonsecutive(new Unit(this, 32, null), new Unit(this, 31, null)));//insgned float



            logicalExpression.AddElement(new AtLeastOne(//logical term {or logical term}
                                            new Unit(this, logicalTerm, null),
                                            new Сonsecutive(new Unit(this, 11, null), new Unit(this, logicalExpression, massExeption[28])))
                                         );

            logicalTerm.AddElement(new AtLeastOne(//logical mul {and logical mul}
                                        new Unit(this, logicalMul, null),
                                        new Сonsecutive(new Unit(this, 12, null), new Unit(this, logicalTerm, massExeption[29])))
                                   );


            //logicalMul.AddElement(new AtLeastOne(
            //        new Unit(this, ration, null),
            //        new Parallel(new Unit(this, 13, null),new Unit(this,logicalMul,null)),//not logic mul
            //        new Parallel(new Unit(this, 27, null), new Unit(this, logicalTerm, massExeption[24]),new Unit(this, 28, massExeption[25]))//[logic term]
            //    ));

            logicalMul.AddElement(
                                  new Сonsecutive(new Unit(this, 13, null), new Unit(this, logicalExpression, null)),//not logic mul
                                  new Сonsecutive(new Unit(this, 27, null), new Unit(this, logicalExpression, massExeption[24]), new Unit(this, 28, massExeption[25])),//[logic term]
                                  new Unit(this, ratio, null)
                                  );

            //logicalMul.AddElement(new Parallel(
            //                        new Unit(this, 13, null), new Unit(this, logicalbracket, null)),
            //                        new Unit(this, ratio, null),
            //                        new Unit(this, logicalbracket, null));

            //logicalbracket.AddElement(
            //                          new Parallel(new Unit(this, 27, null), new Unit(this, logicalExpression, massExeption[24]), new Unit(this, 28, massExeption[25])),//[logic expression]
            //                          new Unit(this, logicalExpression, null)
            //                          );

            ratio.AddElement(new Сonsecutive(
                                    new Unit(this, expression, null),
                                    new Unit(this, signRatio, null),
                                    new Unit(this, expression, null))
                              );

            //signRatio
            signRatio.AddElement(new Unit(this, 14, null),//!=
                                  new Unit(this, 15, null),//<=
                                  new Unit(this, 16, null),//>=
                                  new Unit(this, 17, null),//<
                                  new Unit(this, 18, null),//>
                                  new Unit(this, 19, null)//==
                                  );




            RecursiveDescent.currentLexem = 0;
            progrma.Execute();
            //logicalExpression.Execute();
            //logicalMul.Execute();
        }

    }
}
