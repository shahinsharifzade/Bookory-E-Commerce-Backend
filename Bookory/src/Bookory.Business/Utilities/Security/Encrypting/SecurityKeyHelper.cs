using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bookory.Business.Utilities.Security.Encrypting;

public class SecurityKeyHelper
{
    public static SecurityKey CreateSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}
