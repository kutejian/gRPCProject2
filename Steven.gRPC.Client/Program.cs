using Grpc.DefaultServer;
using Grpc.Net.Client;
using static Grpc.DefaultServer.Greeter;

using var channel = GrpcChannel.ForAddress(address: " http://localhost:5238");
GreeterClient greeterClient = new GreeterClient(channel);
var res = await greeterClient.SayHelloAsync(new HelloRequest
{
    Name = "------------Steven------------"
});
HelloReply res1 = greeterClient.SayHello(new HelloRequest
{
    Name = "------------Steven------------同步方法"
});
Console.WriteLine("返回的结果：" + res.Message);
Console.WriteLine("返回的结果：" + res1.Message);
Console.ReadKey();