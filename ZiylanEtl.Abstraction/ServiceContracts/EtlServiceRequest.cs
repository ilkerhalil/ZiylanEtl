using System.Runtime.Serialization;

namespace ZiylanEtl.Abstraction.ServiceContracts
{
    [DataContract]
    public class EtlServiceRequest
    {
        [DataMember]
        public string ServiceName { get; set; }
    }
}