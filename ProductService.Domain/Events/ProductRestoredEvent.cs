using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Events
{
    public class ProductRestoredEvent
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
    }
}
