using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Translator.StackedAutomatic
{

    class StackedAutomatic
    {
        public  List<Snap> listSnaps = new List<Snap>();
        List<translator.Lexem> lexemList;
        State main;
        IState[] logicalExpression;
        IState[] @operator;
        IState[] operatorList;
        IState[] expression;
        bool isCodeGood = true;

        public StackedAutomatic(List<translator.Lexem> lexemList)
        {
            this.lexemList = lexemList;
        }

        public void NotEqual()
        {
            //a small Workaround
            if (lexemList[State.counterLexem].Code!=10
                && lexemList[State.counterLexem].Code != 33
                && lexemList[State.counterLexem].Code != 3)
            {
                MessageBox.Show("Unexpected lexem № " + State.counterLexem);
                isCodeGood = false;
            }

            try
            {
                State nextState = State.Stack.Pop();
                if (nextState != null) nextState.Execute();
                else throw new Exception("Stack  is empty");
            }
            catch (Exception ex)
            {
                try
                { 
                    if (lexemList[State.counterLexem].Code == 2&& isCodeGood==true) MessageBox.Show("You wrote great code! :)");
                    else MessageBox.Show("Oop,we have problem in " + State.counterLexem + " lexem");
                }
                catch(System.ArgumentOutOfRangeException)
                {
                    MessageBox.Show("The program failed. Exit beyond the array. You may have forgotten about 'end'");
                }
                

            }
        }

        public void Equal()
        {
            State nextState = State.Stack.Pop();
            
            if (nextState != null) nextState.Execute();
            else throw new Exception("Stack  is empty");
        }

        public void NotEqualExit()
        {
            State nextState = State.Stack.Pop();
            if (nextState != null) nextState.Execute();
            else throw new Exception("Stack  is empty");
        }
         public void NotEqualMoveThen()
        {
            State.Stack.Push((State)@operator[64]);
            ((State)operatorList[10]).Execute();
          

        }

        public void NotEqualMoveLogical()
        {
            State.Stack.Push((State)logicalExpression[91]);
            State.counterLexem++;
            NotEqualExit();

        }


       public void NotEqualMoveToExpression25()
        {
            State.Stack.Push((State)@operator[25]);
          //  State.counterLexem++;
            ((State)expression[70]).Execute();
            
        }
       public void  NotEqualMoveToExpression91()
        {
            State.Stack.Push((State)logicalExpression[91]);
            // State.counterLexem++;
            ((State)expression[70]).Execute();

        }
        public void Execute()
        {
            Initialization();
            State.Lexemlist = lexemList;//.GetEnumerator() ;
            State.Stack = new Stack<State>();
            State.SnapList = listSnaps;
            main.Execute();
        }

        public void Initialization()
        {

            IState[] main = new IState[6];
          


             operatorList = new IState[12];
             @operator = new IState[68];
             expression = new State[73];
            IState[] idnList = new IState[82];
            logicalExpression = new IState[94];

           //----------------------------------------------------------------

            main[1] = new State(1, NotEqual, null);
            this.main = (State)main[1];
            main[2] = new State(2, NotEqual, null);
            main[3] = new State(3, NotEqual, null);
            main[4] = new State(4, NotEqual, null);
            main[5] = new State(5, NotEqual, Equal, isEnd: true);

            operatorList[10] = new State(10, NotEqual, null);
            operatorList[11] = new State(11, NotEqual, null,isEnd:true);

            // @operator[21] = new State(21, NotEqual, null);
            @operator[21] = new State(21, NotEqualExit, null);
            @operator[22] = new State(22, NotEqual, null);
            @operator[23] = new State(23, NotEqual, null);
            @operator[24] = new State(24, NotEqual, null);
            @operator[25] = new State(25, NotEqualExit, null);
            @operator[26] = new State(26, NotEqualMoveToExpression25, null);
            @operator[27] = new State(27, NotEqual, null);
            @operator[28] = new State(28, NotEqual, null);
            @operator[29] = new State(29, NotEqual, null);
            @operator[30] = new State(30, NotEqualExit, null);

            @operator[40] = new State(40, NotEqual, null);
            @operator[41] = new State(41, NotEqual, null);
            @operator[42] = new State(42, NotEqual, null);
            @operator[50] = new State(50, NotEqual, null);
            @operator[51] = new State(51, NotEqual, null);
            @operator[52] = new State(52, NotEqual, null);
            @operator[53] = new State(53, NotEqual, null);
            @operator[54] = new State(54, NotEqual, null);
            @operator[55] = new State(55, null, Equal);
            @operator[60] = new State(60, NotEqual, null);
            @operator[61] = new State(61, NotEqual, null);
            @operator[62] = new State(62, NotEqual, null);
            @operator[64] = new State(64, NotEqual, null);
            @operator[65] = new State(65, null, Equal);

            expression[70] = new State(70, NotEqual, null);
            expression[72] = new State(72, NotEqual, null);
            expression[71] = new State(71, NotEqualExit, null);


            logicalExpression[90] = new State(90, NotEqualMoveToExpression91, null);
            logicalExpression[91] = new State(91, NotEqual, null);
            logicalExpression[92] = new State(92, NotEqualExit, null);
            logicalExpression[93] = new State(93, NotEqual, null);

            //--------------------------------------------------------------------
            main[1].AddReference(0, (State)main[2]);//program
            main[2].AddReference(34, (State)main[3]);//id           
            main[3].AddReference(3, (State)main[4]);//enter          
            main[4].AddReference(1, new MapValue(operatorList[10], main[5]));//begin          
            main[5].AddReference(2, null);//end

           
            operatorList[10].AddReference( 3, new MapValue(@operator[21], operatorList[11]));//enter            
            operatorList[11].AddReference(3, new MapValue(@operator[21], operatorList[11]));//enter

            
            @operator[21].AddReference(32, (State)@operator[22]);//unsigned
            @operator[21].AddReference(30, (State)@operator[23]);//int
            @operator[21].AddReference(31, (State)@operator[23]);//float
            @operator[21].AddReference(34, (State)@operator[24]);//id
            @operator[21].AddReference(4, (State)@operator[40]);//read
            @operator[21].AddReference(5, (State)@operator[40]);//read
            @operator[21].AddReference(6, (State)@operator[50]);//do
            @operator[21].AddReference(8, (State)@operator[60]);//if

            
            @operator[22].AddReference(31, (State)@operator[23]);//float     
            @operator[22].AddReference(30, (State)@operator[23]);//int           
            @operator[23].AddReference(34, (State)@operator[24]);//id  
            @operator[24].AddReference(36, (State)@operator[26]);//=
            @operator[25].AddReference(38,null);//  $ 
            @operator[26].AddReference(27, new MapValue(logicalExpression[90],@operator[27]));//[               
            @operator[27].AddReference(28, (State)@operator[28]);//]



            
            @operator[28].AddReference(20, new MapValue(expression[70], @operator[29]));//?           
            @operator[29].AddReference(29, new MapValue(expression[70], @operator[30]));//:           
            @operator[30].AddReference( 38, null);// $            
            @operator[40].AddReference(25, (State)@operator[41]);//(           
            @operator[41].AddReference(34, (State)@operator[42]);//id    
            @operator[42].AddReference(26, (State)@operator[25]);//) 
            @operator[42].AddReference(35, (State)@operator[41]);//id 

            @operator[50].AddReference(7, (State)@operator[51]);//while         
            @operator[51].AddReference(25, new MapValue(logicalExpression[90], @operator[52]));//(          
            @operator[52].AddReference(26, new MapValue(operatorList[11], @operator[55]));//)             
            //@operator[54].AddReference(3, (State)@operator[55]);//enter          
            @operator[55].AddReference(33, null);//enddo
            
            @operator[60].AddReference(25, new MapValue(logicalExpression[90], @operator[61]));//(         
            @operator[61].AddReference(26, (State)@operator[62]);//)          
            @operator[62].AddReference(9, new MapValue(operatorList[11], @operator[65]));//then     
                
            //@operator[64].AddReference(3, (State)@operator[65]);//enter
            @operator[65].AddReference(10, null);//fi




            expression[70].AddReference(34, (State)expression[71]);//id
            expression[70].AddReference(38, (State)expression[71]);//const
            expression[70].AddReference(25, new MapValue(expression[70], expression[72]));//(           
            expression[72].AddReference(26, (State)expression[71]);//)          
            expression[71].AddReference(21, (State)expression[70]);//+
            expression[71].AddReference(22, (State)expression[70]);//-
            expression[71].AddReference(23, (State)expression[70]);//*
            expression[71].AddReference(24, (State)expression[70]);// /

            
            logicalExpression[90].AddReference(13, (State)logicalExpression[90]);//not
            logicalExpression[90].AddReference(27, new MapValue(logicalExpression[90], logicalExpression[91]));//[

            
            logicalExpression[91].AddReference(17, new MapValue(expression[70], logicalExpression[92]));//
            logicalExpression[91].AddReference(15, new MapValue(expression[70], logicalExpression[92]));// <=
            logicalExpression[91].AddReference(19, new MapValue(expression[70], logicalExpression[92]));// ==
            logicalExpression[91].AddReference(14, new MapValue(expression[70], logicalExpression[92]));// !=
            logicalExpression[91].AddReference(18, new MapValue(expression[70], logicalExpression[92]));// >
            logicalExpression[91].AddReference(16, new MapValue(expression[70], logicalExpression[92]));// >=

            
            logicalExpression[92].AddReference(12, (State)logicalExpression[90]);//and
            logicalExpression[92].AddReference(11, (State)logicalExpression[90]);//or

            logicalExpression[93].AddReference(28, (State)logicalExpression[92]);// ]
        }
    }
}
