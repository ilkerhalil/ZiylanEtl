using System;
using System.Runtime.Serialization;

namespace ZiylanEtl.Abstraction.ServiceContracts
{
    [DataContract]
    [Serializable]
    public class EtlServiceRequest
    {
        [DataMember]
        public string ServiceName { get; set; }

        public override string ToString()
        {
            return $"ServiceName: {ServiceName}";
        }
    }
}