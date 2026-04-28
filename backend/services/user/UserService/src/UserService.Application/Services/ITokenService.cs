using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.Services
{
    public interface ITokenService
    {
        string GenerateSecureToken();
        string HashToken(string token);
    }
}
