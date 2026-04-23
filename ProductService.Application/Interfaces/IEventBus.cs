using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IEventBus
    {
        Task PublishAsync(string eventName, object data);
    }
}
