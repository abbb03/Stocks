using System.Text;
using Newtonsoft.Json;

namespace Stocks
{
    public class Quote
    {
        private static string apiKey;
        private GlobalQuote quote;

        public static string ApiKey
        {
            private get
            {
                return apiKey;
            }
            set
            {
                if (value is string)
                {
                    apiKey = value;
                }
                else
                {
                    throw new Exception("Please, set correct API key");
                }
            }
        }

        public void CreateRecord()
        {
            if (quote != null)
            {
                try
                {
                    StreamWriter streamWriter = new StreamWriter(@"/home/alex/Documents/file.txt", true);
                    foreach(var property in typeof(GlobalQuote).GetProperties())
                    {
                        streamWriter.WriteLine(property.Name + " " + property.GetValue(quote));
                    }
                    streamWriter.WriteLine("________________");
                    streamWriter.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something goes wrong...");
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
            else
            {
                throw new Exception("Quote is null");
            }
        }

        public async Task GetQuote()
        {
            string url = GenerateUrl();

            try
            {
                HttpClient client = new HttpClient();
                string response = await client.GetStringAsync(url.ToString());
                quote = ParseResponse(response).GlobalQuote;
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("Something goes wrong...");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private string GenerateUrl()
        {
            if (ApiKey != null)
            {
                StringBuilder url = new StringBuilder("https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=");
                string symbol = GetSymbol();
                url.Append(symbol);
                url.Append("&apikey=" + ApiKey);
                string result = url.ToString();

                return result;
            }
            else
            {
                throw new Exception("Please, set ApiKey");
            }
        }

        private string GetSymbol()
        {
            System.Console.WriteLine("Enter the symbol of stock");
            string? symbol = Console.ReadLine();
            if (symbol != null)
            {
                return symbol;
            }
            else
            {
                throw new Exception("Symbol is null");
            }
        }

        private static dynamic ParseResponse(string response)
        {
            var root = JsonConvert.DeserializeObject<Root>(response);
            if (root != null)
            {
                return root;
            }
            else
            {
                throw new Exception("Response is null");
            }
        }
    }
}
