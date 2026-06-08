namespace YptoTaskManager.BE.IBusiness.Dtos;

public record LoginRequest(
    string Email,
    string Password);
