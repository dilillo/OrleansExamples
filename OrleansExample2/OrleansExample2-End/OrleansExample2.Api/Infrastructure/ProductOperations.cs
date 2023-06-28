using OrleansExample2.Api.Models;

namespace OrleansExample2.Api.Infrastructure
{
    public interface IProductOperations
    {
        Task<ProductDetailsModel> GetDetails(string serialNumber);

        Task Register(string serialNumber, string registerTo);
    }

    public class ProductOperations : IProductOperations
    {
        private readonly IGrainFactory _grains;

        public ProductOperations(IGrainFactory grains)
        {
            _grains = grains;
        }

        public async Task<ProductDetailsModel> GetDetails(string serialNumber)
        {
            var grain = _grains.GetGrain<IProductGrain>(serialNumber);

            var grainState = await grain.GetState();

            return new ProductDetailsModel
            {
                SerialNumber = grain.GetPrimaryKeyString(),
                RegisteredOn = grainState.RegisteredOn,
                RegisteredTo = grainState.RegisteredTo
            };
        }

        public async Task Register(string serialNumber, string registerTo)
        {
            var grain = _grains.GetGrain<IProductGrain>(serialNumber);

            await grain.Register(registerTo);
        }
    }
}
