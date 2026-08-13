using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NotificationService.Domain.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public string  UserId { get; set; }
    public decimal TotalAmount { get; set; }
}
