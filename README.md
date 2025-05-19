# Overview

I'm often caught trying to decide how to deal with different ways to create objects and call methods.
Sometimes performance is critical and even few dozen nanoseconds can add up.
A hot path might have to loop over a few hundred or even thousands of iterations.

This is a single repo and project to keep a record of these performance numbers to help me decide when the performance is alright.

# Results

Since I have access to an ARM64 laptop, I have numbers for both x64 and ARM64.
This data should not be used to say "ARM is better than Intel" or vice versa.
The machines are very different from each other and purchased in different years.

## Empty Constructor

These tests create a new object using a constructor that doesn't take any parameters.

### ARM64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Unknown processor
.NET SDK 9.0.300
  [Host]     : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-PBOPKS : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-YNREAM : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-KFSMLR : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-QIJDWO : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method                 | Runtime              | Mean       | Error     | StdDev    | Ratio  | RatioSD |
|----------------------- |--------------------- |-----------:|----------:|----------:|-------:|--------:|
| DirectCall             | .NET 6.0             |   1.455 ns | 0.0762 ns | 0.0815 ns |   1.22 |    0.07 |
| ActivatorCall          | .NET 6.0             |   7.000 ns | 0.0868 ns | 0.0812 ns |   5.88 |    0.08 |
| ActivatorUtilitiesCall | .NET 6.0             | 160.145 ns | 0.6307 ns | 0.5899 ns | 134.46 |    1.22 |
| DirectCall             | .NET 8.0             |   1.198 ns | 0.0194 ns | 0.0172 ns |   1.01 |    0.02 |
| ActivatorCall          | .NET 8.0             |   4.383 ns | 0.0263 ns | 0.0233 ns |   3.68 |    0.04 |
| ActivatorUtilitiesCall | .NET 8.0             |  30.345 ns | 0.1159 ns | 0.0968 ns |  25.48 |    0.23 |
| DirectCall             | .NET 9.0             |   1.191 ns | 0.0125 ns | 0.0104 ns |   1.00 |    0.01 |
| ActivatorCall          | .NET 9.0             |   5.083 ns | 0.0195 ns | 0.0183 ns |   4.27 |    0.04 |
| ActivatorUtilitiesCall | .NET 9.0             |  51.761 ns | 0.1537 ns | 0.1437 ns |  43.46 |    0.38 |
| DirectCall             | .NET Framework 4.7.2 |   1.216 ns | 0.0604 ns | 0.0535 ns |   1.02 |    0.04 |
| ActivatorCall          | .NET Framework 4.7.2 |  35.127 ns | 0.1946 ns | 0.1820 ns |  29.49 |    0.29 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 262.253 ns | 1.0598 ns | 0.9395 ns | 220.20 |    1.99 |

## Transient Parameters

These tests create a new object that take a transient object as the only constructor parameter.
This means a two new objects are created.

### ARM64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Unknown processor
.NET SDK 9.0.300
  [Host]     : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-PBOPKS : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-YNREAM : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-KFSMLR : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-QIJDWO : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method                 | Runtime              | Mean       | Error     | StdDev    | Ratio  | RatioSD |
|----------------------- |--------------------- |-----------:|----------:|----------:|-------:|--------:|
| DirectCall             | .NET 6.0             |   4.192 ns | 0.0621 ns | 0.0581 ns |   1.02 |    0.02 |
| ActivatorCall          | .NET 6.0             | 209.911 ns | 1.1013 ns | 0.9763 ns |  51.15 |    0.58 |
| ActivatorUtilitiesCall | .NET 6.0             | 323.165 ns | 1.4342 ns | 1.3415 ns |  78.74 |    0.88 |
| DirectCall             | .NET 8.0             |   4.104 ns | 0.0390 ns | 0.0365 ns |   1.00 |    0.01 |
| ActivatorCall          | .NET 8.0             | 108.640 ns | 0.2565 ns | 0.2273 ns |  26.47 |    0.28 |
| ActivatorUtilitiesCall | .NET 8.0             |  56.720 ns | 0.1873 ns | 0.1564 ns |  13.82 |    0.15 |
| DirectCall             | .NET 9.0             |   4.105 ns | 0.0476 ns | 0.0445 ns |   1.00 |    0.01 |
| ActivatorCall          | .NET 9.0             | 113.803 ns | 0.2815 ns | 0.2496 ns |  27.73 |    0.29 |
| ActivatorUtilitiesCall | .NET 9.0             |  55.097 ns | 0.2844 ns | 0.2660 ns |  13.43 |    0.15 |
| DirectCall             | .NET Framework 4.7.2 |   4.136 ns | 0.0392 ns | 0.0347 ns |   1.01 |    0.01 |
| ActivatorCall          | .NET Framework 4.7.2 | 332.083 ns | 1.9958 ns | 1.7692 ns |  80.92 |    0.94 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 666.224 ns | 3.0643 ns | 2.8663 ns | 162.33 |    1.82 |

## Scoped Parameters

These benchmarks test the performance of creating a scope and creating a scope with a scoped parameter passed to the constructor.

### ARM64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Unknown processor
.NET SDK 9.0.300
  [Host]     : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-PBOPKS : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-YNREAM : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-KFSMLR : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-QIJDWO : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method                 | Runtime              | Mean      | Error    | StdDev   | Ratio | RatioSD |
|----------------------- |--------------------- |----------:|---------:|---------:|------:|--------:|
| CreateScope            | .NET 6.0             |  46.78 ns | 0.614 ns | 0.575 ns |  1.69 |    0.02 |
| CreateScope            | .NET 8.0             |  35.23 ns | 0.137 ns | 0.128 ns |  1.27 |    0.01 |
| CreateScope            | .NET 9.0             |  27.65 ns | 0.161 ns | 0.150 ns |  1.00 |    0.01 |
| CreateScope            | .NET Framework 4.7.2 |  64.31 ns | 0.226 ns | 0.200 ns |  2.33 |    0.01 |
|                        |                      |           |          |          |       |         |
| ActivatorUtilitiesCall | .NET 6.0             | 381.40 ns | 1.156 ns | 1.025 ns |  4.45 |    0.02 |
| ActivatorUtilitiesCall | .NET 8.0             |  87.10 ns | 0.527 ns | 0.493 ns |  1.02 |    0.01 |
| ActivatorUtilitiesCall | .NET 9.0             |  85.79 ns | 0.317 ns | 0.296 ns |  1.00 |    0.00 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 743.23 ns | 5.743 ns | 5.372 ns |  8.66 |    0.07 |

## Singleton Parameter

These tests create a new object that take a singleton object as the only constructor parameter.
Only a single new object is created and is passed a pre-existing object that is shared.

### ARM64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Unknown processor
.NET SDK 9.0.300
  [Host]     : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-PBOPKS : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-YNREAM : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-KFSMLR : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-QIJDWO : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method                 | Runtime              | Mean       | Error     | StdDev    | Ratio  | RatioSD |
|----------------------- |--------------------- |-----------:|----------:|----------:|-------:|--------:|
| DirectCall             | .NET 6.0             |   3.224 ns | 0.0421 ns | 0.0394 ns |   1.40 |    0.02 |
| ActivatorCall          | .NET 6.0             | 208.589 ns | 0.8345 ns | 0.7397 ns |  90.76 |    1.22 |
| ActivatorUtilitiesCall | .NET 6.0             | 318.512 ns | 0.5875 ns | 0.4906 ns | 138.60 |    1.82 |
| DirectCall             | .NET 8.0             |   2.322 ns | 0.0232 ns | 0.0206 ns |   1.01 |    0.02 |
| ActivatorCall          | .NET 8.0             | 105.028 ns | 0.3099 ns | 0.2588 ns |  45.70 |    0.60 |
| ActivatorUtilitiesCall | .NET 8.0             |  56.050 ns | 0.2563 ns | 0.2397 ns |  24.39 |    0.33 |
| DirectCall             | .NET 9.0             |   2.299 ns | 0.0335 ns | 0.0313 ns |   1.00 |    0.02 |
| ActivatorCall          | .NET 9.0             | 111.138 ns | 0.4855 ns | 0.4304 ns |  48.36 |    0.66 |
| ActivatorUtilitiesCall | .NET 9.0             |  49.473 ns | 0.1831 ns | 0.1712 ns |  21.53 |    0.29 |
| DirectCall             | .NET Framework 4.7.2 |   2.985 ns | 0.0591 ns | 0.0524 ns |   1.30 |    0.03 |
| ActivatorCall          | .NET Framework 4.7.2 | 330.970 ns | 2.1115 ns | 1.9751 ns | 144.02 |    2.05 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 662.258 ns | 4.4902 ns | 4.2002 ns | 288.17 |    4.15 |

## Method Call

These calls test how fast method calls that perform a simple `Math.Pow(x,y)` call are.
The DirectCall is simply calling the method directly.
The IndirectCall uses a `Func<double, double, double>` and assigns `Math.Pow` to it.
The LambdaCall uses a dynamic Expression that calls `Math.Pow`, the expression is compiled once into a delegate and reused.

### ARM64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Unknown processor
.NET SDK 9.0.300
  [Host]     : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-PBOPKS : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-YNREAM : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-KFSMLR : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-QIJDWO : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method       | Runtime              | Mean     | Error     | StdDev    | Ratio |
|------------- |--------------------- |---------:|----------:|----------:|------:|
| DirectCall   | .NET 6.0             | 4.166 ns | 0.0346 ns | 0.0323 ns |  1.04 |
| DirectCall   | .NET 8.0             | 3.902 ns | 0.0162 ns | 0.0135 ns |  0.98 |
| DirectCall   | .NET 9.0             | 4.000 ns | 0.0261 ns | 0.0244 ns |  1.00 |
| DirectCall   | .NET Framework 4.7.2 | 7.671 ns | 0.0199 ns | 0.0166 ns |  1.92 |
|              |                      |          |           |           |       |
| IndirectCall | .NET 6.0             | 4.226 ns | 0.0131 ns | 0.0116 ns |  1.02 |
| IndirectCall | .NET 8.0             | 4.022 ns | 0.0139 ns | 0.0123 ns |  0.97 |
| IndirectCall | .NET 9.0             | 4.151 ns | 0.0192 ns | 0.0170 ns |  1.00 |
| IndirectCall | .NET Framework 4.7.2 | 7.898 ns | 0.0165 ns | 0.0146 ns |  1.90 |
|              |                      |          |           |           |       |
| LambdaCall   | .NET 6.0             | 5.227 ns | 0.0298 ns | 0.0264 ns |  1.01 |
| LambdaCall   | .NET 8.0             | 5.073 ns | 0.0374 ns | 0.0350 ns |  0.98 |
| LambdaCall   | .NET 9.0             | 5.197 ns | 0.0306 ns | 0.0286 ns |  1.00 |
| LambdaCall   | .NET Framework 4.7.2 | 8.086 ns | 0.0132 ns | 0.0110 ns |  1.56 |
