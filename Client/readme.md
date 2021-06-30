# OAuth2.0인증을 이용한 API 호출
작성자 : 김우창

메일 : nanenchanga2@gmail.com

[<span style="color:rgb(255, 100, 100)">참조사이트</span>] https://identityserver4.readthedocs.io/en/latest/index.html

## 기본설정과 실행을 위한 속성
1. 새 프로젝트 -> 빈 콘솔프로젝트
2. 대상 프레임워크 Core3.1
3. 설치 NugetPakage
    * IdentityModel
    * Newtonsoft.Json

## IdentityServer4 에서 토큰을 생성하여 API 호출
1. API호출을 의한 토큰 발급을 설정
```csharp
// 메타데이터 엔드포인트 지정(호스트 주소를 적으면 엔드포인트는 알아서 붙인다(기본설정시))
var client = new HttpClient();

var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    return;
}

// request token
var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",

    Scope = "api1"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.Json);
Console.WriteLine("\n\n");
```
2. 받아온 토큰을 Bearer 에 묶어 API를 호출한다.
```csharp
// call api
var apiClient = new HttpClient();
apiClient.SetBearerToken(tokenResponse.AccessToken);

var response = await apiClient.GetAsync("https://localhost:6001/identity");
if (!response.IsSuccessStatusCode)
{
    Console.WriteLine(response.StatusCode);
}
else
{
    var content = await response.Content.ReadAsStringAsync();
    Console.WriteLine(JArray.Parse(content));
}
```

___

## 추가설명 

1. 인증을 위한 토큰을 발급하지 않고 API를 불렀을시 '이 요청에 대한 권한 부여가 거부되었습니다.' 라는 메시지가 반환될 것이다.
2. 만약 결과가 제대로 나오면 아래와 비슷할 것이다.
```json
[
    {
        "Type": "nbf",
        "Value": "1625021480"
    },
    {
        "Type": "exp",
        "Value": "1625025080"
    },
    {
        "Type": "iss",
        "Value": "https://localhost:5001"
    },
    {
        "Type": "aud",
        "Value": "https://localhost:5001/resources"
    },
    {
        "Type": "client_id",
        "Value": "client"
    },
    {
        "Type": "jti",
        "Value": "856394E46B525B2E5C8070E2220B09F1"
    },
    {
        "Type": "iat",
        "Value": "1625021480"
    },
    {
        "Type": "scope",
        "Value": "api1"
    }
]
```