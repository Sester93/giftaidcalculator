using System;
using System.ComponentModel.DataAnnotations;

namespace GiftAidCalculator.Models
{
    public class GiftAidDeclarationRequest
    {
        public Guid DeclarationId => Guid.NewGuid();

        [Range(minimum:2.00, maximum:100000.00)]
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
    }
}
