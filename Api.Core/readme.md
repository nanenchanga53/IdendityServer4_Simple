# IdentityServer4 와 OAuth2.0 인증 API 사용(.Core3.X 버전)
작성자 : 김우창

메일 : nanenchanga2@gmail.com

[<span style="color:rgb(255, 100, 100)">참조사이트</span>] https://identityserver4.readthedocs.io/en/latest/index.html

## 기본설정과 실행을 위한 속성
1. 새 프로젝트 -> 비어있음(웹)
2. 대상 프레임워크 Core3.1
3. 설치 NugetPakage
    * IdentityServer4.AccessTokenValidation
4. 'MVC Contorller 비어있음'으로 컨트롤러를 추가한다(API 역할) [ <span style="color:rgb(255,255,0)">IdentityController.cs</span> ]


## IdentityServer4 Server와의 OAuth2.0 인증 설정
1. <span style="color:rgb(255,2555,0)">startup.cs</span> 파일에서 <span style="color:rgb(0,2555,255)">ConfigureServices(</span> 매서를 추가하여 안에 인증에 대한 설정을 정의한다.(아래는 BearerToken을 통해 인증을 하는 app 설정을 추가하는 것)
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();

    // 서버에서 토큰을 취득하기 위한 방식 설정
    services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = "https://localhost:5001/";

            //options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
        });

    // 해당 프로젝트 인증에 사용할 scope 'api1' 등록
    services.AddAuthorization(options =>
    {
        options.AddPolicy("ApiScope", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", "api1");
        });
    });
}
```
2. 이후 미들웨어에 등록하기위해 'app.UseAuthertication()'으로 인증 절차를 등록하고 엔드포인트에 설정한 ApiScope를 지정해 준다.
```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseRouting();

    // 인증에 대한 미들웨어 등록
    app.UseAuthentication();
    app.UseAuthorization();

    //미들웨어에 ApiScope에 대한 엔드 포인트 등록
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers()
            .RequireAuthorization("ApiScope");
    });
}
```
3. 이제 각 컨트롤러에 해당되는 클래스 혹은 메서드 위에 [Authorize] 라는 문구를 추가하면 각 API에 해당되는 메서드나 클래스에 대하여 인증절차를 걸치게 된다. 아래는 그예제다
```csharp
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [Route("identity")]
    [Authorize] //인증 사용
    public class IdentityController : Controller
    {
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
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