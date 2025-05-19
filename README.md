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
  Job-PDEFRX : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-VFMIOD : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-GWWNIA : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-DPTFUX : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method                 | Runtime              | Mean       | Error     | StdDev    | Ratio | RatioSD |
|----------------------- |--------------------- |-----------:|----------:|----------:|------:|--------:|
| DirectCall             | .NET 6.0             |   1.520 ns | 0.0373 ns | 0.0331 ns |  1.24 |    0.03 |
| DirectCall             | .NET 8.0             |   1.240 ns | 0.0198 ns | 0.0165 ns |  1.01 |    0.02 |
| DirectCall             | .NET 9.0             |   1.229 ns | 0.0197 ns | 0.0164 ns |  1.00 |    0.02 |
| DirectCall             | .NET Framework 4.7.2 |   1.322 ns | 0.0233 ns | 0.0218 ns |  1.08 |    0.02 |
|                        |                      |            |           |           |       |         |
| ActivatorCall          | .NET 6.0             |   7.126 ns | 0.1099 ns | 0.1028 ns |  1.46 |    0.04 |
| ActivatorCall          | .NET 8.0             |   4.304 ns | 0.0599 ns | 0.0560 ns |  0.88 |    0.03 |
| ActivatorCall          | .NET 9.0             |   4.892 ns | 0.1429 ns | 0.1336 ns |  1.00 |    0.04 |
| ActivatorCall          | .NET Framework 4.7.2 |  35.633 ns | 0.3296 ns | 0.3083 ns |  7.29 |    0.20 |
|                        |                      |            |           |           |       |         |
| ActivatorUtilitiesCall | .NET 6.0             | 189.976 ns | 1.3685 ns | 1.2132 ns |  6.79 |    0.04 |
| ActivatorUtilitiesCall | .NET 8.0             |  32.716 ns | 0.1520 ns | 0.1347 ns |  1.17 |    0.01 |
| ActivatorUtilitiesCall | .NET 9.0             |  27.962 ns | 0.0873 ns | 0.0682 ns |  1.00 |    0.00 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 258.214 ns | 1.2162 ns | 0.9495 ns |  9.23 |    0.04 |

## Transient Parameters

These tests create a new object that take a transient object as the only constructor parameter.
This means a two new objects are created.

### ARM64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
Unknown processor
.NET SDK 9.0.300
  [Host]     : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-QGUIPM : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-SGCJLH : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-IOUUGT : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-LHBMCR : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method                 | Runtime              | Mean       | Error     | StdDev    | Ratio | RatioSD |
|----------------------- |--------------------- |-----------:|----------:|----------:|------:|--------:|
| DirectCall             | .NET 6.0             |   4.462 ns | 0.0418 ns | 0.0391 ns |  1.04 |    0.02 |
| DirectCall             | .NET 8.0             |   4.288 ns | 0.0469 ns | 0.0416 ns |  1.00 |    0.02 |
| DirectCall             | .NET 9.0             |   4.286 ns | 0.0804 ns | 0.0713 ns |  1.00 |    0.02 |
| DirectCall             | .NET Framework 4.7.2 |   4.485 ns | 0.0715 ns | 0.0634 ns |  1.05 |    0.02 |
|                        |                      |            |           |           |       |         |
| ActivatorCall          | .NET 6.0             | 212.166 ns | 2.8971 ns | 2.5682 ns |  2.04 |    0.03 |
| ActivatorCall          | .NET 8.0             | 106.326 ns | 0.7417 ns | 0.6575 ns |  1.02 |    0.01 |
| ActivatorCall          | .NET 9.0             | 104.175 ns | 0.9313 ns | 0.8712 ns |  1.00 |    0.01 |
| ActivatorCall          | .NET Framework 4.7.2 | 325.685 ns | 3.5074 ns | 3.2808 ns |  3.13 |    0.04 |
|                        |                      |            |           |           |       |         |
| ActivatorUtilitiesCall | .NET 6.0             | 333.580 ns | 2.9662 ns | 2.7746 ns |  5.95 |    0.07 |
| ActivatorUtilitiesCall | .NET 8.0             |  57.071 ns | 0.5672 ns | 0.5306 ns |  1.02 |    0.01 |
| ActivatorUtilitiesCall | .NET 9.0             |  56.025 ns | 0.5484 ns | 0.4862 ns |  1.00 |    0.01 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 664.234 ns | 4.5363 ns | 4.0213 ns | 11.86 |    0.12 |

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
  Job-FJQIHO : .NET 6.0.36 (6.0.3624.51421), Arm64 RyuJIT AdvSIMD
  Job-SMUMBW : .NET 8.0.16 (8.0.1625.21506), Arm64 RyuJIT AdvSIMD
  Job-FVOEEG : .NET 9.0.5 (9.0.525.21509), Arm64 RyuJIT AdvSIMD
  Job-UINACK : .NET Framework 4.8.1 (4.8.9310.0), Arm64 RyuJIT


```
| Method                 | Runtime              | Mean       | Error     | StdDev    | Ratio | RatioSD |
|----------------------- |--------------------- |-----------:|----------:|----------:|------:|--------:|
| DirectCall             | .NET 6.0             |   7.418 ns | 0.0888 ns | 0.0830 ns |  3.13 |    0.08 |
| DirectCall             | .NET 8.0             |   3.677 ns | 0.0245 ns | 0.0205 ns |  1.55 |    0.03 |
| DirectCall             | .NET 9.0             |   2.375 ns | 0.0581 ns | 0.0544 ns |  1.00 |    0.03 |
| DirectCall             | .NET Framework 4.7.2 |   3.110 ns | 0.0463 ns | 0.0433 ns |  1.31 |    0.03 |
|                        |                      |            |           |           |       |         |
| ActivatorCall          | .NET 6.0             | 281.260 ns | 1.1601 ns | 0.9057 ns |  2.80 |    0.02 |
| ActivatorCall          | .NET 8.0             | 104.227 ns | 0.5596 ns | 0.5234 ns |  1.04 |    0.01 |
| ActivatorCall          | .NET 9.0             | 100.323 ns | 0.6399 ns | 0.5672 ns |  1.00 |    0.01 |
| ActivatorCall          | .NET Framework 4.7.2 | 322.673 ns | 3.0736 ns | 2.8751 ns |  3.22 |    0.03 |
|                        |                      |            |           |           |       |         |
| ActivatorUtilitiesCall | .NET 6.0             | 411.976 ns | 2.7505 ns | 2.4382 ns |  8.11 |    0.07 |
| ActivatorUtilitiesCall | .NET 8.0             |  52.300 ns | 0.3029 ns | 0.2834 ns |  1.03 |    0.01 |
| ActivatorUtilitiesCall | .NET 9.0             |  50.792 ns | 0.3267 ns | 0.3056 ns |  1.00 |    0.01 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 658.120 ns | 5.5035 ns | 5.1480 ns | 12.96 |    0.12 |

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
