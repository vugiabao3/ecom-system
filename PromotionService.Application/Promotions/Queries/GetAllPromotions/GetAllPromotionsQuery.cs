using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace PromotionService.Application.Promotions.Queries.GetAllPromotions
{
    public class GetAllPromotionsQuery
     : IRequest<List<PromotionDto>>
    {
    }
}
