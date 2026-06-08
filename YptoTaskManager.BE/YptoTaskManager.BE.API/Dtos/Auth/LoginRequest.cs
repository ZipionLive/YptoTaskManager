namespace YptoTaskManager.BE.API.Dtos.Auth;

public record LoginRequest(
    string Email,
    string Password);
