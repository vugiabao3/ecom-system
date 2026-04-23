using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IPasswordHasher
    {
       
            // Hash password (mã hóa mật khẩu)
            string Hash(string password);

            // So sánh password nhập vào với password đã hash
            bool Verify(string password, string hashedPassword);
        }
    
}
 