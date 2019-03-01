using System.Threading.Tasks;
using GiftAidCalculator.Models;
using GiftAidCalculator.Persistance;
using GiftAidCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GiftAidCalculator.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : Controller
    {
        private readonly IGiftAidCalculator _giftAidCalculator;
        private readonly IGiftAidRepository _giftAidRepository;
        private readonly IPostCodeValidator _postCodeValidator;

        public GiftAidController(IGiftAidCalculator giftAidCalculator, IGiftAidRepository giftAidRepository, IPostCodeValidator postCodeValidator)
        {
            _giftAidCalculator = giftAidCalculator;
            _giftAidRepository = giftAidRepository;
            _postCodeValidator = postCodeValidator;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]CalculateGiftAidRequest request)
        {
            Log.Information($"Received {nameof(CalculateGiftAidRequest)}: {@request}", request);

            var response = new CalculateGiftAidResponse
            {
                DonationAmount = request.Amount,
                GiftAidAmount = _giftAidCalculator.Calculate(request.Amount)
            };

            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]GiftAidDeclarationRequest request)
        {
            Log.Information($"Received {nameof(GiftAidDeclarationRequest)}: {@request}", request);

            var validationResult = await _postCodeValidator.Validate(request.PostCode);

            if (!validationResult.result)
            {
                return BadRequest(new { error = "Invalid PostCode"});
            }

            await _giftAidRepository.CreateGiftAidDeclaration(request);

            var response = new GiftAidDeclaration
            {
                DeclarationId = request.DeclarationId,
                GiftAidAmount = _giftAidCalculator.Calculate(request.Amount)
            };

            return new JsonResult(response);
        }
    }
}