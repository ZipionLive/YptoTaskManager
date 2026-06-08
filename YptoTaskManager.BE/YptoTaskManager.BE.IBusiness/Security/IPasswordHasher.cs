namespace YptoTaskManager.BE.IBusiness.Security;

public interface IPasswordHasher
{
    string ComputePasswordHash(string password, Guid salt);
}
