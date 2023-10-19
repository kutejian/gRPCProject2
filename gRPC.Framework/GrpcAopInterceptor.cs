using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.ClientFactory;

namespace gRPC.Framework;

public class GrpcAopInterceptor : Interceptor
{
    /// <summary>
    /// 方法用于拦截同步的单向调用
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP  同步------------开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP 同步------------ 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP 同步------------ 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("****************简单gRPC-Client-AOP 同步------------ 开始*****************");
        var result = base.BlockingUnaryCall(request, context, continuation);
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP  同步------------结束 &&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP 同步------------ 结束 &&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP 同步------------ 结束 &&&&&&&&&&&&&&&&");
        Console.WriteLine("****************简单gRPC-Client-AOP 同步------------ 结束 *****************");
        return result;
    }
    /// <summary>
    /// 方法用于拦截异步的单向调用。
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        this.ClientLogAOP(context.Method);
        return continuation(request, context);
    }
    private void ClientLogAOP<TRequest, TResponse>(Method<TRequest, TResponse> method)
       where TRequest : class
       where TResponse : class
    {
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&简单gRPC-Client-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("****************简单gRPC-Client-AOP 开始*****************");
        Console.WriteLine($"{method.Name}---{method.FullName}--{method.ServiceName}");
        Console.WriteLine($"Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
        Console.WriteLine("****************Client- AOP 结束*****************");
    }
    /// <summary>
    /// 方法用于拦截客户端流调用，比如双向流
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <returns></returns>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        LogAsyncClientStreamingAOP(context.Method);
        return continuation(context);
    }
    private void LogAsyncClientStreamingAOP<TRequest, TResponse>(Method<TRequest, TResponse> method)
     where TRequest : class
     where TResponse : class
    {
        Console.WriteLine("&&&&&&&&&&&&&&&&客户端流式RPC-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&客户端流式RPC-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("&&&&&&&&&&&&&&&&客户端流式RPC-AOP 开始&&&&&&&&&&&&&&&&");
        Console.WriteLine("****************客户端流式RPC-AOP 开始*****************");
        Console.WriteLine($"{method.Name}---{method.FullName}--{method.ServiceName}");
        Console.WriteLine($"Type: {method.Type}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
        Console.WriteLine("****************客户端流式RPC- AOP 结束*****************");
    }
}