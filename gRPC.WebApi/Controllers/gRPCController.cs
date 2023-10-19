using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using static GrpcService.Client.AdvancedGreeter;

namespace gRPC.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class gRPCController : ControllerBase
    {

        private readonly ILogger<gRPCController> _logger;
        private readonly AdvancedGreeterClient _AdvancedGreeterClient;

        public gRPCController(ILogger<gRPCController> logger, AdvancedGreeterClient AdvancedGreeterClient)
        {
            _logger = logger;
            _AdvancedGreeterClient = AdvancedGreeterClient;
        }
        [HttpGet(Name = "Empty")]
        public async Task<string> Empty()
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5238");
            AdvancedGreeterClient client = new AdvancedGreeterClient(channel);
            var countResult = await client.CountAsync(new Empty());
            Console.WriteLine($"���һ�� {countResult.Count}");
            var rand = new Random(DateTime.Now.Millisecond);
            return "Empty return ok";
        }
        [HttpGet(Name = "Nomal")]
        public async Task<string> Nomal()
        {
            Console.WriteLine("**************************************************");
            Console.WriteLine("*****************���ε���*************************");
            Console.WriteLine("**************************************************");
            {
                //HelloReplyMath helloReplyMath = await _AdvancedGreeterClient.SayHelloAsync(new HelloRequestMath()
                //{
                //    Id = 123,
                //    Name = "����"
                //});
                //Console.WriteLine($"Greeterһ���� ���񷵻ص���Ϣ:{helloReplyMath.Message}");

                //using var channel = GrpcChannel.ForAddress("http://localhost:5238");
                //AdvancedGreeterClient client = new AdvancedGreeterClient(channel);
                //var helloReplyMath1 = await client.PlusAsync(new RequestPara()
                //{
                //    ILeft = 1,
                //    IRight = 2
                //});
                //string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU3RldmVuIiwiRU1haWwiOiIxMTExMUBxcS5jb20iLCJBY2NvdW50IjoiU3RldmVuQDEyMzEyMy5jb20iLCJBZ2UiOiIxOCIsIklkIjoiMTIzIiwiTW9iaWxlIjoiMTMzMTIzMTEyMzEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIlNleCI6IjEiLCJuYmYiOjE2ODY0ODMyNDUsImV4cCI6MTY4NjQ4Njc4NSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MjE1IiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MjE1In0.PmDIisMRKn317tdmy_qM-VeLc0IpAPYUtxxq6tqrbLc";
                //HelloReplyMath helloReplyMath1 = _AdvancedGreeterClient.SayHello(new HelloRequestMath()
                //{
                //    Id = 123,
                //    Name = "����"
                //},
                // new Metadata()
                // {
                //     { "Authorization", $"Bearer {token}" }
                // });
                HelloReplyMath helloReplyMath1 = _AdvancedGreeterClient.SayHello(new HelloRequestMath()
                {
                    Id = 123,
                    Name = "����"
                });
                Console.WriteLine($"Greeterͬ���� ���񷵻ص���Ϣ:{helloReplyMath1.Message}");
            }
            return "Nomal return ok";
        }
      
        [HttpGet(Name = "Client")]
        public async Task<string> Client()
        {
            Console.WriteLine("**************************************************");
            Console.WriteLine("*****************Client��*************************");
            Console.WriteLine("**************************************************");
            //using var channel = GrpcChannel.ForAddress("http://localhost:5238");
            //AdvancedGreeterClient client = new AdvancedGreeterClient(channel);
            var bathCat = _AdvancedGreeterClient.SelfIncreaseClient();
            var r = new Random();
            for (int i = 0; i < 10; i++)
            {
                int rNumber = r.Next(0, 20);
                await bathCat.RequestStream.WriteAsync(new BathTheCatReq() { Id = rNumber });
                await Task.Delay(600);
                Console.WriteLine($"current {i}, thread: {Thread.CurrentThread.ManagedThreadId},Number:{rNumber}");
            }

            Console.WriteLine("�������");
            //�������
            await bathCat.RequestStream.CompleteAsync();

            Console.WriteLine("�ͻ����ѷ�����10��id");
            Console.WriteLine("���ս����");


            foreach (var item in bathCat.ResponseAsync.Result.Number)
            {
                Console.WriteLine($"This is {item} Result");
            }
            Console.WriteLine("**********************************");
            return "ok";
        }
        [HttpGet(Name = "Server")]
        public async Task<string> Server()
        {
            //using var channel = GrpcChannel.ForAddress("http://localhost:5238");
            //AdvancedGreeterClient client = new AdvancedGreeterClient(channel);

            Console.WriteLine("**************************************************");
            Console.WriteLine("*****************�������*************************");
            Console.WriteLine("**************************************************");
            //׼���Ĳ���
            IntArrayModel intArrayModel = new IntArrayModel();
            for (int i = 0; i < 15; i++)
            {
                intArrayModel.Number.Add(i);//Number����ֱ�Ӹ�ֵ��
            }
            var bathCat = _AdvancedGreeterClient.SelfIncreaseServer(intArrayModel);

            var bathCatRespTask = Task.Run(async () =>
            {
                await foreach (BathTheCatResq resp in bathCat.ResponseStream.ReadAllAsync())
                {
                    Console.WriteLine($"���ս����{resp.Message},ThreadId: {Thread.CurrentThread.ManagedThreadId}");
                }
            });
            Console.WriteLine("�ͻ����ѷ�����10��id");
            //��ʼ������Ӧ
            await bathCatRespTask;
            return "ok";
        }
        [HttpGet(Name = "Double")]
        public async Task<string> Double()
        {
            //using var channel = GrpcChannel.ForAddress("http://localhost:5238");
            //AdvancedGreeterClient client = new AdvancedGreeterClient(channel);

            Console.WriteLine("**************************************************");
            Console.WriteLine("*****************˫����*************************");
            Console.WriteLine("**************************************************");
            //����˫����
            {
                //����˫����
                var bathCat = _AdvancedGreeterClient.SelfIncreaseDouble();
                //׼����ȡ, ��ȡ����
                var bathCatRespTask = Task.Run(async () =>
                {
                    await foreach (var resp in bathCat.ResponseStream.ReadAllAsync())
                    {
                        Console.WriteLine($"���ս����{resp.Message},ThreadId: {Thread.CurrentThread.ManagedThreadId}");
                    }
                });


                //��������������
                var r = new Random();
                for (int i = 0; i < 10; i++)
                {
                    int rNumber = r.Next(0, 550);
                    await bathCat.RequestStream.WriteAsync(new BathTheCatReq() { Id = rNumber });
                    await Task.Delay(600);
                    Console.WriteLine($"current {i}, thread: {Thread.CurrentThread.ManagedThreadId},Number:{rNumber}");
                }
                //�������
                await bathCat.RequestStream.CompleteAsync();
                Console.WriteLine("�ͻ����ѷ�����10��id");
                Console.WriteLine("���ս����");
                //��ʼ������Ӧ
                await bathCatRespTask;
            }
            return "ok";
        }
    }
}