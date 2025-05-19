using System;
using System.Linq.Expressions;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using Microsoft.Extensions.DependencyInjection;

namespace ActivatorBenchmarks
{
    public class Program
    {
        public static void Main()
        {
            var baseJob = Job.Default;
            var config = DefaultConfig.Instance
                .AddJob( baseJob.WithRuntime( CoreRuntime.Core90 ).WithBaseline( true ) )
                .AddJob( baseJob.WithRuntime( CoreRuntime.Core80 ) )
                .AddJob( baseJob.WithRuntime( CoreRuntime.Core60 ) )
                .AddJob( baseJob.WithRuntime( ClrRuntime.Net472 ) );

            BenchmarkRunner.Run<EmptyConstructor>( config );
            BenchmarkRunner.Run<TransientParameter>( config );
            BenchmarkRunner.Run<ScopedParameter>( config );
            BenchmarkRunner.Run<SingletonParameter>( config );
            BenchmarkRunner.Run<MethodCall>( config );
        }
    }

    #region Benchmarks

    [HideColumns( "Job" )]
    public class EmptyConstructor
    {
        private IServiceProvider _serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Benchmark]
        public EmptyPoco DirectCall()
        {
            return new EmptyPoco();
        }

        [Benchmark]
        public EmptyPoco ActivatorCall()
        {
            return Activator.CreateInstance<EmptyPoco>();
        }

        [Benchmark]
        public object ActivatorUtilitiesCall()
        {
            return ActivatorUtilities.CreateInstance<EmptyPoco>( _serviceProvider );
        }
    }

    [HideColumns( "Job" )]
    public class TransientParameter
    {
        private IServiceProvider _serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<DependencyA>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Benchmark]
        public SimplePoco DirectCall()
        {
            var dependency = new DependencyA();

            return new SimplePoco( dependency );
        }

        [Benchmark]
        public SimplePoco ActivatorCall()
        {
            var dependency = new DependencyA();

            return ( SimplePoco ) Activator.CreateInstance( typeof( SimplePoco ), dependency );
        }

        [Benchmark]
        public SimplePoco ActivatorUtilitiesCall()
        {
            return ActivatorUtilities.CreateInstance<SimplePoco>( _serviceProvider );
        }
    }

    [HideColumns( "Job" )]
    public class ScopedParameter
    {
        private IServiceProvider _serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<DependencyA>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Benchmark]
        public void CreateScope()
        {
            using ( var scope = _serviceProvider.CreateScope() )
            {
            }
        }

        [Benchmark]
        public SimplePoco ActivatorUtilitiesCall()
        {
            using ( var scope = _serviceProvider.CreateScope() )
            {
                return ActivatorUtilities.CreateInstance<SimplePoco>( _serviceProvider );
            }
        }
    }

    [HideColumns( "Job" )]
    public class SingletonParameter
    {
        private DependencyA _dependency;
        private IServiceProvider _serviceProvider;

        [GlobalSetup]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<DependencyA>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
            _dependency = new DependencyA();
        }

        [Benchmark]
        public SimplePoco DirectCall()
        {
            return new SimplePoco( _dependency );
        }

        [Benchmark]
        public SimplePoco ActivatorCall()
        {
            return ( SimplePoco ) Activator.CreateInstance( typeof( SimplePoco ), _dependency );
        }

        [Benchmark]
        public SimplePoco ActivatorUtilitiesCall()
        {
            return ActivatorUtilities.CreateInstance<SimplePoco>( _serviceProvider );
        }
    }

    [HideColumns( "Job" )]
    public class MethodCall
    {
        private Func<double, double, double> _fn;
        private Func<double, double, double> _expr;

        [GlobalSetup]
        public void Setup()
        {
            _fn = Math.Pow;

            var aExpr = Expression.Parameter( typeof( double ), "a" );
            var bExpr = Expression.Parameter( typeof( double ), "b" );
            var callExpr = Expression.Call( null, typeof( Math ).GetMethod( "Pow" ), aExpr, bExpr );
            var lambdaExpr = Expression.Lambda<Func<double, double, double>>( callExpr, aExpr, bExpr );

            _expr = lambdaExpr.Compile();
        }

        [Benchmark]
        public double DirectCall()
        {
            return Math.Pow( 7, 13 );
        }

        [Benchmark]
        public double IndirectCall()
        {
            return _fn( 7, 13 );
        }

        [Benchmark]
        public double LambdaCall()
        {
            return _expr( 7, 13 );
        }
    }

    #endregion

    #region Test Classes

    public class EmptyPoco
    {
    }

    public class SimplePoco
    {
        private readonly DependencyA _value;

        public SimplePoco( DependencyA value )
        {
            _value = value;
        }
    }

    public class DependencyA
    {
        public DependencyA()
        {
        }
    }

    #endregion
}
