using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YptoTaskManager.BE.IBusiness;
using YptoTaskManager.BE.IBusiness.Dtos;
using YptoTaskManager.BE.IData.Queries;

namespace YptoTaskManager.BE.Business;

public class AuthService : IAuthService
{
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserQueryRepository userQueryRepository,
        IConfiguration configuration)
    {
        _userQueryRepository = userQueryRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userQueryRepository.GetByEmailAsync(
            request.Email,
            cancellationToken) ?? throw new UnauthorizedAccessException("Invalid credentials.");

        var passwordHash =
            ComputePasswordHash(
                request.Password,
                user.PasswordSalt);

        if (!passwordHash.Equals(
                user.PasswordHash,
                StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var token = GenerateJwtToken(user);

        return new LoginResponse(
            token,
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber);
    }

    private string GenerateJwtToken(Domain.Entities.User user)
    {
        var issuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Missing Jwt:Issuer");

        var audience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Missing Jwt:Audience");

        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Missing Jwt:Key");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),

            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
        };

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    private static string ComputePasswordHash(
        string password,
        Guid salt)
    {
        var bytes =
            Encoding.UTF8.GetBytes(
                $"{password}{salt}");

        var hash =
            SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}
