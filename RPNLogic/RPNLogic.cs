using static System.Runtime.InteropServices.JavaScript.JSType;


namespace RPNLogic
{
    public class Token
    {

    }

    public class Parenthesis : Token
    {
        public char Symbol;
        public bool IsClosing;

        public Parenthesis(char symbol)
        {
            Symbol = symbol;
            IsClosing = symbol == ')';
        }
    }

    public class Number : Token
    {
        public double Value;
        public bool IsArg = false;

        public Number(double value)
        {
            Value = value;
        }

        public Number(string str)
        {
            str = str.Replace('.', ',');
            Value = double.Parse(str);
        }

        public Number(char arg)
        {
            IsArg = true;
        }

        public static bool IsX(char symbol)
        {
            return symbol is 'x' or 'X' or 'х' or 'Х';
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static Number operator +(Number a, Number b)
        {
            return new Number(a.Value + b.Value);
        }

        public static Number operator -(Number a, Number b)
        {
            return new Number(a.Value - b.Value);
        }

        public static Number operator *(Number a, Number b)
        {
            return new Number(a.Value * b.Value);
        }

        public static Number operator /(Number a, Number b)
        {
            return new Number(a.Value / b.Value);
        }
    }

    

    public class RPNCalculator
    {
        public static List<Token> RPNList;
        public double Answer;

        public RPNCalculator(string expression)
        {
            RPNList = TransformToRPN(GetTokensList(expression));
        }

        public static List<Token> GetTokensList(string expression)
        {
            expression = expression.Replace(" ", string.Empty);

            List<Token> tokens = new List<Token>();
            string number = string.Empty;

            foreach (char symbol in expression)
            {
                if (Char.IsDigit(symbol) || symbol == ',' || symbol == '.')
                {
                    number += symbol;
                }
                else if (Char.IsLetter(symbol))
                {
                    if (Number.IsX(symbol)) 
                    {
                        tokens.Add(TokenCreator.Create(symbol));
                    }
                    else
                    {
                        number += symbol;
                    }
                }
                else
                {
                    if (number !=  string.Empty)
                    {
                        tokens.Add(TokenCreator.Create(number));
                    }
                    tokens.Add(TokenCreator.Create(symbol));

                    number = string.Empty;
                }
            }

            if (number != string.Empty)
            {
                tokens.Add(TokenCreator.Create(number));
            }

            return tokens;
        }

        public static List<Token> TransformToRPN(List<Token> tokensList)
        {
            Stack<Token> opers = new Stack<Token>();
            List<Token> rpn = new List<Token>();

            foreach (Token token in tokensList)
            {
                if (token is Number)
                {
                    rpn.Add(token);
                }
                else if (token is Parenthesis parenthesis)
                {
                    if (!parenthesis.IsClosing)
                    {
                        opers.Push(parenthesis);
                    }
                    else if (parenthesis.IsClosing)
                    {
                        while (opers.Peek() is not Parenthesis)
                        {
                            rpn.Add(opers.Pop());
                        }
                        opers.Pop();
                    }
                }
                else if (token is Operation listOp)
                {
                    if (opers.Count != 0 && opers.Peek() is Operation stackOp)
                    {
                        if (stackOp >= listOp)
                        {
                            rpn.Add(opers.Pop());
                            opers.Push(listOp);
                        }
                        else opers.Push(listOp);
                    }
                    else opers.Push(listOp);
                }
                else
                {
                    throw new Exception("Invalid Token");
                }
            }

            while (opers.Count != 0)
            {
                rpn.Add(opers.Pop());
            }

            return rpn;
        }

        public double Calculate(double XValue)
        {
            return CalculateRPN(XValue).Value;
        }

        public Number CalculateRPN(double XValue)
        {
            Stack<Number> result = new Stack<Number>();

            foreach (Token token in RPNList)
            {
                if (token is Number number)
                {
                    if (number.IsArg)
                    {
                        result.Push(new Number(XValue));
                    }
                    else result.Push(number);
                }
                else 
                {
                    Operation operation = token as Operation;
                    Number[] args = new Number[operation.ArgsCount];

                    for (int i = operation.ArgsCount - 1; i >= 0; i--)
                    {
                        args[i] = result.Pop();
                    }

                    result.Push(operation.Execute(args));
                }
            }

            return result.Pop();
        }
    }
}