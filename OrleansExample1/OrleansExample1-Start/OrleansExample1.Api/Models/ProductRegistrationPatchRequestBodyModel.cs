using System.ComponentModel.DataAnnotations;

namespace OrleansExample1.Api.Models
{
    public class ProductRegistrationPatchRequestBodyModel
    {
        [Required]
        public required string SerialNumber { get; set; }

        [Required]
        public required string RegisterTo { get; set; }
    }
}
