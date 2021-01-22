using System;
using System.Configuration;
using System.Threading.Tasks;
using IEXSharp;

namespace Stock_Lookup_Console
{
    class Program
    {
        private async static Task GetStockAsync(string stockSymbol) {
            var publishableToken = ConfigurationManager.AppSettings.Get("publishableToken");
            var secretToken = ConfigurationManager.AppSettings.Get("secretToken");

            IEXCloudClient client = new IEXCloudClient(publishableToken, secretToken, false, false);
            try {
                var response = await client.StockPrices.QuoteAsync(stockSymbol);
                if (response.Data == null)
                {
                    Console.WriteLine("No stock was found for symbol: " + stockSymbol);
                }
                else
                {
                
                    Console.WriteLine("\nCompany Name: " + response.Data.companyName);
                    Console.WriteLine("Current Price: " + response.Data.latestPrice);
                    Console.WriteLine("Year High: " + response.Data.week52High);
                    Console.WriteLine("Year Low: " + response.Data.week52Low );
                    Console.WriteLine("Market Cap: " + response.Data.marketCap + "\n");
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Your input cannot be blank.\n");
            }
            
        }
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Stock Lookup!");
            
            while (true)
            {
                Console.WriteLine("Enter the symbol of the stock you'd like to lookup:");
                var stockSymbol = Console.ReadLine();
                GetStockAsync(stockSymbol).GetAwaiter().GetResult();
                Console.WriteLine("Would you like to search again? (y/n)");
                
                var userInput = Console.ReadLine().ToLower();
                if (userInput.Equals("y"))
                {
                    continue;
                }  
                else
                {
                    break;
                }
            }

            Console.WriteLine("Goodbye!");
        }
    }
}
