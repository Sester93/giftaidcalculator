namespace GiftAidCalculator
{
    public interface IGiftAidCalculator
    {
        decimal Calculate(decimal donationAmount);
    }

    public class GiftAidCalculator : IGiftAidCalculator
    {
        private const decimal TaxRate = 20m;

        public decimal Calculate(decimal donationAmount)
        {
            return donationAmount * (TaxRate / (100m - TaxRate));
        }
    }
}
