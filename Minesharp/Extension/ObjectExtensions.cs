using System.Security.Cryptography;
using System.Text;

namespace Minesharp.Extension;

public static class ObjectExtensions
{
    public static byte[] ToSha256(this object value)
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes($"{value}"));
    }
}