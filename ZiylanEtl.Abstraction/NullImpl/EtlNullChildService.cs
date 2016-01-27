using System;
using ZiylanEtl.Abstraction.ServiceContracts;

namespace ZiylanEtl.Abstraction.NullImpl
{
    public class EtlNullChildService : IEtlChildService
    {
        public string ServiceName { get; } = "NULL Child Service";


        public void StartService()
        {
            throw new Exception();
            Console.WriteLine("Null Service");
        }
    }
}