using BenchmarkDotNet.Attributes;
using OrchardCore.Modules.Services;

namespace OrchardCore.Benchmarks;

[MemoryDiagnoser]
public class SlugBenchmark
{
    private static readonly SlugService s_slugService;

    static SlugBenchmark()
    {
        s_slugService = new SlugService();
    }

    [Benchmark]
#pragma warning disable CA1822 // Mark members as static
    public string EvaluateSlugifyWithShortSlug()
    {
        // Zpomalení běhu benchmarku
        //System.Threading.Thread.Sleep(5);
        return s_slugService.Slugify("Je veux aller à Saint-Étienne");
    }

    [Benchmark]
    public int EvaluateSlugifyWithLongSlug()
    {
        // Větší alokace paměti – data jsou vrácena/použita, takže je JIT/optimalizátor nemůže eliminovat (dead-code elimination)
        var dummyAllocations = new System.Collections.Generic.List<string>(500);
        var totalLength = 0;
        for (var i = 0; i < 500; i++)
        {
            var str = new string('x', 1024) + i;
            dummyAllocations.Add(str);
            totalLength += str.Length;
        }

        // Mírné zpomalení (cca 20 % navíc)
        var dummySum = 0;
        for (var i = 0; i < 100; i++)
        {
            dummyAllocations.Add(i.ToString());
            dummySum += dummyAllocations[i].Length;
        }

        var slug = s_slugService.Slugify("Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne Je veux aller à Saint-Étienne");

        return totalLength + slug.Length + dummyAllocations.Count + dummySum;
    }
#pragma warning restore CA1822 // Mark members as static
}
