using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZiylanEtl.Abstraction.ServiceContracts
{
    [DataContract]
    [Serializable]
    public class EtlServiceRequest
    {
        [DataMember]
        public string ServiceName { get; set; }
        [DataMember]
        public Dictionary<string,object> ServiceParameter { get; set; }

        public override string ToString()
        {
            return $"ServiceName: {ServiceName}";
        }
    }
}