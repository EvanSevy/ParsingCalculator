using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//***************
// Evan Sevy
// Circa 2016
// 
// An app to calculate addition, subtraction, multiplication, division and parentheses
// Gets a string after using the gui calculator and runs it through some parsing calculations to get a valid result.
//***************

namespace Calculator_01
{
    public partial class Form1 : Form
    {
        static List<String> calculation = new List<String>();
        public Form1()
        {
            //Form1.ActiveForm.Width = 1600;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calculation.Add("1");
            rtbCalcText.Text += "1 ";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            calculation.Add("2");
            rtbCalcText.Text += "2 ";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            calculation.Add("3");
            rtbCalcText.Text += "3 ";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            calculation.Add("4");
            rtbCalcText.Text += "4 ";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            calculation.Add("5");
            rtbCalcText.Text += "5 ";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            calculation.Add("6");
            rtbCalcText.Text += "6 ";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            calculation.Add("7");
            rtbCalcText.Text += "7 ";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            calculation.Add("8");
            rtbCalcText.Text += "8 ";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            calculation.Add("9");
            rtbCalcText.Text += "9 ";
        }

        private void button0_Click(object sender, EventArgs e)
        {
            calculation.Add("0");
            rtbCalcText.Text += "0 ";
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            calculation.Add("+");
            rtbCalcText.Text += "+ ";
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            calculation.Add("-");
            rtbCalcText.Text += "- ";
        }

        private void multiplyButton_Click(object sender, EventArgs e)
        {
            calculation.Add("*");
            rtbCalcText.Text += "* ";
        }

        private void divisionButton_Click(object sender, EventArgs e)
        {
            calculation.Add("/");
            rtbCalcText.Text += "/ ";
        }

        private void leftParen_Click(object sender, EventArgs e)
        {
            calculation.Add("(");
            rtbCalcText.Text += "( ";
        }

        private void rightParen_Click(object sender, EventArgs e)
        {
            calculation.Add(")");
            rtbCalcText.Text += ") ";
        }

        private void buttonEquals_Click(object sender, EventArgs e)
        {
            if (validParens(calculation) == false)
            {
                MessageBox.Show("Invalid Calculation.  Parentheses Invalid.");
            } else {
                calculation = cleanCalculation(calculation);
                resultBox.Text = solveParen(calculation).ToString();
            }           
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            calculation.Clear();
            rtbCalcText.Text = "";
        }
    


//*********************************************
// Go from higher precedence to lower precedence (Parentheses, multipliction, addition, etc...)
// With parentheses, Find the first openning parentheses, then see if there's anymore open parentheses, until you find a closing parentheses 
    // Then check for as many closing parentheses as you have open parentheses
    // Successively solve each enclosed parentheses, returning a value from the calculation and putting it back into the line of the rest of the calculation
//*********************************************

// Just counts each type of paren, closing & openning, then makes sure they equal the same
        public static Boolean validParens(List<String> calculation)
        {
            int openParenCount = 0;
            int closeParenCount = 0;
            for (int i = 0; i < calculation.Count; i++)
            {
                if (calculation[i] == "(")
                {
                    openParenCount++;
                }
                else if (calculation[i] == ")")
                {
                    closeParenCount++;
                }
            }
            if ( (openParenCount > closeParenCount) || (closeParenCount > openParenCount) )
                return false;
            else
                return true;
        }
// Right now, this is making sure that multiplication is innacted on 'implicit' multiplication calculations 
// (ie. when a number is multiplied by the result of a paren calculation)
        public static List<String> cleanCalculation(List<String> theCalc)
        {
            for (int i = 0; i < theCalc.Count; ++i)
            {
                if (theCalc[i] == "(")
                {
                    if (i != 0)
                    {
                        String item = theCalc[i - 1];
                        if (item != "(" && item != "+" && item != "-" && item != "/" && item != "*")
                        {
                            calculation.Insert(i, "*");
                            i = 0;
                        }
                            
                    }
                }
                if (theCalc[i] == ")")
                {
                    if (i < theCalc.Count-1)
                    {
                        String item = theCalc[i + 1];
                        if (item != ")" && item != "+" && item != "-" && item != "/" && item != "*")
                        {
                            calculation.Insert(i+1, "*");
                            i = 0;
                        }

                    }
                }
            }
            return calculation;
        }

// Takes a List of operands & operators and successively pushes them onto a stack.  Once it finds a closing paren,
// it then goes backwards from there to find it's associated openning paren, popping off the stack until it's found.  When it does it calculates multiplication & division upon the values
// then it passes the remaining calculations to the plus/minus algorithm, taking care of those.  Finally passing the resulting value back to the original
// place where the paren was found, placing this resulting calculation in place of the paren. Then it continues to find other parens and calculate those
// in the same way until all calculations have been found correctly.  If there are calculatoin outside of a paren, those are handled last.
        public static Double solveParen(List<String> calcList)
        {
            Stack<String> parenCalc = new Stack<String>();
            int j = 0;       
                for (int i = 0; i < calcList.Count; ++i)
                {
                    j = 0;
                    int stackAmount = 0;
                    if (calcList[i] == ")")
                    {
                        j = i - 1;
                        while (calcList[j] != "(")
                        {
                            parenCalc.Push(calcList[j]);
                            ++stackAmount;
                            --j;
                        }
                        Double result = solveMultipleDivide(parenCalc, stackAmount);
                        calcList.RemoveRange(j, (i - j + 1));
                        calcList.Insert(j, result.ToString());
                        i = 0;
                    }
                }
                if (calcList.Count == 1)
                {
                    return Convert.ToDouble(calcList[0]);
                }
                else
                {
                    parenCalc.Clear();
                    for (int i = calcList.Count-1; i >= 0; --i)
                    {
                        parenCalc.Push(calcList[i]);
                    }
                        return solveMultipleDivide(parenCalc, calcList.Count);
                }           
        }
// Goes through the provided parentheses Stack, or algorithm snippet, and finds '*' and '/', calculates those calculations, 
// then passes this with what's left of the paren to the 'solvePlusMinus' function
        public static Double solveMultipleDivide(Stack<String> parenCalc, int stackAmount)
        {
            List<String> newResult = new List<String>();
     
// was originally using 'parenCalc.Count' (a Stack's Count), instead of 'stackAmount' (a List's Count).  I found that 'Count' on 'stacks' does not return a stable/valid value.
// Caused me a LOOONG problem! ... I mean, why would you have the 'Count' property if it's worthless!?  I guess it must be for something.
            for (int i = 0; i < stackAmount; i++)
            {                
                newResult.Add(parenCalc.Pop());              
            }
            for (int i = 0; i < newResult.Count; ++i)
            {
                if ((newResult[i] == "*") || (newResult[i] == "/")) {
                    Double aResult = 0;
                    if (newResult[i] == "*")
                    {
                        aResult = Convert.ToDouble(newResult[i-1]) * Convert.ToDouble(newResult[i+1]);
                    } 
                    else if (newResult[i] == "/") 
                    {
                        aResult = Convert.ToDouble(newResult[i-1]) / Convert.ToDouble(newResult[i+1]);
                    }
                    newResult.RemoveRange(i-1, 3);
                    newResult.Insert(i-1, aResult.ToString());
                    i = 0; 
                }
            }
            return solvePlusMinus(newResult); // ?????????????
        }
// Handles plus & minus calculations, and returns the resulting scalar back up the line of "order of operation" functions
        public static Double solvePlusMinus(List<String> nextResult)
        {
            for (int i = 0; i < nextResult.Count; ++i)
            {
                if ((nextResult[i] == "+") || (nextResult[i] == "-"))
                {
                    Double aResult = 0;
                    if (nextResult[i] == "+")
                    {
                        aResult = Convert.ToDouble(nextResult[i - 1]) + Convert.ToDouble(nextResult[i + 1]);
                    }
                    else if (nextResult[i] == "-")
                    {
                        aResult = Convert.ToDouble(nextResult[i - 1]) - Convert.ToDouble(nextResult[i + 1]);
                    }
                    nextResult.RemoveRange(i - 1, 3);
                    nextResult.Insert(i - 1, aResult.ToString());
                    i = 0;
                }
            }
            return Convert.ToDouble(nextResult[0]);
        }
    }
}