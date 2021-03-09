using System;

namespace CorrelationCalculatorCLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                InputParser.ProcessInputParameters(args);
                var calculator = CalculatorFactory.CreateCalculator();
                calculator.CalculateCorrelation();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    internal class CalculatorFactory
    {
        public static Calculator CreateCalculator()
        {
            throw new NotImplementedException();
        }
    }

    internal class Calculator
    {
        public void CalculateCorrelation()
        {
            throw new NotImplementedException();
        }
    }

    internal class InputParser
    {
        public static void ProcessInputParameters(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
