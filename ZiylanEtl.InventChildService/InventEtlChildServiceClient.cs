using ZiylanEtl.Abstraction.ServiceContracts;

namespace ZiylanEtl.InventChildService
{
    /// <summary>
    /// todo:Web Service client implement edilecek
    /// todo:Toplam ne kadar sürdü,web servis metod başına ne kadar sürdü,loglanmalı
    /// todo:IDispose implement edilmeli
    /// </summary>
    public class InventEtlChildServiceClient : IEtlChildService
    {
        public string ServiceName { get; }

        public void StartService()
        {
            throw new System.NotImplementedException();
        }
    }
}
