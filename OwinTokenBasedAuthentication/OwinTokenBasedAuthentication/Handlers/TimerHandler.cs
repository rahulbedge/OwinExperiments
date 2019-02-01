using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OwinTokenBasedAuthentication.Handlers
{
    public class TimerHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start(); 
            var response  = await base.SendAsync(request, cancellationToken);
            watch.Stop();
            response.Headers.Add("x-api-timer", watch.ElapsedMilliseconds + "ms"); 
            return response; 
        }
    }
}