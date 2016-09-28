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
        public static string FileSystemPath { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Booting ReDiPa v{0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            OutputDirectoryFor7z = ConfigurationManager.AppSettings["OutputDirectoryFor7z"] ?? System.Reflection.Assembly.GetExecutingAssembly().Location;
            PathTo7z = ConfigurationManager.AppSettings["PathTo7z"] ?? System.Reflection.Assembly.GetExecutingAssembly().Location;
            FileSystemPath = ConfigurationManager.AppSettings["FileSystemPath"] ?? System.Reflection.Assembly.GetExecutingAssembly().Location;
            string baseAddress = ConfigurationManager.AppSettings["BaseAddress"] ?? "http://localhost:9000/";

            processor.TokenSource = new CancellationTokenSource();

            Console.WriteLine("Starting Task Execution Loop");
            Task.Factory.StartNew(x => processor.DoTasks()
                                  , TaskCreationOptions.LongRunning
                                  , processor.TokenSource.Token);
            Console.WriteLine("Task Execution Loop started.");

            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine("OWIN started at {0}", baseAddress);
                Console.WriteLine("Press any key for status, press \"exit\" to exit.");
                //// Create HttpCient and make a request to api/values 
                //HttpClient client = new HttpClient();
                //var response = client.GetAsync(baseAddress + "api/task").Result;
                while (Console.ReadLine().ToLower() != "exit")
                {
                    foreach (var item in processor.GetTasks())
                    {
                        Console.WriteLine("======================================================");
                        Console.WriteLine("ID         :" + item.ID);
                        Console.WriteLine("User       :" + item.User);
                        Console.WriteLine("State      :" + item.State.ToString());
                        Console.WriteLine("Source     :" + item.Source);
                        Console.WriteLine("Destination:" + item.Destination);
                        Console.WriteLine("Priority   :" + item.Priority);
                        Console.WriteLine("======================================================");
                    }
                    Console.WriteLine("Press any key for status, press \"exit\" to exit.");
                }
            }
        }
    }
}
