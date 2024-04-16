using System;
using System.ServiceModel;
using WcfServiceLibrary;

namespace NetTcpClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ITcpService client = null;

            ChannelFactory<ITcpService> factory = new ChannelFactory<ITcpService>();

            string address = "net.tcp://127.0.0.1:50851/CalculatorService";
            factory.Endpoint.Address = new EndpointAddress(address);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            factory.Endpoint.Binding = binding;
            factory.Endpoint.Contract.ContractType = typeof(ITcpService);

            client = factory.CreateChannel();

            try
            {
                string answer = client.Hello("Host");

                Console.WriteLine(answer);
                Console.ReadLine();
            }
            catch (TimeoutException timeProblem)
            {
                Console.WriteLine("The service operation timed out. " + timeProblem.Message);
                Console.Read();
            }
            catch (FaultException<GreetingFault> greetingFault)
            {
                Console.WriteLine(greetingFault.Detail.Message);
                Console.ReadLine();
            }
            catch (FaultException unknownFault)
            {
                Console.WriteLine("An unknown exception was received. " + unknownFault.Message);
                Console.ReadLine();
            }
            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message);
                Console.Read();
            }
        }
    }
}
