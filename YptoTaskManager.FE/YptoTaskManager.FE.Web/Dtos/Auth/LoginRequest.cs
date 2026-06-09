using System.Runtime.CompilerServices;

namespace YptoTaskManager.FE.Web.Dtos.Auth;

public record LoginRequest(
    string Email,
    string Password);