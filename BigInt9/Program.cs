using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace BigInt9;

public static class Program
{
    private static BigInteger CalculatePiUsingArcsinSeries(int digits)
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

    private static BigInteger CalculatePiUsingMachinFormula(int digits)
    {
        const int guardDigits = 20;
        var scale = BigInteger.Pow(10, digits + guardDigits);
        var pi = 16 * CalculateScaledArctangent(5, scale) - 4 * CalculateScaledArctangent(239, scale);

        return pi / BigInteger.Pow(10, guardDigits);
    }

    private static BigInteger CalculateScaledArctangent(int reciprocalDenominator, BigInteger scale)
    {
        var power = scale / reciprocalDenominator;
        var squaredDenominator = reciprocalDenominator * reciprocalDenominator;
        var sum = BigInteger.Zero;
        var denominator = 1;
        var sign = 1;

        while (power > 0)
        {
            sum += sign * power / denominator;
            power /= squaredDenominator;
            denominator += 2;
            sign = -sign;
        }

        return sum;
    }

    public static async Task Main()
    {
        const int digits = 100_000;

        var oracle = await File.ReadAllTextAsync("PI_100000.txt", Encoding.UTF8);
        var expectedResult = oracle[..(digits + 1)];

        Console.WriteLine($"Digits: {digits}");

        var arcsinTask = Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            var result = CalculatePiUsingArcsinSeries(digits);
            stopwatch.Stop();

            return (Result: result, stopwatch.Elapsed);
        });

        var machinTask = Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            var result = CalculatePiUsingMachinFormula(digits);
            stopwatch.Stop();

            return (Result: result, stopwatch.Elapsed);
        });

        var results = await Task.WhenAll(arcsinTask, machinTask);
        var arcsinResult = results[0];
        var machinResult = results[1];

        Console.WriteLine($"Arcsin execution time: {arcsinResult.Elapsed}");
        Console.WriteLine($"Arcsin correct: {arcsinResult.Result.ToString().Equals(expectedResult)}");
        Console.WriteLine($"Machin execution time: {machinResult.Elapsed}");
        Console.WriteLine($"Machin correct: {machinResult.Result.ToString().Equals(expectedResult)}");
    }
}
