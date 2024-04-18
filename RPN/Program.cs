using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using RPN_Logic;

class Program
{
    static void Main()
    {
        Console.Write("Введите выражение: ");
        string expression = Console.ReadLine();
        Console.Write("Введите значение переменной: ");
        string argument = Console.ReadLine();
        RPNCalculator rpn = new RPNCalculator(expression, argument);
        double answer = rpn.Answer;
        Console.WriteLine($"Ответ: {answer}");
    }
}
