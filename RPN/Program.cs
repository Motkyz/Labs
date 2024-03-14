using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;

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

    public Number(string str)
    {
        Value = double.Parse(str);
    }

    public Number(double value)
    {
        Value = value;
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

public class Operation : Token
{
    public char Symbol;
    public int Priority;

    public Operation(char symbol)
    {
        Symbol = symbol;
        Priority = GetPriority(symbol);
    }

    public override string ToString() 
    {
        return Symbol.ToString();
    }

    public static int GetPriority(char operation)
    {
        Dictionary<char, int> priorities = new()
        {
            {'+', 1 },
            {'-', 1 },
            {'*', 2 },
            {'/', 2 }
        };

        return priorities[operation];
    }

    public static bool operator >(Operation a, Operation b)
    {
        return (a.Priority > b.Priority);
    }

    public static bool operator <(Operation a, Operation b)
    {
        return (a.Priority < b.Priority);
    }

    public static bool operator >=(Operation a, Operation b)
    {
        return (a.Priority >= b.Priority);
    }

    public static bool operator <=(Operation a, Operation b)
    {
        return (a.Priority <= b.Priority);
    }

    public static bool operator ==(Operation a, Operation b)
    {
        return (a.Priority == b.Priority);
    }

    public static bool operator !=(Operation a, Operation b)
    {
        return (a.Priority != b.Priority);
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите выражение: ");
        string expression = Console.ReadLine();
        List<Token> tokens = GetTokensList(expression);
        List<Token> rpn = RPNCalculator.TransformToRPN(tokens);
        RPNCalculator.ShowRPN(rpn);
        Number answer = RPNCalculator.CalculateRPN(rpn);
        Console.WriteLine($"Ответ: {answer}");
    }

    static List<Token> GetTokensList(string expression)
    {
        expression = expression.Replace(" ", string.Empty);

        List<Token> tokensList = new List<Token>();
        string number = string.Empty;

        foreach (char symbol in expression)
        {
            if (Char.IsDigit(symbol) || symbol == ',')
            {
                number += symbol;
            }
            else
            {
                if (number != string.Empty)
                {
                    tokensList.Add(new Number(number));
                    number = string.Empty;
                }

                if (symbol == '(' || symbol == ')')
                {
                    tokensList.Add(new Parenthesis(symbol));
                }
                else
                {
                    tokensList.Add(new Operation(symbol));
                }
            }
        }

        if (number != string.Empty)
        {
            tokensList.Add(new Number(number));
        }

        return tokensList;
    }
}

class RPNCalculator
{
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
            else if (token is Operation operFromList)
            {
                if (opers.Count != 0 && opers.Peek() is Operation operFromStack)
                {
                    if (operFromStack >= operFromList)
                    {
                        rpn.Add(opers.Pop());
                        opers.Push(operFromList);
                    }
                    else opers.Push(operFromList);
                }
                else opers.Push(operFromList);
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

    public static Number CalculateRPN(List<Token> listRPN)
    {
        Stack<Number> result = new Stack<Number>();

        foreach (Token token in listRPN)
        {
            if (token is Number number)
            {
                result.Push(number);
            }
            else if (token is Operation operation)
            {
                Number secondNumber = result.Pop();
                Number firstNumber = result.Pop();
                result.Push(Calculate(firstNumber, secondNumber, operation));
            }
        }

        return result.Pop();
    }

    public static Number Calculate(Number firstNumber, Number secondNumber, Operation operation)
    {
        switch (operation.Symbol)
        {
            case '+': return firstNumber + secondNumber;
            case '-': return firstNumber - secondNumber;
            case '*': return firstNumber * secondNumber;
            case '/': return firstNumber / secondNumber;
        }

        throw new Exception("Invalid symbol");
    }

    public static void ShowRPN(List<Token> rpn)
    {
        Console.WriteLine("\nВаше выражение в обратной польской записи: ");
        foreach (Token token in rpn)
        {
            if (token is Number number)
            {
                Console.Write($"{number} ");
            }

            if (token is Operation operation)
            {
                Console.Write($"{operation} ");
            }
        }
        Console.WriteLine("\n");
    }
}