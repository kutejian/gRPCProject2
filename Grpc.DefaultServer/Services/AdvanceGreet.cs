using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.DefaultServer;
using GrpcService.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Grpc.DefaultServer.Services;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdvancedGreeterService : AdvancedGreeter.AdvancedGreeterBase
{
    private readonly ILogger<AdvancedGreeterService> _logger;
    public AdvancedGreeterService(ILogger<AdvancedGreeterService> logger)
    {
        _logger = logger;
    }

    public override async Task<ResponseResult> Plus(RequestPara request, ServerCallContext context)
    {
        return await Task.FromResult(new ResponseResult
        {
            Message = "1",
            Result = 2
        });
    }
    public override async Task<HelloReplyMath> SayHello(HelloRequestMath request, ServerCallContext context)
    {
        return await Task.FromResult(new HelloReplyMath
        {
            Message = "Hello steven"
        });
    }

    public override async Task<IntArrayModel> SelfIncreaseClient(IAsyncStreamReader<BathTheCatReq> requestStream, ServerCallContext context)
    {
        IntArrayModel intArrayModel = new IntArrayModel();
        while (await requestStream.MoveNext())
        {
            intArrayModel.Number.Add(requestStream.Current.Id);
            Console.WriteLine($"SelfIncreaseClient Number： {requestStream.Current.Id} 全部获取到，且处理");
            await Task.Delay(100);
        }
        return await Task.FromResult(intArrayModel);
    }
    public override async Task SelfIncreaseServer(IntArrayModel request, IServerStreamWriter<BathTheCatResq> responseStream, ServerCallContext context)
    {
        foreach (var item in request.Number)
        {
            Console.WriteLine($"Server current {item} ");
            await responseStream.WriteAsync(new BathTheCatResq()
            {
                Message = $"number+={item}"
            });
            //Thread.Sleep(500);
            await Task.Delay(500);
        }
    }

    public override async Task SelfIncreaseDouble(IAsyncStreamReader<BathTheCatReq> requestStream, IServerStreamWriter<BathTheCatResq> responseStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext())
        {
            Console.WriteLine($"SelfIncreaseDouble Number {requestStream.Current.Id} 获取到并处理.");
            await responseStream.WriteAsync(new BathTheCatResq() { Message = $"number++ ={requestStream.Current.Id + 1}！" });
            await Task.Delay(500);//此处主要是为了方便客户端能看出流调用的效果
        }
    }
    /// <summary>
    /// 空参数
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<CountResult> Count(Empty request, ServerCallContext context)
    {

        Console.WriteLine("mei有任何参数, 请求来了~~");
        return await Task.FromResult(new CountResult()
        {
            Count = 77777
        });
    }
}
