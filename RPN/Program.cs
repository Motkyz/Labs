using System;
using System.Collections.Generic;
using System.Linq;

public class Token
{

}

public class Parenthesis : Token
{
    public char par;
}

public class Number : Token
{
    public double num;
}

public class Operation : Token
{
    public char oper;
    public static int GetPriority(char oper)
    {
        return oper switch
        {
            ('+') => 1,
            ('-') => 1,
            ('*') => 2,
            ('/') => 2,
            ('(') => 0,
            (')') => 0,
            _ => 3,
        };
    }

    public static double CalculateRPN(List<Token> listRPN)
    {
        Stack<double> result = new Stack<double>();

        foreach (Token token in listRPN) 
        {
            if (token is Number number)
            {
                result.Push(number.num);
            }

            else if (token is Operation operation)
            {
                double secondNumber = result.Pop();
                double firstNumber = result.Pop();
                result.Push(Calculate(firstNumber, secondNumber, operation.oper));
            }
        }

        double answer = result.Pop();
        return answer;
    }

    public static double Calculate(double firstNumber, double secondNumber, char oper)
    {
        switch (oper)
        {
            case ('+'): return firstNumber + secondNumber;
            case ('-'): return firstNumber - secondNumber;
            case ('*'): return firstNumber * secondNumber;
            case ('/'): return firstNumber / secondNumber;
            default: return double.NaN;
        }
    }
}

class Program
{
    public static void Main()
    {
        Console.Write("Введите выражение: ");
        string expression = Console.ReadLine();//"421 * (2 / (423 + 1) - 4) * 2";
        expression = expression.Replace(" ", string.Empty);
        Console.WriteLine($"Ваше выражение: {expression}\n");
        List<Token> tokens = GetTokensList(expression);
        List<Token> rpn = DoRPN(tokens);
        ToShowRPN(rpn);
        double answer = Operation.CalculateRPN(rpn);
        Console.WriteLine($"Ответ: {answer}");
    }

    public static List<Token> GetTokensList(string expression)
    {
        List<Token> tokensList = new List<Token>();

        string number = string.Empty;

        foreach (char symbol in expression)
        {
            if (Char.IsDigit(symbol))
            {
                number += symbol;
            }

            else if (symbol == ',' || symbol == '.')
            {
                number += ',';
            }

            else if (symbol == '(')
            {
                tokensList.Add(new Parenthesis() { par = symbol });
            }

            else
            {
                if (number != string.Empty)
                {
                    tokensList.Add(new Number() { num = double.Parse(number) });
                }

                if (symbol == ')')
                {
                    tokensList.Add(new Parenthesis() { par = symbol });
                    number = string.Empty;
                }

                else
                {
                    tokensList.Add(new Operation() { oper = symbol });
                    number = string.Empty;
                }
            }

        }

        if (number != string.Empty)
        {
            tokensList.Add(new Number() { num = double.Parse(number) });
        }

        return tokensList;
    }

    public static List<Token> DoRPN(List<Token> tokensList)
    {
        Stack<Token> opers = new Stack<Token>();
        List<Token> rpn = new List<Token>();

        foreach (var token in tokensList)
        {
            if (token is Number)
            {
                rpn.Add(token);
            }

            else if (token is Parenthesis parenthesis)
            {

                if (parenthesis.par == '(')
                {
                    opers.Push(parenthesis);
                }

                else if (parenthesis.par == ')')
                {
                    while (!(opers.Peek() is Parenthesis))
                    {
                        rpn.Add(opers.Pop());
                    }
                    opers.Pop();
                }
            }

            else if (token is Operation operFromList)
            {
                if (opers.Count != 0 && opers.Peek() is Operation operFromStack)
                {
                    opers.Pop();
                    if (Operation.GetPriority(operFromStack.oper) >= Operation.GetPriority(operFromList.oper))
                    {
                        rpn.Add(operFromStack);
                        opers.Push(operFromList);
                    }
                    else opers.Push(operFromList);
                }
                else opers.Push(operFromList);
            }
        }

        while (opers.Count != 0)
        {
            rpn.Add(opers.Pop());
        }

        return rpn;
    }

    public static void ToShowRPN(List<Token> rpn)
    {
        Console.WriteLine("Ваше выражение в обратной польской записи: ");
        foreach (Token obj in rpn)
        {
            if (obj is Number number)
            {
                Console.Write($"{number.num} ");
            }
            if (obj is Operation operation)
            {
                Console.Write($"{operation.oper} ");
            }
        }
        Console.WriteLine("\n");
    }
}