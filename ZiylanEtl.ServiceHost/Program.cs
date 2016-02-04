using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Topshelf;
using Topshelf.Unity;

namespace ZiylanEtl.ServiceHost
{
    internal static class Program
    {
        public static string ServiceName { get; set; }


        public static string Description { get; set; }

        public static string ServiceDisplayName { get; set; }

        static Program()
        {
            ServiceName = "ZiylanEtl.ServiceHost.WindowsService";
            Description = "ZiylanEtl.ServiceHost";
            ServiceDisplayName = "ZiylanEtl.ServiceHost.WindowsService";
            
        }


        public static IUnityContainer UnityContainer { get; private set; }

        private static void Main()
        {
            UnityContainer = BuildUnityContainer();
            HostFactory.Run(configureCallback =>
            {
                configureCallback.UseUnityContainer(UnityContainer);
                configureCallback.SetDisplayName(ServiceDisplayName);
                configureCallback.SetDescription(Description);
                configureCallback.SetServiceName(ServiceName);
                configureCallback.Service<IWindowsService>(
                    settings =>
                    {
                        settings.ConstructUsingUnityContainer();
                        settings.WhenStarted(tc => tc.Start());
                        settings.WhenStopped(tc => tc.Stop());
                        settings.WhenPaused(tc => tc.Pause());
                        settings.WhenContinued(tc => tc.Continue());
                    });
                configureCallback.RunAsLocalSystem();

            });



        }
        static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.LoadConfiguration();
            Logger.SetLogWriter(new LogWriterFactory().Create());

            IConfigurationSource config = ConfigurationSourceFactory.Create();
            ExceptionPolicyFactory factory = new ExceptionPolicyFactory(config);
            ExceptionManager exceptionManager = factory.CreateManager();
            ExceptionPolicy.SetExceptionManager(exceptionManager);
            return container;
        }
    }
}
