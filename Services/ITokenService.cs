using MinimalAPI.Models;

namespace MinimalAPI.Services;

public interface ITokenService
{
    string GetToken(string key, string issuer, string audience, UserModel user);
}
