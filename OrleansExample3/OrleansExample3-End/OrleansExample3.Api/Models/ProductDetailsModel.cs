namespace OrleansExample3.Api.Models
{
    public class ProductDetailsModel
    {
        public required string SerialNumber { get; set; }

        public DateTime? RegisteredOn { get; set; }

        public string? RegisteredTo { get; set; }
    }
}
