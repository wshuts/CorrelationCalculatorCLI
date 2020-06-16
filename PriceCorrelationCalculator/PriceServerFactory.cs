namespace PriceCorrelationCalculator
{
    public class PriceServerFactory : IPriceServerFactory
    {
        public PriceServer CreatePriceServer()
        {
            return new PriceServer();
        }
    }
}