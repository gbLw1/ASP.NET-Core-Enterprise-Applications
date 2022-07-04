using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace NSE.WebAPI.Core.Extensions;

public static class PollyExtensions
{
    /* ------------------ (https://github.com/App-vNext/Polly) ------------------ */
    public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
    {
        var retry = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            });

        return retry;
    }
}
