namespace FantasySky.CustomDF.DistributedLocking;

public interface IDistributedLockKeyNormalizer
{
    string NormalizeKey(string name);
}
