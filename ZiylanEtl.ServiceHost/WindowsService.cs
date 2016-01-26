using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Configuration;
using Unity.Wcf;
using ZiylanEtl.Abstraction.ServiceContracts;
using ZiylanEtl.EtlWebService;

namespace ZiylanEtl.ServiceHost
{
    public class WindowsService : IWindowsService
    {
        //private readonly IEtlService _etlWebService;
        private readonly System.ServiceModel.ServiceHost _serviceHost;

        public WindowsService(IEtlService etlService)
        {
            //_serviceHost = new UnityServiceHost(Program.UnityContainer, typeof(DefaultEtlWebService));
            //_serviceHost = new System.ServiceModel.ServiceHost(typeof (DefaultEtlWebService));

            //var section = ConfigurationManager.GetSection("system.serviceModel/services") as ServicesSection;
            //var element = section.Services.Cast<ServiceElement>().Single();
            //var address = element.Endpoints.Cast<ServiceEndpointElement>().Select(s => s.Address);
            _serviceHost = new UnityServiceHost(Program.UnityContainer, typeof(DefaultEtlWebService));

        }

        public void Start()
        {

            _serviceHost.Open();

        }

        public void Stop()
        {
            _serviceHost.Close();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Continue()
        {
            throw new NotImplementedException();
        }
    }
}