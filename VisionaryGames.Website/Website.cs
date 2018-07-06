using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net.Sockets;

namespace VisionaryGames.Website
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Website : StatelessService
    {
        public Website(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
            new ServiceInstanceListener(
                serviceContext =>
                    new HttpSysCommunicationListener(
                        serviceContext,
                        "ServiceEndpoint",
                        (url, listener) =>
                        {
                            ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting HttpSys on {url}");

                            return new WebHostBuilder()
                                .UseHttpSys()
                                .ConfigureServices(
                                    services => services
                                        .AddSingleton<HttpClient>(new HttpClient())
                                        .AddSingleton<FabricClient>(new FabricClient())
                                        .AddSingleton<StatelessServiceContext>(serviceContext))
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup<Startup>()
                                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                .UseUrls(url)
                                .Build();
                        }))
            };
        }
    }

    public static class KestrelServerOptionsHttpsX509StoreExtensions
    {
        public static ListenOptions UseHttps(
            this ListenOptions options,
            string subjectName,
            StoreName storeName,
            StoreLocation storeLocation)
        {
            var certStore = new X509Store(storeName, storeLocation);
            certStore.Open(OpenFlags.ReadOnly);
            var certificates = certStore.Certificates
                .Find(X509FindType.FindByThumbprint, subjectName, validOnly: false);

            var certificate = certificates.OfType<X509Certificate2>().First();

            return options.UseHttps(certificate);
        }
    }

}
