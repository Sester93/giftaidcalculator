using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GiftAidCalculator.Tests.Unit
{
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage _fakeResponse;
        public HttpRequestMessage Request { get; private set; }

        public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            return await Task.FromResult(_fakeResponse);
        }
    }
}
