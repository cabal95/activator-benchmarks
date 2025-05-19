# Overview

I'm often caught trying to decide how to deal with different ways to create objects and call methods.
Sometimes performance is critical and even few dozen nanoseconds can add up.
A hot path might have to loop over a few hundred or even thousands of iterations.

This is a single repo and project to keep a record of these performance numbers to help me decide when the performance is alright.

# Results

Since I have access to an ARM64 laptop, I have numbers for both x64 and ARM64.
This data should not be used to say "ARM is better than Intel" or vice versa.
The machines are very different from each other and purchased in different years.
These results are meant purely for seeing the performance differences between the different runtimes and methods of creating/calling objects within the same architecture.

BenchmarkDotNet doesn't recognize the ARM64 hardware, but for the record it was a late 2024 Microsoft Surface 7 laptop with a Snapdragon X Elite X1E-80-100 CPU.

For the record, there are 1,000,000,000 nano seconds in a second.

## Empty Constructor

These tests create a new object using a constructor that doesn't take any parameters.

### X64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.204
  [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-ZZWBDH : .NET 6.0.36 (6.0.3624.51421), X64 RyuJIT AVX2
  Job-KSYKED : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-KHFHGX : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-LYLTTW : .NET Framework 4.8.1 (4.8.9310.0), X64 RyuJIT VectorSize=256


```
| Method                 | Runtime              | Mean       | Error     | StdDev    | Ratio | RatioSD |
|----------------------- |--------------------- |-----------:|----------:|----------:|------:|--------:|
| DirectCall             | .NET 6.0             |   2.464 ns | 0.0910 ns | 0.0807 ns |  0.68 |    0.03 |
| DirectCall             | .NET 8.0             |   3.640 ns | 0.0904 ns | 0.0846 ns |  1.01 |    0.03 |
| DirectCall             | .NET 9.0             |   3.611 ns | 0.1046 ns | 0.0979 ns |  1.00 |    0.04 |
| DirectCall             | .NET Framework 4.7.2 |   1.277 ns | 0.0907 ns | 0.0848 ns |  0.35 |    0.02 |
|                        |                      |            |           |           |       |         |
| ActivatorCall          | .NET 6.0             |   8.286 ns | 0.2165 ns | 0.2026 ns |  0.85 |    0.03 |
| ActivatorCall          | .NET 8.0             |   9.153 ns | 0.2119 ns | 0.1982 ns |  0.94 |    0.03 |
| ActivatorCall          | .NET 9.0             |   9.784 ns | 0.2542 ns | 0.2720 ns |  1.00 |    0.04 |
| ActivatorCall          | .NET Framework 4.7.2 |  37.438 ns | 0.7344 ns | 0.6869 ns |  3.83 |    0.13 |
|                        |                      |            |           |           |       |         |
| ActivatorUtilitiesCall | .NET 6.0             | 226.314 ns | 3.2100 ns | 3.0026 ns |  6.80 |    0.13 |
| ActivatorUtilitiesCall | .NET 8.0             |  34.776 ns | 0.6751 ns | 0.6315 ns |  1.04 |    0.02 |
| ActivatorUtilitiesCall | .NET 9.0             |  33.293 ns | 0.5533 ns | 0.5176 ns |  1.00 |    0.02 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 455.264 ns | 3.5444 ns | 2.7673 ns | 13.68 |    0.22 |

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

### X64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.204
  [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-ZZWBDH : .NET 6.0.36 (6.0.3624.51421), X64 RyuJIT AVX2
  Job-KSYKED : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-KHFHGX : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-LYLTTW : .NET Framework 4.8.1 (4.8.9310.0), X64 RyuJIT VectorSize=256


```
| Method                 | Runtime              | Mean         | Error      | StdDev     | Ratio | RatioSD |
|----------------------- |--------------------- |-------------:|-----------:|-----------:|------:|--------:|
| DirectCall             | .NET 6.0             |    11.472 ns |  0.2056 ns |  0.1923 ns |  1.65 |    0.06 |
| DirectCall             | .NET 8.0             |     6.908 ns |  0.2084 ns |  0.2481 ns |  0.99 |    0.05 |
| DirectCall             | .NET 9.0             |     6.980 ns |  0.2099 ns |  0.2246 ns |  1.00 |    0.04 |
| DirectCall             | .NET Framework 4.7.2 |     4.837 ns |  0.1592 ns |  0.1704 ns |  0.69 |    0.03 |
|                        |                      |              |            |            |       |         |
| ActivatorCall          | .NET 6.0             |   300.622 ns |  6.0400 ns |  6.2027 ns |  1.87 |    0.05 |
| ActivatorCall          | .NET 8.0             |   163.778 ns |  3.2532 ns |  3.1950 ns |  1.02 |    0.03 |
| ActivatorCall          | .NET 9.0             |   161.011 ns |  3.2735 ns |  3.0620 ns |  1.00 |    0.03 |
| ActivatorCall          | .NET Framework 4.7.2 |   504.495 ns |  7.5593 ns |  7.0710 ns |  3.13 |    0.07 |
|                        |                      |              |            |            |       |         |
| ActivatorUtilitiesCall | .NET 6.0             |   491.299 ns |  8.7363 ns |  8.1720 ns |  8.09 |    0.20 |
| ActivatorUtilitiesCall | .NET 8.0             |    63.511 ns |  1.3318 ns |  1.4250 ns |  1.05 |    0.03 |
| ActivatorUtilitiesCall | .NET 9.0             |    60.728 ns |  1.2464 ns |  1.1659 ns |  1.00 |    0.03 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 1,148.655 ns | 21.5436 ns | 21.1587 ns | 18.92 |    0.49 |

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

### X64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.204
  [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-ZZWBDH : .NET 6.0.36 (6.0.3624.51421), X64 RyuJIT AVX2
  Job-KSYKED : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-KHFHGX : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-LYLTTW : .NET Framework 4.8.1 (4.8.9310.0), X64 RyuJIT VectorSize=256


```
| Method                 | Runtime              | Mean        | Error     | StdDev    | Ratio | RatioSD |
|----------------------- |--------------------- |------------:|----------:|----------:|------:|--------:|
| CreateScope            | .NET 6.0             |    63.24 ns |  1.129 ns |  1.056 ns |  1.65 |    0.05 |
| CreateScope            | .NET 8.0             |    38.47 ns |  0.751 ns |  0.702 ns |  1.00 |    0.03 |
| CreateScope            | .NET 9.0             |    38.34 ns |  0.789 ns |  0.939 ns |  1.00 |    0.03 |
| CreateScope            | .NET Framework 4.7.2 |    88.67 ns |  1.414 ns |  1.254 ns |  2.31 |    0.06 |
|                        |                      |             |           |           |       |         |
| ActivatorUtilitiesCall | .NET 6.0             |   578.22 ns |  9.204 ns |  8.610 ns |  5.22 |    0.11 |
| ActivatorUtilitiesCall | .NET 8.0             |   112.70 ns |  2.030 ns |  1.899 ns |  1.02 |    0.02 |
| ActivatorUtilitiesCall | .NET 9.0             |   110.83 ns |  1.983 ns |  1.855 ns |  1.00 |    0.02 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 1,294.44 ns | 20.845 ns | 19.498 ns | 11.68 |    0.26 |

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

### X64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.204
  [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-ZZWBDH : .NET 6.0.36 (6.0.3624.51421), X64 RyuJIT AVX2
  Job-KSYKED : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-KHFHGX : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-LYLTTW : .NET Framework 4.8.1 (4.8.9310.0), X64 RyuJIT VectorSize=256


```
| Method                 | Runtime              | Mean         | Error      | StdDev     | Ratio | RatioSD |
|----------------------- |--------------------- |-------------:|-----------:|-----------:|------:|--------:|
| DirectCall             | .NET 6.0             |     4.892 ns |  0.0845 ns |  0.0749 ns |  1.07 |    0.05 |
| DirectCall             | .NET 8.0             |     4.675 ns |  0.1568 ns |  0.1540 ns |  1.02 |    0.05 |
| DirectCall             | .NET 9.0             |     4.598 ns |  0.1634 ns |  0.2007 ns |  1.00 |    0.06 |
| DirectCall             | .NET Framework 4.7.2 |     2.195 ns |  0.1113 ns |  0.1041 ns |  0.48 |    0.03 |
|                        |                      |              |            |            |       |         |
| ActivatorCall          | .NET 6.0             |   300.654 ns |  5.9874 ns |  6.4064 ns |  1.91 |    0.05 |
| ActivatorCall          | .NET 8.0             |   160.411 ns |  3.1758 ns |  2.9707 ns |  1.02 |    0.02 |
| ActivatorCall          | .NET 9.0             |   157.310 ns |  2.3840 ns |  2.2300 ns |  1.00 |    0.02 |
| ActivatorCall          | .NET Framework 4.7.2 |   501.914 ns |  9.1399 ns |  8.5494 ns |  3.19 |    0.07 |
|                        |                      |              |            |            |       |         |
| ActivatorUtilitiesCall | .NET 6.0             |   476.253 ns |  7.1240 ns |  6.6638 ns |  7.97 |    0.16 |
| ActivatorUtilitiesCall | .NET 8.0             |    61.397 ns |  0.9035 ns |  0.8452 ns |  1.03 |    0.02 |
| ActivatorUtilitiesCall | .NET 9.0             |    59.780 ns |  1.0054 ns |  0.9405 ns |  1.00 |    0.02 |
| ActivatorUtilitiesCall | .NET Framework 4.7.2 | 1,126.346 ns | 15.1496 ns | 14.1709 ns | 18.85 |    0.37 |

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

### X64

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.4061)
11th Gen Intel Core i7-11800H 2.30GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.204
  [Host]     : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-ZZWBDH : .NET 6.0.36 (6.0.3624.51421), X64 RyuJIT AVX2
  Job-KSYKED : .NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-KHFHGX : .NET 9.0.5 (9.0.525.21509), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  Job-LYLTTW : .NET Framework 4.8.1 (4.8.9310.0), X64 RyuJIT VectorSize=256


```
| Method       | Runtime              | Mean     | Error    | StdDev   | Ratio | RatioSD |
|------------- |--------------------- |---------:|---------:|---------:|------:|--------:|
| DirectCall   | .NET 6.0             | 13.76 ns | 0.226 ns | 0.212 ns |  1.09 |    0.02 |
| DirectCall   | .NET 8.0             | 12.87 ns | 0.284 ns | 0.279 ns |  1.02 |    0.02 |
| DirectCall   | .NET 9.0             | 12.65 ns | 0.172 ns | 0.160 ns |  1.00 |    0.02 |
| DirectCall   | .NET Framework 4.7.2 | 22.50 ns | 0.272 ns | 0.241 ns |  1.78 |    0.03 |
|              |                      |          |          |          |       |         |
| IndirectCall | .NET 6.0             | 13.50 ns | 0.239 ns | 0.212 ns |  1.04 |    0.02 |
| IndirectCall | .NET 8.0             | 13.31 ns | 0.194 ns | 0.182 ns |  1.03 |    0.02 |
| IndirectCall | .NET 9.0             | 12.97 ns | 0.181 ns | 0.160 ns |  1.00 |    0.02 |
| IndirectCall | .NET Framework 4.7.2 | 23.61 ns | 0.425 ns | 0.398 ns |  1.82 |    0.04 |
|              |                      |          |          |          |       |         |
| LambdaCall   | .NET 6.0             | 13.33 ns | 0.230 ns | 0.215 ns |  1.04 |    0.02 |
| LambdaCall   | .NET 8.0             | 13.19 ns | 0.208 ns | 0.195 ns |  1.03 |    0.02 |
| LambdaCall   | .NET 9.0             | 12.83 ns | 0.146 ns | 0.136 ns |  1.00 |    0.01 |
| LambdaCall   | .NET Framework 4.7.2 | 23.66 ns | 0.292 ns | 0.273 ns |  1.84 |    0.03 |

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
