using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using System.Net.Security; // ProtectionLevel

// 서비스 계약의 기본 정의가 포함되어 있습니다.
namespace WcfServiceLibrary
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface ICalculator
    {
        // 요청/회신 패턴
        [OperationContractAttribute]
        string Hello(string greeting);
        // 단방향 패턴
        [OperationContractAttribute(IsOneWay = true)]
        void Hello2(string greeting);
        // 메시지 보호 수준 지정
        [OperationContractAttribute(ProtectionLevel = ProtectionLevel.EncryptAndSign)] // 암호화되고 서명된 메시지에 반환
        string Hello3(string greeting);
        // SOAP 오류를 '선언된' 상태로 설정
        [OperationContract]
        [FaultContractAttribute(
            typeof(GreetingFault),
            Action = "http://localhost:8888/GreetingFault",
            ProtectionLevel = ProtectionLevel.EncryptAndSign
        )]
        string FaultContractSampleMethod(string msg);


        [OperationContract]
        double Add(double n1, double n2);
        [OperationContract]
        double Subtract(double n1, double n2);
        [OperationContract]
        double Multiply(double n1, double n2);
        [OperationContract]
        double Divide(double n1, double n2);
    }



    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IService1"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: 여기에 서비스 작업을 추가합니다.
    }

    // 아래 샘플에 나타낸 것처럼 데이터 계약을 사용하여 복합 형식을 서비스 작업에 추가합니다.
    // XSD 파일을 프로젝트에 추가할 수 있습니다. 프로젝트를 빌드한 후 프로젝트에 정의된 데이터 타입을 네임스페이스 "WcfServiceLibrary.ContractType"와 함께 직접 사용할 수 있습니다.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContractAttribute]
    public class GreetingFault
    {
        private string report;

        public GreetingFault(string message)
        {
            this.report = message;
        }

        [DataMemberAttribute]
        public string Message
        {
            get { return this.report; }
            set { this.report = value; }
        }
    }

    // Define a duplex service contract.
    // A duplex contract consists of two interfaces.
    // The primary interface is used to send messages from client to service.
    // The callback interface is used to send messages from service back to client.
    // ICalculatorDuplex allows one to perform multiple operations on a running result.
    // The result is sent back after each operation on the ICalculatorCallback interface.
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples",
                     SessionMode = SessionMode.Required, CallbackContract = typeof(ICalculatorDuplexCallback))] // 이중 계약 패턴 콜백
    public interface ICalculatorDuplex
    {
        [OperationContract(IsOneWay = true)]
        void Clear();
        [OperationContract(IsOneWay = true)]
        void AddTo(double n);
        [OperationContract(IsOneWay = true)]
        void SubtractFrom(double n);
        [OperationContract(IsOneWay = true)]
        void MultiplyBy(double n);
        [OperationContract(IsOneWay = true)]
        void DivideBy(double n);
    }

    // The callback interface is used to send messages from service back to client.
    // The Equals operation will return the current result after each operation.
    // The Equation operation will return the complete equation after Clear() is called.
    public interface ICalculatorDuplexCallback
    {
        [OperationContract(IsOneWay = true)]
        void Equals(double result);
        [OperationContract(IsOneWay = true)]
        void Equation(string eqn);
    }
}
