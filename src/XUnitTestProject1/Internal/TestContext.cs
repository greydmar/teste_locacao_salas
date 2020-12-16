using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Ag3.Util;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace mtgroup.locacao.Internal
{
    /// <summary>
    /// Simple dependency injector Context
    /// </summary>
    public sealed class TestContext: IServiceProvider
    {
        private static TestContext _instance;

        private readonly IServiceCollection _services;
        private readonly ServiceProvider _provider;

        public static TestContext Provider
        {
            get
            {
                return _instance ??= TestContext.Initialize();
            }
        }

        private TestContext()
        {
            _services = new ServiceCollection();
            _provider = _services.BuildServiceProvider();

        }

        private TestContext(ServiceCollection serviceCollection, ServiceProvider provider)
        {
            _services = serviceCollection;
            _provider = provider;
        }

        private static TestContext Initialize()
        {
            return new TestContext();
        }

        public object GetService(Type serviceType)
        {
            return _provider.GetService(serviceType);
        }

        internal static void InitializeOnce<TStartupClass>() 
            where TStartupClass : class, new()
        {
            if (_instance != null)
                return;

            var wrapper = StartupClassWrapper.TryInitialize<TStartupClass>();

            var serviceCollection = new ServiceCollection();
            wrapper.ConfigureServices(serviceCollection);

            var provider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions()
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            InterlockedUtils.ThreadSafeReplacement(ref _instance, () => new TestContext(serviceCollection, provider));
        }
    }

    public sealed class TestContextFactory<TStartupClass>: Component
        where TStartupClass: class, new()
    {
        public TestContextFactory()
        {
            TestContext.InitializeOnce<TStartupClass>();
        }

    }


    internal class StartupClassWrapper
    {
        private readonly object _startupClass;
        private readonly MethodInfo _mInfo;

        private StartupClassWrapper(object startupClass, MethodInfo mInfo)
        {
            _startupClass = startupClass;
            _mInfo = mInfo;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _mInfo.Invoke(_startupClass, new[] {services});
        }

        public static StartupClassWrapper TryInitialize<TStartupClass>() 
            where TStartupClass : class, new()
        {
            var mInfo = typeof(TStartupClass)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(m => m.GetParameters().Length == 1
                                     && m.GetParameters().First().ParameterType == typeof(IServiceCollection));

            if (mInfo == null)
                throw new InvalidOperationException(
                    $"Startup class \"{typeof(TStartupClass).FullName}\" should have at least " +
                    $"one public instance method with \"{typeof(IServiceCollection).FullName}\" parameter!");

            return new StartupClassWrapper(new TStartupClass(), mInfo);
        }
    }

    public interface IContextAwareFixture<TStartupClass> : IClassFixture<TestContextFactory<TStartupClass>>
        where TStartupClass: class, new() { }
}
