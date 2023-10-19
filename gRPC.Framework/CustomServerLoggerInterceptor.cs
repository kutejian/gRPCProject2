using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace gRPC.Framework;

public class CustomServerLoggerInterceptor : Interceptor
{

    private readonly ILogger<CustomServerLoggerInterceptor> _logger;

    public CustomServerLoggerInterceptor(ILogger<CustomServerLoggerInterceptor> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 方法用于拦截单向调用，
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        ServerLogAOP<TRequest, TResponse>(MethodType.Unary, context);
        var r = continuation(request, context);
        ServerLogAOPFinish<TRequest, TResponse>(MethodType.Unary, context);
        return r;
    }
    /// <summary>
    /// 用于拦截服务端流调用
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        Console.WriteLine("&&&&&&&&&&&&&&&&服务端流调用 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&服务端流调用 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&服务端流调用 开始&&&&&&&&&&&&&&&&");
        return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
    }
    /// <summary>
    /// 用于拦截双向流调用
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        Console.WriteLine("&&&&&&&&&&&&&&&&Server双向流-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&Server双向流-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&Server双向流-AOP 开始&&&&&&&&&&&&&&&&");
        return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
    }
    private void ServerLogAOP<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
        where TRequest : class
        where TResponse : class
    {
        Console.WriteLine("&&&&&&&&&&&&&&&&Server-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&Server-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&Server-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("****************AOP 开始*****************");
        Console.WriteLine($"{context.RequestHeaders[0]}---{context.Host}--{context.Method}--{context.Peer}");
        Console.WriteLine($"Type: {methodType}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
        Console.WriteLine("****************Server-AOP 结束*****************");
    }

    private void ServerLogAOPFinish<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
  where TRequest : class
  where TResponse : class
    {
        Console.WriteLine("&&&&&&&&&&&&&&&&Server-AOP 结束&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&Server-AOP 结束&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&Server-AOP 结束&&&&&&&&&&&&&&&&");
        Console.WriteLine("****************AOP 结束*****************");
        Console.WriteLine($"{context.RequestHeaders[0]}---{context.Host}--{context.Method}--{context.Peer}");
        Console.WriteLine($"Type: {methodType}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
        Console.WriteLine("****************Server-AOP 结束*****************");
    }
}