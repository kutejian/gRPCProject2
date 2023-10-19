using gRPC.Framework;
using Grpc.DefaultServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
//builder.Services.AddGrpc();
#region AdvancedGreeterService

builder.Services.AddGrpc(options =>
{
    //支持服务端AOP注册
    options.Interceptors.Add<CustomServerLoggerInterceptor>();
});
#endregion

#region jwt授权
JWTTokenOptions tokenOptions = new JWTTokenOptions();
builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//Scheme
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //JWT有一些默认的属性，就是给鉴权时就可以筛选了
            ValidateIssuer = true,//是否验证Issuer
            ValidateAudience = true,//是否验证Audience
            ValidateLifetime = true,//是否验证失效时间
            ValidateIssuerSigningKey = true,//是否验证SecurityKey
            ValidAudience = tokenOptions.Audience,//
            ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//拿到SecurityKey
        };
    });
builder.Services.AddAuthorization();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<AdvancedGreeterService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
#region 鉴权授权
app.UseAuthentication(); //鉴权
app.UseAuthorization(); //授权
#endregion
app.Run();
