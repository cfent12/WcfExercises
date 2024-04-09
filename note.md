# 서비스 계약

서비스 계약은 다음을 지정합니다.

-  계약이 노출하는 작업

-  교환되는 메시지와 관련된 작업 서명

-  이러한 메시지의 데이터 형식

-  작업의 위치

-  서비스와의 통신을 지원하는 데 사용되는 특정 프로토콜 및 serialization 형식

---

관리되는 인터페이스의 모든 장점이 서비스 계약 인터페이스에 적용됩니다.

- 서비스 계약 인터페이스는 다른 여러 서비스 계약 인터페이스를 확장할 수 있습니다.

- 단일 클래스는 그러한 서비스 계약 인터페이스를 구현하여 원하는 만큼 서비스 계약을 구현할 수 있습니다.

-  인터페이스 구현을 변경하여 서비스 계약의 구현을 수정할 수 있지만 서비스 계약은 그대로 유지됩니다.

- 기존 인터페이스와 새 인터페이스를 구현하여 서비스에 버전을 지정할 수 있습니다. 기존 클라이언트는 원래 버전에 연결하며, 새 클라이언트는 새 버전에 연결합니다.

## 서비스 계약 디자인
### 요청/회신
요청/회신 패턴은 요청 발신자(클라이언트 애플리케이션)가 요청과 상호 관련된 회신을 받는 패턴입니다. 이 패턴은 하나 이상의 매개 변수가 작업에 전달되고 반환값이 다시 호출자에게 전달되는 작업을 지원하므로 기본 MEP입니다. 예를 들어, 다음 C# 코드 예제에서는 문자열 하나를 사용하여 문자열을 반환하는 기본적인 서비스 작업을 보여 줍니다.

```cs
[OperationContractAttribute]  
string Hello(string greeting);  
```
다른 기본 메시지 패턴을 지정하지 않으면 void(Visual Basic의 Nothing)를 반환하는 서비스 작업이 요청/회신 메시지 교환입니다. 작업 결과, 클라이언트가 작업을 비동기적으로 호출하지 않으면 일반적인 경우 메시지가 비어 있어도 클라이언트가 반환 메시지를 받을 때까지 처리를 중지합니다.
> return 값이 `void`여도 동기로 동작한다.

### 단방향
WCF 서비스 애플리케이션의 클라이언트가 작업이 완료될 때까지 기다리지 않아도 되고 SOAP 오류가 처리되지 않을 경우 작업에 단방향 메시지 패턴을 지정할 수 있습니다. 단방향 작업은 클라이언트가 작업을 호출하고 WCF에서 네트워크에 메시지를 작성한 후에 처리를 계속하는 작업입니다. 일반적으로 아웃바운드 메시지에 보내는 데이터가 아주 크지 않은 경우 데이터를 보내는 데 문제가 없다면 클라이언트가 바로 계속해서 실행됩니다. 이러한 형식의 메시지 교환 패턴은 클라이언트에서 서비스 애플리케이션으로 이벤트와 비슷한 동작을 지원합니다.

메시지를 보내지만 반환되는 메시지가 없는 메시지 교환은 `void` 이외의 반환 값을 지정하는 서비스 작업을 지원할 수 없습니다. 이 경우 InvalidOperationException 예외가 throw됩니다.

반환 메시지가 없다는 것은 처리 또는 통신 오류를 나타내는 SOAP 오류가 반환되지 않았음을 의미합니다. 단방향 작업인 경우 통신 오류 정보에 이중 메시지 교환 패턴이 필요합니다.

```cs
[OperationContractAttribute(IsOneWay=true)]  
void Hello(string greeting);  
```

### 이중
이중 패턴의 특징은 단방향 메시징을 사용하든 요청/회신 메시징을 사용하든 관계없이 서비스와 클라이언트 모두 서로 독립적으로 메시지를 보낼 수 있는 기능입니다. 이와 같은 형식의 양방향 통신은 클라이언트와 직접 통신해야 하는 서비스에 유용하며, 메시지를 교환하는 양측에 이벤트와 유사한 동작을 비롯하여 비동기 환경을 제공하는 데도 유용합니다.

이중 패턴은 클라이언트와 통신하기 위한 추가 메커니즘 때문에 요청/회신 또는 단방향 패턴보다 조금 복잡합니다.

이중 계약을 디자인하려면 *콜백 계약*도 디자인하고 서비스 계약을 표시하는 CallbackContract 특성의 ServiceContractAttribute 속성에 해당 콜백 계약의 형식을 지정해야 합니다.

이중 패턴을 구현하려면 클라이언트에서 호출되는 메서드 선언을 포함하는 다른 인터페이스를 만들어야 합니다.

> 주의
> 
> 서비스가 이중 메시지를 받으면 들어오는 해당 메시지에서 ReplyTo 요소를 확인하여 회신을 보낼 위치를 결정합니다. 메시지를 받는 데 사용되는 채널이 보안되지 않으면 신뢰할 수 없는 클라이언트가 대상 컴퓨터의 `ReplyTo`를 사용하여 악의적인 메시지를 보낼 수 있으므로 해당 대상 컴퓨터의 서비스 거부(DOS)가 발생할 수 있습니다.

## 비동기 서비스 작업 구현
다음 세 가지 방법 중 하나를 사용하여 비동기 작업을 구현할 수 있습니다.

1. 작업 기반 비동기 패턴

2. 이벤트 기반 비동기 패턴

3. IAsyncResult 비동기 패턴

### 1. 작업 기반 비동기 패턴

작업 기반 비동기 패턴은 가장 쉽고 단순하기 때문에 비동기 작업을 구현하는 데 가장 선호하는 방법입니다. 이 방법을 사용하려면 서비스 작업을 구현하고 반환 형식으로 Task<T>를 지정하면 됩니다.

```cs
public class SampleService:ISampleService
{
   // ...  
   public async Task<string> SampleMethodTaskAsync(string msg)
   {
      return Task<string>.Factory.StartNew(() =>
      {
         return msg;
      });
   }  
   // ...  
}  
```

### 2. 이벤트 기반 비동기 패턴

이벤트 기반 비동기 패턴을 지원하는 서비스에는 MethodNameAsync라는 작업이 하나 이상 있습니다. 이러한 메서드는 현재 스레드에서 동일한 작업을 수행하는 동기 버전을 미러링할 수 있습니다. 또한 이 클래스에는 MethodNameCompleted 이벤트가 있을 수 있고 MethodNameAsyncCancel 메서드가 있거나 단순히 CancelAsync 메서드가 있을 수 있습니다. 작업을 호출하려는 클라이언트는 작업이 완료될 때 호출할 이벤트 처리기를 정의합니다.

```cs
public class AsyncExample  
{  
    // Synchronous methods.  
    public int Method1(string param);  
    public void Method2(double param);  
  
    // Asynchronous methods.  
    public void Method1Async(string param);  
    public void Method1Async(string param, object userState);  
    public event Method1CompletedEventHandler Method1Completed;  
  
    public void Method2Async(double param);  
    public void Method2Async(double param, object userState);  
    public event Method2CompletedEventHandler Method2Completed;  
  
    public void CancelAsync(object userState);  
  
    public bool IsBusy { get; }  
  
    // Class implementation not shown.  
}  
```

### 3. IAsyncResult 비동기 패턴

서비스 작업은 .NET Framework 비동기 프로그래밍 패턴을 사용하고 AsyncPattern 속성이 `true`로 설정된 `<Begin>` 메서드를 표시하여 비동기 방식으로 구현할 수 있습니다. 이 경우 비동기 작업은 동기 작업과 동일한 형태로 메타데이터에 노출됩니다. 즉, 요청 메시지와 관련 응답 메시지가 포함된 단일 작업으로 노출됩니다. 그런 다음 클라이언트 프로그래밍 모델에서는 둘 중 하나를 선택할 수 있습니다. 즉, 서비스가 호출될 때 요청-응답 메시지 교환이 발생하는 한 이 패턴을 동기 작업이나 비동기 작업으로 나타낼 수 있습니다.

일반적으로 시스템의 비동기 특성을 사용하는 경우 스레드에 대한 종속성을 사용하지 않아야 합니다. 작업 디스패치 처리의 다양한 단계에 데이터를 전달하는 가장 안정적인 방법은 확장을 사용하는 것입니다.

클라이언트 애플리케이션에서 호출되는 방식에 관계없이 비동기적으로 실행되는 계약 작업 X를 정의하려면 다음을 수행합니다.

- 패턴 `BeginOperation` 및 `EndOperation`을 사용하여 두 개의 메서드를 정의합니다.

- `BeginOperation` 메서드는 작업에 대한 `in` 및 `ref` 매개 변수를 포함하고 *IAsyncResult* 형식을 반환합니다.

- `EndOperation` 메서드는 *IAsyncResult* 및 `out` 매개 변수뿐 아니라 `ref` 매개 변수를 포함하고 작업 반환 형식을 반환합니다.

```cs
[OperationContract(AsyncPattern=true)]
IAsyncResult BeginDoWork(string data,
                         ref string inout,
                         AsyncCallback callback,
                         object state);

int EndDoWork(ref string inout, out string outonly, IAsyncResult result);  
```

# Tips

1. WCF 구성 편집
    - config file 우클릭 - WCF 구성 편집으로 진입
    - 바인딩 추가 - 서비스.끝점에 바인딩 설정

2. OperationContract 계약 생성시 return값을 void로 설정해도 동기로 동작한다.
클라이언트가 빈 메시지를 응답으로 받을 때까지 대기한다.
