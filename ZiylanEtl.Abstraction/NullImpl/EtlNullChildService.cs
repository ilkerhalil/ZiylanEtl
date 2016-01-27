using System;
using ZiylanEtl.Abstraction.ServiceContracts;

namespace ZiylanEtl.Abstraction.NullImpl
{
    public class EtlNullChildService : BaseEtlChildService
    {
        public override string ServiceName { get; } = "NULL Child Service";


        public override void StartService()
        {
            throw new Exception();
            //Console.WriteLine("Null Service");
        }
    }
}