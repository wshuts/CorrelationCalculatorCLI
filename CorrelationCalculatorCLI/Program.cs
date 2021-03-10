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
}
