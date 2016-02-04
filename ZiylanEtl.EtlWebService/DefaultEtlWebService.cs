using System;
using System.Linq;
using ZiylanEtl.Abstraction.ServiceContracts;

namespace ZiylanEtl.EtlWebService
{
    public class DefaultEtlWebService : IEtlService
    {
        private readonly IEtlChildService[] _services;

        public DefaultEtlWebService(IEtlChildService[] services)
        {
            _services = services;
        }

        public void StartChildService(EtlServiceRequest etlServiceRequest)
        {
            try
            {
                var etlChildService = _services.Single(s => s.ServiceName == etlServiceRequest.ServiceName);
                    
                    if (etlServiceRequest.ServiceParameter != null)
                {
                    if (etlServiceRequest.ServiceParameter.Any())
                    {
                        foreach (var serviceParameter in etlServiceRequest.ServiceParameter)
                        {
                            etlChildService.ChildServiceParameters.Add(serviceParameter);
                        }
                    }
                }
                etlChildService.StartService();
            }
            catch (Exception exception)
            {
                exception.Data.Add("etlServiceRequest", etlServiceRequest);
                throw;
            }
        }
    }
}
