using System.ComponentModel.DataAnnotations;

namespace OrleansExample2.Api.Models
{
    public class ProductRegistrationPatchRequestBodyModel
    {
        [Required]
        public required string SerialNumber { get; set; }

        [Required]
        public required string RegisterTo { get; set; }
    }
}
