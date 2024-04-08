using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// WcfServiceLibrary 참조 추가
// System.ServiceModel 어셈블리 참조 추가
// 프로젝트 속성 - 애플리케이션 탭 - 시작 개체 - ConsoleApp.Program

using System.ServiceModel;
using System.ServiceModel.Description;
using WcfServiceLibrary;

namespace HostConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create a URI to serve as the base address.
            // 서비스의 기본 주소를 보유할 Uri 클래스의 인스턴스를 만듭니다.
            // 기본 주소가 포함된 URL에는 서비스를 식별하는 선택적 URI가 있습니다.
            // 기본 주소는 <transport>://<machine-name or domain><:optional port #>/<optional URI segment>와 같이 형식이 지정됩니다.
            // 계산기 서비스의 기본 주소는 HTTP 전송, localhost, 포트 8888 및 URI 세그먼트인 GettingStarted를 사용합니다.
            Uri baseAddress = new Uri("http://localhost:8888/GettingStarted/");

            // Step 2: Create a ServiceHost instance.
            // 서비스를 호스팅하는 데 사용하는 ServiceHost 클래스의 인스턴스를 만듭니다.
            // 생성자는 서비스 계약을 구현하는 클래스 형식과 서비스의 기본 주소, 두 가지 매개 변수를 사용합니다.
            ServiceHost selfHost = new ServiceHost(typeof(CalculatorService), baseAddress);

            try
            {
                // Step 3: Add a service endpoint.
                // ServiceEndpoint 인스턴스를 만듭니다.
                // 서비스 엔드포인트는 주소, 바인딩 및 서비스 계약으로 구성되어 있습니다.
                // ServiceEndpoint 생성자는 서비스 계약 인터페이스 형식, 바인딩 및 주소로 구성됩니다.
                // 서비스 계약은 사용자가 정의한 ICalculator이며 서비스 형식에 구현합니다.
                // 이 샘플의 바인딩은 기본 제공 바인딩이고 WS - *사양을 준수하는 엔드포인트에 연결되는 WSHttpBinding입니다.
                // 엔드포인트를 식별하기 위해 주소를 기본 주소에 추가합니다.
                // 이 코드는 주소를 CalculatorService로 지정하고 엔드포인트의 정규화된 주소를 http://localhost:8888/GettingStarted/CalculatorService로 지정합니다.
                // .NET Framework 버전 4 이상에서는 서비스 엔드포인트를 추가하는 것이 선택 사항입니다.
                selfHost.AddServiceEndpoint(typeof(ICalculator), new WSHttpBinding(), "CalculatorService");

                // Step 4: Enable metadata exchange.
                // 메타데이터 교환을 사용하도록 설정합니다.
                // 클라이언트는 메타데이터 교환을 사용하여 서비스 작업을 호출하기 위한 프록시를 생성합니다.
                // 메타데이터 교환을 사용하도록 설정하려면 ServiceMetadataBehavior 인스턴스를 만들고 해당 HttpGetEnabled 속성을 true로 설정한 다음, ServiceMetadataBehavior 개체를 ServiceHost 인스턴스의 Behaviors 컬렉션에 추가합니다.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                selfHost.Description.Behaviors.Add(smb);

                // Step 5: Start the service.
                // ServiceHost를 열어 들어오는 메시지를 수신합니다.
                // 애플리케이션은 사용자가 Enter 키를 누를 때까지 기다립니다.
                // 애플리케이션이 ServiceHost를 인스턴스화한 후 try/catch 블록을 실행합니다.
                selfHost.Open();
                Console.WriteLine("The service is ready.");

                // Close the ServiceHost to stop the service.
                Console.WriteLine("Press <Enter> to terminate the service.");
                Console.WriteLine();
                Console.ReadLine();
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }
        }
    }
}
