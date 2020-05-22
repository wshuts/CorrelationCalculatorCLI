using System;
using System.Collections.Generic;
using System.Linq;

namespace PriceCorrelationCalculator
{
    public class Statistics
    {
        public static double Mean(IList<double> vector)
        {
            var sum = vector.Sum();

            var mean = sum / vector.Count;
            return mean;
        }

        public static double StandardDeviation(IList<double> vector)
        {
            var mean = Mean(vector);

            var sum = vector.Select(element => element - mean).Select(difference => Math.Pow(difference, 2.0)).Sum();

            var meanSquare = sum / vector.Count;
            var standardDeviation = Math.Sqrt(meanSquare);
            return standardDeviation;
        }

        public static double Correlation(IList<double> vector1, IList<double> vector2)
        {
            IList<double> differences1 = new List<double>();
            IList<double> differences2 = new List<double>();
            IList<double> differenceProducts = new List<double>();
            int index;

            var mean1 = Mean(vector1);
            var mean2 = Mean(vector2);
            var standardDeviation1 = StandardDeviation(vector1);
            var standardDeviation2 = StandardDeviation(vector2);

            differences1.Clear();
            foreach (var element1 in vector1) differences1.Add(element1 - mean1);

            differences2.Clear();
            foreach (var element2 in vector2) differences2.Add(element2 - mean2);

            differenceProducts.Clear();
            for (index = 0; index < differences1.Count; index++)
            {
                var diff1 = differences1[index];
                var diff2 = differences2[index];
                differenceProducts.Add(diff1 * diff2);
            }

            var correlation = Mean(differenceProducts) / (standardDeviation1 * standardDeviation2);
            return correlation;
        }
    }
}