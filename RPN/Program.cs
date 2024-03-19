using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using RPN_Logic;

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
        RPNCalculator rpn = new RPNCalculator(expression);
        double answer = rpn.Answer;
        Console.WriteLine($"Ответ: {answer}");
    }
}
