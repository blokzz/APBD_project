namespace APBD_PROJEKT.Services;
using Dtos.Auth;

public interface IAuthService
{
    Task<string> Login(LoginDto dto);
    Task Register(RegisterDto dto);
}