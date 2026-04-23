using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.GetUserActivity
{
    public class GetUserActivityResponse
    {
        public string Action { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}