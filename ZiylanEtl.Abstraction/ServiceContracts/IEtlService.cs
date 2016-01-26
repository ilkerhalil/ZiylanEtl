using System.ServiceModel;
using System.ServiceModel.Web;

namespace ZiylanEtl.Abstraction.ServiceContracts
{
    [ServiceContract]
    public interface IEtlService
    {
        [OperationContract(IsOneWay = true)]
        [WebInvoke]
        void StartChildService(EtlServiceRequest etlServiceRequest);
    }
}
