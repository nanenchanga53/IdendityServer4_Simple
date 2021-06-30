# IdentitySever4 서버 기본 작성 방법

[<span style="color:rgb(255, 100, 100)">참조사이트</span>] https://identityserver4.readthedocs.io/en/latest/index.html

1. Core3.1 버전으로 (웹)프로젝트 시작
2. Nuget Package 설치 - <span style="color:rgb(255,0,255)">IdentityServer4 </span>
3. <span style="color:rgb(255,2555,0)">launchSettings.json</span> 파일에서 Selfhost를 실행하도록 변경

```json
 {
  "profiles": {
    "SelfHost": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:5001"
    }
  }
}
```

4. 우선 사용자 정보를 정의하기 위한 모델을 정의한다. <span style="color:rgb(255,2555,0)">Config.cs</span>클래스를 생성하여 사용자 정보, 사용API SCOPE에 대한 정보 모델을 생성한다(DB를 사용한다면 불러오는 소스코드 혹은 등록, 삭제하는 코드를 구현한다)
```c#

public static IEnumerable<ApiScope> ApiScopes =>
    new List<ApiScope>
    {
        new ApiScope("api1", "My API")
    };

//클라이언트 정보 정의
public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            // 클라이언트 ID
            ClientId = "client",

            // 아래방식은 유저등록을 따로 하지 않을때(OpenID) 사용 방식
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            ClientSecrets =
            {
                new Secret("secret".Sha256()) //암호/ 암호 저장 방식
            },

            AllowedScopes = { "api1" } //해당 Scope
        }
    };
```
5. <span style="color:rgb(255,2555,0)">startup.cs</span> 파일에서 <span style="color:rgb(0,2555,255)">ConfigureServices(</span> 매서드 안에 아래의 소스를 추가하여 인증에 대한 정의를 추가한다

```c#
var builder = services.AddIdentityServer(
                    options =>
                    {
                        options.AccessTokenJwtType = "JWT";
                        options.EmitStaticAudienceClaim = true;
                    }
                ) // 인증서비스 등록방식 생성(JWT 방식을 쓰는 이유는 Framework와 호환을 위해)
                .AddDeveloperSigningCredential() //개발시에만 사용하자(배포시 보여지면 안되는 정보도 포함되어 보여짐).
                .AddInMemoryApiScopes(Config.ApiScopes) //API SCOPE 등록
                .AddInMemoryClients(Config.Clients); //클라이언트 등록
```


6. 이어서 <span style="color:rgb(0,2555,255)">Configure</span> 매서드 안에 아래의 소스를 추가하여 미들웨어를 등록한다
```csharp
app.UseIdentityServer();
```
<hr>
## 추가 설명
1. 콘솔로 시작되어 그 안에 실행하는 로그를 보려면 Nuget Package <span style="color:rgb(255,0,255)">Serilog.AspNetCore</span>를 설치한다
2. [http://localhost:[설정된 포트]/.well-known/openid-configuration]를 통해 현재 등록된 정보들의 대략적인 정보들을 볼 수 있다 