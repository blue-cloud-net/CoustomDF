using FantasySky.CustomDF.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FantasySky.CustomDF.Random;

[Dependency(typeof(IRandomGenerator), ServiceLifetime.Transient)]
public class RandomGenerator : IRandomGenerator
{
    public byte[] Create(int byteCount)
        => System.Security.Cryptography.RandomNumberGenerator.GetBytes(byteCount);

    public string CreateString(int byteCount) => Base64UrlEncoder.Encode(this.Create(byteCount));

    public static string NewString(int byteCount = 48) => Base64UrlEncoder.Encode(new RandomGenerator().Create(byteCount));
}
