using Orleans.Runtime;

namespace OrleansExample3.Api.Infrastructure
{
    public interface IProductGrain : IGrainWithStringKey
    {
        Task<ProductGrainState> GetState();

        Task Register(string registerTo);
    }

    public class ProductGrain : Grain, IProductGrain
    {
        private readonly ILogger<ProductGrain> _logger;
        private readonly IPersistentState<ProductGrainState> _state;

        public ProductGrain(
            ILogger<ProductGrain> logger,
            [PersistentState(stateName: "product", storageName: "products")] IPersistentState<ProductGrainState> state)
        {
            _logger = logger;
            _state = state;
        }

        public Task<ProductGrainState> GetState() => Task.FromResult(_state.State);

        public async Task Register(string registerTo)
        {
            var canRegister = await ComplexTimeConsumingOrUnreliableBusinessLogic();

            if (canRegister)
            {
                _state.State = new()
                {
                    RegisteredOn = DateTime.Now,
                    RegisteredTo = registerTo
                };

                await _state.WriteStateAsync();

                _logger.LogInformation("Registered product {SerialNumber} to {RegisteredTo}", this.GetPrimaryKeyString(), _state.State.RegisteredTo);
            }
        }

        private const int ComplexTimeConsumingOrUnreliableBusinessLogicDelay = 0;

        private async Task<bool> ComplexTimeConsumingOrUnreliableBusinessLogic()
        {
            await Task.Delay(ComplexTimeConsumingOrUnreliableBusinessLogicDelay);

            return _state.State?.RegisteredOn == null;
        }
    }

    [GenerateSerializer]
    public record class ProductGrainState
    {
        [Id(0)]
        public DateTime? RegisteredOn { get; set; }

        [Id(1)]
        public string? RegisteredTo { get; set; }
    }
}