using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } // SUCCESS / FAILED

        public DateTime CreatedAt { get; set; }
    }
}
