using NUnit.Framework;

namespace GiftAidCalculator.Tests.Unit
{
    public class GiftAidCalculatorTests
    {
        private IGiftAidCalculator _giftAidCalculator;

        [SetUp]
        public void SetUp()
        {
            _giftAidCalculator = new GiftAidCalculator();
        }

        [TestCase(100.0, 25.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(-10.0, -2.5)]
        public void Calculate_DonationAmount_ReturnsCorrectGiftAidAmount(decimal donationAmount, decimal giftAidAmount)
        {
            var result = _giftAidCalculator.Calculate(donationAmount);

            Assert.That(result, Is.EqualTo(giftAidAmount));
        }
    }
}
