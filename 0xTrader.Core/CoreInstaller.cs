using _0xTrader.Core.Services;
using _0xTrader.Core.Services.Abstractions;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nethereum.Web3;
using System;

namespace _0xTrader.Core
{
    public class CoreInstaller : IWindsorInstaller
    {
        private readonly CoreConfiguration _config;

        public CoreInstaller()
        {
            // Todo
            _config = new CoreConfiguration();
        }

        public CoreInstaller(CoreConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<CoreConfiguration>().Instance(_config));
            container.Register(Component.For<IBlockchainAccessor>().ImplementedBy<BlockchainAccessorService>());
            container.Register(Component.For<IBlockchainListener>().ImplementedBy<BlockchainListenerService>());
            container.Register(Component.For<IRepoAccessor>().ImplementedBy<RepoAccessorService>());
            container.Register(Component.For<IWeb3>().UsingFactoryMethod(k =>
            {
                return new Web3(_config.NodeUrl);
            }));
        }
    }
}
