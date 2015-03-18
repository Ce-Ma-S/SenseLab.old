using CeMaS.Common.Services;
using System;

namespace SenseLab.Environments.Service
{
    static class Program
    {
        static void Main(params string[] arguments)
        {
            using (var service = new ServiceHostController(Local.Environment.CreateHost()))
            {
                service.Run(arguments);

                Console.WriteLine();

                TestClient("net");
                TestClient("ws");

                Console.WriteLine("Press any key to stop.");
                Console.ReadKey();
            }
        }

        private static void TestClient(string endpointConfigurationName)
        {
            Console.WriteLine(string.Format("Client '{0}':", endpointConfigurationName));
            using (var client = Remote.Environment.Create(endpointConfigurationName).Result)
            {
                Console.WriteLine("Created:\n" + client.ToString());
            }
            Console.WriteLine();
        }
    }
}
