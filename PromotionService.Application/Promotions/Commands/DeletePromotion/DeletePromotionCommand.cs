using MediatR;
using PromotionService.Application.Promotions.Queries.GetAllPromotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PromotionService.Application
.Promotions.Commands.DeletePromotion;

public class DeletePromotionCommand: IRequest<DeletePromotionResponse>
{
    public Guid Id { get; set; }
}