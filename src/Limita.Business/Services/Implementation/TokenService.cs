namespace Limita.Business.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;

    public TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public string CreateToken(int userId)
    {
        var jwtSection = configuration.GetSection("Jwt");

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ];

        SymmetricSecurityKey key = new(
            Encoding.UTF8.GetBytes(jwtSection["Key"]!));

        SigningCredentials credentials = new(
            key,
            SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(jwtSection["ExpiryMinutes"]!)),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
