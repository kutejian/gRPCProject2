using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.DefaultServer;
using Grpc.Net.Client;
using GrpcService.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Grpc.DefaultServer.Greeter;
using static GrpcService.Client.AdvancedGreeter;

namespace gRPC.MyClient;

public class gRPCRequest
{
    public async Task TestHello()
    {
        using var channel = GrpcChannel.ForAddress(address: " http://localhost:5238");
        GreeterClient greeterClient = new GreeterClient(channel);
        var res1 = greeterClient.SayHello(new HelloRequest
        {
            Name = "ola ola steven 111111111111"
        });
        Console.WriteLine("返回的结果：" + res1.Message);

        var res = await greeterClient.SayHelloAsync(new HelloRequest
        {
            Name = "ola ola steven 222222222222222"
        });
        Console.WriteLine("返回的结果：" + res.Message);
        Console.ReadLine();
    }
    public async Task TestgRPCService()
    {
        using (var channel = GrpcChannel.ForAddress("http://localhost:5238"))
        {
            AdvancedGreeterClient client = new AdvancedGreeterClient(channel);
            #region 普通调用
            //Console.WriteLine("**************************************************");
            //Console.WriteLine("*****************单次调用*************************");
            //Console.WriteLine("**************************************************");
            //{
            //    HelloReplyMath helloReplyMath = client.SayHello(new HelloRequestMath()
            //    {
            //        Id = 123,
            //        Name = "天蓝"
            //    });
            //    Console.WriteLine($"Greeter 服务返回的消息:{helloReplyMath.Message}");


            //    HelloReplyMath helloReplyMath1 = await client.SayHelloAsync(new HelloRequestMath()
            //    {
            //        Id = 123,
            //        Name = "天蓝asdadadadadadaad"
            //    });
            //    Console.WriteLine($"Greeter 服务返回的消息:{helloReplyMath1.Message}");
            //}
            #endregion

            #region 空参数
            //Console.WriteLine("**************************************************");
            //Console.WriteLine("*****************空参数*************************");
            //Console.WriteLine("**************************************************");
            //var countResult = await client.CountAsync(new Empty());
            //Console.WriteLine($"随机一下 {countResult.Count}");
            //var rand = new Random(DateTime.Now.Millisecond);
            #endregion

            #region 客户端流

            //Console.WriteLine("**************************************************");
            //Console.WriteLine("*****************客户端流*************************");
            //Console.WriteLine("**************************************************");

            //var bathCat = client.SelfIncreaseClient();
            //var r = new Random();
            //for (int i = 0; i < 10; i++)
            //{
            //    int rNumber = r.Next(0, 20);
            //    await bathCat.RequestStream.WriteAsync(new BathTheCatReq() { Id = rNumber });
            //    await Task.Delay(600);
            //    Console.WriteLine($"current {i}, thread: {Thread.CurrentThread.ManagedThreadId},Number:{rNumber}");
            //}

            //Console.WriteLine("**********************************");
            ////发送完毕
            //await bathCat.RequestStream.CompleteAsync();

            //Console.WriteLine("客户端已发送完10个id");
            //Console.WriteLine("接收结果：");


            //foreach (var item in bathCat.ResponseAsync.Result.Number)
            //{
            //    Console.WriteLine($"This is {item} Result");
            //}
            //Console.WriteLine("**********************************");
            #endregion

            #region 服务端流

            //Console.WriteLine("**************************************************");
            //Console.WriteLine("*****************服务端流*************************");
            //Console.WriteLine("**************************************************");
            //{
            //    //准备的参数
            //    IntArrayModel intArrayModel = new IntArrayModel();
            //    for (int i = 51; i < 61; i++)
            //    {
            //        intArrayModel.Number.Add(i);//Number不能直接赋值，
            //    }
            //    var bathCat = client.SelfIncreaseServer(intArrayModel);
            //    Console.WriteLine("客户端已发送完10个id");
            //    var bathCatRespTask = Task.Run(async () =>
            //    {
            //        await foreach (var resp in bathCat.ResponseStream.ReadAllAsync())
            //        {
            //            Console.WriteLine($"接收结果：{resp.Message},ThreadId: {Thread.CurrentThread.ManagedThreadId}");
            //        }
            //    });
            //    //开始接收响应
            //    await bathCatRespTask;

            //}
            #endregion

            #region 双向流

            Console.WriteLine("**************************************************");
            Console.WriteLine("*****************双向流*************************");
            Console.WriteLine("**************************************************");
            {
                //调用双向流
                var bathCat = client.SelfIncreaseDouble();
                //准备读取, 获取数据
                var bathCatRespTask = Task.Run(async () =>
                {
                    await foreach (var resp in bathCat.ResponseStream.ReadAllAsync())
                    {
                        Console.WriteLine($"接收结果：{resp.Message},ThreadId: {Thread.CurrentThread.ManagedThreadId}");
                    }
                });


                //传入多个多服务端流
                var r = new Random();
                for (int i = 0; i < 10; i++)
                {
                    int rNumber = r.Next(0, 550);
                    await bathCat.RequestStream.WriteAsync(new BathTheCatReq() { Id = rNumber });
                    await Task.Delay(600);
                    Console.WriteLine($"current {i}, thread: {Thread.CurrentThread.ManagedThreadId},Number:{rNumber}");
                }
                //发送完毕
                await bathCat.RequestStream.CompleteAsync();
                Console.WriteLine("客户端已发送完10个id");
                Console.WriteLine("接收结果：");
                //开始接收响应
                await bathCatRespTask;
            }
            #endregion

        }
    }

}
