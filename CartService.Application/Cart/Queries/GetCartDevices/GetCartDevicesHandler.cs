using MediatR;
namespace CartService.Application.Cart.Queries.GetCartDevices { public class GetCartDevicesHandler : IRequestHandler<GetCartDevicesQuery, List<string>> { public Task<List<string>> Handle(GetCartDevicesQuery r, CancellationToken ct) => Task.FromResult(new List<string>()); } }
