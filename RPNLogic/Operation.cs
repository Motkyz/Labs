using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNLogic
{
    abstract class Operation : Token
    {
        public abstract string Name { get; }
        public abstract int Priority { get; }
        public abstract bool IsFunction { get; }
        public abstract int ArgsCount { get; }
        public abstract Number Execute(params Number[] args);

        public override string ToString()
        {
            return Name;
        }

        public static bool operator >=(Operation a, Operation b)
        {
            return (a.Priority >= b.Priority);
        }

        public static bool operator <=(Operation a, Operation b)
        {
            return (a.Priority <= b.Priority);
        }
    }
    class Plus : Operation
    {
        public override string Name => "+";
        public override int Priority => 1;
        public override bool IsFunction => false;
        public override int ArgsCount => 2;
        public override Number Execute(params Number[] args)
        {
            Number num1 = args[0];
            Number num2 = args[1];
            return num1 + num2;
        }
    }
    class Minus : Operation
    {
        public override string Name => "-";
        public override int Priority => 1;
        public override bool IsFunction => false;
        public override int ArgsCount => 2;
        public override Number Execute(params Number[] args)
        {
            Number num1 = args[0];
            Number num2 = args[1];
            return num1 - num2;
        }
    }
    class Multiply : Operation
    {
        public override string Name => "*";
        public override int Priority => 2;
        public override bool IsFunction => false;
        public override int ArgsCount => 2;
        public override Number Execute(params Number[] args)
        {
            Number num1 = args[0];
            Number num2 = args[1];
            return num1 * num2;
        }
    }
    class Divide : Operation
    {
        public override string Name => "/";
        public override int Priority => 2;
        public override bool IsFunction => false;
        public override int ArgsCount => 2;
        public override Number Execute(params Number[] args)
        {
            Number num1 = args[0];
            Number num2 = args[1];
            return num1 / num2;
        }
    }
    class Log : Operation
    {
        public override string Name => "log";
        public override int Priority => 3;
        public override bool IsFunction => true;
        public override int ArgsCount => 2;
        public override Number Execute(params Number[] args)
        {
            Number num1 = args[0];
            Number num2 = args[1];
            return new Number(Math.Log(num2.Value, num1.Value));
        }
    }
    class Ln : Operation
    {
        public override string Name => "ln";
        public override int Priority => 3;
        public override bool IsFunction => true;
        public override int ArgsCount => 1;
        public override Number Execute(params Number[] args)
        {
            Number num = args[0];
            return new Number(Math.Log(num.Value));
        }
    }
    class Power : Operation
    {
        public override string Name => "^";
        public override int Priority => 3;
        public override bool IsFunction => false;
        public override int ArgsCount => 2;
        public override Number Execute(params Number[] args)
        {
            Number num1 = args[0];
            Number num2 = args[1];
            return new Number(Math.Pow(num1.Value, num2.Value));
        }
    }
    class Sqrt : Operation
    {
        public override string Name => "sqrt";
        public override int Priority => 3;
        public override bool IsFunction => false;
        public override int ArgsCount => 1;
        public override Number Execute(params Number[] args)
        {
            Number num = args[0];
            return new Number(Math.Sqrt(num.Value));
        }
    }
    class Root : Operation
    {
        public override string Name => "rt";
        public override int Priority => 3;
        public override bool IsFunction => false;
        public override int ArgsCount => 2;
        public override Number Execute(params Number[] args)
        {
            Number num1 = args[0];
            Number num2 = args[1];
            return new Number(Math.Pow(num2.Value, 1/num1.Value));
        }
    }
    class Sin : Operation
    {
        public override string Name => "sin";
        public override int Priority => 3;
        public override bool IsFunction => true;
        public override int ArgsCount => 1;
        public override Number Execute(params Number[] args)
        {
            Number num = args[0];
            return new Number(Math.Sin(num.Value));
        }
    }
    class Cos : Operation
    {
        public override string Name => "cos";
        public override int Priority => 3;
        public override bool IsFunction => true;
        public override int ArgsCount => 1;
        public override Number Execute(params Number[] args)
        {
            Number num = args[0];
            return new Number(Math.Cos(num.Value));
        }
    }
    class Tg : Operation
    {
        public override string Name => "tg";
        public override int Priority => 3;
        public override bool IsFunction => true;
        public override int ArgsCount => 1;
        public override Number Execute(params Number[] args)
        {
            Number num = args[0];
            return new Number(Math.Tan(num.Value));
        }
    }
    class Ctg : Operation
    {
        public override string Name => "ctg";
        public override int Priority => 3;
        public override bool IsFunction => true;
        public override int ArgsCount => 1;
        public override Number Execute(params Number[] args)
        {
            Number num = args[0];
            return new Number(1/Math.Tan(num.Value));
        }
    }
}
