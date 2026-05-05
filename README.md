# BigInt9 — High-Precision π Calculator

Calculates π to an arbitrary number of decimal digits using `BigInteger` arithmetic in C# (.NET 10).

## Formula

The algorithm is based on the identity:

$$\pi = 6 \cdot \arcsin\!\left(\tfrac{1}{2}\right)$$

Substituting `x = ½` into the arcsin power series:

$$\arcsin(x) = \sum_{n=0}^{\infty} \frac{(2n)!}{4^n \cdot (n!)^2 \cdot (2n+1)} \cdot x^{2n+1}$$

gives the absolutely convergent series:

$$\pi = 3 + \frac{1}{8} + \frac{9}{640} + \frac{15}{7168} + \cdots$$

Each term is derived from the previous one by a simple integer recurrence, making the series efficient to evaluate with `BigInteger` — no floating-point arithmetic is involved.

### Recurrence

Maintaining a running integer `x` scaled by `10^(20 + digits)`:

```
x₀ = 3 · 10^(20 + digits)

xₙ = xₙ₋₁ · (2n − 1) / (4 · 2n)          (numerator step)
πₙ = xₙ / (2n + 1)                       (term added to sum)
```

The loop terminates naturally when `x` rounds down to zero, guaranteeing full convergence.

### Precision Guard

The working value is scaled by an extra `10^20` beyond the requested digits. This provides a buffer that absorbs integer-division truncation errors accumulated over thousands of iterations. The guard digits are stripped before returning.

## Project Structure

```
BigInt9/
├── Program.cs        # C# implementation (BigInteger)
├── pi.html           # JavaScript implementation (BigInt) for browser benchmarking
├── PI_100000.txt     # Reference file: first 100,000 digits of π
└── BigInt9.csproj    # .NET 10 project file
```

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Running

```bash
dotnet run --project BigInt9
```

The program calculates π to **10,000 digits**, verifies the result against `PI_100000.txt`, and prints the elapsed time:

```
Digits: 10000
Execution time: 00:00:01.234
Correct: True
```

## Browser Benchmark

Open `BigInt9/pi.html` in any modern browser to run the same algorithm in JavaScript using the native `BigInt` type. Clicking the button computes 25,000 digits and reports the time in seconds — useful for comparing JS engine `BigInt` performance against .NET `BigInteger`.
