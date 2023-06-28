using OrleansExample1.Api.Models;

namespace OrleansExample1.Api.Infrastructure
{
    public interface IProductOperations
    {
        Task<ProductDetailsModel> GetDetails(string serialNumber);

        Task Register(string serialNumber, string registerTo);
    }

    public class ProductOperations : IProductOperations
    {
        public Task<ProductDetailsModel> GetDetails(string serialNumber) => Task.FromResult(
            new ProductDetailsModel
            {
                SerialNumber = serialNumber
            });
        

        public Task Register(string serialNumber, string registerTo) => Task.CompletedTask;
    }
}
