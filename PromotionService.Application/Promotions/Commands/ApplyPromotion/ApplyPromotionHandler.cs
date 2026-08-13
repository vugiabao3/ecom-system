using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromotionService.Application.Interfaces;


namespace PromotionService.Application.Promotions.Commands.ApplyPromotion
{
    public class ApplyPromotionHandler
        : IRequestHandler<ApplyPromotionCommand, ApplyPromotionResponse>
    {
        private readonly IPromotionRepository _repo;

        public ApplyPromotionHandler(IPromotionRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApplyPromotionResponse> Handle(
     ApplyPromotionCommand request,
     CancellationToken cancellationToken)
        {
            Console.WriteLine("🔥 ApplyPromotionHandler CALLED");

            var coupon = await _repo.GetByCodeAsync(request.CouponCode);

            if (coupon == null)
            {
                return new ApplyPromotionResponse
                {
                    isValid = false,
                    discountAmount = 0,
                    finalAmount = request.TotalAmount, // 🔥 FIX
                    message = "Coupon not found"
                };
            }

            if (!coupon.IsActive)
            {
                return new ApplyPromotionResponse
                {
                    isValid = false,
                    discountAmount = 0,
                    finalAmount = request.TotalAmount,
                    message = "Coupon inactive"
                };
            }

            if (coupon.EndDate < DateTime.UtcNow)
            {
                return new ApplyPromotionResponse
                {
                    isValid = false,
                    discountAmount = 0,
                    finalAmount = request.TotalAmount,
                    message = "Coupon expired"
                };
            }

            // 🔥 tính discount
            var discount = request.TotalAmount * coupon.DiscountPercent / 100;
            var final = request.TotalAmount - discount;

            return new ApplyPromotionResponse
            {
                isValid = true,
                discountAmount = discount,
                finalAmount = final,
                message = "Success"
            };
        }
    }
}