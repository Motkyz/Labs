using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using RPNLogic;

class Program
{
    static void Main()
    {
        Console.Write("Введите выражение: ");
        string expression = Console.ReadLine();
        Console.Write("Введите значение переменной: ");
        string argument = Console.ReadLine();
        RPNCalculator calculator = new RPNCalculator(expression);
        Number answer = calculator.CalculateRPN(argument);
        Console.WriteLine($"Ответ: {answer}");
    }
}
