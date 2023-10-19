
using gRPC.Framework;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static GrpcService.Client.AdvancedGreeter;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Grpc
//添加依赖注入IOC
builder.Services.AddGrpcClient<AdvancedGreeterClient>(opt =>
{
    //opt.Address = new Uri("http://localhost:5238");
    opt.Address = new Uri("https://localhost:5239");
    //opt.InterceptorRegistrations.Add(new GrpcAopInterceptor());
    opt.Interceptors.Add(new GrpcAopInterceptor());
})
    .ConfigureChannel(grpcOptions =>
{
    grpcOptions.HttpHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
                (message, certificate2, arg3, arg4) => true  //忽略掉证书异常
    };

    var callCredentials = CallCredentials.FromInterceptor(async (context, metadata) =>
    {
        //string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU3RldmVuIiwiRU1haWwiOiIxMTExMUBxcS5jb20iLCJBY2NvdW50IjoiU3RldmVuQDEyMzEyMy5jb20iLCJBZ2UiOiIxOCIsIklkIjoiMTIzIiwiTW9iaWxlIjoiMTMzMTIzMTEyMzEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIlNleCI6IjEiLCJuYmYiOjE2ODY0ODAwNDIsImV4cCI6MTY4NjQ4MzU4MiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MjE1IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MjE1In0.QNPklR0YgoZ6d8joAIVaBDSN3lPd3RjkaXqCI5KNCRs";
        string token = await JWTTokenHelper.GetJWTToken();
        Console.WriteLine($"token:{token}");
        metadata.Add("Authorization", $"Bearer {token}");
        await Task.CompletedTask;
    });

    grpcOptions.Credentials = ChannelCredentials.Create(new SslCredentials(), callCredentials);
    //请求都带上token，也可以在调用方法时传递： var replyPlus = await client.PlusAsync(requestPara, headers);

});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
