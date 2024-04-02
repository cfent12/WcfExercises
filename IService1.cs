using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IService1"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IService1
    {
        // TODO: 여기에 서비스 작업을 추가합니다.
        //[OperationContract]
        [OperationContractAttribute(IsOneWay = true, Action = "http://localhost:50849/Service1/Send")]
        void Send(string requestData);

        //[OperationContract]
        [OperationContractAttribute(Action = "http://localhost:50849/Service1/SendRequest", ReplyAction = "http://localhost:50849/Service1/SendRequestResponse")]
        string SendRequest(string requestData);
    }
}
