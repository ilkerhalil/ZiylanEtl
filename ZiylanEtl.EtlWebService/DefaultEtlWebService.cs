using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

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

            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint =
                prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            string ip = endpoint.Address;

            Console.WriteLine("Tetiklendi IP : " + ip);

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
