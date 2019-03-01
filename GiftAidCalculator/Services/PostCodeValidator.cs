using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GiftAidCalculator.Config;
using GiftAidCalculator.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace GiftAidCalculator.Services
{
    public interface IPostCodeValidator
    {
        Task<PostCodeValidationResponse> Validate(string postCode);
    }

    public class PostCodeValidator : IPostCodeValidator
    {
        private readonly PostCodeValidationConfig _config;
        private readonly HttpClient _httpClient;

        public PostCodeValidator(HttpClient httpClient, IOptions<PostCodeValidationConfig> config)
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<PostCodeValidationResponse> Validate(string postCode)
        {
            var uri = new Uri($"{_config.BaseUrl}/postcodes/{postCode}/validate");
            var response = await _httpClient.GetAsync(uri, new CancellationToken());

            if (!response.IsSuccessStatusCode)
            {
                Log.Information($"Invalid PostCode: {postCode}", postCode);
                return null;
            }

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PostCodeValidationResponse>(stringContent);
        }
    }
}
