using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromotionService.Application.Interfaces;
using PromotionService.Domain.Entities;

namespace PromotionService.Application.Promotions.Queries.GetPromotionsBySeller
{
    public class GetPromotionsBySellerQuery : IRequest<List<Promotion>>
    {
        public Guid SellerId { get; set; }
    }
}
