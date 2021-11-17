using System;
using System.Numerics;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace BigInt472
{
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

		[SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
		public static void Main()
		{
			const int digits = 25000;
			var oracle = File.ReadAllText("PI_25000.txt", Encoding.UTF8);

			var sw = new Stopwatch();

			sw.Start();
			var result = Calc(digits);
			sw.Stop();

			var isCorrect = result.ToString().Equals(oracle.Substring(0, digits + 1));

			Console.WriteLine($"Digits: {digits}");
			Console.WriteLine($"Execution time: {sw.Elapsed.ToString()}");
			Console.WriteLine($"Correct: {isCorrect}");
		}
	}
}