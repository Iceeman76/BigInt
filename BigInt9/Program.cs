using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace BigInt9;

public static class Program
{
    /// <summary>
    /// Calculates an approximation of π (pi) to the specified number of digits.
    /// </summary>
    /// <remarks>
    /// Uses the formula π = 6 · arcsin(½), evaluated via its power series:
    /// arcsin(x) = Σ (n=0 to ∞) [(2n)! / (4ⁿ · (n!)² · (2n+1))] · x^(2n+1)
    /// </remarks>
    /// <param name="digits">The number of decimal digits to calculate for π.</param>
    /// <returns>A <see cref="BigInteger"/> representing the approximated value of π, scaled to include the specified number of digits.</returns>
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
        const int digits = 10_000;
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
