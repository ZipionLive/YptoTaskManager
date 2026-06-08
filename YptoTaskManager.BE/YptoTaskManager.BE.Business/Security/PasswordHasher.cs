using System.Security.Cryptography;
using System.Text;
using YptoTaskManager.BE.IBusiness.Security;

namespace YptoTaskManager.BE.Business.Security;

public class PasswordHasher : IPasswordHasher
{
    public string ComputePasswordHash(string password, Guid salt)
    {
        var bytes = Encoding.UTF8.GetBytes($"{password}{salt}");
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}
