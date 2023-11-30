using System;
using System.Collections.Generic;
using System.Linq;

public class Token
{

}

public class Parenthesis : Token
{
    public char parenthesis;
}

public class Number : Token
{
    public double num;

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

    public static double CalculateRPN(List<Token> listRPN)
    {
        int i = 0;
        while (listRPN.Count != 1)
        {
            if (listRPN[i] is Operation)
            {
                Number firstNumber = (Number)listRPN[i - 2];
                Number secondNumber = (Number)listRPN[i - 1];
                Operation oper = (Operation)listRPN[i];

                double number = Calculate(firstNumber.num, secondNumber.num, oper.oper);
                listRPN.RemoveAt(i);
                listRPN.RemoveAt(i - 1);
                listRPN.RemoveAt(i - 2);
                listRPN.Insert(i - 2, new Number() { num = number });
                i = 0;
            }
            i++;
        }
        Number answer = (Number)listRPN[0];
        return answer.num;
    }
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

}

class Program
{
    public static void Main()
    {

        GetExpression(Console.ReadLine());

    }

    public static void GetExpression(string expression)
    {
        expression = expression.Replace(" ", string.Empty);
        Console.WriteLine($"Ваше выражение: {expression}");
        GetListToRPN(expression);
    }

    public static void GetListToRPN(string expression)
    {
        List<Token> toRPN = new List<Token>();

        string toBuildNumber = "";

        foreach (char symbol in expression)
        {
            if (Char.IsDigit(symbol))
            {
                toBuildNumber += symbol;
            }

            else if (symbol == '(')
            {
                toRPN.Add(new Parenthesis() { parenthesis = symbol });
            }

            else
            {
                if (toBuildNumber != "")
                {

                    toRPN.Add(new Number() { num = double.Parse(toBuildNumber) });
                }
                if (symbol == ')')
                {
                    toRPN.Add(new Parenthesis() { parenthesis = symbol });
                    toBuildNumber = "";
                }
                else
                {
                    toBuildNumber = "";
                        toRPN.Add(new Operation() { oper = symbol });
                }
            }

        }

        toRPN.Add(new Number() { num = double.Parse(toBuildNumber) });
        DoRPN(toRPN);
    }

    public static void DoRPN(List<Token> toRPN)
    {
        Stack<Token> opers = new Stack<Token>();
        List<Token> RPN = new List<Token>();

        foreach (var obj in toRPN)
        {
            if (obj is Number)
            {
                RPN.Add(obj);
            }

            else if (obj is Parenthesis)
            {

                Parenthesis par = obj as Parenthesis;
                if (par.parenthesis == '(')
                {
                    opers.Push(par);
                }

                else if (par.parenthesis == ')')
                {
                    while (!(opers.Peek() is Parenthesis))
                    {
                        RPN.Add(opers.Peek());
                        opers.Pop();
                    }
                    opers.Pop();
                }
            }

            else if(obj is Operation)
            {
                Operation operFromList = obj as Operation;
                if (opers.Count != 0 && !(opers.Peek() is Parenthesis))
                {
                    Operation operFromStack = (Operation)opers.Peek();
                    if (Operation.GetPriority(operFromStack.oper) >= Operation.GetPriority(operFromList.oper))
                    {
                        RPN.Add(operFromStack);
                        opers.Pop();
                        opers.Push(operFromList);
                    }
                    else opers.Push(operFromList);
                }
                else opers.Push(operFromList);
            }
        }

        while (opers.Count != 0)
        {
            RPN.Add((Operation)opers.Peek());
            opers.Pop();
        }

        Console.WriteLine("Ваше выражение в обратной польской записи: ");
        foreach (Token obj in RPN)
        {
            if (obj is Number)
            {
                Console.Write($"{(obj as Number).num} ");
            }
            if (obj is Operation)
            {
                Console.Write($"{(obj as Operation).oper} ");
            }
        }
        Console.WriteLine();

        double answer = Number.CalculateRPN(RPN);
        Console.WriteLine($"Ответ: {answer}");
    }
}
