using System;
using System.ServiceModel;
using WcfServiceLibrary;

namespace NetTcpHostConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host;

            string localIP = "127.0.0.1";
            string Host_ListenPort = "50851";
            string Host_ServiceName = "CalculatorService";

            string address = string.Format("net.tcp://{0}:{1}/{2}", localIP, Host_ListenPort, Host_ServiceName);

            NetTcpBinding tcpBinding = new NetTcpBinding();
            // TransactionFlow 사용 안함
            tcpBinding.TransactionFlow = false;
            // Security 사용 안함
            //tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            //tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            tcpBinding.Security.Mode = SecurityMode.None;

            // 받는 메시지가 없을 때 Host가 내려가는 것 방지를 위해 ReceiveTimeout을 증가
            tcpBinding.ReceiveTimeout = TimeSpan.Parse("24.20:31:23.6470000");

            // 받는 메시지량 증가
            tcpBinding.MaxBufferSize = 1073741824;
            tcpBinding.MaxReceivedMessageSize = 1073741824;
            //tcpBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
            //tcpBinding.ReaderQuotas.MaxArrayLength = 1073741824;

            // ReliableSession 사용 안함
            tcpBinding.ReliableSession.Ordered = true;
            tcpBinding.ReliableSession.InactivityTimeout = TimeSpan.Parse("24.20:31:23.6470000");
            tcpBinding.ReliableSession.Enabled = false;

            // Service Host create
            host = new ServiceHost(typeof(TcpService));

            // End Point add
            host.AddServiceEndpoint(typeof(ITcpService), tcpBinding, address);

            // Service Host start
            if (host.State != CommunicationState.Closed)
            {
                host.Close();

                // Service Host create
                host = new ServiceHost(typeof(TcpService));

                // End Point add
                host.AddServiceEndpoint(typeof(ITcpService), tcpBinding, address);
            }

            host.Open();

            System.Threading.Thread.Sleep(100);

            if (host.State == CommunicationState.Opened)
            {
                Console.WriteLine("The service is ready.");
                Console.WriteLine("Press <Enter> to terminate the service.");
                Console.WriteLine();
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
