using FantasySky.CustomDF.Domain.Guids;

namespace FantasySky.CustomDF.Ddd.Domain.UnitTests.Guids;

public class SequentialGuidGeneratorTests
{
    [Test]
    public void SequentialGuidGeneratorTests_OnceCreate_SuccessTest()
    {
        var generator = new SequentialGuidGenerator();

        var guid = generator.Create();

        Console.WriteLine(guid);
    }

    [Test]
    public void SequentialGuidGeneratorTests_100000CreateDuplicateCheck_SuccessTest()
    {
        var generator = new SequentialGuidGenerator();

        List<Guid> guids = new(1000);
        for (int i = 0; i < 100000; i++)
        {
            var guid = generator.Create();
            guids.Add(guid);
            Console.WriteLine(guid);
        }

        Assert.That(guids.Count == guids.Distinct().Count());
    }
}