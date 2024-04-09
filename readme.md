# microsoft Learn의 WCF 문서 기반

https://learn.microsoft.com/ko-kr/dotnet/framework/wcf/getting-started-tutorial

# 기본 작업

수행할 기본 작업 순서는 다음과 같습니다.

1. [ServiceContract] 서비스 계약을 정의합니다. 서비스 계약은 서비스 서명, 서비스 교환 날짜 및 계약에 필요한 기타 데이터를 지정합니다.

2. [OperationContract] 계약을 구현합니다. 서비스 계약을 구현하려면 계약을 구현하는 클래스를 만들고 런타임에 필요한 사용자 지정 동작을 지정합니다.

3. [endpoint] 엔드포인트 및 기타 동작 정보를 지정하여 서비스를 구성합니다.

4. 서비스를 호스트합니다.

5. 클라이언트 애플리케이션을 빌드합니다.

# Tips

1. WCF 구성 편집
    - config file 우클릭 - WCF 구성 편집으로 진입
    - 바인딩 추가 - 서비스.끝점에 바인딩 설정

