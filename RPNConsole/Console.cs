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
        double answer = calculator.Calculate(double.Parse(argument));
        Console.WriteLine($"Ответ: {answer}");
    }
}
