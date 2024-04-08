using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// System.ServiceModel 어셈블리 참조 추가
// 서비스 참조 추가 - 검색 - 추가

// ServiceModel Metadata 유틸리티 도구(Svcutil.exe)를 사용하여 프록시 클래스 파일을 생성하는 방법을 보여 줍니다.
// 이 도구는 프록시 클래스 파일과 App.config 파일을 생성합니다.
// HostConcoleApp을 실행하여 서비스를 실행한 상태에서, 콘솔에서 ClientConsoleApp 폴더로 이동한 후, 하기 명령어를 입력한다.
// svcutil.exe /language:cs /out:generatedProxy.cs /config:app.config http://localhost:8888/WcfServiceLibrary/CalculatorService/

using ClientConsoleApp.ServiceReference1;

namespace ClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Create an instance of the WCF proxy.
            CalculatorClient client = new CalculatorClient();

            // Step 2: Call the service operations.
            // Call the Add service operation.
            double value1 = 100.00D;
            double value2 = 15.99D;
            double result = client.Add(value1, value2);
            Console.WriteLine("Add({0},{1}) = {2}", value1, value2, result);

            // Call the Subtract service operation.
            value1 = 145.00D;
            value2 = 76.54D;
            result = client.Subtract(value1, value2);
            Console.WriteLine("Subtract({0},{1}) = {2}", value1, value2, result);

            // Call the Multiply service operation.
            value1 = 9.00D;
            value2 = 81.25D;
            result = client.Multiply(value1, value2);
            Console.WriteLine("Multiply({0},{1}) = {2}", value1, value2, result);

            // Call the Divide service operation.
            value1 = 22.00D;
            value2 = 7.00D;
            result = client.Divide(value1, value2);
            Console.WriteLine("Divide({0},{1}) = {2}", value1, value2, result);

            // Step 3: Close the client to gracefully close the connection and clean up resources.
            Console.WriteLine("\nPress <Enter> to terminate the client.");
            Console.ReadLine();
            client.Close();
        }
    }
}
