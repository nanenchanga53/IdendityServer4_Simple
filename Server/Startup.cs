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
                ) // �������� ��Ϲ�� ����(JWT ����� ���� ������ Framework�� ȣȯ�� ����)
                .AddDeveloperSigningCredential() //���߽ÿ��� �������(������ �������� �ȵǴ� ������ ���ԵǾ� ������).
                .AddInMemoryApiScopes(Config.ApiScopes) //API SCOPE ���
                .AddInMemoryClients(Config.Clients); //Ŭ���̾�Ʈ ���

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer(); //�������� �̵���� ���

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
