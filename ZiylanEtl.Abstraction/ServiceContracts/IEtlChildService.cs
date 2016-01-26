using System.Security.Cryptography;
using System.ServiceModel;

namespace ZiylanEtl.Abstraction.ServiceContracts
{
    [ServiceContract]
    public interface IEtlChildService
    {

        string ServiceName { get; }

        [OperationContract(IsOneWay = true)]
        void StartService();

    }
}