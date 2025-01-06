using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public interface IAuthenticationService
{
    //Task<bool> IsTokenRevoked(string token);
    //Task RevokeTokens(string username);
    Task<string> GenerateAccessJwtToken(User user);
    DateTime GetAccessTokenExpiration();
    Task<ClaimsPrincipal?> ValidateToken(string token);
}

public class AuthenticationService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : IAuthenticationService
{
    //public async Task<bool> IsTokenRevoked(string token)
    //{
    //    // change the implementation to use the redis

    //    //var revokedToken = await _repository.JwtTokens!
    //    //        .FindByCondition(t => t.TokenValue == token && !t.IsRevoked)
    //    //        .FirstOrDefaultAsync();
    //    //return revokedToken == null;

    //    throw new NotImplementedException();
    //}

    public async Task<ClaimsPrincipal?> ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["JwtBearer:Key"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration["JwtBearer:Issuer"],
            ValidAudience = configuration["JwtBearer:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };

        try
        {
            var principal = await Task.Run(() => tokenHandler.ValidateToken(token, validationParameters, out var validatedToken));
            return principal;
        }
        catch
        {
            return null;
        }
    }

    //public async Task RevokeTokens(string username)
    //{
    //    // change the implementation to use the redis

    //    //var tokens = await repository.JwtTokens!.FindByCondition(t => t.UserName == username).ToListAsync();

    //    //foreach (var token in tokens)
    //    //{
    //    //    token.IsRevoked = true;
    //    //    token.RevocationDate = DateTime.UtcNow;
    //    //    repository.JwtTokens.Update(token);
    //    //}

    //    //await repository.SaveAsync();

    //    throw new NotImplementedException();
    //}

    public async Task<string> GenerateAccessJwtToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(configuration["JwtBearer:Key"]!);

        var tokenOptions = GenerateTokenOptions(
            new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            await GetClaims(user),
            GetAccessTokenExpiration());

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        // change the implementation to use the redis

        //repository.JwtTokens!.Add(new JwtTokens()
        //{
        //    UserName = user.UserName!,
        //    TokenValue = token,
        //    ExpirationDate = GetAccessTokenExpiration()
        //});
        //await repository.SaveAsync();

        return token;
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await userManager.GetRolesAsync(user);

        claims.AddRange(
            roles.Select(role => new Claim(ClaimTypes.Role, role)
            ));

        foreach (var r in roles)
        {
            var role = await roleManager.Roles
                .Where(m => m.Name!.ToUpper() == r.ToUpper())
                .FirstOrDefaultAsync();

            if (role != null)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims, DateTime expiration)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: configuration["JwtBearer:Issuer"],
            audience: configuration["JwtBearer:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

    public DateTime GetAccessTokenExpiration() => DateTime.UtcNow.AddDays(Convert.ToDouble(configuration["JwtBearer:TokenExpiration"]));
}