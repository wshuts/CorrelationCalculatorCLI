namespace PriceCorrelationCalculator
{
    public interface IPriceServerFactory
    {
        public PriceServer CreatePriceServer();
    }
}