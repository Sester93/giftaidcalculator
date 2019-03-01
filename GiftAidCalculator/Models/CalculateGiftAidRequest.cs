using System.ComponentModel.DataAnnotations;

namespace GiftAidCalculator.Models
{
    public class CalculateGiftAidRequest
    {
        [Range(minimum: 2.00, maximum: 100000.00)]
        public decimal Amount { get; set; }
    }
}
