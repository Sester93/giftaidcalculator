using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GiftAidCalculator.Config;
using GiftAidCalculator.Models;
using GiftAidCalculator.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GiftAidCalculator.Tests.Unit.Services
{
    public class PostCodeValidatorTests
    {
        private PostCodeValidator _postCodeValidator;
        private HttpClient _httpClient;
        private HttpMessageHandler _fakeMessageHandler;
        private IOptions<PostCodeValidationConfig> _config;

        [SetUp]
        public void SetUp()
        {
            _config = Options.Create(new PostCodeValidationConfig{BaseUrl = "https://baseurl.com"});
        }

        [Test]
        public async Task Validate_ValidPostCode_ReturnsValidResult()
        {
            var validApiResponse = new PostCodeValidationResponse
            {
                result = true
            };
            SetupHttpClient(validApiResponse);
            _postCodeValidator = new PostCodeValidator(_httpClient, _config);

            var response = await _postCodeValidator.Validate("postCode");

            Assert.IsTrue(response.result);
        }

        [Test]
        public async Task Validate_InvalidPostCode_ReturnsInvalidResult()
        {
            var validApiResponse = new PostCodeValidationResponse
            {
                result = false
            };
            SetupHttpClient(validApiResponse);
            _postCodeValidator = new PostCodeValidator(_httpClient, _config);

            var response = await _postCodeValidator.Validate("postCode");

            Assert.IsFalse(response.result);
        }

        private void SetupHttpClient(PostCodeValidationResponse mockResponse)
        {
            _fakeMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(mockResponse))
            });

            _httpClient = new HttpClient(_fakeMessageHandler);
        }
    }
}
