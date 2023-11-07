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
        if (expression == string.Empty)
        {
            Console.WriteLine("Вы не написали выражение");
            Console.WriteLine("Попробуйте снова");
            GetExpression(Console.ReadLine());
        }

        expression = expression.Replace(" ", string.Empty);
        Console.WriteLine($"Ваше выражение: {expression}");
        GetLists(expression);
    }
    public static void GetLists(string expression)
    {

        List<string> numbers = new List<string>();
        List<char> operators = new List<char>();
        string toBuildNumber = "";

        foreach (char symbol in expression)
        {
            if (Char.IsDigit(symbol))
            {
                toBuildNumber += symbol;
            }
            else if (symbol == ',' || symbol == '.')
            {
                toBuildNumber += '.';
            }

            else
            {
                numbers.Add(toBuildNumber);
                toBuildNumber = "";
                operators.Add(symbol);
            }
        }

        numbers.Add(toBuildNumber);

        while (numbers.Contains(""))
        {
            numbers.RemoveAt(numbers.IndexOf(""));
        }

        OutputBlock(numbers, "Ваши числа: ");
        OutputBlock(FromCharToStr(operators), "Ваши операторы: ");

        string answer = GetAnswer(numbers, operators);
        Console.WriteLine($"Ответ: {answer}");
    }

    public static string GetAnswer(List<string> numbers, List<char> operators)
    {

        int i = 0;
        while (i < operators.Count & operators.Count != 0 & numbers.Count != 1)
        {

            double number;

            if (operators.Count == 0 & numbers.Count == 1)
            {
                break;
            }

            else if (operators.Contains('('))
            {
                DoBrackets(numbers, operators);
            }

            else if (operators[i] == '/' || operators[i] == '*')
            {

                number = Calculate(numbers[i], numbers[i + 1], operators[i]);
                numbers.RemoveAt(i);
                numbers[i] = $"{number}";
                operators.RemoveAt(i);

                i = 0;
            }

            else if (!(operators.Contains('/') || operators.Contains('*')))
            {

                number = Calculate(numbers[i], numbers[i + 1], operators[i]);
                numbers.RemoveAt(i);
                numbers[i] = $"{number}";
                operators.RemoveAt(i);

                i = 0;
            }

            else i++;
        }

        return numbers[0];
    }

    public static void DoBrackets(List<string> numbers, List<char> operators)
    {

        List<string> numsInBrackets = new List<string>();
        List<char> opersInBrackets = new List<char>();

        int indStart = operators.IndexOf('(');
        int indEnd = operators.IndexOf(')');

        List<int> indexOfOpenBrackets = new List<int>();
        int count = 0;
        while (operators.IndexOf('(', indStart + 1) != -1 & operators.IndexOf('(', indStart + 1) < indEnd)
        {
            operators.Remove('(');
            indexOfOpenBrackets.Add(indStart);
            indStart = operators.IndexOf('(');
            indEnd = operators.IndexOf(')');
            count++;

        }

        for (int j = indStart + 1; j < indEnd; j++)
        {
            numsInBrackets.Add(numbers[j - 1]);
            opersInBrackets.Add(operators[j]);
        }
        numsInBrackets.Add(numbers[indEnd - 1]);

        string number = GetAnswer(numsInBrackets, opersInBrackets);

        numbers.RemoveRange(indStart, indEnd - indStart);
        numbers.Insert(indStart, number);
        operators.RemoveRange(indStart, indEnd - indStart + 1);

        int i = 0;
        while (i < count)
        {
            operators.Insert(indexOfOpenBrackets.Max(), '(');
            indexOfOpenBrackets.Remove(indexOfOpenBrackets.Max());
            i++;
        }

        GetAnswer(numbers, operators);
    }

    public static double Calculate(string firstNumber, string secondNumber, char oper)
    {
        switch (oper)
        {
            case '*': return (double.Parse(firstNumber) * double.Parse(secondNumber));
            case '/': return (double.Parse(firstNumber) / double.Parse(secondNumber));
            case '+': return (double.Parse(firstNumber) + double.Parse(secondNumber));
            case '-': return (double.Parse(firstNumber) - double.Parse(secondNumber));
            default: return double.NaN;
        }
    }

    public static void OutputBlock(List<string> inputList, string content)
    {
        Console.Write(content);
        foreach (string str in inputList) Console.Write($"{str} ");
        Console.WriteLine();
    }

    public static List<string> FromCharToStr(List<char> charList)
    {
        List<string> strList = new List<string>();
        foreach (char ch in charList) strList.Add(ch.ToString());
        return strList;
    }
}
