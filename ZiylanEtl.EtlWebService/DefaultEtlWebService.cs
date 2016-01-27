using System.Collections.Generic;
using System.Linq;
using ZiylanEtl.Abstraction.ServiceContracts;

namespace ZiylanEtl.EtlWebService
{
    public class DefaultEtlWebService : IEtlService
    {
        private readonly IEnumerable<IEtlChildService> _services;

        public DefaultEtlWebService(IEtlChildService[] services)
        {
            _services = services;
        }

        public void StartChildService(EtlServiceRequest etlServiceRequest)
        {
            var etlChildService = _services.Single(s => s.ServiceName == etlServiceRequest.ServiceName);
        }
    }
}
