# IdentityServer4 와 OAuth2.0 인증 API 사용(.NetFrameWork 4.X 버전)
작성자 : 김우창

메일 : nanenchanga2@gmail.com

[<span style="color:rgb(255, 100, 100)">참조사이트</span>] https://identityserver4.readthedocs.io/en/latest/index.html

## 기본설정과 실행을 위한 속성
1. 새 프로젝트 -> .NET WebFrameWork 웹 프로젝트 -> 비어있음
2. 대상 프레임워크 4.7.2 
3. 설치 NugetPakage
    
    * Microsoft.Owin.Host.SystemWeb
    * Microsoft.AspNet.WebApi.Owin
    * IdentityServer3.Contrib.AccessTokenValidation

4. 추가하는 파일은 OWIN시작 클래스 [ <span style="color:rgb(255,255,0)">Startup.cs</span> ], 비어있는 API Controller 2 [ <span style="color:rgb(255,255,0)">IdentityController.cs</span> ]두가지다
![OWIN시작클래스](https://user-images.githubusercontent.com/39551265/123902756-5ba1de00-d9a8-11eb-8295-91d9f3647544.JPG)


## IdentityServer4 Server와의 OAuth2.0 인증 설정
1. <span style="color:rgb(255,2555,0)">startup.cs</span> 파일에서 <span style="color:rgb(0,2555,255)">ConfigureServices(</span> 매서를 추가하여 안에 인증에 대한 정의를 추가한다.(아래는 BearerToken을 통해 인증을 하는 app 설정을 추가하는 것)
```csharp
public void Configuration(IAppBuilder app)
{
    //Jwt 설정에 대해 초기화
    JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
    app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions()
    {
        Authority = "https://localhost:5001", //인증 Endpoint
        //ClientId = "client1",
        //ClientSecret = "secret",
        RequiredScopes = new[] { "api1" } // API 에서 사용될 Scope 묶음

    });

    var config = new HttpConfiguration();
    config.MapHttpAttributeRoutes();

    // 인증 방식 등록
    config.Filters.Add(new AuthorizeAttribute());

    app.UseWebApi(config);
}
```
2. 이제 각 컨트롤러에 해당되는 클래스 혹은 메서드 위에 [Authorize] 라는 문구를 추가하면 각 API에 해당되는 메서드나 클래스에 대하여 인증절차를 걸치게 된다. 아래는 그예제다
```csharp
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Api.FrameWork.Controllers
{

    public class IdentityController : ApiController
    {
        [Route("Identity")]
        [Authorize]
        public IHttpActionResult Get()
        {
            var caller = User as ClaimsPrincipal;
            
            return Json(from c in caller.Claims select new { c.Type, c.Value });
        }

    }
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