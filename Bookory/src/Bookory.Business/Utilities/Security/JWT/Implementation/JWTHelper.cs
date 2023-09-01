using Bookory.Business.Utilities.DTOs.AuthDtos;
using Bookory.Business.Utilities.Security.Encrypting;
using Bookory.Business.Utilities.Security.JWT.Interface;
using Bookory.Core.Models.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Bookory.Business.Utilities.Security.JWT.Implementation;
public class JWTHelper : ITokenHelper
{
    private readonly IConfiguration? _configuration;
    private readonly TokenOption _tokenOption;
    private readonly DateTime _accessTokenExpiration;

    public JWTHelper(IConfiguration? configuration)
    {
        _configuration = configuration;
        _tokenOption = _configuration.GetSection("TokenOptions").Get<TokenOption>();
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
    }

    public TokenResponseDto CreateToken(List<Claim> claims)
    {
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOption.SecurityKey);
        var signinCredentials = SigninCredentialsHelper.CreateSigningCredentials(securityKey);
        JwtHeader jwtHeader = new JwtHeader(signinCredentials);

        JwtPayload jwtPayload = new JwtPayload(

            issuer: _tokenOption.Issuer,
            audience: _tokenOption.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: _accessTokenExpiration
         );

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(jwtHeader, jwtPayload);

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        return new TokenResponseDto(token, _accessTokenExpiration);
    }
}
