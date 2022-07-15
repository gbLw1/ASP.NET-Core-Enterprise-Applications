using Core.Messages.Integration;
using EasyNetQ;
using EasyNetQ.Internals;
using Polly;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace NSE.MessageBus;

public class MessageBus : IMessageBus
{
    private IBus? _bus;
    private IAdvancedBus _advancedBus;
    private readonly string _connectionString;

    public MessageBus(string connectionString)
    {
        _connectionString = connectionString;
        TryConnect();
    }

    public bool IsConnected => _bus?.Advanced.IsConnected ?? false;

    public IAdvancedBus AdvancedBus => _bus?.Advanced ?? throw new NullReferenceException(nameof(_bus));

    public void Publish<T>(T message) where T : IntegrationEvent
    {
        TryConnect();
        _bus.PubSub.Publish(message);
    }

    public async Task PublishAsync<T>(T message) where T : IntegrationEvent
    {
        TryConnect();
        await _bus.PubSub.PublishAsync(message);
    }

    public TResponse Request<TRequest, TResponse>(TRequest request)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus.Rpc.Request<TRequest, TResponse>(request);
    }

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return await _bus.Rpc.RequestAsync<TRequest, TResponse>(request);
    }

    public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
    {
        TryConnect();
        _bus.PubSub.Subscribe(subscriptionId, onMessage);
    }

    public void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
    {
        TryConnect();
        _bus.PubSub.SubscribeAsync(subscriptionId, onMessage);
    }

    public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus.Rpc.Respond(responder);
    }

    public AwaitableDisposable<IDisposable> RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus.Rpc.RespondAsync(responder);
    }

    [MemberNotNull(nameof(_bus), nameof(_advancedBus))]
    private void TryConnect()
    {
        if (IsConnected && _bus is not null && _advancedBus is not null)
        {
            return;
        }

        var policy = Policy.Handle<EasyNetQException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetry(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));


        policy.Execute([MemberNotNull(nameof(_bus), nameof(_advancedBus))] () =>
        {
            _bus = RabbitHutch.CreateBus(_connectionString);
            _advancedBus = _bus.Advanced;
            _advancedBus.Disconnected += OnDisconnect;
        });

        if (_bus is null || _advancedBus is null)
        {
            throw new NullReferenceException(nameof(_bus));
        }
    }

    private void OnDisconnect(object? s, EventArgs e)
    {
        var policy = Policy.Handle<EasyNetQException>()
            .Or<BrokerUnreachableException>()
            .RetryForever();

        policy.Execute(TryConnect);
    }

    public void Dispose()
    {
        _bus?.Dispose();
    }
}
