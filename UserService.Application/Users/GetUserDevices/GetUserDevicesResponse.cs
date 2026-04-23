using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Users.GetUserDevices
{
    public class GetUserDevicesResponse
    {
        public List<DeviceItem> Devices { get; set; } = new();
    }

    public class DeviceItem
    {
        public string DeviceInfo { get; set; }
        public string IpAddress { get; set; }
        public DateTime LoginAt { get; set; }
    }
}
