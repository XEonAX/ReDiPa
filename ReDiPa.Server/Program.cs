using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReDiPa.Server
{
    class Program
    {

        public static Processor processor = new Processor();

        public static string OutputDirectoryFor7z { get; set; }
        public static string PathTo7z { get; set; }

        static void Main(string[] args)
        {

            OutputDirectoryFor7z = ConfigurationManager.AppSettings["OutputDirectoryFor7z"];
            PathTo7z = ConfigurationManager.AppSettings["PathTo7z"];

            string baseAddress = ConfigurationManager.AppSettings["BaseAddress"];

            processor.TokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(x => processor.DoTasks()
                                  , TaskCreationOptions.LongRunning
                                  , processor.TokenSource.Token);

            using (WebApp.Start<Startup>(baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();
                var response = client.GetAsync(baseAddress + "api/task").Result;
                Console.ReadLine();

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }

            Console.ReadLine();
        }

        private void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
