using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer(); //인증서버 미들웨어 등록

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
