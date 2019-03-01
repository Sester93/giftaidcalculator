using System;
using System.Threading.Tasks;
using GiftAidCalculator.Controllers;
using GiftAidCalculator.Models;
using GiftAidCalculator.Persistance;
using GiftAidCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using NSubstitute;

namespace GiftAidCalculator.Tests.Unit.Controllers
{
    public class GiftAidControllerTests
    {
        private GiftAidController _controller;
        private IGiftAidCalculator _giftAidCalculator;
        private IGiftAidRepository _giftAidRepository;
        private IPostCodeValidator _postCodeValidator;

        [SetUp]
        public void SetUp()
        {
            _giftAidCalculator = Substitute.For<IGiftAidCalculator>();
            _giftAidRepository = Substitute.For<IGiftAidRepository>();
            _postCodeValidator = Substitute.For<IPostCodeValidator>();
            _postCodeValidator.Validate(Arg.Any<string>())
                .Returns(new PostCodeValidationResponse {result = true, status = 200});
        }

        [Test]
        public void Get_ValidAmount_ReturnsGiftAidAmount()
        {
            var request = new CalculateGiftAidRequest
            {
                Amount = 100m
            };
            _giftAidCalculator.Calculate(request.Amount).Returns(20);
            _controller = new GiftAidController(_giftAidCalculator, _giftAidRepository, _postCodeValidator);

            var response = _controller.Get(request) as JsonResult;
            var giftAidResponse = response.Value as CalculateGiftAidResponse;

            _giftAidCalculator.Received(1).Calculate(request.Amount);

            Assert.That(giftAidResponse.GiftAidAmount, Is.EqualTo(20));
        }

        [Test]
        public async Task Post_ValidRequest_ReturnsGiftAidDeclaration()
        {
            var request = new GiftAidDeclarationRequest
            {
                Amount = 100m,
                Name = "name",
                PostCode = "postCode"
            };
            _giftAidCalculator.Calculate(request.Amount).Returns(20);
            _controller = new GiftAidController(_giftAidCalculator, _giftAidRepository, _postCodeValidator);

            var response = await _controller.PostAsync(request) as JsonResult;
            var declarationResponse = response.Value as GiftAidDeclaration;

            await _giftAidRepository.Received(1).CreateGiftAidDeclaration(Arg.Is(request));
            _giftAidCalculator.Received(1).Calculate(request.Amount);

            Assert.That(declarationResponse.GiftAidAmount, Is.EqualTo(20));
            Assert.That(declarationResponse.DeclarationId.GetType(), Is.EqualTo(typeof(Guid)));
        }
    }
}
