using System;
using System.Collections.Generic;

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
        List<object> toRPN = new List<object>();

        string toBuildNumber = "";
        foreach (char symbol in expression)
        {
            if (Char.IsDigit(symbol))
            {
                toBuildNumber += symbol;
            }

            else
            {
                toRPN.Add(toBuildNumber);
                toBuildNumber = "";
                toRPN.Add(symbol);
            }

        }
        toRPN.Add(toBuildNumber);
        DoRPN(toRPN);
    }

    public static void DoRPN(List<object> toRPN)
    {
        Stack<char> opers = new Stack<char>();
        List<object> listRPN = new List<object>();
        foreach (var obj in toRPN)
        {
            if (obj is string)
            {
                listRPN.Add(obj);
            }
            else
            {
                char sign = (char)obj;

                if (sign == '(') opers.Push(sign);

                else if (sign == ')')
                {
                    while (opers.Peek() != '(')
                    {
                        listRPN.Add(opers.Peek());
                        opers.Pop();
                    }
                    opers.Pop();
                }

                else if (opers.Count != 0 && GetPriority(opers.Peek()) >= GetPriority(sign))
                {
                    listRPN.Add(opers.Peek());
                    opers.Pop();
                    opers.Push(sign);
                }
                else opers.Push(sign);
            }

        }

        while (opers.Count != 0)
        {
            listRPN.Add(opers.Peek());
            opers.Pop();
        }

        while (listRPN.Contains(string.Empty))
        { 
            listRPN.Remove(string.Empty); 
        }

        Console.WriteLine("Ваше выражение в обратной польской записи: ");
        foreach (var obj in listRPN) Console.Write($"{obj} ");
        Console.WriteLine();

        string answer = CalculateRPN(listRPN);
        Console.WriteLine($"Ответ: { answer}");
    }

    public static string CalculateRPN(List<object> listRPN)
    {
        int i = 0;
        while (listRPN.Count != 1)
        {
            if (listRPN[i] is char)
            {
                string number = Calculate((string)listRPN[i - 2], (string)listRPN[i - 1], (char)listRPN[i]).ToString();
                listRPN.RemoveAt(i);
                listRPN.RemoveAt(i - 1);
                listRPN.RemoveAt(i - 2);
                listRPN.Insert(i - 2, number);
                i = 0;
            }
            i++;
        }
        return (string)listRPN[0];
    }
    public static int GetPriority(char sign)
    {
        switch (sign)
        {
            case ('+'): return 1;
            case ('-'): return 1;
            case ('*'): return 2;
            case ('/'): return 2;
            case ('('): return 0;
            case (')'): return 0;
            default: return 3;
        }
    }

    public static double Calculate(string firstNumber, string secondNumber, char oper)
    {
        switch (oper)
        {
            case ('+'): return (double.Parse(firstNumber) + double.Parse(secondNumber));
            case ('-'): return (double.Parse(firstNumber) - double.Parse(secondNumber));
            case ('*'): return (double.Parse(firstNumber) * double.Parse(secondNumber));
            case ('/'): return (double.Parse(firstNumber) / double.Parse(secondNumber));
            default: return double.NaN;
        }
    }
}