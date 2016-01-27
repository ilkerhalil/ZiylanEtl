using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;

namespace ZiylanEtl.Abstraction.ServiceContracts
{
    [ServiceContract]
    public interface IEtlChildService
    {

        string ServiceName { get; }

        [OperationContract(IsOneWay = true)]
        void StartService();

        IDictionary<string, object> ChildServiceParameters { get; }


    }
}