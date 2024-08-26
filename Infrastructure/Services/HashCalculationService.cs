using System.Security.Cryptography;
using System.Text;
using Scrutor;

namespace Infrastructure.Services;

[ServiceDescriptor]
public class HashCalculationService : IHashCalculationService
{

    public string Hash(string toHash)
    {
        return Hash(Encoding.UTF8.GetBytes(toHash));
    }

    public string Hash(byte[] bytes)
    {
        using var hasher = MD5.Create();

        var hashedBytes = hasher.ComputeHash(bytes);

        var stringBuilder = new StringBuilder();
        foreach (var num in hashedBytes)
            stringBuilder.Append(num.ToString("x2"));

        return stringBuilder.ToString();
    }
}