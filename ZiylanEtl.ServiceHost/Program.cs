using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Topshelf;
using Topshelf.Unity;
using Unity.Wcf;

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
            return container.LoadConfiguration();
            //section.Configure(container);
            //return container;
        }
    }
}
