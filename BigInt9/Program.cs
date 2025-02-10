using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace BigInt9;

public static class Program
{
    private static BigInteger Calc(int digits)
    {
        BigInteger i = 1;
        var x = 3 * BigInteger.Pow(10, 20 + digits);
        var pi = x;

        while (x > 0)
        {
            x = x * i / ((i + 1) * 4);
            pi += x / (i + 2);
            i += 2;
        }

        return pi / BigInteger.Pow(10, 20);
    }

    public static void Main()
    {
        const int digits = 25_000;
        var oracle = File.ReadAllText("PI_100000.txt", Encoding.UTF8);

        var sw = new Stopwatch();

        sw.Start();
        var result = Calc(digits);
        sw.Stop();

        var isCorrect = result.ToString().Equals(oracle[..(digits + 1)]);

        Console.WriteLine($"Digits: {digits}");
        Console.WriteLine($"Execution time: {sw.Elapsed.ToString()}");
        Console.WriteLine($"Correct: {isCorrect}");
    }
}
