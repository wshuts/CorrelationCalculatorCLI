using System;
using System.Collections;

namespace PriceCorrelationCalculator
{
    public class Statistics
    {
        public static double Mean(IList vector)
        {
            double sum=0;
            double mean;

            foreach(double element in vector)
            {
                sum=sum+element;
            }
			
            mean=sum/vector.Count;
            return mean;
        }
		
        public static double StandardDeviation(IList vector)
        {
            double mean;
            double difference;
            double square;
            double sum=0;
            double meanSquare;
            double standardDeviation;

            mean=Mean(vector);
			
            foreach(double element in vector)
            {
                difference=element-mean;
                square=Math.Pow(difference,2.0);
                sum=sum+square;
            }
			
            meanSquare=sum/vector.Count;
            standardDeviation=Math.Sqrt(meanSquare);
            return standardDeviation;

        }

        public static double Correlation(IList vector1,IList vector2)
        {
            double mean1;
            double mean2;
            double standardDeviation1;
            double standardDeviation2;
            IList differences1=new ArrayList();
            IList differences2=new ArrayList();
            int index;
            IList differenceProducts=new ArrayList();
            double correlation;

            mean1=Mean(vector1);
            mean2=Mean(vector2);
            standardDeviation1=StandardDeviation(vector1);
            standardDeviation2=StandardDeviation(vector2);

            differences1.Clear();
            foreach(double element1 in vector1)
            {
                differences1.Add(element1-mean1);
            }
			
            differences2.Clear();
            foreach(double element2 in vector2)
            {
                differences2.Add(element2-mean2);
            }
			
            differenceProducts.Clear();
            for(index=0;index<differences1.Count;index++)
            {
                var diff1 = differences1[index];
                var diff2 = differences2[index];
                if (diff1 == null) continue;
                if (diff2 != null)
                    differenceProducts.Add((double) diff1 * (double) diff2);
            }
			
            correlation=Mean(differenceProducts)/(standardDeviation1*standardDeviation2);
            return correlation;
        }
    }
}