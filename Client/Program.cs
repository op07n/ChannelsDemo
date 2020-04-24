using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExampleClient
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                await GenerateTestData();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }

        private static async Task GenerateTestData()
        {
            var pid = Process.GetCurrentProcess().Id;

            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };

            var measurementID = 0;
            var rnd = new Random();
            while (true)
            {
                measurementID++;

                var request = new
                {
                    MeasurementID = $"{pid}/{measurementID}",
                    Value = rnd.Next(1, 101),
                };

                var response = await PostAsJsonAsync(client, "/api/Measurements", request);
                response.EnsureSuccessStatusCode();
            }
        }

        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString, Encoding.UTF8, "application/json");
            return httpClient.PostAsync(url, content);
        }
    }
}
