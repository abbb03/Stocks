namespace Stocks
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var alphaVantageApiKey = builder.Configuration["AlphaVantage:ApiKey"];

            Quote.ApiKey = alphaVantageApiKey;
            
            Quote quote = new Quote();
            await quote.GetQuote();
            quote.CreateRecord();
        }
    }
}
