using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Events
{
    public class ProductDeletedEvent
    {
        public Guid Id { get; set; }
    }
}
