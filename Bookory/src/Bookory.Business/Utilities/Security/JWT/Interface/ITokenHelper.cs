using Bookory.Business.Utilities.DTOs.AuthDtos;
using System.Security.Claims;

namespace Bookory.Business.Utilities.Security.JWT.Interface;

public interface ITokenHelper
{
    TokenResponseDto CreateToken(List<Claim> claims);
}
