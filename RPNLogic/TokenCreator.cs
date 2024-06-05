using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RPNLogic
{
    static class TokenCreator
    {
        private static List<Operation> _availableOperations;

        public static Token Create(string str)
        {
            if (char.IsDigit(str.First()))
            {
                return new Number(str);
            }

            return CreateOperation(str);
        }

        public static Token Create(char symbol)
        {
            if(Number.IsX(symbol))
            {
                return new Number(symbol); 
            }

            if (symbol == '(' || symbol == ')') 
            {
                return new Parenthesis(symbol);
            }

            return CreateOperation(symbol.ToString());
        }

        private static Operation CreateOperation(string name)
        {
            Operation operation = FindAvailableOperationByName(name);
            if (operation == null) 
            {
                throw new ArgumentException($"Invalid Operation: {name}");
            }

            return operation;
        }

        private static Operation FindAvailableOperationByName(string name)
        {
            if (_availableOperations == null)
            {
                Type parent = typeof(Operation);
                var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                var types = allAssemblies.SelectMany(x => x.GetTypes());
                var inheritingTypes = types.Where(t => parent.IsAssignableFrom(t) && !t.IsAbstract).ToList();

                _availableOperations = inheritingTypes.Select(type => (Operation)Activator.CreateInstance(type)).ToList();
            }

            return _availableOperations.FirstOrDefault(operation => operation.Name.Equals(name));
        }
    }
}
